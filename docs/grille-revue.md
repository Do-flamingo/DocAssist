# Grille de revue — code généré par l'agent

À appliquer sur chaque diff avant de commiter.

## 1. Preuves et périmètre
- [ ] L'agent a montré la sortie de `dotnet build` et `dotnet test`, pas seulement « ça passe ».
- [ ] `dotnet test` est vert chez moi aussi.
- [ ] `git diff --stat` : uniquement les fichiers autorisés par l'US.
- [ ] `git diff <commit des tests> -- tests/` est vide : mes tests sont intacts.

## 2. Conformité à l'US
- [ ] Chaque critère d'acceptation est couvert (par un test, ou par une vérification manuelle faite).
- [ ] Rien du « Hors périmètre » n'a été fait.
- [ ] Aucun comportement non demandé. Sinon : l'accepter dans l'US avec un test, ou le retirer.

## 3. Tests
- [ ] Les tests traduisent les critères de l'US, un par un.
- [ ] Ils sont assez stricts pour faire échouer une implémentation fausse.
- [ ] Pas de surapprentissage : un test ajouté après coup passe sans retouche.
- [ ] Aucun test n'appelle Azure ni le réseau.

## 4. Sécurité et coût
- [ ] Aucun secret, clé, endpoint ni ID de tenant dans le diff.
- [ ] `AzureCliCredential` inchangé ; aucun package ajouté sans justification.
- [ ] Pas de nouvel appel au modèle (sauf si l'US le demande), pas de `Temperature`, `MaxOutputTokens` ≥ 2000.
- [ ] Aucune donnée professionnelle ou privée.

## 5. Lisibilité et complexité
- [ ] Le code est le plus simple possible pour l'US : pas de sur-ingénierie.
- [ ] Les noms respectent les conventions (domaine en français, technique en anglais).
- [ ] Les commentaires expliquent le pourquoi ; pas de code mort ni commenté.
- [ ] `dotnet format --verify-no-changes` ne signale rien.

## 6. Points positifs
- Ce que l'agent a bien fait, à garder comme modèle.

## Verdict
- [ ] Accepté   - [ ] À corriger (lister)   - [ ] Refusé (corriger l'US, relancer)