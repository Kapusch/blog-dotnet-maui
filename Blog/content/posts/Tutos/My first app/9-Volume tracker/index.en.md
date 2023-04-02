---
Topic: "Volume tracker"
Title: "Volume display"
Category: "Tutos"
Subcategory: "My first app"
series: ["My first app"]
Index: "9"
PublishDate: "2023-03-20 00:00:09Z"
Language: "English"
Description: "We are almost done with the first big implementation phase of our main page. We'll now move on to the volume control!"
Tags: ["Image","ImageButton","DataTrigger","Slider","MultiTrigger",".NET Community Toolkit","C# markup"]
featuredImagePreview: 'featured-image-preview-en'
resources:
- name: 'featured-image-preview-en'
  src: 'featured-image-preview-en.jpeg'
draft: false
---

<!--more-->


{{< admonition type=info title="‎ " open=true >}}
To ease your read, please resume <a href="../7-time-tracker/">from this chapter</a> where we started setting up the music player controls.
{{< /admonition >}}

In the last chapter, we worked on setting up the media playback controls. We discovered in particular a new type of button: the [ImageButton](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/imagebutton) component.

Today we're going to further develop our user interface with the integration of volume controls. This is the final stretch before we start implementing the core of our music player, so hang on!

# The art of reproduction

Let's go back to the mock-up, here is what we will have to reproduce in the app:

<p align="center"><img max-width="100%" max-height="100%" src="./images/DF050A3B8CBC66BE41161183706F5D44.png" /></p>
<figure><figcaption class="image-caption">It should be quick this time, there are only two controls!</figcaption></figure>



As you can see, it's nothing more than a mute button and a volume control bar. It shouldn't take long, just start by downloading the different images used to display the volume status.

{{< link href="./files/Volume_Tracker_-_Images.zip" content="Volume_Tracker_-_Images.zip" title="Download Volume_Tracker_-_Images.zip" download="Volume_Tracker_-_Images.zip" card=true >}}




After you unzipped this file, you will see 4 new images:

<p align="center"><img max-width="100%" max-height="100%" src="./images/D513BCCE090CD9A8DB1344EB11150F81.png" /></p>
<figure></figure>



Here, we have an image for each volume level: when it is very loud, medium or very low. Moreover, there is even one for when the sound will be muted.

Now that you have the images, all you have to do is including them in the *Resources/Images* folder, just like last time!


{{< admonition type=info title="‎ " open=true >}}
In case of any doubts, you can refer <a href="../8-media-control/">to the previous chapter</a> .
{{< /admonition >}}

# On your marks, get set! Code!

All set? Let's go to the code!

For this, we need an *ImageButton* to mute the sound and a *Slider* to control the sound volume precisely.


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Ah, but I already know those ones!
{{< /admonition >}}



