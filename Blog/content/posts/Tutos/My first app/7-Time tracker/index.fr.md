---
Topic: "Time tracker"
Title: "Le temps d’écoute"
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
  src: 'featured-image-preview-fr.jpeg'
draft: false
---

<!--more-->

<style>
.img-sizes{min-height:50px;max-height:600px;min-width:50px;max-width:600px;height:auto;width:auto}
</style>

{{< admonition type=info title="‎ " open=true >}}
Pour assurer le bon déroulement de cet article, je t’invite à repartir du projet reprenant les différentes étapes appliquées dans les articles précédents. Pour cela, [télécharge mon cours depuis GitHub](https://github.com/Kapusch/blog-dotnet-maui) si ce n’est pas déjà fait, et ouvre le projet NightClub situé dans le dossier *“3 - Page Scaffolding”*.
{{< /admonition >}}

Dans l’article précédent, on a mis en place un quadrillage pour y disposer tous les contrôles de notre lecteur musical. On va donc pouvoir ajouter des éléments concrets pour l’utilisateur !

### Afficher le minutage

Commençons déjà avec la partie dédiée au minutage du titre musical. D’après la maquette, on aura un texte à gauche pour le temps d’écoute en cours, et à droite, pour la durée totale de la piste :

<figure><p align="center"><img class="img-sizes" src="./images/FA80B1E1F42328E22E779783E27C557F.png"></p><figcaption class="image-caption">L’application indique que le morceau est joué depuis 36 secondes et qu’il dure 2 minutes et 57 secondes. </figcaption></figure>



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



### Ajuster la tête de lecture

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

<figure><p align="center"><img class="img-sizes" src="./images/42375B164D301F432E78BF870C997012.png"></p></figure>




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

<figure><p align="center"><img class="img-sizes" src="./images/6A71FCFF86082A1FEF1D2C5C1840643B.png"></p><figcaption class="image-caption">Disposé comme ça, le Slider n’a pas bonne mine.</figcaption></figure>



Pas de panique ! On va voir tout de suite comment réajuster cela.

### Bien utiliser les Grid

Si tu te rappelles, on a initialement divisé le *BottomLayout* en 7 colonnes. Or, si notre code positionne bien notre *Slider* à partir de la case n°3 de la première ligne, il ne lui dit pas clairement où s’arrêter ! On pourrait alors simplement dire au *Slider* de s’étaler sur les colonnes suivantes, comme ceci :

<figure><p align="center"><img class="img-sizes" src="./images/EC9010D37B4268DF0FCDE7480DD1156F.png"></p><figcaption class="image-caption">Le Slider est à cheval sur les cases n°3, n°4 et n°5.</figcaption></figure>



Pour cela, on utilise la méthode *ColumnSpan()* pour demander au *Slider* de s’étendre jusqu’à 3 colonnes à partir de la case n°3 :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
TimeTracker.Row(0).Column(2).ColumnSpan(3)
```




Et voilà on en a vraiment fini ! Relançons l’application pour voir ce que ça donne :

<figure><p align="center"><img class="img-sizes" src="./images/F625F9944D83A64D3BE00195F96757B2.png"></p></figure>



Bon, c’est vrai, c’est beau mais ça ne fait pas grand chose ! Mais on va d’abord finir la partie visuelle avant de basculer vers la partie plus fonctionnelle de l’application.

D’ailleurs, on a encore plein de boutons à mettre en place ! Notre objectif pour <a href="../8-media-control/">le prochain chapitre</a> : contrôler la lecture du média.

---
Plus d'articles dans la même série:
{{< series "My first app" >}}