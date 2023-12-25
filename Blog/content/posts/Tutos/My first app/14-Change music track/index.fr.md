---
Topic: "Change music track"
Title: "Changer de morceau"
Category: "Tutos"
Subcategory: "My first app"
series: ["My first app"]
Index: "14"
PublishDate: "2023-12-26 00:00:14Z"
Language: "French"
Description: "L’application est bien, mais ça serait plus sympa si on pouvait changer de morceau. Ça tombe bien, on va intégrer aujourd’hui une playlist musicale !"
Tags: ["MediaElement","MVVM","ImageButton","Data Binding"]
featuredImagePreview: 'featured-image-preview-fr'
resources:
- name: 'featured-image-preview-fr'
  src: 'featured-image-preview-fr.png'
draft: false
---

<!--more-->


{{< admonition type=info title="‎ " open=true >}}
Afin d’assurer le bon déroulement de cet article, je t’invite à repartir depuis <a href="../10-play-music/">ce chapitre</a> où l’on a configuré le *MediaElement*.
{{< /admonition >}}
La dernière fois, nous avions vu comment télécharger de la musique directement depuis l’app. Seulement, c’est la même chanson depuis le début !

L’application serait bien plus sympa si on pouvait changer de morceau, non ? Ça tombe bien, on va y intégrer aujourd’hui une playlist musicale.

# Création de la playlist
Pour cela, définissons déjà la liste des morceaux disponibles dans l’application comme ceci :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerViewModel.cs</code></p>

```csharp
static readonly MusicTrack[] playlist = new MusicTrack[]
{
    new MusicTrack()
    {
        AudioURL = "https://prod-1.storage.jamendo.com/?trackid=1890762&format=mp31&from=b5bSbOTAT1kXawaT8EV9IA%3D%3D%7CGcDX%2BeejT3P%2F0CfPwtSyYA%3D%3D",
        AudioDownloadURL = "https://prod-1.storage.jamendo.com/download/track/1890762/mp32/",
        Author = "Alfonso Lugo",
        Title = "Baila",
    },
    new MusicTrack()
    {
        AudioURL = "https://prod-1.storage.jamendo.com/?trackid=619144&format=mp31&from=%2BJv5PkdWd%2BvsByBkyrboJA%3D%3D%7Co%2FKvdc5gcd6iQLjnqacjYA%3D%3D",
        AudioDownloadURL = "https://prod-1.storage.jamendo.com/download/track/619144/mp32/",
        Author = "Pablo Gómez",
        Title = "Devastation (remastered)",
    },
    new MusicTrack()
    {
        AudioURL = "https://prod-1.storage.jamendo.com/?trackid=1399476&format=mp31&from=LQFaB9%2FDVAE6QaK%2BsXtl%2FA%3D%3D%7CouuozaATpW3zoEvVwprgRw%3D%3D",
        AudioDownloadURL = "https://prod-1.storage.jamendo.com/download/track/1399476/mp32/",
        Author = "Singularity",
        Title = "How many times",
    },
    new MusicTrack()
    {
        AudioURL = "https://prod-1.storage.jamendo.com/?trackid=946449&format=mp31&from=blTB635bS8UiDVL%2FzZC2Xw%3D%3D%7CQO1Fj6AWgTrjIu7LELLCLA%3D%3D",
        AudioDownloadURL = "https://prod-1.storage.jamendo.com/download/track/946449/mp32/",
        Author = "Julien Gathy",
        Title = "Octave (HQ)",
    },
    new MusicTrack()
    {
        AudioURL = "https://prod-1.storage.jamendo.com/?trackid=1026396&format=mp31&from=nWYOo%2FxFcd1oJBINLSQAXg%3D%3D%7CI8xQbXqZfz2bfgmtqxmqyA%3D%3D",
        AudioDownloadURL = "https://prod-1.storage.jamendo.com/download/track/1026396/mp32/",
        Author = "dj alike",
        Title = "dj alike (new trance edition)",
    }
};
```
Rien d’extraordinaire ici, si ce n’est la démonstration de l’utilité de notre **Model** !


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Oh oui, 5 chansons, ça va groover !! 🙊
{{< /admonition >}}
Mais c’est ça oui, moque-toi ! 😂 

