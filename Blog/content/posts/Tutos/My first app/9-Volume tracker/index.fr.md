---
Topic: "Volume tracker"
Title: "Ajuster le volume"
Category: "Tutos"
Subcategory: "My first app"
Index: "9"
PublishDate: "2023-02-12 00:00:09Z"
Language: "French"
Description: "Allez c’est bientôt la fin du premier gros de notre page principale. On passe désormais au contrôle du volume !"
Tags: ["Image","ImageButton","DataTrigger","Slider","MultiTrigger",".NET Community Toolkit","C# markup"]
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
Et enfin, la dernière ligne de notre lecteur musical !

<figure><p align="center"><img class="img-sizes" src="./images/DF050A3B8CBC66BE41161183706F5D44.png"></p></figure>

Allez, pour commencer, télécharge les différentes images utilisées pour afficher l’état du volume.

{{< link href="./files/Volume_Tracker_-_Images.zip" content="Volume_Tracker_-_Images.zip" title="Download Volume_Tracker_-_Images.zip" download="Volume_Tracker_-_Images.zip" card=true >}}


En décompressant ce fichier, tu verras 4 nouvelles images:

<figure><p align="center"><img class="img-sizes" src="./images/D513BCCE090CD9A8DB1344EB11150F81.png"></p></figure>

Il y en aura une pour chaque niveau de volume identifié, et même quand le son sera coupé.

Maintenant, inclus ces nouvelles images dans le dossier *Resources/Images*, exactement comme la dernière fois !

Une fois que c’est fait, tu peux définir les composants de cette dernière ligne, à savoir un *ImageButton* pour couper le son et un *Slider* pour contrôler le volume du son de manière précise. Essaye par toi-même si tu veux avant de regarder le code qui suit !

```csharp
ImageButton MuteButton = new ImageButton
{
    HeightRequest = 25,
    WidthRequest = 25,
    Source = "volume_medium"
};

Slider VolumeTracker = new Slider
{
    Minimum = 0,
    MinimumTrackColor = Colors.Black,
    Maximum = 100,
    MaximumTrackColor = Colors.Gray,
    Value = 60
};
```


On rajoute ensuite les contrôles dans le *BottomLayout*:

```csharp
MuteButton.Row(2).Column(1),
VolumeTracker.Row(2).Column(2).ColumnSpan(3),
```


Et voilà ! 

<figure><p align="center"><img class="img-sizes" src="./images/B6353871FE88CE680890EE873B635A4D.png"></p></figure>

Ça devient plus facile avec tout cet entraînement, est-ce que tu commences à prendre le coup de main ? Bon, il n’y avait que deux composants à définir cette fois, alors on va aller un petit plus loin. Tu te rappelles des différentes images pour notre volume ? Il est temps de leur trouver une utilité !

