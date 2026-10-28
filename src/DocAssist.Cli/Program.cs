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

    List<ChatMessage> messages =
    [
        new (ChatRole.System,
        "Tu es DocAssist, un assistant expert de la documentation Kubernetes. Réponds en texte brut, sans Markdown." +
        "Réponds en français, en 5 phrases maximum. Si tu ne sais pas, dis-le."),
        new (ChatRole.User, "Quelle est la différence entre un Pod et un Deployment ?")
    ];

var chrono = Stopwatch.StartNew();
ChatResponse response = await client.GetResponseAsync(
    messages, new ChatOptions { MaxOutputTokens = 500 });
chrono.Stop();

// Prix par million de tokens : à recopier depuis la page de tarifs Azure OpenAI
const decimal prixEntree = 0m;
const decimal prixSortie = 0m;
long entree = response.Usage?.InputTokenCount ?? 0;
long sortie = response.Usage?.OutputTokenCount ?? 0;
decimal cout = entree * prixEntree / 1_000_000m + sortie * prixSortie / 1_000_000m;

Console.WriteLine(response.Text);
Console.WriteLine();
Console.WriteLine($"Tokens envoyés : {entree}");
Console.WriteLine($"Tokens reçus   : {sortie}");
Console.WriteLine($"Durée          : {chrono.ElapsedMilliseconds} ms");
Console.WriteLine($"Coût estimé    : {cout:F6}");