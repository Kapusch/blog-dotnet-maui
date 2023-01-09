---
Topic: "How to navigate"
Title: "D’une page à l’autre"
Category: "Tutos"
Subcategory: "My first app"
series: ["My first app"]
Index: "5"
PublishDate: "2023-01-16 00:00:05Z"
Language: "French"
Description: "On attaque désormais la création de la page principale de l’application. Mais qui dit nouvelle page dit aussi: permettre à l’utilisateur de se rendre sur cette page ! Voyons donc comment implémenter la navigation d’une page à une autre."
Tags: ["Navigation"]
featuredImagePreview: 'featured-image-preview-fr'
resources:
- name: 'featured-image-preview-fr'
  src: 'featured-image-preview-fr.jpeg'
draft: false
---

<!--more-->

<style>
.img-sizes{min-height:50px;max-height:600px;min-width:50px;max-width:600px;height:auto;width:auto}
</style>

{{< admonition type=info title="‎ " open=true >}}
Pour assurer le bon déroulement de cet article, je t’invite à repartir du projet reprenant les différentes étapes appliquées dans les articles précédents. Pour cela, [télécharge le projet](https://github.com/Kapusch/blog-dotnet-maui) si ce n’est pas déjà fait, et ouvre le projet NightClub situé dans le dossier “*1 - MVVM*”.
{{< /admonition >}}

Commençons déjà par ajouter notre nouvelle page. Pour cela, clic droit sur le dossier *Views* pour ajouter un nouveau fichier, puis choisir le template “.NET MAUI ContentPage (C#)” depuis la catégorie “.NET MAUI”. On va nommer ce fichier : `MusicPlayerView.cs`.

<figure><p align="center"><img class="img-sizes" src="./images/4C95EF7DF978364F1FBDE99A614BF58D.png"></p></figure>



Comme tu l’auras remarqué, le template utilisé crée la page avec un contenu par défaut. On n’a donc plus qu’à naviguer vers notre nouvelle page ! Dans le fichier `HomeViewModel.cs`, modifie la méthode *Enter()* de la façon suivante :

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>HomeViewModel.cs</code></p>

```csharp
[RelayCommand]
async Task Enter()
{
    await Application.Current.MainPage.Navigation.PushAsync(
        new MusicPlayerView());
}
```





{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Ah! Et c’est tout, je peux tester alors ?
{{< /admonition >}}



On y est presque ! En effet, on doit simplement initialiser la navigation dans l’app en lui informant quelle en sera la page racine. Pour cela, on fait appel à une [NavigationPage](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pages/navigationpage#create-the-root-page) pour contenir notre *HomeView* en modifiant la méthode *OnStart()* du fichier `App.cs` de cette façon:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>App.cs</code></p>

```csharp
protected override void OnStart()
{
    base.OnStart();

    Console.WriteLine("[NightClub] App - OnStart");

    MainPage = new NavigationPage(new HomeView());
}
```




Voilà c’est bon, relance l’application et clique sur le bouton *Enter* !

<figure><p align="center"><img class="img-sizes" src="./images/0F2CA118C086F82599BC1C6BC4E0D61A.png"></p></figure>



Comme tu l’auras sûrement remarqué, notre page d’accueil est désormais contenue dans une page configurée pour la navigation, elle contiendra donc une en-tête de navigation :

<figure><p align="center"><img class="img-sizes" src="./images/82596E59FF4122F0C481FF2E6E218521.png"></p></figure>



Ce n’est pas nécessairement ce que l’on souhaite alors on va vite voir comment supprimer cette en-tête. Rends-toi dans le fichier `HomeView.cs` et fais appel à la méthode *SetHasNavigationBar()* dans le constructeur de la *HomeView* comme ceci:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>HomeView.cs</code></p>

```csharp
public HomeView()
{
	...
	BindingContext = new HomeViewModel();
	
	NavigationPage.SetHasNavigationBar(this, false);
	BackgroundColor = Colors.Black;
	...
}
```




Voilà, et si tu relances l’app, c’est quand même plus joli !

<figure><p align="center"><img class="img-sizes" src="./images/2B7DCB34C546A4051261C0D200380452.png"></p></figure>



Allez il est temps de passer à un nouveau chapitre, l’élaboration de notre page principale !

---
Plus d'articles dans la même série:
{{< series "My first app" >}}
