# TP .NET et Serverless

### Pré-requis
1. Installer
    - [.NET Core](https://dotnet.microsoft.com/download/dotnet-core)
    - [Visual Studio Code](https://code.visualstudio.com/)
    - [Azure Functions extension for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)

### 1. Hello World!
1. Suivre les [instructions officielles](https://docs.microsoft.com/en-us/dotnet/core/get-started) pour créer votre premier programme en .NET
2. Naviguez vers le dossier *sample1* et modifiez le programme en y ajoutant une classe *Personne* avec (Testez le snippet *prop* puis la touche *TAB* ):
    1. Une propriété **nom** de type *string*
    2. Une propriété **age** de type *int*
    3. (optionnel) Une méthode **Hello(bool isLowercase)** qui renvoit **"hello *name*, you are *age*** si **isLowercase** vaut *true*, ou la même chaîne mais en majuscule sinon.
    4. Créez une variable de type *Personne* en lui assignant un nom et un âge ([docs](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/instance-constructors))
    5. Affichez à l'écran **hello *name*, you are *age*** (au lieu du *Hello World!* d'origine)

Pour recompiler et exécuter votre code, depuis le dossier du projet:
> dotnet run

Pour recompiler uniquement
> dotnet build

### 2. NuGet packages
Comme tout langage moderne, .NET bénéficie d'une communauté de développeurs partageant des librairies prêtes à l'emploi, en l'occurence [NuGet](https://nuget.org). Dans cette exercice nous allons utiliser la librairie la plus populaire de NuGet: **newtonsoft.json** qui permet de facilement travailler avec des données au format json.

1. Toujours sur la base du programme Hello World, installez le package **newtonsoft.json** en exécutant la commande *dotnet add package Newtonsoft.Json* depuis le dossier *sample1*
2. Modifiez le programme afin de sérialiser dans une chaîne de caractère la variable de type *Personne* créée précédemment en utilisant [**JsonConvert.SerializeObject**](https://www.newtonsoft.com/json/help/html/SerializingJSON.htm#JsonConvert) (n'oubliez d'inclure le namespace adéquat en ajoutant **using Newtonsoft.Json** en tête de fichier)
3. (optionnel) Formattez la sérialisation précédente en utilisant un **Formatting==Indented**
4. Affichez le json à l'écran à la place du texte précédemment présent

### 3. Traitement d'image locale
Nous allons maintenant utiliser le package [ImageSharp](https://github.com/SixLabors/ImageSharp).

1. Toujours sur le même projet, ajoutez le package **SixLabors.ImageSharp**
2. En vous basant sur [ces exemples](https://docs.sixlabors.com/articles/imagesharp/gettingstarted.html), redimensionnez une image (ou effectuez une autre transformation!) et sauvegardez la. Pour préciser des noms de chemin Windows, je vous conseille la syntaxe **@"c:\chemin\monimage.jpeg"** qui permet de garder lisibles les backslashes
3. (optionnel avancé) La gestion du parallélisme et de l'asynchrone est un point fort de .NET. Utilisez **Parallel.ForEach** pour redimensionner plusieurs images en parallèle.

### 4. Portage vers une Azure Function
Nous allons maintenant porter ce petit programme pour pouvoir l'héberger au sein d'une Azure Function. Elle se déclenchera sur un appel [HttpTrigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-blob-trigger?tabs=csharp) en POST.

1. Créez un nouveau dossier local *ResizeFunction*
2. Retournez à la page d'accueil du portail
    - Créez une Function App (ex: lucvovan-fa)
    - Ciblant *.NET Core 3.1*
    - Peu importe la localisation
    - Laissez Azure créer un nouveau Storage Account pour héberger le code de la fonction
    - Sélectionnez un hosting *Consumption*
3. Dans l'onglet Azure de Visual Studio Code, section *FUNCTIONS* [créez un nouveau projet Azure Functions depuis Visual Studio Code](https://docs.microsoft.com/fr-fr/azure/azure-functions/create-first-function-vs-code-csharp)
    - Dans le dossier *ResizeFunction*
    - Choisissez *C#*
    - Faites bien attention de sélectionner **HttpTrigger** (quelques secondes sont nécessaires à l'affichage)
    - Sélectionnez *Anonymous* comme type d'authentification
    - Nommez votre fonction **ResizeHttpTrigger**
4. Depuis le dossier *ResizeFunction*, ajoutez le package [ImageSharp](https://github.com/SixLabors/ImageSharp).
5. Dans le fichier *.vscode/settings.json*, modifiez **~2** en **~3** (le template n'étant pas à jour au 12/01/2021)
6. Modifiez le fichier afin de récupérer le corps (body) de la requête et le charger en tant qu'image dans ImageSharp ([aide](https://stackoverflow.com/questions/54944607/how-to-retrieve-bytes-data-from-request-body-in-azure-function-app))
    - Pour renvoyer les octets de la nouvelle image en tant que réponse à la requête, utilisez **return new FileContentResult(targetImageBytes, "image/jpeg");**
    - Ajoutez des paramètres **h** et **w** (au niveau de l'URL) qui permettent à l'appelant de spécifier les dimensions cibles
    - Si vous avez des difficultés, collez le contenu du [fichier préparé](https://github.com/lvovan/AA-Serverless-NET/blob/master/ResizeHttpTrigger-incomplete.cs) qui implémente pour vous les éléments très techniques/tuyaux

        - Complétez les différents TODO
            - Récupérez les paramètres **w** et **h** de la requête avec **req.Query[*key*]** et utilisez les respectivement comme dimensions cibles pour les largeurs et hauteur de la nouvelle image. Attention au typage!
            - Les MIME types sont documentés [ici](https://docs.w3cub.com/http/basics_of_http/mime_types/complete_list_of_mime_types.html)


Récupérez l'adresse de votre fonction depuis le portail Azure en allant sur votre Azure Function, dans la section **Functions**, sélectionnez **ResizeHttpTrigger** et cliquez sur le bouton *Get Function Url* en haut de la page
8. Déployez votre code et appelez fonction avec curl () ou via un testeur de web service comme [Postman](https://www.postman.com/downloads/).

> curl --data-binary "@chaussures_abimees.jpg" -X POST "https://votrefonction.azurewebsites.net/api/ResizeHttpTrigger?w=100&h=100" -v > output.jpeg

9. (optionnel) Changez **AuthorizationLevel.Anonymous** à **AuthorizationLevel.Function** puis récupérez la clé d'API de votre fonction sur le portail. Modifiez ensuite votre requête pour qu'elle s'authentifie avec succès. 