L’idée c’est que notre *MuteButton* change d’apparence en fonction du niveau de volume demandé. On aura donc une référence directe au *VolumeTracker* afin de pouvoir trouver la bonne image à associer au *MuteButton*. Pour cela, on utilisera un [DataTrigger](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/triggers#data-triggers) qui permet de modifier la propriété d’un objet lorsqu’une valeur est détectée.

Prenons le cas le plus simple, celui où l’on détecte un volume sonore à 0:

```csharp
DataTrigger VolumeOffTrigger => new(typeof(ImageButton))
{
    Binding = new Binding(nameof(Slider.Value), source: VolumeTracker),
    Value = 0d,
    Setters = {
            new Setter { Property = ImageButton.SourceProperty, Value = "volume_off" }
        }
};
```


Dans un premier temps, on précise sur quel type d’objet appliquer la modification, en l’occurence un *ImageButton* qui caractérise *MuteButton*:

```csharp
DataTrigger VolumeOffTrigger => new(typeof(ImageButton))
```


Cette modification est d’ailleurs définie par un *Setter* pour changer la source d’image du *MuteButton* et appliquer l’icône correspondant au volume éteint:

```csharp
new Setter { Property = ImageButton.SourceProperty, Value = "volume_off" }
```


Quant au moment où la modification est déclenchée, il est prévu quand la valeur du *VolumeTracker* atteindra 0:

```csharp
 Value = 0d
```


Ceci est rendu possible par la technique du *Binding*, qui va créer un lien vers ce composant pour suivre l’évolution de la propriété *Value*:

```csharp
Binding = new Binding(nameof(Slider.Value), source: VolumeTracker)
```


Et le changement d’icône du bouton s’opérera une fois la valeur cible atteinte.


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Donc là on a un déclencheur pour modifier l’icône au moment où l’utilisateur abaissera la valeur du *Slider* à zéro. Mais pourquoi mettre un “d” après le “0” ?
{{< /admonition >}}

Bien vu, ce n’était pas une erreur typographique 😄

En fait, la [documentation du Slider](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/slider) précise que la propriété *Value* est de type [double](https://learn.microsoft.com/en-us/dotnet/api/system.double?view=net-6.0), pour représenter un nombre avec des décimales. Cela donne à l’utilisateur un peu plus de contrôle quand il manipule un *Slider*. Or, si tu tentes de supprimer le “d” et que tu passes ta souris au-dessus du “0” tu pourras constater qu’il n’est pas considéré comme un double, mais un [int](https://learn.microsoft.com/en-us/dotnet/api/system.int32?view=net-6.0) !

<figure><p align="center"><img class="img-sizes" src="./images/647B2233C389ED14831E420996C84B23.png"></p></figure>

Et comme la propriété *Value* de notre *DataTrigger* accepte potentiellement n’importe quel type de valeur (puisqu’il est de type *object*), on doit explicitement lui indiquer comment considérer ce “0”: comme un double !  Et pour cela, on doit rajouter le “d” juste après:

<figure><p align="center"><img class="img-sizes" src="./images/6563EBE2B8F0FD2DCE8BF8C037A56960.png"></p></figure>

Ne reste plus qu’à rajouter ce trigger à notre composant *MuteButton* en utilisant une méthode dédiée *InitMuteButton* et ce afin de garder notre code clair:

```csharp
public MusicPlayerView()
{
    ...

    InitMuteButton();

    Content = new Grid
    {
			...
    };
}

void InitMuteButton()
{
		MuteButton.Triggers.Add(VolumeOffTrigger);
}
```


Et voilà ! Essaye maintenant de glisser la valeur du *Slider* tout à gauche:

<figure><p align="center"><img class="img-sizes" src="./images/D29C869CE4D06ACBBDF56655AEC1C047.gif"></p></figure>


{{< admonition type=tip title="‎ " open=true >}}
Aller plus loin —> Comprendre pourquoi on a déclaré *MuteButton* avec un “=” au lieu de “=>” comme pour les autres composants
{{< /admonition >}}

Enfin pour gérer tous les différents états du bouton, nous aurons besoin que:

* l’icône du volume bas s’affiche pour une valeur comprise entre 1 et 15,
* l’icône du volume modéré s’affiche pour une valeur comprise entre 16 et 50,
* l’icône du volume élevé s’affiche pour une valeur comprise entre 51 et 100.
Pour cela, nous allons utiliser un nouveau déclencheur spécifique, le [MultiTrigger](/5db3d3ad31534fcf93a5022fcd2e381d). Celui-ci reprend le même principe que pour le *VolumeOffTrigger*, à la différence qu’au lieu de ne se référer qu’à une seule valeur cible, on définira des plages de valeur.

Déclarons d’abord nos 3 nouveaux états:

```csharp
MultiTrigger VolumeLowTrigger = new(typeof(ImageButton))
{
    Setters = {
            new Setter { Property = ImageButton.SourceProperty, Value = "volume_low" }
        }
};

MultiTrigger VolumeMediumTrigger = new(typeof(ImageButton))
{
    Setters = {
            new Setter { Property = ImageButton.SourceProperty, Value = "volume_medium" }
        }
};

MultiTrigger VolumeHighTrigger = new(typeof(ImageButton))
{
    Setters = {
            new Setter { Property = ImageButton.SourceProperty, Value = "volume_high" }
        }
};
```


Puis, dans la méthode *InitMuteButton*, nous allons définir les conditions de déclenchement pour ces 3 états:

```csharp
void InitMuteButton()
{
    BindingCondition CreateRangeCondition(OperatorType comparison, double value) => new BindingCondition
    {
        Binding = new Binding(
                    nameof(Slider.Value),
                    source: VolumeTracker,
                    converter: new CompareConverter
                    {
                        ComparisonOperator = comparison,
                        ComparingValue = value
                    }),
        Value = true
    };

    BindingCondition CreateMinRangeCondition(double value) => CreateRangeCondition(OperatorType.GreaterOrEqual, value);
    BindingCondition CreateMaxRangeCondition(double value) => CreateRangeCondition(OperatorType.SmallerOrEqual, value);

    VolumeLowTrigger.Conditions.Add(CreateMinRangeCondition(1d));
    VolumeLowTrigger.Conditions.Add(CreateMaxRangeCondition(15d));
    VolumeMediumTrigger.Conditions.Add(CreateMinRangeCondition(16d));
    VolumeMediumTrigger.Conditions.Add(CreateMaxRangeCondition(50d));
    VolumeHighTrigger.Conditions.Add(CreateMinRangeCondition(51d));
    VolumeHighTrigger.Conditions.Add(CreateMaxRangeCondition(100d));

    MuteButton.Triggers.Add(VolumeOffTrigger);
    MuteButton.Triggers.Add(VolumeLowTrigger);
    MuteButton.Triggers.Add(VolumeMediumTrigger);
    MuteButton.Triggers.Add(VolumeHighTrigger);
}
```


A ce stade, quelques erreurs auront probablement été mises en évidence par Visual Studio. Pour les résoudre, il te faudra déclarer les en-têtes suivantes tout en haut du fichier:

```csharp
using CommunityToolkit.Maui.Converters;
using static CommunityToolkit.Maui.Converters.CompareConverter<object>;
```


Cette étape est requise pour indiquer au compilateur à quoi correspondent les nouveaux types d’objet utilisés (*CompareConverter* et *Operator.Type*).

En décortiquant un peu le code, tu pourras voir qu’on définit d’abord une méthode locale de base *CreateRangeCondition* qui reprend le même principe que pour définir le premier *DataTrigger* utilisé pour le *VolumeOffTrigger*:

```csharp
BindingCondition CreateRangeCondition(OperatorType comparison, double value) => new BindingCondition
{
    Binding = new Binding(
                nameof(Slider.Value),
                source: VolumeTracker,
                converter: new CompareConverter
                {
                    ComparisonOperator = comparison,
                    ComparingValue = value
                }),
    Value = true
};
```


Cette méthode retourne une des deux *BindingCondition* nécessaires à la définition de nos plages de valeur. La première représente la valeur minimale de déclenchement de l’état, et la seconde la valeur maximale. On a d’ailleurs créé deux nouvelles méthodes locales pour cela:

```csharp
BindingCondition CreateMinRangeCondition(double value) => CreateRangeCondition(OperatorType.GreaterOrEqual, value);
BindingCondition CreateMaxRangeCondition(double value) => CreateRangeCondition(OperatorType.SmallerOrEqual, value);
```


On n’a alors plus qu’à définir les plages de valeurs pour nos différents états:

```csharp
VolumeLowTrigger.Conditions.Add(CreateMinRangeCondition(1d));
VolumeLowTrigger.Conditions.Add(CreateMaxRangeCondition(15d));
VolumeMediumTrigger.Conditions.Add(CreateMinRangeCondition(16d));
VolumeMediumTrigger.Conditions.Add(CreateMaxRangeCondition(50d));
VolumeHighTrigger.Conditions.Add(CreateMinRangeCondition(51d));
VolumeHighTrigger.Conditions.Add(CreateMaxRangeCondition(100d));
```


Avant de finalement pouvoir ajouter nos déclencheurs d’états au *MuteButton*:

```csharp
MuteButton.Triggers.Add(VolumeOffTrigger);
MuteButton.Triggers.Add(VolumeLowTrigger);
MuteButton.Triggers.Add(VolumeMediumTrigger);
MuteButton.Triggers.Add(VolumeHighTrigger);
```


Et voilà, vas-y réessaye !

<figure><p align="center"><img class="img-sizes" src="./images/69F6E9FE1F935AB3D276A2E35ED17A31.gif"></p></figure>

