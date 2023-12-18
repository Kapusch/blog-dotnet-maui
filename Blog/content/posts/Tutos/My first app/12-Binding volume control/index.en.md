---
Topic: "Binding volume control"
Title: "Control the sound volume"
Category: "Tutos"
Subcategory: "My first app"
series: ["My first app"]
Index: "12"
PublishDate: "2023-12-10 00:00:12Z"
Language: "English"
Description: "Now we are going to implement volume control! As with any good music player, we want our user to be able to turn the volume up or down as desired."
Tags: ["MVVM","Data Binding","MediaElement","Slider"]
featuredImagePreview: 'featured-image-preview-en'
resources:
- name: 'featured-image-preview-en'
  src: 'featured-image-preview-en.png'
draft: false
---

<!--more-->


{{< admonition type=info title="‎ " open=true >}}
To ease your read, please resume <a href="../10-play-music/">from this chapter</a> where we have set up the *MediaElement*.
{{< /admonition >}}
In the previous post, we saw how to move the playhead with our own *Slider* control. And I know it was a bit long! Hang on, we are not far from the end of this series 🙂

So today we are going to look at how to implement volume control, also with the help of a *Slider*. Yes, as with any good music player, we want our user to be able to turn the volume up or down as desired.

# Set the volume level
If you remember, in the <a href="../9-volume-tracker/">chapter on volume display</a>, we introduced 2 components:

* the `MuteButton`, which is a *ImageButton* control for muting the sound,

* and the *Slider* `VolumeTracker`, for precise volume control.

As usual, let's now make them usable with the help of **Data Binding**, and all this in an initialization method named *InitVolumeTracker()* :

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
// Every piece of code has always its own region!
#region Volume Tracker
    ...
    Slider VolumeTracker = new Slider
    {
        Minimum = 0,
        MinimumTrackColor = Colors.Black,
        Maximum = 100,
        MaximumTrackColor = Colors.Gray,
        // We don't need dummy data anymore, you can remove this line of code
        // Value = 60
    };

    void InitVolumeTracker() // And here is the new initialization method
    {
        VolumeTracker.Bind(
            Slider.ValueProperty,
            nameof(MusicPlayer.Volume),
            source: MusicPlayer,
            convert: (double mediaElementVolume) => mediaElementVolume * 100,
            convertBack: (double sliderValue) => sliderValue / 100);
    }
#endregion
```
No problems so far? In the end, it's very similar to what we have set up for the playhead control!


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Oh yes! But uh... is *convertBack* new?
{{< /admonition >}}
Hey, you miss nothing!

Concretely, we have associated the position of the cursor on the *Slider* (`VolumeTracker.Value`) with the volume level that is exposed by the *MediaElement* (`MusicPlayer.Volume`). And if you have read the [Slider documentation](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/slider?view=net-maui-8.0) carefully, it says that the association of the *Slider*'s `Value` property is bidirectional (`BindingMode.TwoWay`), meaning that:

* Any change in value from the source component (here, the `MusicPlayer`) will have an impact on the target component (the `VolumeTracker`),

* And vice versa, any new value from the target component (`VolumeTracker`) will have an impact on the source component (`MusicPlayer`)!

In other words, if the *MediaElement*'s volume went down to 0, then the *Slider*'s cursor would slide all the way to the left, and if the user moved the cursor all the way to the right, then the *MediaElement*'s volume would be set to 1.


{{< admonition type=info title="‎ " open=true >}}
The *convert* property acts in the "source → target" direction, while the *convertBack* property acts in the opposite direction, "target → source".
{{< /admonition >}}
However, the [MediaElement documentation](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/views/mediaelement#properties) specifies that the `Volume` property only accepts *double* values between 0 and 1.

This is why, in the *convert*, we need to multiply the `MusicPlayer.Volume` value by 100 to define the cursor position on the *Slider* (`VolumeTracker.Value`). In reverse, while *convertBack*, we must divide by 100 the value chosen by the user via the *Slider* to correctly modify the value of the *MediaElement*.

Finally, all you have to do is calling the *InitVolumeTracker()* initialization method from the page constructor:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
...
namespace NightClub.Views;
public class MusicPlayerView : ContentPage
{
    public MusicPlayerView()
    {
        ...
        InitVolumeTracker(); // That's it, we are good to go!
        ...
    }
    ...
}
```
Now relaunch the project and check that you can modify the volume!


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Already!? Heeey, what about the `MuteButton`?
{{< /admonition >}}
Sometimes all it takes is a little piece of code to unleash new features! Take the opportunity to check that it's working properly, and we will meet up again right afterwards for the rest!

# Mute audio
For the `MuteButton`, it's even simpler! It's already embedded in the *MediaElement* with the *boolean* `ShouldMute` property. So all we have to do is detect the user's click on the button and modify its value.

For this, let's define first a new event called *MuteButton_Clicked()*…

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
#region Events
    ...
    void MuteButton_Clicked(object sender, EventArgs e)
    {
        MusicPlayer.ShouldMute = !MusicPlayer.ShouldMute;
    }
#endregion
```
... which will be initialized from our existing method, in the *InitMuteButton()*:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
#region Mute Button Visual States
    ...
    void InitMuteButton()
    {
        ...
        MuteButton.Clicked += MuteButton_Clicked;
    }
#endregion
```
This event is pretty simple, isn't it? We simply invert the value of the `MusicPlayer.ShouldMute` property to alternately mute or unmute the sound.

But we are not done yet! Because if you ever wanted to try it, here is what it looks like for now:

<p align="center"><img max-width="100%" max-height="100%" src="./images/90E63FF9DAAB6EE5608B15F4876B682F.gif" /></p>
<figure><figcaption class="image-caption">The button works well! But there is something visually incorrect.</figcaption></figure>

