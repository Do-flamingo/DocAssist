using System.Diagnostics;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string? question = LectureQuestion.Depuis(args);

if (question is null)
{
    Console.WriteLine("Usage : DocAssist.Cli \"<question>\"");
    Environment.ExitCode = 1;
    return;
}

string endpoint = config["AZURE_OPENAI_ENDPOINT"] 
    ?? throw new InvalidOperationException("Secret AZURE_OPENAI_ENDPOINT manquant");

string deployment = config["AZURE_OPENAI_GPT_NAME"]
    ?? throw new InvalidOperationException("Secret AZURE_OPENAI_GPT_NAME manquant");

   IChatClient client = new AzureOpenAIClient(new Uri(endpoint), new AzureCliCredential())
    .GetChatClient(deployment)
    .AsIChatClient();

const string systemPrompt = """
    Tu es DocAssist, un assistant expert de la documentation officielle de Kubernetes.
    Des développeurs t'utilisent pour comprendre Kubernetes rapidement.

    Règles :
    - Réponds en français, en texte brut, en 5 phrases maximum.
    - Si la question ne concerne pas Kubernetes, mets HorsSujet à true
      et réponds en une phrase que tu ne traites que Kubernetes.
    - N'invente jamais. Si tu n'es pas sûr, mets Certitude à Faible et dis-le.
      Une réponse fausse est pire qu'une absence de réponse.
    - ObjetsKubernetes liste les objets Kubernetes cités dans ta réponse.

    <exemples>
      <exemple>
        <question>Qu'est-ce qu'un Service ?</question>
        <comportement>HorsSujet faux, réponse courte, ObjetsKubernetes : Service, Pod, Certitude Elevee.</comportement>
      </exemple>
      <exemple>
        <question>Quel temps fera-t-il demain à Paris ?</question>
        <comportement>HorsSujet vrai, ObjetsKubernetes vide, réponse : « Je réponds uniquement aux questions sur Kubernetes. »</comportement>
      </exemple>
    </exemples>
    """;

List<ChatMessage> messages = new()
{
    new ChatMessage(ChatRole.System, systemPrompt),
    new ChatMessage(ChatRole.User, question)
};

var chrono = Stopwatch.StartNew();

ChatResponse<ReponseDocAssist> response = await client.GetResponseAsync<ReponseDocAssist>(
    messages, new ChatOptions { MaxOutputTokens = 2000 });

chrono.Stop();

Console.WriteLine($"Q : {question}");
if (response.TryGetResult(out ReponseDocAssist? r))
{
    Console.WriteLine($"  Hors sujet : {r.HorsSujet}");
    Console.WriteLine($"  Certitude  : {r.Certitude}");
    Console.WriteLine($"  Objets     : {string.Join(", ", r.ObjetsKubernetes)}");
    Console.WriteLine($"  Réponse    : {r.Reponse}");
}
else
{
    Console.WriteLine($"  Réponse non conforme au schéma : {response.Text}");
}

Console.WriteLine($"  Tokens     : {response.Usage?.InputTokenCount} envoyés, {response.Usage?.OutputTokenCount} reçus");    

    // Prix par million de tokens : à recopier depuis la page de tarifs Azure OpenAI
const decimal prixEntree = 0.75m;
const decimal prixSortie = 4.50m;
long entree = response.Usage?.InputTokenCount ?? 0;
long sortie = response.Usage?.OutputTokenCount ?? 0;
decimal cout = CalculCout.Estimer(entree, sortie, prixEntree, prixSortie);

Console.WriteLine($"Durée          : {chrono.ElapsedMilliseconds} ms");
Console.WriteLine($"Coût estimé    : {cout:F6}");
Console.WriteLine();
