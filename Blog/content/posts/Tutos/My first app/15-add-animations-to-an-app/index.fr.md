---
PostURL: "add-animations-to-an-app"
Title: "Créer des animations avec .NET MAUI pour une ambiance disco"
Category: "Tutos"
Subcategory: "My first app"
series: ["My first app"]
Index: "15"
PublishDate: "2024-01-16 00:00:15Z"
Language: "French"
Description: "Dans cet épisode final, nous allons créer des animations avec .NET MAUI pour instaurer une ambiance de folie. Mets vite tes lunettes car il va y avoir des flashs de partout !"
Tags: ["Animation","AbsoluteLayout","Round Effect","Brush"]
featuredImagePreview: 'featured-image-preview-fr'
resources:
- name: 'featured-image-preview-fr'
  src: 'featured-image-preview-fr.png'
draft: false
---

<!--more-->


{{< admonition type=info title="‎ " open=true >}}
Cet article fait parti d’un cours (<a href="..">*”Ma Première App”*</a>).
Pour assurer son bon déroulement, je t’invite à récupérer le projet reprenant toutes les étapes appliquées jusqu’ici. Pour cela, réfère-toi au <a href="../2-setup-the-project/">guide d’installation du projet</a> et repars de l’exemple situé dans le dossier *“5 - Music Player”*.
{{< /admonition >}}
Bienvenue dans l’épisode final de cette série ! ✌️ Si tu as suivi le cours jusqu’ici, alors tu devrais avoir une application aboutie pour écouter quelques morceaux de musique.

Seulement voilà, l’ambiance reste un peu terne… Or je t’avais promis une ambiance disco <a href="../1-introduction/">au tout début de ce cours</a> ! Du coup, aujourd’hui nous allons apprendre à utiliser les [animations en .NET MAUI](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/animation/basic) pour mettre le feu sur la piste.

Alors mets vite tes lunettes car il va y avoir des flashs de partout ! 🪩

## Mise en place des projecteurs
Tu l’auras compris, je veux qu’on ait l’impression d’être dans une boîte de nuit. Alors pour commencer, on va ajouter quelques spots lumineux dans la partie supérieure de l’écran qui est représentée par notre *Grid* `TopLayout`.

Dans l’idéal, il faudrait disposer des projecteurs un peu partout, avec différentes tailles et couleurs… Là on parle clairement des paramètres d’un objet ! En effet, nous allons définir une nouvelle classe dédiée aux spots de lumière : la classe `Spotlight`.

Commence donc par créer un nouveau dossier *Components* dans le projet. Puis crée un nouveau fichier *Spotlight.cs* dans ce dossier avec le contenu suivant :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>Spotlight.cs</code></p>

```csharp
using Microsoft.Maui.Layouts;

namespace NightClub.Views.Components;
public class Spotlight : BoxView
{
    public Spotlight(Color color, double size, double positionX, double positionY)
    {
        this.Color = color;
        CornerRadius = size / 2;

        AbsoluteLayout.SetLayoutBounds(this, new Rect(positionX, positionY, size, size));
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.PositionProportional);
    }
}
```
Comme tu vois, cette classe prend en paramètre les 4 propriétés suivantes :

* la couleur (`color`) du spot lumineux,

* sa taille (`size`),

* et enfin, son alignement par rapport à l’axe horizontal (`positionX`) et vertical (`positionY`).

Au fait, as-tu noté que `Spotlight` héritait de [BoxView](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/controls/boxview) ? Souviens-toi, on avait déjà utilisé cet objet pour <a href="../6-arrange-elements-on-a-page/#vérification-du-rendu-à-lécran">vérifier la disposition des éléments</a> de la *MusicPlayerView*.


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ D’accord mais une *BoxView*, ce n’est pas rond du tout !
{{< /admonition >}}
Promis, nos projecteurs ne seront pas rectangulaire. On va en effet les arrondir à l’aide de la propriété `CornerRadius`, comme on l’avait fait pour <a href="../8-music-player-display-media-playback/#découverte-des-imagebutton">le bouton de lecture</a> ! Pour cela, la valeur du `CornerRadius` devra être deux fois inférieure à la taille demandée (`size`).

Justement, voyons maintenant comment la taille et la position du `Spotlight` ont été définies :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>Spotlight.cs</code></p>

