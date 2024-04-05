# TP .NET et Serverless
### Rendu Grégoire Desjonquères et Jules Rigaud

Nous avons réalisé le TP au fur et à mesure des commits, c'est à dire que certaines réponses aux questions des parties précédentes ont été supprimées en fonction de leur utilité dans la partie suivante. 

### 1. Hello World!

Choix de .NET 8 (lts), finalement comme les fonctions nouvelle de ce runtime vis-à-vis de la version 6 utilisée pour les __Azure functions__  ne sont pas utilisée il ne devrait pas y avoir de problème de partoabilité.

### 2. NuGet packages


### 3. Traitement d'image locale

Pour le benchmark, nous utilisons 8 images générées par Copilot, nous leur appliquons un effet peinture à l'huile `OilPaint` de la librairie `ImageSharp`.

Le gain de temps que j'ai mesuré ne semble pas extrêmement significatif, il me semble que c'est parce que la paraléllisation à été lancée aussi sur les lectures/écriture dans le système de fichier, ce qui me semble moins paraléllisable.

### 4. Portage vers une Azure Function

Cas de test, nous avons pris beaucoup de temps pour installer et faire fonctionner le setup de test d'Azure function en local (avec l'outil Core Tools, que l'extension Azure manipule seule, donc pas besoin de connaître la CLI de Core Tools), difficultée rencontrée : il faut ajouter les caractères d'échappement avant les doubles guillemets dans l'option <br> `--data '{\"name\":\"Azure\"}'`.

Commande utilisée pour la fonction Azure de test en local :<br>
```powershell
curl.exe -i
         -X POST http://localhost:7071/api/ResizeHttpTrigger
         -H "content-type: application/json"
         -H "Accept: application/json"
         -d '{ \"name\":\"AzureBoy\"}'
```
Cela permet simplement de faire fonctionner le template de code **C#** qui est généré automatiquement.

_Implémentation de la fonction de resizing :_

Après implémentation de la fonction de resizing, nous avons continué à utiliser PowerShell avec `curl.exe` mais nous n'arrivions pas à ouvrir le fichier `output.jpeg` qui était pourtant bien reçu (l'appllication Photos n'arrivait pas à afficher le contenu, j'ai laissé ce genre de fichier à l'adresse sample1/img/output.txt).

Après adaptation sur **WSL2** (Debian) avec la commande :
```bash
 curl --data-binary "@robot.jpeg" -X POST "http://$(hostname).local:7071/api/ResizeHttpTrigger?h=100&w=100" -v > output.jpeg
```

A noter qu'il faut remplacer localhost par `$(hostname).local` pour accéder aux port de l'hôte depuis WSL.

### 5. Intégration avec Logic Apps
