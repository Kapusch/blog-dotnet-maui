---
PostURL: "music-player-display-time-tracker"
Title: "Affichage du lecteur MP3 - Durée d'écoute"
Category: "Tutos"
Subcategory: "My first app"
series: ["My first app"]
Index: "7"
PublishDate: "2023-02-06 00:00:07Z"
Language: "French"
Description: "Maintenant que les fondations de la page principale sont prêtes, on va pouvoir commencer à disposer tous les éléments de contrôle. Commençons avec le minutage !"
Tags: ["Accessibility","Slider","Label","ColumnSpan"]
featuredImagePreview: 'featured-image-preview-fr'
resources:
- name: 'featured-image-preview-fr'
  src: 'featured-image-preview-fr.png'
draft: false
---

<!--more-->


{{< admonition type=info title="‎ " open=true >}}
Pour assurer le bon déroulement de cet article, je t’invite à récupérer le projet reprenant toutes les étapes appliquées jusqu’ici dans ce cours. Pour cela, réfère-toi au <a href="../2-setup-the-project/">guide d’installation du projet</a> et repars de l’exemple situé dans le dossier *“3 - Page Scaffolding”*.
{{< /admonition >}}
Dans l’article précédent, on a mis en place un quadrillage pour y disposer tous les contrôles de notre lecteur musical. On va donc pouvoir ajouter des éléments concrets pour l’utilisateur !

## Afficher le minutage
Commençons déjà avec la partie dédiée au minutage du titre musical. D’après la maquette, on aura un texte à gauche pour le temps d’écoute en cours, et à droite, pour la durée totale de la piste :

<p align="center"><img max-width="100%" max-height="100%" src="./images/A83271A3987417E4A849D31C30191E32.png" /></p>
<figure><figcaption class="image-caption">L’application indique que le morceau est joué depuis 36 secondes et qu’il dure 2 minutes et 57 secondes. </figcaption></figure>



Pour reproduire cela dans l’application, on utilisera le composant [Label](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/controls/label) qui permet d’afficher du texte à l’écran :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
Label ElapsedTime => new Label
{
    FontSize = 14,
    Text = "0:36",
    TextColor = Colors.White
}.TextCenter();

Label TotalTime => new Label
{
    FontSize = 14,
    Text = "2:57",
    TextColor = Colors.White
}.TextCenter();
```


Ces deux nouveaux éléments sont définis pour afficher un texte statique écrit en blanc et avec une taille relativement petite. Cependant, la taille des caractères s’adaptera automatiquement au niveau d’agrandissement du texte défini par l’utilisateur dans les réglages d’accessibilité de son téléphone.




{{< admonition type=tip title="‎ " open=true >}}
Aller plus loin avec [l’accessibilité pour les applications mobiles](https://learn.microsoft.com/fr-fr/dotnet/maui/fundamentals/accessibility).
{{< /admonition >}}


## Ajuster la tête de lecture
Quant à l’élément du milieu, il remplit deux fonctions à la fois : afficher et contrôler la position de lecture dans le morceau. En effet, l’utilisateur s’en sert pour avancer ou reculer dans le morceau en faisant glisser son doigt horizontalement. De plus, ce composant possède une partie en rose qui représente le temps écoulé, et une autre en noir pour le temps restant.

Pour reproduire cette barre de navigation, on utilisera le composant [Slider](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/controls/slider) :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
Slider TimeTracker => new Slider
{
    Minimum = 0,
    MinimumTrackColor = Colors.LightSalmon,
    Maximum = 100,
    MaximumTrackColor = Colors.Black,
    Value = 20
};
```


Un *Slider* est un contrôle doté d’un curseur permettant à l’utilisateur de sélectionner une valeur précise parmi toute une plage de valeurs possibles. Dans notre cas, c’est comme si l’on avait une règle graduée de 0 à 100 avec un curseur positionné sur le 20.

<p align="center"><img max-width="100%" max-height="100%" src="./images/121C071E9377094E14C9B07AF2D49C8F.png" /></p>
<figure></figure>




{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Ah oui je vois, on se situe en fait à 20% du morceau ! On aura donc une barre rose de 0 jusqu’à 20, et une barre noire de 20 jusqu’à 100.
{{< /admonition >}}


Oui, c’est ça ! Bon évidemment ici, toutes les valeurs de configuration sont statiques, mais l’idée finale est de permettre à l’utilisateur d’avancer ou de reculer l’écoute de la piste à la seconde près.

___
Voilà, on en a terminé avec la première rangée d’éléments du lecteur musical. Pour vérifier que l’affichage est correct, j’aimerais que tu effaces tous les éléments enfants du *BottomLayout* pour lui assigner tous les composants définis dans ce chapitre.

Pour cela, tu seras sûrement tenté d’écrire quelque chose comme :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
Grid BottomLayout => new Grid
{
    ...
    Children =
    {
        ElapsedTime.Row(0).Column(1),
        TimeTracker.Row(0).Column(2),
        TotalTime.Row(0).Column(5)
    }
};
```


À première vue, ça semble pas mal ! Cependant, si tu démarres l’application, tu constateras qu’il y a un léger problème d’affichage :

<p align="center"><img max-width="100%" max-height="100%" src="./images/F4B63C493DFDA670F2EE8A4F45A93D87.png" /></p>
<figure><figcaption class="image-caption">Disposé comme ça, le Slider n’a pas bonne mine.</figcaption></figure>



Pas de panique ! On va voir tout de suite comment réajuster cela.

## Bien utiliser les Grid
Si tu te rappelles, on a initialement divisé le *BottomLayout* en 7 colonnes. Or, si notre code positionne bien notre *Slider* à partir de la case n°3 de la première ligne, il ne lui dit pas clairement où s’arrêter ! On pourrait alors simplement dire au *Slider* de s’étaler sur les colonnes suivantes, comme ceci :

<p align="center"><img max-width="100%" max-height="100%" src="./images/2822BA46A84A307DD610D66916169DF5.png" /></p>
<figure><figcaption class="image-caption">Le Slider est à cheval sur les cases n°3, n°4 et n°5.</figcaption></figure>



Pour cela, on utilise la méthode *ColumnSpan()* pour demander au *Slider* de s’étendre jusqu’à 3 colonnes à partir de la case n°3 :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
TimeTracker.Row(0).Column(2).ColumnSpan(3)
```


Et voilà on en a vraiment fini ! Relançons l’application pour voir ce que ça donne :

<p align="center"><img max-width="100%" max-height="100%" src="./images/2907217A274375D0ED0A93A0EEB41D9F.png" /></p>
<figure></figure>



Bon, c’est vrai, c’est beau mais ça ne fait pas grand chose ! Mais on va d’abord finir la partie visuelle avant de basculer vers la partie plus fonctionnelle de l’application.

D’ailleurs, on a encore plein de boutons à mettre en place ! Notre objectif pour <a href="../8-music-player-display-media-playback/">le prochain chapitre</a> : contrôler la lecture du média.

___
Plus d'articles dans la même série:
{{< series "My first app" >}}