```csharp
AbsoluteLayout.SetLayoutBounds(this, new Rect(positionX, positionY, size, size));
```
Avec la méthode *SetLayoutBounds()*, on applique la position et la taille demandée à notre *Spotlight* (`this`), en lui passant un objet de type [Rect](https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.graphics.rect.-ctor#microsoft-maui-graphics-rect-ctor(system-double-system-double-system-double-system-double)). Il s’agit d’une structure représentant les coordonnées de notre composant, ainsi que ses dimensions.

Enfin, on fait appel à la méthode *SetLayoutFlags()* pour que cette position soit prise en compte de façon proportionnelle :

```csharp
AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.PositionProportional);
```
En résumé, la taille du *Spotlight* sera bien configurée en valeurs absolues, mais ses coordonnées seront passées en valeurs proportionnelles. Seulement, ces méthodes ne peuvent fonctionner que sur des éléments contenus dans un *AbsoluteLayout*.


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Ah, on n’utilise plus de *Grid* finalement ?
{{< /admonition >}}
Si, si ! Mais disons que notre *Grid* `TopLayout` jouera le rôle de conteneur principal, alors que les projecteurs eux, seront ajoutés à un composant intermédiaire de type [AbsoluteLayout](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/layouts/absolutelayout).

En effet, ce composant permet lui aussi d’aligner des éléments, mais de façon beaucoup plus libre. C’est d’ailleurs pour ça manipule des coordonnées ! On peut alors facilement placer nos objets où bon nous semble.


{{< admonition type=tip title="‎ " open=true >}}
D’ailleurs, il est possible d’aligner des éléments en dehors des limites de la zone établie par l’*AbsoluteLayout*.
{{< /admonition >}}
Voilà pour la classe `Spotlight` ! Il ne nous reste donc plus qu’à définir un *Absolut*e*Layout*, lui ajouter quelques *Spotlight*, et rattacher le tout au `TopLayout`.

Pour cela, déclarons une nouvelle méthode *InitDanceFloor()* dans la *MusicPlayerView* :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
// Ne pas oublier d'ajouter "using NightClub.Views.Components;" !
#region Dance Floor

void InitDanceFloor()
{
    // On définit un conteneur intermédiaire de type AbsoluteLayout... 
    AbsoluteLayout spotlights = new AbsoluteLayout()
    {
        Children =
        {
            // ... auquel on ajoute quelques Spotlight...
            new Spotlight(Colors.Blue, 200, 0, 0),
            new Spotlight(Colors.Orange, 150, 0.8, 0.9),
            new Spotlight(Colors.Pink, 100, 0.9, 0.2),
            new Spotlight(Colors.Yellow, 120, 0.3, 0.6),
        }
    };

    // ... et le tout est rajouté au conteneur principal !
    TopLayout.Add(spotlights);
}

#endregion
```
Puis, il faut appeler cette méthode depuis le constructeur de la *View* :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
public MusicPlayerView()
{
    ...
    InitDanceFloor();
    ...
}
```
Et enfin, pour que cela fonctionne, on doit modifier la déclaration du `TopLayout` pour ne plus qu’il soit immuable :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
Grid TopLayout = new Grid // Le signe “=>” a été remplacé par “=”
{
    BackgroundColor = Colors.Black
};
```
Tadam ! Voici ce que ça donne :

<p align="center"><img max-width="100%" max-height="100%" src="./images/2D84AB449FE9843B02AB40D147939346.png" /></p>
<figure><figcaption class="image-caption">Quelques cercles colorés… on est loin de la discothèque !</figcaption></figure>

Comment ça, tu es déçu·e ?! 🙊

Bon je sais, ce n’est pas très convaincant…  pour l’instant ! Alors passons vite à la suite pour obtenir un meilleur visuel.

## Peaufiner le visuel pour un peu plus de réalisme
Jusqu’à présent, pour colorer nos éléments visuels il suffisait de définir une couleur et la magie opérait en arrière-plan. Cependant, il existe différentes techniques de coloriage qui vont nous aider à obtenir un meilleur visuel : ce sont les [Brush](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/brushes/).

Par exemple, nous allons utiliser le [RadialGradientBrush](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/brushes/radialgradient) pour appliquer un joli dégradé radial :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>Spotlight.cs</code></p>

```csharp
public Spotlight(Color color, double size, double positionX, double positionY)
{
    // Tu peux désormais supprimer la ligne suivante :
    // this.Color = color;

    Background = new RadialGradientBrush()
    {
        GradientStops = new GradientStopCollection
        {
            new GradientStop(color, 0),
            new GradientStop(Colors.Transparent, 1)
        }
    };
    ...
}
```
Concrètement, on a défini un dégradé de couleurs depuis le centre du *Spotlight* jusqu’aux bords, grâce à l’utilisation de deux [GradientStop](https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.controls.gradientstop.-ctor/#microsoft-maui-controls-gradientstop-ctor(microsoft-maui-graphics-color-system-single)) :

1. Avec l’instruction `GradientStop(color, 0)`, le centre du *Spotlight* (`0`) est d’abord peint de la couleur demandée (`color`),

1. Puis un dégradé s’opère avec `GradientStop(Colors.Transparent, 1)` pour aller vers la [transparence](https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.graphics.colors.transparent/#microsoft-maui-graphics-colors-transparent) la plus totale (`Colors.Transparent`) au niveau de la circonférence du *Spotlight* (`1`).

Et grâce à l’effet de transparence, on verra alors apparaître le fond noir du `TopLayout`.


{{< admonition type=info title="‎ " open=true >}}
Plus besoin de la propriété `Color` en utilisant la propriété `Background`.
{{< /admonition >}}
Ensuite, nous allons utiliser le [Shadow](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/shadow) pour donner un effet de brillance :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>Spotlight.cs</code></p>

```csharp
public Spotlight(Color color, double size, double positionX, double positionY)
{
    ...
    Shadow = new Shadow()
    {
        Radius = (float)(size / 2),
        Brush = new SolidColorBrush(color)
    };
    ...
}
```
Avec ce code, on ajoute une ombre au *Spotlight* qui est à la fois colorée et floue :

* C’est la propriété `Radius` qui donne cet effet flouté, accentué selon la taille de l’objet (`size / 2`),

* Et l’ombre est peinte avec une couleur pleine, grâce à l’utilisation du [SolidColorBrush](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/brushes/solidcolor).

Voyons de nouveau ce que ça rend à l’écran :

<p align="center"><img max-width="100%" max-height="100%" src="./images/0FBBAB1BB2166CA9C695633CA64B8C5E.png" /></p>
<figure><figcaption class="image-caption">Les spots sont bien plus réalistes !</figcaption></figure>

C’est mieux là, non ? 🙂

Mais ces nouveaux ajustements contrastent un peu avec le `BottomLayout`, il faudrait rendre ce *Grid* moins opaque.


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Justement, on ne peut pas jouer avec la propriété `Opacity` ?
{{< /admonition >}}
Pas vraiment… sinon c’est tout le contenu du `BottomLayout` qui deviendra transparent ! On ne verrait alors plus nos contrôles, ce ne serait pas très pratique.

On va plutôt changer sa couleur en y ajoutant un peu de transparence, comme ceci :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
Grid BottomLayout => new Grid
{
    BackgroundColor = Colors.DimGray.WithAlpha(0.4f),
    ...
};
```
Tu vois, c’est toujours la même couleur ! Mais elle contient maintenant [40% de transparence](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/graphics/colors/#modify-a-color) grâce à la méthode *WithAlpha()*.

Et voilà le résultat !

<p align="center"><img max-width="100%" max-height="100%" src="./images/1F2D95853E0DF3ADF6894BBC8D808A0F.png" /></p>
<figure><figcaption class="image-caption">Cette transparence, ça fait plus moderne !</figcaption></figure>

Bon c’est joli tout ça, mais ça reste quand même très statique. Il nous faudrait un peu de folie, de l’animation quoi ! Ça tombe bien, on découvre [les animations en .NET MAUI](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/animation/basic) juste après.

## C’est l’heure du disco!
Dans cette partie, nous allons voir comment apporter un peu de dynamisme à nos spots. Après tout, ce qu’on veut c’est une véritable ambiance de boîte de nuit ! 🙂

Pour cela, on va modifier encore la classe *Spotlight* pour faire en sorte que nos spots clignotent, un peu comme des flashs, tu vois ? On utilisera pour cela une [animation de type fondu](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/animation/basic/#fading) pour progressivement faire apparaître ou disparaître nos projecteurs à l’écran.

Allez, mettons-nous au travail ! Commence par appliquer les modifications suivantes :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>Spotlight.cs</code></p>

```csharp
public class Spotlight : BoxView
{
    // On a ajouté 3 nouvelles propriétés...
    const string AnimationName = "fadeInAndOut";
    uint AnimationLength { get; set; }
    Animation SpotlightAnimation { get; set; }

    // ... ainsi qu'un nouveau paramètre au constructeur ! 
    public Spotlight(Color color, double size, double positionX, double positionY, uint animationLength = 0)
    {
        ...
        // Et enfin, on persiste la durée d'animation.
        this.AnimationLength = animationLength;
        SetAnimation();
    }
}
```
Tout d’abord, nous avons ajouté un nouveau paramètre : `animationLength`. C’est pour définir la durée (en millisecondes) sur laquelle l’animation va se répéter en boucle.

On en a aussi profité pour rajouter quelques propriétés que nous allons utiliser un peu plus tard. Parmi elles, il y a `SpotlightAnimation` qui contiendra la définition de l’animation du *Spotlight*.

Justement, ajoutons une nouvelle méthode à la classe *Spotlight* pour définir cette animation :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>Spotlight.cs</code></p>

```csharp
void SetAnimation()
{
    if (AnimationLength <= 0) return;

    var fadeInAnimation = new Animation(v => Opacity = v, start: 0, end: 1, Easing.CubicOut);
    var fadeOutAnimation = new Animation(v => Opacity = v, start: 1, end: 0, Easing.CubicOut);

    SpotlightAnimation = new Animation
    {
        { 0, 0.5, fadeInAnimation }, // En action de 0 à 50% de l'exécution
        { 0.5, 1, fadeOutAnimation } // En action de 50 à 100% de l'exécution
    };

    StartAnimation();
}
```
C’est un peu technique, mais pas compliqué. Je vais t’expliquer ! 🤓

La propriété `SpotlightAnimation` est une [animation personnalisée](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/animation/custom) composée de deux animations sous-jacentes :

1. Un fondu entrant qui modifie progressivement l’opacité du *Spotlight* de 0 à 1, pendant toute la première moitié de l’animation : `{ 0, 0.5, fadeInAnimation }`,

1. Et un fondu sortant, qui fait exactement l’inverse durant toute la deuxième moitié de l’animation : `{ 0.5, 1, fadeOutAnimation }`.

Et quant à l’option `Easing.CubicOut`, il s’agit seulement d’un effet de style pour que l’animation ralentisse rapidement après le début de son exécution.


{{< admonition type=tip title="‎ " open=true >}}
Aller plus loin avec [les différents types de rendu d’animation](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/animation/easing).
{{< /admonition >}}
Voilà, c’est tout pour la méthode *SetAnimation()*… enfin, presque !


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Quoi, quoi ?! Qu’est-ce qu’on a loupé ? 🙈
{{< /admonition >}}
À la fin de la méthode, tu peux observer l’instruction `StartAnimation();`. C’est elle qui va déclencher l’exécution de l’animation fraichement initialisée.

En voici la définition :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>Spotlight.cs</code></p>

```csharp
public void StartAnimation()
{
    if (AnimationLength <= 0) return;

    SpotlightAnimation.Commit(this, AnimationName, length: AnimationLength, repeat: () => true);
}
```
Tu l’auras compris, c’est la méthode [Commit()](https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.controls.animation.commit) qui permet de lancer l’animation définie plus haut. Décomposons les paramètres passés lors du *Commit*, on a :

* Un nom d’animation (`AnimationName`), défini arbitrairement par une constante,

* Une durée d’exécution (`AnimationLength`), passée en paramètre du constructeur de la classe *Spotlight*,

* Et un mode de répétition (`repeat: () => true`), pour que l’animation soit jouée en boucle indéfiniment.

Prends un moment pour assimiler tous ces nouveaux changements !


{{< admonition type=note title="‎ " open=true >}}
Dans certains cas, les animations peuvent être désactivées par le système. Par exemple : pour des raisons d’accessibilité ou d’économie d’énergie.
{{< /admonition >}}
Et quand tu te sens prêt·e, reviens dans le code de la *MusicPlayerView* pour modifier la méthode *InitDanceFloor()* et adapter l’initialisation des *Spotlight*, comme ceci :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
AbsoluteLayout spotlights = new AbsoluteLayout()
{
    Children =
    {
        // Une durée d'animation est maintenant définie pour chaque Spotlight
        new Spotlight(Colors.Blue, 200, 0, 0, 2000),
        new Spotlight(Colors.Orange, 150, 0.8, 0.9, 1000),
        new Spotlight(Colors.Pink, 100, 0.9, 0.2, 500),
        new Spotlight(Colors.Yellow, 120, 0.3, 0.6, 1500),
    }
};
```
On a simplement défini une durée d’animation différente pour que chacun de nos projecteurs soit unique.

Recompile vite le projet pour voir le résultat !

<p align="center"><img max-width="100%" max-height="100%" src="./images/32EED84F7C3FC4031160AAB335312DDF.gif" /></p>
<figure><figcaption class="image-caption">Les projecteurs s’allument et s’éteignent à différentes vitesses, ça c’est disco !</figcaption></figure>

Tadaaaam ! On s’y croirait presque, qu’en penses-tu ?


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Top !! Mais si les animations se répètent à l’infini, ça ne s’arrêtera donc jamais ?
{{< /admonition >}}
En effet, c’est pratique ce bouclage infini, mais ça pourrait poser problème pour l’expérience utilisateur. Imagine qu’il mette la musique en pause ! Ce serait mieux si l’animation s’arrêtait de jouer aussi.

On voit ça juste après !

### La touche finale
Tiens bon ! On va aborder encore quelques petites améliorations et ce sera la fin de ce cours.

Commençons déjà par voir comment [arrêter une animation en cours d’exécution](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/animation/custom/#cancel-an-animation). Pour cela, reviens dans la classe *Spotlight* et rajoute la méthode suivante :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>Spotlight.cs</code></p>

```csharp
public void StopAnimation()
{
    this.AbortAnimation(AnimationName);
    this.Opacity = 0;
}
```
On a seulement besoin d’appeler la méthode *AbortAnimation()* pour annuler une animation en cours d’exécution, à l’aide de son nom (`AnimationName`). Rappelle-toi, c’est un des paramètres que l’on avait passé dans la méthode *Commit()* !


{{< admonition type=info title="‎ " open=true >}}
On ne peut pas prévoir dans quel état une animation va s’arrêter. Du coup, on bascule l’opacité du *Spotlight* à 0 pour le cacher complètement quand la musique ne joue plus.
{{< /admonition >}}
Voyons ensuite comment jouer l’animation selon si la musique est en cours de lecture ou non. Pour cela, on doit établir un lien entre le *Spotlight* et le *MediaElement*.

Commençons par modifier la classe `Spotlight` :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>Spotlight.cs</code></p>

```csharp
// Attention, 3 nouveaux using sont nécessaires !
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;
...
public class Spotlight : BoxView
{
    ...
    // Un nouveau paramètre est ajouté au constructeur...
    public Spotlight(Color color, double size, double positionX, double positionY, uint animationLength = 0, MediaElement bindableMediaElement = null)
    {
        ...
        // ... pour les besoins de la configuration de l'animation.
        SetAnimation(bindableMediaElement);
    }
    ...
}
```
Pour éviter la répétition de code, on a ajouté un nouveau paramètre `bindableMediaElement` de type *MediaElement*. En effet, on va passer notre composant `MusicPlayer` à l’initialisation de chaque *Spotlight* pour les besoins de la configuration de l’animation.

Nous allons justement modifier la méthode *SetAnimation()* pour appliquer les nouvelles conditions d’exécution de l’animation :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>Spotlight.cs</code></p>

```csharp
void SetAnimation(MediaElement mediaElement = null)
{
    ...
    if(mediaElement != null)
    {
        this.Bind(
            source: mediaElement,
            path: nameof(mediaElement.CurrentState),
            convert: (MediaElementState currentState) =>
            {
                if (currentState != MediaElementState.Playing)
                    StopAnimation();
                else
                    StartAnimation();

                return true;
            });
    }
}
```
Maintenant qu’on a accès à notre *MediaElement*, on a appliqué la technique du *Data Binding* sur la propriété `CurrentState` pour que l’animation soit :

* Démarrée quand la musique est en cours de lecture,

* Ou arrêtée si la musique est mise en pause.

Et comme toujours, on n’oublie pas d’adapter l’initialisation des *Spotlight* dans la méthode *InitDanceFloor()*, comme ceci :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
AbsoluteLayout spotlights = new AbsoluteLayout()
{
    Children =
    {
        // On passe bien le MusicPlayer à chaque Spotlight
        new Spotlight(Colors.Blue, 200, 0, 0, 2000, MusicPlayer),
        new Spotlight(Colors.Orange, 150, 0.8, 0.9, 1000, MusicPlayer),
        new Spotlight(Colors.Pink, 100, 0.9, 0.2, 500, MusicPlayer),
        new Spotlight(Colors.Yellow, 120, 0.3, 0.6, 1500, MusicPlayer),
    }
};
```
Ici, on a simplement passé le `MusicPlayer` à l’initialisation des *Spotlight* pour que chacun des projecteurs puisse être éteint ou allumé au gré de la musique.

Voilà, je te fais confiance pour vérifier que ça fonctionne bien avant qu’on ajoute la touche finale. Ben oui, notre piste manque encore de quelques danseurs passionnés !

Commence donc par télécharger les deux nouvelles images suivantes au format *.svg* :

* La première représente des gros haut-parleurs (*speakers.svg*),

* Et l’autre, nos fameux danseurs (*joyful_dancers.svg*).

{{< link href="./files/Dance_Floor_-_Images.zip" content="Dance_Floor_-_Images.zip" title="Download Dance_Floor_-_Images.zip" download="Dance_Floor_-_Images.zip" card=true >}}
Une fois le fichier *.zip* décompressé, ajoute les images au dossier *Resources/Images*.


{{< admonition type=info title="‎ " open=true >}}
Si tu as des doutes sur comment faire, tu peux te référer <a href="../8-music-player-display-media-playback/">à ce chapitre</a>.
{{< /admonition >}}
Enfin, rajoute-les au `TopLayout` de cette façon :

```csharp
void InitDanceFloor()
{
    ...
    TopLayout.Add(spotlights);
    // .NET MAUI convertit les fichiers SVG au format PNG.
    TopLayout.Add(new Image() { Source = "speakers.png" }.Bottom());
    TopLayout.Add(new Image() { Source = "joyful_dancers.png" }.Bottom());
}
```
Avec la méthode *Add()*, on ajoute l’un après l’autre tous nos objets dans l’unique case qui compose le *Grid* `TopLayout`.


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Une seule case pour 3 objets ? Mais comment c’est possible du coup ?
{{< /admonition >}}
En effet, si tu te rappelles la [documentation](https://learn.microsoft.com/fr-fr/dotnet/maui/user-interface/layouts/grid/#rows-and-columns), le *Grid* ne possède qu’une seule ligne et qu’une seule colonne par défaut. Autrement dit, il ne s’agit ni plus ni moins d’une grille composée d’une seule case.

Par conséquent, les éléments vont s’empiler les uns par dessus les autres ! C’est justement ce que l’on cherche à faire pour donner un véritable effet de profondeur :

* En arrière-plan, il y aura nos différents projecteurs (`spotlights`),

* Puis les haut-parleurs,

* Et enfin, nos danseurs seront positionnés au premier plan.

Allez c’est parti, relance vite le projet !

<p align="center"><img max-width="100%" max-height="100%" src="./images/F0A9AB44253D46567E3073BF454BD300.png" /></p>
<figure><figcaption class="image-caption">Là, ça sent clairement la fête !!</figcaption></figure>

J’espère que tu es content·e du résultat. En tout cas, bravo pour le travail accompli jusqu’ici ! 🤓


{{< admonition type=info title="‎ " open=true >}}
Si les images ne s’affichent pas, pense à nettoyer et recompiler le projet entier.
{{< /admonition >}}
## Conclusion
Cet article marque la fin de ce cours, un total de 2h30 de lecture qui je l’espère t’aura plu et t’inspirera plein d’autres idées. Et merci pour ta fidélité si tu as suivi l’intégralité de ce cours ! Si tu as un moment, écris-moi un commentaire ou un e-mail pour me dire ce que tu en as pensé.

Au fait, si tu veux t’amuser à aller plus loin (et je t’y encourage chaudement ! 🙂), tu peux essayer de générer les projecteurs de façon complètement aléatoire. Tu pourras toujours comparer ta solution avec la mienne en regardant directement dans le code final du projet !


{{< admonition type=info title="‎ " open=true >}}
Pour récupérer le projet reprenant toutes les étapes appliquées jusqu’ici dans ce cours, réfère-toi au <a href="../2-setup-the-project/">guide d’installation du projet</a> et repars de l’exemple situé dans le dossier *“Full Solution”*.
{{< /admonition >}}
Et si tu en veux encore plus, alors voici d’autres pistes d’amélioration à creuser :

* Calquer la luminosité des spots sur le volume de la musique,

* Ajuster leur vitesse d’animation selon le tempo des chansons,

* Regénérer tous les spots lumineux quand on change de chanson, etc.

J’adorerais voir ta créativité à l’œuvre, alors si tu peux, partage-moi en commentaire un lien vers ton répo GitHub !

Voilà, c’est tout pour aujourd’hui. À bientôt pour de nouveaux tutos ! 👋

___
Plus d'articles dans la même série:
{{< series "My first app" >}}
