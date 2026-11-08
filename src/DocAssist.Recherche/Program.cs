
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using System.Numerics.Tensors;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string endpoint = config["AZURE_OPENAI_ENDPOINT"]
    ?? throw new InvalidOperationException("Secret AZURE_OPENAI_ENDPOINT manquant");
string deployment = config["AZURE_OPENAI_EMBEDDING_NAME"]
    ?? throw new InvalidOperationException("Secret AZURE_OPENAI_EMBEDDING_NAME manquant");

IEmbeddingGenerator<string, Embedding<float>> generateur =
    new AzureOpenAIClient(new Uri(endpoint), new AzureCliCredential())
        .GetEmbeddingClient(deployment)
        .AsIEmbeddingGenerator();

string[] phrases =
[
    "Un Pod regroupe un ou plusieurs conteneurs qui partagent le réseau.",
    "Le plus petit objet déployable dans Kubernetes est le Pod.",
    "A Pod is the smallest deployable unit in Kubernetes.",
    "Un Service expose un ensemble de Pods sous une adresse stable.",
    "Pour rendre une application accessible dans le cluster, on crée un Service.",
    "Un PersistentVolume fournit un stockage qui survit aux Pods.",
    "Quand un conteneur redémarre, les fichiers qu'il avait écrits sont perdus.",
    "Le pod de café est coincé dans la machine à expresso.",
    "La recette du couscous demande de la semoule et des légumes.",
    "Il pleut sur Paris depuis ce matin."
];

GeneratedEmbeddings<Embedding<float>> vecteurs = await generateur.GenerateAsync(phrases);

Console.WriteLine($"Dimension : {vecteurs[0].Vector.Length}");
Console.WriteLine($"Tokens    : {vecteurs.Usage?.InputTokenCount}");
Console.WriteLine();

for (int i = 0; i < phrases.Length; i++)
{
    var voisines = Enumerable.Range(0, phrases.Length)
        .Where(j => j != i)
        .Select(j => (Index: j, Score: TensorPrimitives.CosineSimilarity(
            vecteurs[i].Vector.Span, vecteurs[j].Vector.Span)))
        .OrderByDescending(v => v.Score)
        .Take(2);

    Console.WriteLine($"[{i + 1}] {phrases[i]}");
    foreach (var (index, score) in voisines)
        Console.WriteLine($"      {score:F3}  [{index + 1}] {phrases[index]}");
}