Le nombre de chansons est limité à 5 pour les besoins de ce cours, mais imagine si on permettait à l’utilisateur d’explorer des titres du monde entier !


{{< admonition type=info title="‎ " open=true >}}
Il n’est pas improbable que je réfléchisse à une suite de ce cours, peut-être en vidéo !
{{< /admonition >}}
Mais revenons à nos moutons.

La logique que l’on va implémenter consiste à passer d’un morceau à un autre parmi notre `playlist`. C’est un peu comme un pointeur qui glisse sur les cases d’un tableau d’éléments de type `MusicTrack` pour définir la prochaine musique à jouer.

Pour cela, nous allons améliorer la logique autour de la propriété `CurrentTrack` introduite dans <a href="../13-download-music/">le chapitre précédent</a> avec le code suivant :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerViewModel.cs</code></p>

```csharp
#region Properties
...

// Une nouvelle propriété pour définir la position du "pointeur"
[ObservableProperty]
[NotifyPropertyChangedFor(nameof(CurrentTrack))]
int currentTrackPosition = 0;

// Attention à bien renommer la propriété avec un "C" majuscule
public MusicTrack CurrentTrack => playlist[CurrentTrackPosition];

#endregion

public MusicPlayerViewModel()
{
    // Nous n'avons plus besoin du code ci-après, à supprimer !
    // CurrentTrack = new MusicTrack()
    // {
    //     ...
    // };
}
```
On a ajouté une nouvelle propriété nommée `currentTrackPosition` qui correspond en quelque sorte à notre pointeur évoqué plus haut. C’est elle qui va définir la position de la chanson qu’il faut jouer parmi la `playlist`, grâce à ce nouvel attribut : [NotifyPropertyChangedFor()](https://learn.microsoft.com/fr-fr/dotnet/communitytoolkit/mvvm/generators/observableproperty#notifying-dependent-properties).

Concrètement, à chaque changement de valeur de la propriété `currentTrackPosition`, cet attribut va déclencher une notification vers la **View** pour l’informer que la propriété `CurrentTrack` vient d’être mise à jour.

En effet, la propriété `CurrentTrack` a été adaptée pour retourner un titre musical depuis la `playlist` à la position demandée (`playlist[CurrentTrackPosition]`).


{{< admonition type=info title="‎ " open=true >}}
D’ailleurs, nous n’avons plus besoin d’initialiser `CurrentTrack` depuis le constructeur du *MusicPlayerViewModel* ! Tu peux donc supprimer ce bout de code.
{{< /admonition >}}
C’est déjà l’heure de relancer l’application !


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Mais je n’ai vu aucun changement, c’est normal ?
{{< /admonition >}}
Exact, c’est encore le même morceau de musique qui est joué ! Et c’est tout à fait normal.

As-tu remarqué la valeur initiale de la propriété `currentTrackPosition` ? Elle est égale à 0, ce qui correspond donc à la première position du tableau `playlist`, qui n’est autre que la chanson “Baila” jouée depuis <a href="../10-play-music/">le chapitre n°10</a>.

Mais passons vite à la suite, car on veut écouter les autres morceaux !

# Binding des composants
Attention, révélation : on va désormais s’attaquer aux derniers composants factices de l’application !

Si tu te rappelles bien du <a href="../8-media-control/">chapitre sur l’affichage des contrôles de lecture</a>, il nous reste 3 composants de type *ImageButton* à implémenter :

* `SkipNextButton` pour passer à la chanson suivante,

* `SkipPreviousButton` pour revenir à la chanson précédente,

* et `RepeatOnceButton` pour rejouer la piste musicale (une seule fois).

Pour passer à la prochaine chanson, c’est très simple. Commence par ajouter la commande suivante dans le **ViewModel** :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerViewModel.cs</code></p>

```csharp
#region Commands

[RelayCommand]
void GoToNextTrack()
{
    if (CurrentTrackPosition + 1 < playlist.Length) CurrentTrackPosition++;
    else CurrentTrackPosition = 0;
}
...
#endregion
```
Puis associe la nouvelle commande au bouton `SkipNextButton` depuis la **View** :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
#region Media Control Panel
...
ImageButton SkipNextButton = new ImageButton // Le signe “=>” a été remplacé par “=”
{
    HeightRequest = 75,
    WidthRequest = 75,
    Source = "skip_next.png"
} .BindCommand("GoToNextTrackCommand"); // et voilà la commande associée
...
#endregion
```
Le comportement défini dans la méthode *GoToNextTrack()* consiste à simplement déplacer d’un cran en avant la position du pointeur sur le tableau. Bien sûr, quand la dernière case du tableau est atteinte, alors on retourne au début de la `playlist`.

Et si on testait ?

<p align="center"><img max-width="100%" max-height="100%" src="./images/65EB486D8A0AC5564134E3CFA92FB507.gif" /></p>
<figure><figcaption class="image-caption">Super, on peut enfin écouter d’autres chansons !</figcaption></figure>


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ C’était rapide ! Et pour revenir en arrière dans la playlist, on fait juste l’inverse ?
{{< /admonition >}}
Presque ! Pour revenir à la chanson précédente, on va en effet implémenter une logique similaire à une condition près. En effet, j’aimerais d’abord que la chanson redémarre avant de basculer vers la chanson précédente.

Je vais t’expliquer comment. Ajoute déjà la commande suivante dans le **ViewModel** :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerViewModel.cs</code></p>

```csharp
#region Commands

[RelayCommand]
void GoToPreviousTrack(double elapsedTimeForCurrentTrack)
{
    if (elapsedTimeForCurrentTrack < 2)
    {
        // Soit on revient un cran en arrière dans la playlist
        if (CurrentTrackPosition - 1 >= 0) CurrentTrackPosition--;
        else CurrentTrackPosition = playlist.Length - 1;
    }
    else
    {
        // Ou bien on réinitialise le morceau en cours
        // en forçant la notification vers la View
        OnPropertyChanged("CurrentTrack");
    }
}
...
#endregion
```
Le comportement défini dans la méthode *GoToPreviousTrack()* consiste à simplement déplacer d’un cran en arrière la position du pointeur sur le tableau. Bien sûr, quand la première case du tableau est atteinte, alors on bascule à la dernière chanson de la `playlist`.

Mais as-tu remarqué la condition imposée dans le `if`? On ne doit revenir à la chanson précédente que si la chanson en cours a été jouée depuis moins de 2 secondes.


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Mais comment va-t’on faire pour obtenir cette information ?
{{< /admonition >}}
Jusqu’à présent, nous n’avions vu que des commandes sans paramètre. Mais cette fois-ci, on va partager l’information `TimeTracker.Value` en paramètre de la commande à associer au `SkipPreviousButton`.

Pour cela, applique les changements suivants dans la **View** :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
#region Media Control Panel
...
ImageButton SkipPreviousButton = new ImageButton // Le signe “=>” a été remplacé par “=”
{
    HeightRequest = 75,
    WidthRequest = 75,
    Source = "skip_previous.png"
} .BindCommand("GoToPreviousTrackCommand", // et voilà la commande associée, avec un paramètre !
    parameterPath: nameof(TimeTracker.Value),
    parameterSource: TimeTracker);
...
#endregion
```
On emploie toujours *BindCommand()*, mais on utilise deux paramètres supplémentaires pour les besoins de notre méthode *GoToPreviousTrack()* :

* L’information à passer en paramètre (`parameterPath`), c’est le `TimeTracker.Value` correspondant au temps de lecture écoulé,

* Et la source de cette propriété (`parameterSource`), c’est bien sûr le `TimeTracker`.

Enfin, dans le cas où l’on doit redémarrer la chanson, on fait preuve d’un peu d’astuce en utilisant la méthode *OnPropertyChanged()*. En effet, cela va forcer le déclenchement d’une notification vers la **View** pour l’informer que la propriété `CurrentTrack` vient d’être mise à jour (bien que sa valeur n’ait pas changé du tout !).

Et ça y’est ! Voyons tout de suite ce que ça donne :

<p align="center"><img max-width="100%" max-height="100%" src="./images/40805480AF240635975DCC72C674F6B6.gif" /></p>
<figure><figcaption class="image-caption">Au premier clic, la chanson redémarre. Et les clics suivants permettent de revenir aux chansons précédentes.</figcaption></figure>

Eh bien voilà ! Il ne nous reste plus qu’un bouton à implémenter pour permettre la répétition du morceau qui est en cours de lecture.

# Rejouer le morceau une fois
C’est la dernière ligne droite de ce cours, accroche-toi !

Pour permettre la répétition d’un morceau, nous devons d’abord ajouter la propriété suivante dans le **ViewModel** :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerViewModel.cs</code></p>

```csharp
#region Properties
...
[ObservableProperty]
bool mustRepeatCurrentTrackOnce;

#endregion
```
Il s’agit d’un booléen classique dont l’état sera modifié via sa commande dédiée :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerViewModel.cs</code></p>

```csharp
#region Commands

[RelayCommand]
void ToggleRepeatOnce()
{
    MustRepeatCurrentTrackOnce = !MustRepeatCurrentTrackOnce;
}
...
#endregion
```
L’idée est qu’à chaque clic de l’utilisateur, on inverse l’état du booléen afin d’activer ou désactiver la répétition. Il ne nous reste alors plus qu’à associer cette commande au bouton `RepeatOnceButton` depuis la **View** :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
#region Media Control Panel
...
ImageButton RepeatOnceButton = new ImageButton // Le signe “=>” a été remplacé par “=”
    {
        CornerRadius = 5,
        HeightRequest = 25,
        WidthRequest = 25,
        Source = "repeat_once.png",
        BackgroundColor = Colors.Black,
        BorderColor = Colors.GreenYellow // On rajoute une bordure colorée au bouton
    } .BindCommand("ToggleRepeatOnceCommand"); // Et voilà la commande associée
...
#endregion
```
Comme tu l’auras peut-être remarqué, j’ai défini une couleur pour le contour du `RepeatOnceButton` avec la propriété `BorderColor`. C’est pour améliorer le rendu visuel quand la répétition est enclenchée !

Pour cela, on va se baser évidemment sur la propriété `MustRepeatCurrentTrackOnce`. Modifie donc la méthode *InitMediaControlPanel()* pour appliquer les changements suivants :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
void InitMediaControlPanel()
{
    ...
    RepeatOnceButton.Bind(
        targetProperty: ImageButton.BorderWidthProperty,
        path: nameof(MusicPlayerViewModel.MustRepeatCurrentTrackOnce),
        convert: (bool isEnabled) => isEnabled ? 2 : 0);
}
```
Avec ce code, on joue avec l’épaisseur des bordures (`BorderWidth`) du bouton `RepeatOnceButton` selon si l’option de répétition est activée ou non. En effet, il faut savoir que l’épaisseur du contour est par défaut négative !

<p align="center"><img max-width="100%" max-height="100%" src="./images/60DAE0F96E9985F1F116474E43F6167B.png" /></p>
<figure><figcaption class="image-caption">On ne risque pas de voir de contour avec une épaisseur négative !</figcaption></figure>

Autrement dit, on ne verra aucune bordure à moins de modifier nous-mêmes la valeur du `BorderWidth`.

C’est pourquoi, dans le *convert*, on définit une valeur positive pour faire apparaître un contour coloré quand `MustRepeatCurrentTrackOnce` est égal à `True`.

<p align="center"><img max-width="100%" max-height="100%" src="./images/86B48EF1E89B9C492E24323D2F50DE83.gif" /></p>
<figure><figcaption class="image-caption">On voit bien quand la répétition est active. Bon après, les goûts et les couleurs, c’est autre chose !</figcaption></figure>


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ C’est très bien, mais ça ne va pas nous aider à boucler le morceau, non ?
{{< /admonition >}}
C’est vrai ! En fait, on va se baser sur le paramètre `MustRepeatCurrentTrackOnce` pour savoir s’il faut redémarrer la chanson à la fin du morceau, ou passer à la chanson suivante.

Et cette logique sera définie dans le **ViewModel** avec cette nouvelle méthode publique :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerViewModel.cs</code></p>

```csharp
public void AssessRepeatOrSkip() // La méthode doit être publique
{
    if (MustRepeatCurrentTrackOnce)
    {
        // Soit on réinitialise le morceau en cours
        // en forçant la notification vers la View
        OnPropertyChanged("CurrentTrack");
        MustRepeatCurrentTrackOnce = false;
    }
    else
    {
        // Ou bien on se déplace d'une position vers l'avant dans la playlist
        GoToNextTrack();
    }
}
```
Selon que `MustRepeatCurrentTrackOnce` est à `True` ou `False`, on fait soit à nouveau appel à la technique du *OnPropertyChanged()* que l’on a vue plus haut, ou bien on passe à la chanson suivante avec la méthode *GoToNextTrack()*.

Au fait, on ne veut répéter la chanson qu’une seule fois ! Alors n’oublie pas de désactiver la répétition en passant la propriété `MustRepeatCurrentTrackOnce` à `False`.

Enfin, pour détecter le moment où la chanson se termine, on va se baser sur l’évènement *MediaEnded* exposé par le *MediaElement*, comme ceci :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
void InitMusicPlayer()
{
    ...
    MusicPlayer.MediaEnded += MusicPlayer_MediaEnded;
}
```
Et c’est au moment où l’évènement se déclenche que l’on va demander à répéter la chanson en cours ou passer au morceau suivant :

<p align="center" style="margin-bottom:-10px"><strong>Nom du fichier :</strong><code>MusicPlayerView.cs</code></p>

```csharp
#region Events
...
void MusicPlayer_MediaEnded(object sender, EventArgs e)
{
    (BindingContext as MusicPlayerViewModel).AssessRepeatOrSkip();
}

#endregion
```
Pour cela, on s’appuie sur le `BindingContext` de la *MusicPlayerView* afin d’appeler la méthode *AssessRepeatOrSkip()* que l’on vient de définir dans le *MusicPlayerViewModel*.

C’est l’heure de la démo ! Recompile le projet et vérifie que le morceau en cours redémarre après avoir cliqué sur le bouton de répétition :

<p align="center"><img max-width="100%" max-height="100%" src="./images/E2F9E7696167B73A0592778C3BF8C004.gif" /></p>
<figure><figcaption class="image-caption">Si la répétition a été demandée, alors la chanson redémarre (une seule fois !).</figcaption></figure>

Puis laisse la chanson se terminer pour vérifier qu’on passe bien à la suivante :

<p align="center"><img max-width="100%" max-height="100%" src="./images/26C3D60C40D443F7ECB0AA75D9858D53.gif" /></p>
<figure><figcaption class="image-caption">Dès que la chanson se termine, ça passe à la suivante !</figcaption></figure>

# Conclusion
Bon eh bien voilà, est-ce que le résultat te plaît ?

On peut désormais profiter de l’application et écouter quelques titres à la suite, au volume souhaité, réécouter quelques passages, et même télécharger les morceaux ! 😎


{{< admonition type=info title="‎ " open=true >}}
Pour récupérer le projet reprenant toutes les étapes appliquées jusqu’ici dans ce cours, réfère-toi au [guide d’installation du projet](http://localhost:1313/posts/tutos/my-first-app/2-setup-the-project/) et repars de l’exemple situé dans le dossier *“5 - Music Player”*.
{{< /admonition >}}
Mais ça manque un peu d’animation pour une ambiance “boîte de nuit”… Or c’était la <a href="../1-introduction/">promesse du début</a> !

On va remédier à ça dans le prochain article, et ce sera le dernier de cette série !!

___
Plus d'articles dans la même série:
{{< series "My first app" >}}