The audio is muted when the `MuteButton` is clicked once, and reactivated the next time it is clicked. Not bad at all!

However, something is visually disturbing. It gives a strange effect when the sound is muted:

* The `MuteButton` icon should have changed to represent the audio as muted,

* And the `VolumeTracker` cursor should have slid all the way to the left, for the same reason.

Actually, these two controls would have needed to adapt to the volume level... Do you have any ideas? 😊


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Well, we could just change the volume level ourselves!
{{< /admonition >}}
That's right! But have you thought about what happens when the user reactivates the sound?

Ideally, the volume should be reset to the level it was before being muted. So we are going to record the volume level when the user clicks on the `MuteButton`!

To do this, we need another variable:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
#region Properties

    bool mustResumePlayback;
    double savedVolumeBeforeGoingMute; // Another variable of double type

#endregion
```
The `savedVolumeBeforeGoingMute` variable must be of *double* type to match the value contained in the *MediaElement*'s `Volume` property.

And now, you only have to modify the *MuteButton_Clicked()* event like this:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
#region Events
    ...
    void MuteButton_Clicked(object sender, EventArgs e)
    {
        if (!MusicPlayer.ShouldMute)
        {
            // We must save the current volume before we mute...
            savedVolumeBeforeGoingMute = MusicPlayer.Volume;
            MusicPlayer.Volume = 0;
        }
        else
        {
            // ... and we set it back when we unmute!
            MusicPlayer.Volume = savedVolumeBeforeGoingMute;
        }
				
        // Obviously here, nothing changes!
        MusicPlayer.ShouldMute = !MusicPlayer.ShouldMute;
    }
    ...
#endregion
```
Now, when we detect that the sound is about to be turned off, we store the volume value in the `savedVolumeBeforeGoingMute` variable, and then change ourselves the value of the `MusicPlayer.Volume` property to 0.

By the way, I wondered why it wasn't already integrated into the [.NET MAUI Community Toolkit](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/) library. So I asked the feature's author directly on [Github](https://github.com/CommunityToolkit/Maui/discussions/950#discussioncomment-7245223)! That's why Open Source is fabulous.


{{< admonition type=tip title="‎ " open=true >}}
By reading the answer to my question, you may have realized that there is another way of implementing audio deactivation. Share your experience with me in the comments!
{{< /admonition >}}
So let’s see now what does it look like with these recent changes:

<p align="center"><img max-width="100%" max-height="100%" src="./images/6C0BFDD6BF2BF8B1FB33D588D55EEA6E.gif" /></p>
<figure><figcaption class="image-caption">Well now it is clearer when you mute the sound!</figcaption></figure>

Great! But we need to cover every conceivable use case... And that’s what we are going to look at in the next section.

# A few enhancements
We are going to apply a few improvements, as there are still two situations where the behavior of our functionality is problematic.

For example, in the first case, the volume cursor remains locked at the far left:

<p align="center"><img max-width="100%" max-height="100%" src="./images/EFF7E071F18949EAEEF026775B1C56B1.gif" /></p>
<figure><figcaption class="image-caption">No matter how many times you try to unmute, nothing happens!</figcaption></figure>

This is completely normal! Although the volume has been reduced to 0, the variable `MusicPlayer.ShouldMute` has not been set to true after all. Hence technically, the user is not reactivating the sound!


{{< admonition type=tip title="‎ " open=true >}}
Try putting some debugging points in the *MuteButton_Clicked()* method!
{{< /admonition >}}
In the second unexpected case, the sound simply cannot be reactivated:

<p align="center"><img max-width="100%" max-height="100%" src="./images/A8DF0BDE074472BBB305AD3314187DC5.gif" /></p>
<figure><figcaption class="image-caption">After you have muted the sound, you cannot unmute by turning up the volume!</figcaption></figure>

But then again, there is nothing strange about that. Actually, the root cause is the same as in the previous case! Even though the volume was increased, the variable `MusicPlayer.ShouldMute` was never set to false. So, once again, the user is not reactivating the sound!


{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ But then, everything we did implement is bad?
{{< /admonition >}}
No, not at all! Of course, there are dozens of other ways to implement all this (and you are free to give a try!), but in our case, there is only one small remaining part to code.

So, what do you think is missing? Here is a hint: it all starts with the change in volume value.

And that is only possible when the cursor is moved over the *Slider*... So let’s optimize the current behavior with a new event called *VolumeTracker_DragCompleted()*:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
#region Events
	...
	void VolumeTracker_DragCompleted(object sender, EventArgs e)
	{
	// WARN: always update the control that is source of the event
	// through the sender object to not introduce any conflict updates
		if (sender is Slider volumeTrackerControl)
		{
			if (volumeTrackerControl.Value == 0)
			{
				// To improve the user experience, we must always turn
				// back the volume to a positive level when he unmutes
				savedVolumeBeforeGoingMute = 0.2;
				MusicPlayer.ShouldMute = true;
			}
			else if(MusicPlayer.ShouldMute)
			{
				// User can unmute after having moved the cursor
				// to a positive value
				MusicPlayer.ShouldMute = false;
			}
		}
	}
#endregion
```
And of course, don't forget to initialize this event from the right method …

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>MusicPlayerView.cs</code></p>

```csharp
#region Volume Tracker
	...
	void InitVolumeTracker()
	{
		VolumeTracker.DragCompleted += VolumeTracker_DragCompleted;
		...
	}
#endregion
```
Relaunch the project, check that everything is working, and above all, enjoy the results of your efforts!

I hope you have learned a lot of new things today. In general, take inspiration from what you observe to experiment with new things. And as you go a little further each time, you will get more and more comfortable!

See you soon for a new chapter! 🙂

___
More articles in the series:
{{< series "My first app" >}}
