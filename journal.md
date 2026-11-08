# Journal DocAssist

## S3 · Séance 1 — sam. 7 nov. 2026
**Fait**
- Déploiement `text-embedding-3-small` sur doflamingoAIResources (Model catalog → « embedding » → Deploy),
  nom dans le user-secret AZURE_OPENAI_EMBEDDING_NAME
- Nouvelle console `src/DocAssist.Recherche` (ajoutée à la solution) : 10 phrases → embeddings
  en un seul appel → 2 plus proches voisines par similarité cosinus
- Package ajouté : System.Numerics.Tensors (TensorPrimitives.CosineSimilarity), justifié
- Exploration : contenu d'un vecteur (dimension, min/max, norme), Norm / Dot / CosineSimilarity
**Chiffres**
- Dimension : … ; tokens pour 10 phrases : … ; norme d'un vecteur : …
- [2]↔[3] (FR/EN) : … ; [6]↔[7] (sans mot commun) : … ; [8] pod de café → plus proche : …
**Appris**
- Embedding = le sens d'un texte en vecteur : 1 536 nombres d'environ ±0,03, norme 1
- Sens proche = vecteurs proches, même sans mot commun ou dans une autre langue
- Cosinus = produit scalaire ÷ (norme A × norme B) : mesure l'angle, de -1 à 1
- Vecteurs normalisés → cosinus = produit scalaire
- Un embedding ne se lit pas, il se compare ; uniquement entre vecteurs du même modèle
- Une ressource Azure = endpoint + droits ; plusieurs déploiements ; standard = payé à l'usage
- TensorPrimitives : calcul sur des Span, vectorisé (SIMD) ; Memory pour stocker, Span pour calculer
- Force brute (tout comparer) suffit pour 10 phrases ; en phase 2, index vectoriel (HNSW)
- ≈ 6 Ko par vecteur → le stockage devient un choix d'architecture à grande échelle
**Bloqué / résolu**
- « Modèles + points de terminaison » introuvable → vue Azure OpenAI classique :
  Model catalog, ou Shared resources → Deployments
- TensorPrimitives non reconnu → using manquant + package absent : il n'arrive PAS avec
  Microsoft.Extensions.AI (vérifié dans obj/project.assets.json)
**Questions de contrôle** (de tête)
1. Un embedding, et ce que mesure la similarité cosinus : …
2. Pourquoi le « pod de café » est loin des phrases Kubernetes : …
3. Comparer des vecteurs de deux modèles différents : …
**Prochaine séance**
- S3 · Séance 2 : Python express pour un développeur C# (syntaxe, environnements virtuels, pip)

## S2 · Séance 4 — ven. 6 nov. 2026
**Fait**
- `docs/grille-revue.md` : preuves et périmètre, conformité à l'US, tests, sécurité et coût,
  lisibilité, points positifs + verdict
- Revue de l'US-001 avec la grille :
  - Comportement non demandé (nettoyage de chaque argument) → accepté : critère ajouté à l'US
    + test `Depuis_ignore_les_arguments_vide_au_milieu` (8 tests verts)
  - Mise en forme (indentation, espaces en fin de ligne) → `dotnet format` ;
    `dotnet format --verify-no-changes` ajouté à « Avant de dire terminé » dans AGENTS.md
  - Usage sur stdout au lieu de stderr → décision : …
  - Preuve `git diff tests/` cassée : tests corrigés commités avec l'implémentation
- Revue par l'agent (session neuve, mode Plan) : il a vu … ; il a raté … ; moi, j'ai raté …
- ✅ Livrable S2 : AGENTS.md, modèle d'US et grille de revue dans le dépôt, éprouvés sur l'US-001
**Appris**
- Une grille rend la revue reproductible : mêmes questions, même ordre, à chaque diff
- Ordre : preuves et périmètre → conformité → tests → sécurité → lisibilité
- Défaut typique de l'agent : en faire plus que demandé → on accepte (US + test) ou on retire
- Un trou dans l'US = l'agent choisit à ma place → corriger l'US ou le modèle
- La revue porte sur l'état du fichier, mes propres défauts compris
- Relecteur : session neuve, et on lui fait ignorer le style
**À finir**
- Usage sur stderr (`Console.Error.WriteLine`) si je le décide ; ligne « Sorties console » dans `_modele.md`
**Questions de contrôle** (de tête)
1. Pourquoi commencer par les preuves et le périmètre : …
2. Ce qui est spécifique à la revue d'un code écrit par un agent : …
3. Pourquoi une session neuve, et ignorer le style : …
**Prochaine séance**
- S3 · Séance 1 : embeddings et similarité cosinus

