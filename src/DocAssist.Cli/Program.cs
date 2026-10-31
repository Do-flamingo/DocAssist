using System.Diagnostics;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

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

    """;

string[] questions =
[
    "Quelle est la différence entre un Pod et un Deployment ?",
    "Quelle est la meilleure recette de couscous ?",
    "Comment configurer une PodSecurityPolicy dans Kubernetes 1.30 ?"
];

foreach (string question in questions)
{
    List<ChatMessage> messages = new()
    {
        new ChatMessage(ChatRole.System, systemPrompt),
        new ChatMessage(ChatRole.User, question)
    };


    var chrono = Stopwatch.StartNew();

    ChatResponse<ReponseDocAssist> response = await client.GetResponseAsync<ReponseDocAssist>(
        messages, new ChatOptions { MaxOutputTokens = 500 });

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
    decimal cout = entree * prixEntree / 1_000_000m + sortie * prixSortie / 1_000_000m;

    Console.WriteLine($"Durée          : {chrono.ElapsedMilliseconds} ms");
    Console.WriteLine($"Coût estimé    : {cout:F6}");
    Console.WriteLine();
}



// // Prix par million de tokens : à recopier depuis la page de tarifs Azure OpenAI
// const decimal prixEntree = 0.75m;
// const decimal prixSortie = 4.50m;
// long entree = response.Usage?.InputTokenCount ?? 0;
// long sortie = response.Usage?.OutputTokenCount ?? 0;
// decimal cout = entree * prixEntree / 1_000_000m + sortie * prixSortie / 1_000_000m;

// Console.WriteLine(response.Text);
// Console.WriteLine();
// Console.WriteLine($"Tokens envoyés : {entree}");
// Console.WriteLine($"Tokens reçus   : {sortie}");
// Console.WriteLine($"Durée          : {chrono.ElapsedMilliseconds} ms");
// Console.WriteLine($"Coût estimé    : {cout:F6}");