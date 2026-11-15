using Azure.AI.OpenAI;
using Azure.Identity;
using CommunityToolkit.VectorData.InMemory;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.VectorData;

const string dossierCorpus = "data/website/content/en/docs/concepts";

string[] pages =
[
    "overview/components.md",
    "overview/working-with-objects/labels.md",
    "overview/working-with-objects/namespaces.md",
    "workloads/pods/_index.md",
    "workloads/pods/pod-lifecycle.md",
    "workloads/pods/init-containers.md",
    "workloads/controllers/deployment.md",
    "workloads/controllers/replicaset.md",
    "workloads/controllers/statefulset.md",
    "workloads/controllers/daemonset.md",
    "workloads/controllers/job.md",
    "workloads/autoscaling/horizontal-pod-autoscale.md",
    "services-networking/service.md",
    "services-networking/ingress.md",
    "services-networking/network-policies.md",
    "storage/volumes.md",
    "storage/persistent-volumes.md",
    "configuration/configmap.md",
    "configuration/secret.md",
    "containers/images.md"
];

if (!Directory.Exists(dossierCorpus))
{
    Console.Error.WriteLine($"Corpus introuvable : {dossierCorpus}. Lance la console depuis la racine du dépôt.");
    Environment.ExitCode = 1;
    return;
}

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string endpoint = config["AZURE_OPENAI_ENDPOINT"]
    ?? throw new InvalidOperationException("Secret AZURE_OPENAI_ENDPOINT manquant");
string deployment = config["AZURE_OPENAI_EMBEDDING_NAME"]
    ?? throw new InvalidOperationException("Secret AZURE_OPENAI_EMBEDDING_NAME manquant");

IEmbeddingGenerator<string, Embedding<float>> generateur =
    new AzureOpenAIClient(new Uri(endpoint), new AzureCliCredential())
        .GetEmbeddingClient(deployment)
        .AsIEmbeddingGenerator();

// 1. Découper les pages en passages
List<Passage> passages = [];
foreach (string page in pages)
    passages.AddRange(Decoupage.EnPassages(page, File.ReadAllText(Path.Combine(dossierCorpus, page))));
for (int i = 0; i < passages.Count; i++)
    passages[i].Cle = i;

//2.Calculer les embeddings par lots et les ranger dans le magasin de vecteurs
var magasin = new InMemoryVectorStore();
VectorStoreCollection<int, Passage> collection = magasin.GetCollection<int, Passage>("passages");
await collection.EnsureCollectionExistsAsync();

long tokens = 0;
foreach (Passage[] lot in passages.Chunk(50))
{
    var vecteurs = await generateur.GenerateAsync(lot.Select(p => p.Texte));
    for (int i = 0; i < lot.Length; i++)
        lot[i].Vecteur = vecteurs[i].Vector;
    await collection.UpsertAsync(lot);
    tokens += vecteurs.Usage?.InputTokenCount ?? 0;
}
Console.WriteLine($"{passages.Count} passages indexés, {tokens} tokens");

// 3. Chercher les 3 passages les plus proches de chaque question
while (true)
{
    Console.Write("\nQuestion (vide pour quitter) : ");
    string? question = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(question))
        break;

    ReadOnlyMemory<float> vecteurQuestion = await generateur.GenerateVectorAsync(question);
    await foreach (VectorSearchResult<Passage> resultat in collection.SearchAsync(vecteurQuestion, top: 3))
    {
        string apercu = resultat.Record.Texte.ReplaceLineEndings(" ");
        if (apercu.Length > 160)
            apercu = apercu[..160] + "…";

        Console.WriteLine($"  {resultat.Score:F3}  {resultat.Record.Source} › {resultat.Record.Titre}");
        Console.WriteLine($"         {apercu}");
    }
}