That's right! Technically, it's all déjà vu, so try to reproduce these controls yourself before looking at the following code:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
ImageButton MuteButton = new ImageButton
{
    HeightRequest = 25,
    WidthRequest = 25,
    Source = "volume_medium.png"
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




So we have defined an *ImageButton* with a default image, and a *Slider* to control the volume from 0% to 100%. As you may have noticed, the *Slider* is composed of a black bar representing the current volume, and a gray bar for the upper volume available.

Now, all you have to do is adding the controls to the *BottomLayout*:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
MuteButton.Row(2).Column(1),
VolumeTracker.Row(2).Column(2).ColumnSpan(3),
```




So, did you remember to apply a *ColumnSpan*? 😛 We need it to display the volume bar across three columns in our *Grid*.

Come on, it's time to relaunch the app! Let's see what it looks like:

<p align="center"><img max-width="100%" max-height="100%" src="./images/B6353871FE88CE680890EE873B635A4D.png" /></p>
<figure><figcaption class="image-caption">The way it looks now, it almost seems like the mobile app is complete…</figcaption></figure>

# Switching images when the sound is muted

It's getting easier with all this training, are you starting to get the hang of it? 🙂

Well, this time there were only two components to define, so we'll go a little further. Remember the different images we have for our volume? It's time make use for them!



The idea is that our `MuteButton` changes its appearance according to the requested volume level. We will therefore have a direct reference to the `VolumeTracker` in order to find the right image to associate with the `MuteButton`. To do this, we will use a [DataTrigger](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/triggers#data-triggers) which allows us to modify the property of an object when a target value is detected.

Let's take the simplest case where the detected volume is 0 :

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
DataTrigger VolumeOffTrigger => new DataTrigger(typeof(ImageButton))
{
	Binding = new Binding(nameof(Slider.Value), source: VolumeTracker),
	Value = 0d,
	Setters = {
		new Setter { Property = ImageButton.SourceProperty, Value = "volume_off.png" }
	}
};
```




First of all, we specify the type of object to which we want to apply the changes. In our case, it will be an *ImageButton* since we want to change the image of the `MuteButton`:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
DataTrigger VolumeOffTrigger => new DataTrigger(typeof(ImageButton))
```




Then, with the help of a *Setter*, we ask the `VolumeOffTrigger` to change the image source of the `MuteButton` with the corresponding icon for the volume being turned off:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
new Setter { Property = ImageButton.SourceProperty, Value = "volume_off.png" }
```




However, the change should only apply if the `VolumeTracker` value reaches 0!

This is possible with the *Binding* technique. So, we create a link to this component to monitor the *Value* property evolution:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
Binding = new Binding(nameof(Slider.Value), source: VolumeTracker)
```




Finally, the target value to be reached is defined in the `VolumeOffTrigger` as follows:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
 Value = 0d
```




In summary, we have a trigger that will change the icon the moment the user lowers the *Slider* value to zero.


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ But why put a “d” after the “0”?
{{< /admonition >}}



Hehe, good call! Indeed, it wasn't a typo 😄

In fact, the [Slider documentation](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/slider) states that the *Value* property is of type [double](https://learn.microsoft.com/en-us/dotnet/api/system.double?view=net-6.0). This gives the user a bit more control when manipulating a *Slider* (to the last comma!).

However, if you remove the “d” and move your mouse over the “0”, you will see that it is no longer considered a double, but an [int](https://learn.microsoft.com/en-us/dotnet/api/system.int32?view=net-6.0)!

<p align="center"><img max-width="100%" max-height="100%" src="./images/647B2233C389ED14831E420996C84B23.png" /></p>
<figure><figcaption class="image-caption">Visual Studio is very clear on this question, “0” is an integer!</figcaption></figure>



And as the *Value* property of our trigger is of type *object*, it potentially accepts any type of value. We must therefore explicitly tell it how to consider this “0”: as a double!

So the “d” must be added just after:

<p align="center"><img max-width="100%" max-height="100%" src="./images/6563EBE2B8F0FD2DCE8BF8C037A56960.png" /></p>
<figure><figcaption class="image-caption">Now “0” is a double! Visual Studio did not notice a thing.</figcaption></figure>



All that remains is attaching this trigger to our `MuteButton` component. Moreover, as it will be subject to additional configurations, we will isolate its initialization in an `InitMuteButton()` method. This will keep our code clear:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

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




That's it! Now try dragging the value of the *Slider* to the far left:

<p align="center"><img max-width="100%" max-height="100%" src="./images/86F584C6E28CF548239A7662F004E645.gif" /></p>
<figure><figcaption class="image-caption">The muted icon appears as soon as the cursor is moved to the far left.</figcaption></figure>



# A button in all its forms!

Now that you know how triggers work, let's create some more to handle all the different states of the button.

Functionally, here is what we would like to put in place:

* the low volume icon will appear for all values between 1 and 15,
* between 16 and 50, the moderate volume icon will be displayed,
* and for the high volume icon it will be between 51 and 100.


For all these cases, the trigger no longer depends on a single specific value, but rather on a whole range of values. We will therefore call upon a new specific trigger, the [MultiTrigger](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/triggers?view=net-maui-7.0#multi-triggers). It’s the same principle as for the *DataTrigger*, with the difference that the *MultiTrigger* will instead depend on the result of several conditions. I'll explain it right after.



For now, let's declare our three new possible states:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
MultiTrigger VolumeLowTrigger = new MultiTrigger(typeof(ImageButton))
{
	Setters = {
		new Setter { Property = ImageButton.SourceProperty, Value = "volume_low.png" }
	}
};

MultiTrigger VolumeMediumTrigger = new MultiTrigger(typeof(ImageButton))
{
	Setters = {
		new Setter { Property = ImageButton.SourceProperty, Value = "volume_medium.png" }
	}
};

MultiTrigger VolumeHighTrigger = new MultiTrigger(typeof(ImageButton))
{
	Setters = {
		new Setter { Property = ImageButton.SourceProperty, Value = "volume_high.png" }
	}
	};
```




All we did was to define the aspect changes using *Setters*, for low, medium or high volume.

We can now define the trigger conditions for these three states. Modify the `InitMuteButton()` method as shown below:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

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




Yes I know, it can be a lot at once, but it's not that hard to understand. Actually, you can see that some operations are repeated in this piece of code!


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Yeah, not even afraid! What should I do next?
{{< /admonition >}}



At this stage, some errors will probably have been pointed out by Visual Studio. To resolve them, declare the following headers at the very top of the file:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
using CommunityToolkit.Maui.Converters;
using static CommunityToolkit.Maui.Converters.CompareConverter<object>;
```





{{< admonition type=info title="‎ " open=true >}}
This step is required for the compiler to understand what these new objects are: *CompareConverter* and *Operator.Type*.
{{< /admonition >}}



Now it's time for an explanation. Let's break down a bit this `InitMuteButton()` method, starting with this piece of code:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
VolumeLowTrigger.Conditions.Add(CreateMinRangeCondition(1d));
VolumeLowTrigger.Conditions.Add(CreateMaxRangeCondition(15d));
VolumeMediumTrigger.Conditions.Add(CreateMinRangeCondition(16d));
VolumeMediumTrigger.Conditions.Add(CreateMaxRangeCondition(50d));
VolumeHighTrigger.Conditions.Add(CreateMinRangeCondition(51d));
VolumeHighTrigger.Conditions.Add(CreateMaxRangeCondition(100d));
```




It seems simpler that way, right? All we are doing here is to add two conditions for each of the triggers that are necessary to change the `MuteButton` icon.

For example, if you look at the `VolumeLowTrigger`, you'll see that the first trigger condition is tied to a minimum value of 1, while the other condition depends on a maximum value of 15. Does this sound familiar now?


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ For sure, yes! It's so that the low volume icon appears as soon as the volume is between 1 and 15!
{{< /admonition >}}



Absolutely! And the same logic applies for the `VolumeMediumTrigger` and `VolumeHighTrigger`. 🙂

Okay, but that's not magic either! The creation of these conditions is based on the `CreateMinRangeCondition(double value)` and `CreateMaxRangeCondition(double value)` methods:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
BindingCondition CreateMinRangeCondition(double value) => CreateRangeCondition(OperatorType.GreaterOrEqual, value);
BindingCondition CreateMaxRangeCondition(double value) => CreateRangeCondition(OperatorType.SmallerOrEqual, value);
```




The first method represents the minimum value for triggering the new condition, and the second the maximum value. To create these conditions, we need a target value, and a type of operator: `GreaterOrEqual` or `SmallerOrEqual`.

These parameters are taken into account by a basic method defined at the beginning of the `InitMuteButton()`. It follows the same principle as for the *DataTrigger* which is used for the `VolumeOffTrigger`:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

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




The only change here is that a numerical target value is no longer desired (as we did for “0”). Instead, it is based on the result of a comparison.

The purpose of the `CreateRangeCondition(OperatorType comparison, double value)` method is to create a trigger condition based on a standard value and a comparison type. And if you look closely, you'll see that it defines a *Binding* on the VolumeTracker value while applying a [CompareConverter](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/converters/compare-converter) to it.

The idea is simple, we want to define conditions so that they are only met if the result of the comparison between the standard value and the detected value is true.


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Uh... But how does it look with a concrete example? 🙊
{{< /admonition >}}



It’s coming, don't worry! Suppose we have created a lambda condition, and that this condition is only met when the detected value is greater than or equal to 80.

If the current volume is 50, would you agree that it won't matter? Well, now imagine that you raise the volume to 88... Boom, that's it! Your condition is now satisfied and that will logically trigger something 🙂



You're almost there! All that's left is to add our three new triggers to the `MuteButton`. And I know you can do that! So let’s proceed as follows:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
MuteButton.Triggers.Add(VolumeOffTrigger);
MuteButton.Triggers.Add(VolumeLowTrigger);
MuteButton.Triggers.Add(VolumeMediumTrigger);
MuteButton.Triggers.Add(VolumeHighTrigger);
```




That's it! Go ahead and try the app again!

<p align="center"><img max-width="100%" max-height="100%" src="./images/349ADF33416FB9302753C8D26C368B30.gif" /></p>
<figure><figcaption class="image-caption">It’s magic! The image of the button live changes according to the position of the cursor.</figcaption></figure>



The mobile app is seriously starting to take shape, be proud of yourself!

Except... all we have at the moment is visual. Nothing really happens when you manipulate all the buttons!

But don't worry, you're now ready to develop the key features of the app, starting with the next chapter!

---
More articles in the series:
{{< series "My first app" >}}
