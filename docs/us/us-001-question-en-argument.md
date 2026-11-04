# US-001 — La console lit sa question en argument

## Contexte
Program.cs pose aujourd'hui 3 questions écrites en dur. Pour utiliser DocAssist
comme un vrai outil, la question doit venir de la ligne de commande.

## Demande
En tant qu'utilisateur de DocAssist, je veux passer ma question en argument,
afin d'interroger la doc Kubernetes sans modifier le code.

## Comportement attendu
- `dotnet run --project src/DocAssist.Cli -- "Qu'est-ce qu'un Pod ?"` pose cette question
  et affiche la réponse comme aujourd'hui.
- Plusieurs arguments sans guillemets sont réunis par des espaces :
  `-- Explique les Services` donne « Explique les Services ».
- Les espaces au début et à la fin sont retirés.
- Sans argument, ou avec un argument vide : afficher `Usage : DocAssist.Cli "<question>"`,
  code de sortie 1, SANS appeler le modèle.

## Interface
- Nouvelle classe `LectureQuestion` dans `src/DocAssist.Cli/LectureQuestion.cs` :
  `public static string? Depuis(string[] args)` → la question, ou `null` si elle manque.
- Program.cs l'appelle au début et remplace la liste des 3 questions.

## Critères d'acceptation
- [ ] `Depuis([])` renvoie `null`.
- [ ] `Depuis(["   "])` renvoie `null`.
- [ ] `Depuis(["Explique", "les", "Services"])` renvoie `"Explique les Services"`.
- [ ] `Depuis(["  Qu'est-ce qu'un Pod ?  "])` renvoie `"Qu'est-ce qu'un Pod ?"`.
- [ ] Sans question : message d'usage, code de sortie 1, aucun appel au modèle.
- [ ] `dotnet build` et `dotnet test` verts, sans nouvel avertissement.

## Contraintes
- Aucun nouveau package NuGet (pas de System.CommandLine).
- Rien d'autre ne change : prompt System, schéma, MaxOutputTokens, affichage, coût.

## Fichiers
- Autorisés : `src/DocAssist.Cli/Program.cs`, `src/DocAssist.Cli/LectureQuestion.cs` (nouveau).
- Interdits : tout `tests/` (les tests sont écrits par l'auteur), et tout le reste.

## Hors périmètre
- Mode interactif, options (`--help`…), limite de longueur, conversation multi-tours (S7).

## Vérification
- L'agent lance dotnet build + dotnet test et montre la sortie.
- L'auteur (pas l'agent : c'est payant) lance une fois avec une question, puis sans argument.

## Méthode
1. Lire cette US et AGENTS.md, proposer un plan, attendre ma validation.
2. Implémenter uniquement dans les fichiers autorisés.
3. Lancer dotnet build + dotnet test et montrer la sortie.
4. Lister les fichiers modifiés et la raison de chaque changement.