# Journal DocAssist

## S1 · Séance 2 — mer. 28 oct. 2026 (nouveau PC)
**Fait**
- Nouveau PC : Azure CLI installé, `az login`, abonnement sélectionné, user-secrets recréés
- Solution `DocAssist.slnx` + projet `src/DocAssist.Cli`
  (Azure.AI.OpenAI, Azure.Identity, Microsoft.Extensions.AI.OpenAI, UserSecrets)
- Premier appel via `IChatClient` + `AzureCliCredential` : « Pod vs Deployment » → réponse en français
- Affichage des tokens, de la durée et du coût estimé
- Clés Key1/Key2 régénérées (`-o none`), secret AZURE_OPENAI_API_KEY supprimé
**Chiffres**
- 55 tokens envoyés, 134 reçus, 2 838 ms
- Coût : prix de gpt-5.4-mini à renseigner dans Program.cs → … par requête
**Appris**
- `IChatClient` = interface unique vers le modèle ; AzureOpenAIClient seulement à la construction
- Rôles : System = consignes, User = question, Assistant = réponses précédentes
- API sans état : tout l'historique (System compris) est renvoyé et payé à chaque appel
- gpt-5.4-mini = modèle de raisonnement : tokens de réflexion, pas de Temperature, MaxOutputTokens large
- Local : `az login` + AzureCliCredential ; AKS : Workload Identity. Jamais de secret.
**Bloqué / résolu**
- 'az' non reconnu → Azure CLI absent sur ce PC → `winget install`, nouveau terminal
- az login « aucun abonnement » → bon tenant, puis choix de l'abonnement dans la liste
- VS Code : serveur C# en OutOfMemory → ouvrir uniquement le dossier du dépôt + Reload Window
- Resource not found (404) → endpoint et déploiement alignés sur la même ressource (doflamingoAIResources / gpt-5.4-mini)
- Clé API visible sur une capture → Key1 et Key2 régénérées. Plus jamais de secret à l'écran.
- `git add .` lancé depuis src/DocAssist.Cli → solution non commitée → commits depuis la racine
**Questions de contrôle** (de tête)
1. Pourquoi IChatClient plutôt qu'AzureOpenAIClient ? …
2. System renvoyé à chaque appel : impact ? …
3. AzureCliCredential en local, quoi sur AKS ? …
**Prochaine séance**
- S1 · Séance 3 : prompting et sortie structurée en JSON
- Avant : renseigner les prix dans Program.cs ; tester « Réponds en texte brut, sans Markdown »

## S1 · Séance 1 — lun. 26 oct. 2026
**Fait**
- Dépôt GitHub `docassist` créé et cloné, Git installé
- `.gitignore` (dotnet new gitignore + bloc DocAssist), `journal.md`, `plus-tard.md`
- Compte Azure + alerte budget à 20 €
- Théorie : Microsoft Learn « Understand tokens » (livre pas encore reçu), tokenizer
**Chiffres**
- Tokenizer, même phrase : … tokens en français / … en anglais
**Appris**
- Token = unité de facturation, de limite et de débit
- Fenêtre de contexte = entrée + sortie ensemble
- Coût = tokens envoyés × prix d'entrée + tokens reçus × prix de sortie
- Température = part de hasard : basse = stable, haute = varié, voire absurde
- Le français consomme plus de tokens que l'anglais
**Bloqué / résolu**
- Température non testable en code : pas de programme ce jour-là, puis gpt-5.4-mini refuse
  le paramètre → principe vu avec la démo interactive
**Questions de contrôle** (de tête)
1. Pourquoi un long contexte coûte plus cher ? …
2. Si la doc dépasse la fenêtre de contexte ? …
3. Température basse ou haute pour DocAssist, et pourquoi ? …
**Prochaine séance**
- Premier appel au modèle depuis une console .NET (fait le 28 oct.)

## S0 · Préparation — sam. 24 oct. 2026
**Fait**
- Dépôt GitHub créé et cloné, Git installé
- Premier programme Azure OpenAI qui résume benefits.md
**Bloqué / résolu**
- CredentialUnavailableException → installé Azure CLI, `az login`,
  puis remplacé DefaultAzureCredential par AzureCliCredential
**Appris**
- user-secrets : les secrets restent hors du dépôt