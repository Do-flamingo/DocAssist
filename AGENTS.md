# AGENTS.md - DocAssist

## Le projet
DocAssist est un assistant de documentation sur Kubernetes. Il répond au question et cite aussi ses sources.
J'ai créé ce projet pour ma formation personnel sur le developpement .NET IA

## Stack technique
- .NET 10, c#
- Microsft.Extensions.AI (IChatClient) + AZURE OPENAI (déploiement 'gpt-5.4-mini', modèle de raisonnement)
- Authentification sans clé : 'AzureCliCredential' en local
- Embeddings : déploiement `text-embedding-3-small` (`IEmbeddingGenerator`), sur la même ressource Azure
  
## Structure
- `DocAssist.slnx` : solution, à la racine
- `src/DocAssist.Cli/` : console de questions (appel au modèle, réponse JSON typée)
- `src/DocAssist.Recherche/` : embeddings et similarité cosinus ; deviendra la recherche sémantique
- `tests/DocAssist.Cli.Tests/` : tests xUnit de la console
- `docs/us/` : user stories (`_modele.md` = modèle vide) ; `docs/grille-revue.md` : grille de revue
- `journal.md`, `plus-tard.md` : suivi personnel de l'auteur
- Cible, à ne PAS créer sans demande : `src/Api`, `src/Ingestion.Worker`, `eval/`, `deploy/helm`, `docs/adr`

## Commandes
- Vérification obligatoire avant de dire " terminé " : `dotnet build` puis `dotnet test`, depuis la racine 
- Prérequis pour lancer une console : `az login` fait ; user-secrets `AZURE_OPENAI_ENDPOINT` +
  `AZURE_OPENAI_GPT_NAME` (Cli) ou `AZURE_OPENAI_EMBEDDING_NAME` (Recherche).
  Si un secret manque : s'arrêter et le signaler, ne jamais l'écrire dans un fichier.

## Tests
- Projet : `tests/DocAssist.Cli.Tests` (xUnit).
- Un test n'appelle jamais Azure ni le réseau : remplacer `IChatClient` par un faux.

## Conventions
- Le code du projet dépend de IChatClient; 'AzureOpenAIClient' n'apparait qu' à la construction du client
- Les réponses du modèle sont typées avec 'GetResponseAsync<T>'
- Pas de refactoring non demandé
- Nommage : domaine en français (ReponseDocAssist, CalculCout), termes techniques .NET en anglais.
- Nullable activé, aucun nouvel avertissement.
- Une classe par fichier.
- Les commentaires expliquent le pourquoi, pas le quoi.

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
- Lister les fichiers modifiés, avec la raison de chaque changement.
- Si une consigne est ambiguë : poser une question plutôt que deviner.
- dotnet format --verify-no-changes

## Pièges connus
- gpt-5.4-mini est un modèle de raisonnement : ne jamais passer `Temperature` (l'API le refuse).
  Garder `MaxOutputTokens` ≥ 2000 : la réflexion consomme des tokens de sortie, sinon la réponse est vide.
- Garder `AzureCliCredential` : `DefaultAzureCredential` peut choisir un autre compte connecté sur le PC.
- Les user-secrets sont propres à chaque PC : leur absence n'est pas un bug du code.
- Environnement : Windows + PowerShell. Pas de syntaxe bash.

## User stories
- Les US sont dans `docs/us/`. Implémenter une US = respecter ses critères et ses fichiers autorisés, rien de plus.
- Ne jamais modifier un test écrit par l'auteur pour le faire passer : signaler le désaccord.