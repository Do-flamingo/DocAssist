# Agent.md - DocAssist

## Le projet
DocAssist est un assistant de documentation sur Kubernetes. Il répond au question et cite aussi ses sources.
J'ai créé ce projet pour ma formation personnel sur le developpement .NET IA

## Stack technique
- .NET 10, c#
- Micorosft.Extensions.AI (ICHatClient) + AZURE OPENAI (déploiement 'gpt-5.4-mini', modèle de raisonement)
- Authentification sans clé : 'AzureCliCredential' en local

## Structure
- `DocAssist.slnx`: solution, à la racine
- `src\DocAssist.Cli`: console actuelle (appel au modèle + réponse en JSON typée)
- `journal.md`, `plus-tard.md` : suivi personnel de l'auteur
- Cible, à ne PAS créer sans demande : `src/Api`, `src/Ingestion.Worker`, `eval/`, `deploy/helm`, `docs/adr`

## Commandes
-Prérequis: `az login` fait, user-secrets `AZURE_OPENAI_ENDPOINT` et `AZURE_OPENAI_GPT_NAME` définis

sur quel projet tu trava
## Conventions
- Le code du projet dépend de IChatClient; 'AzureOpenAIClient' n'apparait qu' à la construction du client
- Les réponses du modèle sont typées avec 'GetResponseAsync<T>'
- Pas de refactoring non demandé

## Interdits
- Jamais de clé d'API, secret, endpoint ou ID de tenant dans le code, la config ou un fichier commité.
  Les secrets passent par `dotnet user-secrets`. Raison : le dépôt est public sur GitHub.
- Ne jamais remplacer `AzureCliCredential` par une clé d'API.
- Ne pas modifier `journal.md`, `plus-tard.md` ni `.gitignore` sans demande explicite.
- Ne pas ajouter de package NuGet sans le signaler et le justifier.
- Aucune donnée professionnelle ou privée : corpus public uniquement.
- Ne jamais lancer `git commit` ni `git push` : l'auteur relit et commite lui-même.
- Ne jamais lancer de commande `az` qui crée, modifie ou supprime une ressource Azure.
- ne fais pas de 'dotnet run', car le code appel le modèle et c'est payant

## Avant de dire « terminé »
- `dotnet build` passe sans nouvel avertissement.
- Lister les fichiers modifiés, avec la raison de chaque changement.
- Si une consigne est ambiguë : poser une question plutôt que deviner.