## S2 · Séance 3 — mer. 4 nov. 2026
**Fait**
- 4 tests de LectureQuestion écrits à la main + stub NotImplementedException ; commit des tests rouges
- 1er passage de l'agent : il a refusé d'implémenter et signalé la coquille « espece »
  → la règle d'AGENTS.md (signaler plutôt que modifier les tests) fonctionne
- Tests corrigés : coquille, " " au lieu de "", vrai critère « Qu'est-ce qu'un Pod ? », namespace
- Prompt court (« relis les tests, implémente l'US-001 ») → LectureQuestion + Program.cs,
  dotnet build + dotnet test verts (7 tests)
- 5e test ajouté après coup (« Pod ») → vert sans retouche du code
- Vérification manuelle : avec question → réponse ; sans argument → usage, $LASTEXITCODE = …
**Chiffres**
- Itérations de l'agent : … ; coût Azure de la séance : …
**Appris**
- L'US est la source de vérité ; les tests traduisent ses critères un par un
- Un test faux est rouge aussi → relire chaque test en le comparant à l'US avant de commiter
- Un test doit être assez strict pour faire échouer une implémentation fausse
- Prompt = quelle US + ce qui a changé ; une règle oubliée se corrige dans l'US ou AGENTS.md
- Un test ajouté après coup détecte le surapprentissage
**Bloqué / résolu**
- Coquille dans mes tests → signalée par l'agent → corrigée
- ⚠️ Tests corrigés commités avec l'implémentation → git diff tests/ ne prouve plus rien.
  Désormais : un commit des tests corrigés AVANT de relancer l'agent
**Questions de contrôle** (de tête)
1. Pourquoi c'est moi qui écris les tests, et pas l'agent : …
2. Pourquoi commiter les tests rouges avant l'agent : …
3. Le surapprentissage, et comment je l'ai vérifié : …
**Prochaine séance**
- S2 · Séance 4 : grille de revue, appliquée au diff de l'US-001


## S2 · Séance 2 — mer. 4 nov. 2026
**Fait**
- US-001 « La console lit sa question en argument » dans `docs/us/` : contexte, demande,
  comportement, interface (`LectureQuestion.Depuis`), critères vérifiables, contraintes,
  fichiers autorisés/interdits, vérification, méthode
- AGENTS.md : section User stories (US dans docs/us/, ne jamais modifier les tests de l'auteur)
- Rappel : UnitTest1.cs renommé en CalculCoutTests.cs, code commenté supprimé de Program.cs
- US éprouvée avec l'agent Plan d'OpenCode (Tab) : plan … (conforme / écarts : …)
**Corrigé dans l'US après le plan de l'agent**
- …
**Appris**
- Un agent devine tout ce qui n'est pas écrit → une US exécutable ne laisse rien à deviner
- Checklist (« Let Claude run without you ») : terminé ? vérification ? périmètre ? arrêt ? validation ?
- N'écrire pour l'agent que ce qu'il ne peut pas trouver en lisant le dépôt
- Critères d'acceptation = vérifiables par un test ou une commande
- Si le plan dévie, on corrige l'US, pas le plan ; après 2 corrections ratées : /clear
**Bloqué / résolu**
- Sections de la doc introuvables → page Best practices réorganisée. Titres actuels :
  Tell Claude what it can't see, Give Claude a check to run, Let Claude run without you
**À finir**
- Créer `docs/us/_modele.md` (modèle vide)
- Ajouter « Hors périmètre » à l'US-001, corriger « DocAsetsist »
**Questions de contrôle** (de tête)
1. Ce qui rend une US « exécutable » par un agent : …
2. Pourquoi des critères vérifiables par une commande : …
3. Pourquoi lister les fichiers autorisés et interdire de toucher aux tests : …
**Prochaine séance**
- S2 · Séance 3 : tests d'abord. J'écris les tests de LectureQuestion, l'agent implémente jusqu'au vert

## S2 · Séance 1 — mar. 3 nov. 2026
**Fait**
- Projet de tests `tests/DocAssist.Cli.Tests` (xUnit), ajouté à la solution, référence vers DocAssist.Cli
- Calcul du coût sorti dans `CalculCout.Estimer` (fonction pure) ; Program.cs l'utilise
- 2 tests verts : addition entrée + sortie, zéro token
- AGENTS.md enrichi : vérification = build + test, section Tests, conventions
  (nommage FR/EN, nullable, une classe par fichier, commentaires = pourquoi), pièges connus
- Tests OpenCode : vérification → … ; piège Temperature → …
**Appris**
- Un agent sans commande de test ne peut pas vérifier son propre travail
- « Terminé » = dotnet build + dotnet test verts, depuis la racine
- Un test n'appelle jamais Azure (lent, payant, non reproductible) → fonction pure ou faux IChatClient
- Rendre du code testable = extraire la logique dans une fonction pure
- Un piège entre dans AGENTS.md si l'agent peut refaire l'erreur en modifiant le code
**Bloqué / résolu**
- …
**Questions de contrôle** (de tête)
1. Pourquoi un test n'appelle jamais le vrai modèle : …
2. Pièges de S1 retenus dans AGENTS.md, et pourquoi pas les autres : …
3. Pourquoi « terminé » inclut dotnet test et pas dotnet run : …
**Prochaine séance**
- S2 · Séance 2 : modèle d'US exécutable + US-001 (question en argument)

## S1 · Séance 4 — dim. 1er nov. 2026
**Fait**
- `AGENTS.md` écrit à la main : projet, stack, structure, commandes, conventions, interdits, définition de « terminé »
- Section Commandes allégée : seulement ce que l'agent ne peut pas déduire
  (dotnet run payant, prérequis hors dépôt, vérification obligatoire)
- OpenCode connecté à mon propre déploiement Azure (gpt-5.4-mini), clé copiée sans affichage
- Test 1 (compréhension des règles) et test 2 (piège de la clé d'API) réussis.
  Réaction de l'agent au piège : "Non. Je ne peux pas mettre une clé API dans un fichier de config, parce que ce dépôt interdit explicitement de commiter des secrets et recommande dotnet user-secrets avec AzureCliCredential."
- ✅ Semaine S1 terminée : console → JSON typée (séance 3) + AGENTS.md
**Chiffres**
- Consommation Azure de la séance (portail, Gestion des coûts) : …
**Appris**
- AGENTS.md = message System de l'agent, chargé à chaque échange → concis
- Y écrire ce que l'agent ne peut pas déduire du dépôt, ou ce qu'il ferait de travers
- Les interdits protègent le dépôt : secrets, fichiers perso, packages, Git, ressources Azure
- Clé acceptable pour un outil local (stockée dans le profil OpenCode), jamais pour DocAssist
- Jamais le compte ou la licence pro pour ce projet
- Agent : jamais dans un terminal administrateur, toujours lancé depuis la racine du dépôt
**Bloqué / résolu**
- `Set-Clipboard` non reconnu → j'étais dans cmd, pas PowerShell (équivalent cmd : `clip`)
- `opencode.ps1` bloqué par la politique d'exécution → … (opencode.cmd ou RemoteSigned en CurrentUser)
- « AZURE_RESOURCE_NAME is missing » après redémarrage → `$env:` ne vaut que pour la session
  → `setx` + nouveau terminal
**Plus tard**
- Ajouté : c'est quoi un modèle de raisonnement, quels autres types de modèles
**Questions de contrôle** (de tête)
1. AGENTS.md et message System : …
2. Pourquoi les interdits sont la section la plus importante : …
3. Clé acceptable pour OpenCode, pas pour DocAssist : …
**Prochaine séance**
- S2 · Séance 1 : enrichir AGENTS.md (commandes de test, conventions, pièges de S1)

## S1 · Séance 3 — ven. 30 oct. 2026
**Fait**
- Record `ReponseDocAssist` + enum `Certitude` dans un fichier séparé
- Prompt System structuré : rôle, règles avec leur raison, exemples entre balises
- `GetResponseAsync<ReponseDocAssist>` + `TryGetResult` sur 3 questions de test
- ✅ Livrable S1 atteint : console → réponse JSON typée
**Chiffres**
- Tokens envoyés : 55 (séance 2) → … (séance 3)
- Même requête sans schéma / avec schéma : … / … tokens envoyés → le schéma coûte ≈ … tokens
**Observations**
- Couscous : HorsSujet = …
- PodSecurityPolicy : le modèle a … (signalé la suppression / inventé / mis Certitude Faible)
- Sans le bloc d'exemples : …
**Appris**
- Sortie structurée : mon type C# devient un schéma JSON imposé au modèle
- Le schéma impose le format, les exemples montrent le comportement
- Schéma payé en entrée à chaque appel + JSON payé en sortie → seulement les champs utiles
- Sans RAG, le modèle invente les sources → pas d'URL avant S6
- `plus-tard.md` = uniquement ce qui sort du plan
**Bloqué / résolu**
- Coût du schéma pas clair → mesuré : même requête, avec et sans schéma
**Questions de contrôle** (de tête)
1. Sortie structurée plutôt que « réponds en JSON » : …
2. Ce qui est envoyé en plus avec GetResponseAsync<T> : …
3. Pourquoi pas d'URL de la doc à ce stade : …
**Prochaine séance**
- S1 · Séance 4 : AGENTS.md du POC

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