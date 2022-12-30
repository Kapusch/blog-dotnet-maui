---
Topic: "Scaffolding"
Title: "Les fondations"
Category: "Tutos"
Subcategory: "My first app"
series: ["My first app"]
Index: "6"
PublishDate: "2023-01-22 00:00:06Z"
Language: "French"
Description: "Quand on développe une nouvelle page, le plus important c’est de réfléchir à l’organisation des éléments sur la page. Comme tu vas le voir, c’est un vrai travail d’architecte !"
Tags: ["C# markup","Grid"]
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
Pour assurer le bon déroulement de cet article, je t’invite à repartir du projet reprenant les différentes étapes appliquées dans les articles précédents. Pour cela, [télécharge le projet](https://github.com/Kapusch/blog-dotnet-maui) si ce n’est pas déjà fait, et ouvre le projet NightClub situé dans le dossier “*2 - Navigation*”.
{{< /admonition >}}

Quand on développe une nouvelle page, le plus important c’est de réfléchir à l’organisation des éléments sur la page, et .NET MAUI nous offre [une palette de possibilités pour structurer notre page](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/layouts/). Tu vas voir, c’est un vrai travail d’architecte !


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Mais au fait, à quoi doit ressembler notre page ?
{{< /admonition >}}

Très bonne question ! Si tu veux un conseil, commence toujours par élaborer le design de ta page au crayon sur une feuille. En effet, les écrans de téléphone sont petits et il n’est donc pas toujours facile de disposer tous les éléments souhaités. Et quand on est suffisamment satisfait du design sur papier, on crée une maquette sur ordinateur pour un rendu réaliste qui facilitera l’intégration de la page.

Dans notre cas, on visera le résultat suivant :

<figure><p align="center"><img class="img-sizes" src="./images/F25FC1F576D94B299848D78DBA0AF729.png"></p></figure>

Ici on remarque que les éléments sont disposés de façon assez régulière. On peut facilement imaginer des lignes pour délimiter les espaces et aligner nos éléments les uns par rapport aux autres. C’est un exercice un peu spécial mais tu verras qu’avec le temps, ça deviendra de plus en plus facile:

<figure><p align="center"><img class="img-sizes" src="./images/DC7E5A20A7CA1D0976AD5613BCC16209.png"></p></figure>

Ces lignes te feront peut-être penser à une grille… et si c’est le cas, bien vu ! En effet, on utilisera ici majoritairement le composant [Grid](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/layouts/grid) pour disposer nos éléments sur une grille, littéralement.

La première chose à faire ici, c’est de supprimer l’en-tête de navigation pour que notre page remplisse tout l’écran, comme on l’avait fait pour la page d’accueil. Commence par supprimer le contenu généré par défaut et remplace-le par ce qui suit:

```csharp
public MusicPlayerView()
{
    Console.WriteLine("[NightClub] MusicPlayerView - Constructor");

    NavigationPage.SetHasNavigationBar(this, false);
}
```


Puis on va diviser la page en deux:

* La partie du haut qui n’affiche rien d’autre pour le moment qu’un fond noir,
* Et celle du bas qui représente notre lecteur de musique.
Si tu as l’oeil, tu remarqueras que la partie supérieure est légèrement plus grande que la partie inférieure. On peut même dire que la partie noire s’étale verticalement sur 60% de la page, ce qui laisse 40% d’espace pour le lecteur.

Ce sont des données utiles pour notre première utilisation de la *Grid*:

```csharp
public MusicPlayerView()
{
    ...

    Content = new Grid
    {
				RowDefinitions = Rows.Define(
            Stars(60),
            Stars(40)),
        RowSpacing = 0,
        Children =
        {
            TopLayout.Row(0),
            BottomLayout.Row(1),
        }
    };
}
```


Ici tu peux voir qu’on définit le contenu de notre page dans le constructeur de *MusicPlayerView*, avec comme base un *Grid*. Et avec le paramètre *RowDefinitions*, on décompose cette grille en 2 lignes, la première pouvant s’étendre jusqu’à 60% de la page verticalement contre 40% pour la deuxième, comme sur notre design !

A l’initialisation du *Grid*, on définit deux autres paramètres :

* *RowSpacing* à 0 pour n’avoir aucun espace entre nos deux lignes,
* *Children* pour contenir les éléments de notre grille sur la première ligne et la deuxième ligne.
Et si tu te demandes d’où sortent ces fameux *TopLayout* et *BottomLayout*, ce sont deux nouveaux conteneurs d’éléments que j’ai définis en dehors du constructeur de notre vue. En effet, pour ces deux contrôles, on utilise encore deux *Grid* pour nous aider dans le placement des éléments. L’un est défini avec un fond noir et l’autre avec un fond gris foncé:

```csharp
#region Controls

Grid TopLayout => new Grid
{
    BackgroundColor = Colors.Black
};

Grid BottomLayout => new Grid
{
    BackgroundColor = Colors.DimGray
};

#endregion
```


Concentrons-nous dans un premier temps sur le contenu du *BottomLayout*. En regardant d’un peu plus près le design, on perçoit:

* de haut en bas, 3 lignes de taille identique,
* de gauche à droite, 7 colonnes (2 petites, 3 grandes et 2 petites à nouveau)
<figure><p align="center"><img class="img-sizes" src="./images/15825CA1D0297C02D5C7C653EDA5BEA7.png"></p></figure>

Et pour cela, nous allons définir les lignes et colonnes qui composent notre *Grid*:

```csharp
Grid BottomLayout => new Grid
{
    BackgroundColor = Colors.DimGray,
    RowDefinitions = Rows.Define(
            Stars(1),
            Stars(1),
            Stars(1)),
    RowSpacing = 0,
    ColumnDefinitions = Columns.Define(
            Stars(10),
            Stars(10),
            Stars(20),
            Stars(20),
            Stars(20),
            Stars(10),
            Stars(10)),
    ColumnSpacing = 0
};
```


Comme tu vois, le code ci-dessus propose un découpage en 3 lignes de même taille et 7 colonnes.


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Euh… ouais j’ai bien un total de 100% si j’additionne les tailles de chaque colonne, mais pour ce qui est des lignes y’a un problème non ?
{{< /admonition >}}

Très bonne remarque ! En fait si je suis parti au début avec un échelonnement des tailles sur 100%, c’est parce que c’est souvent plus facile à comprendre. En réalité, quand tu écris:

```csharp
RowDefinitions = Rows.Define(
    Stars(60),
    Stars(40))
```


tu demandes à ton programme d’établir une taille dynamique avec un poids de 60 pour la première ligne, contre un poids de 40 pour la deuxième. Tu peux voir ce poids comme un coefficient multiplicateur. Par exemple, ce même bout de code peut être réécrit de cette façon:

```csharp
RowDefinitions = Rows.Define(
    Stars(1.5),
    Stars(1))
```


A toi de choisir ce qui te semble plus cohérent !

Pour revenir à notre quadrillage de la partie inférieure de l’écran en 3 lignes et 7 colonnes, je vais te donner une astuce pour vérifier rapidement que notre découpage est bien celui attendu et voir s’il y a quelque ajustement à faire. Pour cela, définis simplement une *BoxView* dans chaque case du quadrillage avec chacune sa propre couleur:

```csharp
Grid BottomLayout => new Grid
{
		...

		Children =
		{
		    new BoxView { Color = Color.FromArgb("#ffffff") }.Row(0).Column(0),
		    new BoxView { Color = Color.FromArgb("#d0d0d0") }.Row(0).Column(1),
		    new BoxView { Color = Color.FromArgb("#a2a3a3") }.Row(0).Column(2),
		    new BoxView { Color = Color.FromArgb("#777879") }.Row(0).Column(3),
		    new BoxView { Color = Color.FromArgb("#4e5051") }.Row(0).Column(4),
		    new BoxView { Color = Color.FromArgb("#292b2c") }.Row(0).Column(5),
		    new BoxView { Color = Color.FromArgb("#000405") }.Row(0).Column(6),
		    new BoxView { Color = Color.FromArgb("#f3f337") }.Row(1).Column(0),
		    new BoxView { Color = Color.FromArgb("#a2eb5b") }.Row(1).Column(1),
		    new BoxView { Color = Color.FromArgb("#4edb80") }.Row(1).Column(2),
		    new BoxView { Color = Color.FromArgb("#00c89f") }.Row(1).Column(3),
		    new BoxView { Color = Color.FromArgb("#00b1b1") }.Row(1).Column(4),
		    new BoxView { Color = Color.FromArgb("#0098b2") }.Row(1).Column(5),
		    new BoxView { Color = Color.FromArgb("#177ea2") }.Row(1).Column(6),
		    new BoxView { Color = Color.FromArgb("#bf7aef") }.Row(2).Column(0),
		    new BoxView { Color = Color.FromArgb("#ea6cd4") }.Row(2).Column(1),
		    new BoxView { Color = Color.FromArgb("#ff63b3") }.Row(2).Column(2),
		    new BoxView { Color = Color.FromArgb("#ff6590") }.Row(2).Column(3),
		    new BoxView { Color = Color.FromArgb("#ff716e") }.Row(2).Column(4),
		    new BoxView { Color = Color.FromArgb("#ff844e") }.Row(2).Column(5),
		    new BoxView { Color = Color.FromArgb("#f89832") }.Row(2).Column(6),
		}
};
```


Et si tu lances l’application, tu pourras alors confirmer avec certitude que notre emploi des *Grid* est effectivement correct:

<figure><p align="center"><img class="img-sizes" src="./images/F9032472788C6B235E788E30A9EE9C21.png"></p></figure>

Ce n’est toujours pas fonctionnel, mais tu as déjà passé un premier cap !


{{< admonition type=tip title="‎ " open=true >}}
Aller plus loin avec les différents types de layout (Grid, StackLayout, Flex, …)
{{< /admonition >}}



---
Plus d'articles dans la même série:
{{< series "My first app" >}}
