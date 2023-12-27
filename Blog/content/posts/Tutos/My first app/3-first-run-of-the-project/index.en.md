---
Topic: "first-run-of-the-project"
Title: "First run of the app .NET MAUI"
Category: "Tutos"
Subcategory: "My first app"
series: ["My first app"]
Index: "3"
PublishDate: "2023-01-02 00:00:03Z"
Language: "English"
Description: "It's now time to get the app running.
We'll also take the opportunity to look at the source code 🙂"
Tags: ["Visual Studio","C# markup"]
featuredImagePreview: 'featured-image-preview-en'
resources:
- name: 'featured-image-preview-en'
  src: 'featured-image-preview-en.png'
draft: false
---

<!--more-->


{{< admonition type=info title="‎ " open=true >}}
To ease your read, please start from the project containing all the different steps applied so far in this tutorial. To do so, refer to the <a href="../2-setup-the-project/">project setup guide</a> and resume from the sample located in the folder *"0 - Get Started"*.
{{< /admonition >}}


It's now time to get the app running. We'll also take the opportunity to look at the source code 🙂

But for our application to work, we still have two points to address: the app initialization and above all, managing its execution state.

## App initialization
For each targeted platform (iOS, Android, ...), a .NET MAUI application is initialized from a single entry point via the method `CreateMauiApp()` which is located in the file [MauiProgram.cs](https://github.com/Kapusch/blog-dotnet-maui/blob/main/Samples/NightClub/0%20-%20Get%20Started/NightClub/MauiProgram.cs). This is where you configure everything the app needs to work the way you want it to: web services, external dependencies, or specific fonts.

## App state management
But we also need something to define the current state of our application (which page to display, what if the application stops, ...). And for that, we have an `Application` class declared in the file [App.cs](https://github.com/Kapusch/blog-dotnet-maui/blob/main/Samples/NightClub/0%20-%20Get%20Started/NightClub/App.cs).

## Executing the app
Come on, it's time to get the application running! So choose the platform you want to target, then click on the red boxed button to start the project:

<p align="center"><img max-width="100%" max-height="100%" src="./images/5BCE13CE6AA3C962B851589A1333882C.png" /></p>
<figure></figure>



First, you will see your project compiling with the chosen configuration (this is the build stage):

<p align="center"><img max-width="100%" max-height="100%" src="./images/C02BA696FE457042E46B611AC7B6653C.png" /></p>
<figure><figcaption class="image-caption">The current status of the project can easily be viewed at the bottom left of Visual Studio for Mac.</figcaption></figure>



Then your application will automatically be deployed to the chosen platform (here, a simulated iPhone 13, iOS 15.5).

<p align="center"><img max-width="100%" max-height="100%" src="./images/145B5C24E2EF3848C0ECF5417F91416D.png" /></p>
<figure></figure>



And here's the result!

<p align="center"><img max-width="100%" max-height="100%" src="./images/4DFB7678FB4CCC0E102C886277A46E7B.png" /></p>
<figure></figure>




{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Heeeey, but nothing happens when I click on the button!
{{< /admonition >}}
This is just the beginning of our app, the biggest part is ahead !

## The home page
The screen you just saw is the home page for which the content is specified in the file `HomeView.cs`. It’s a [ContentPage](https://docs.microsoft.com/en-us/dotnet/maui/user-interface/pages/contentpage) (basically a page with content) which is the most common type of page.



And as you can see, what it is composed about is very simple:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>HomeView.cs</code></p>

```csharp
public HomeView()
{
    BackgroundColor = Colors.Black;

    Content = new VerticalStackLayout
    {
        Children =
        {
            NightClubImage,
            EnterButton
        }
    }.CenterVertical();
}
```


It’s a black background page whose content is defined by an image and a button that are stacked vertically in a [VerticalStackLayout](https://docs.microsoft.com/en-us/dotnet/maui/user-interface/layouts/verticalstacklayout).

And if you want to know how these two components are defined, just look a little further down in the code on the page:

<p align="center" style="margin-bottom:-10px"><strong>Filename:</strong><code>HomeView.cs</code></p>

```csharp
#region Controls

Image NightClubImage => new Image
{
    // .NET MAUI converts SVG files to PNG files.
    Source = "night_club.png"
};

Button EnterButton => new Button
{
    Text = "ENTER",
    TextColor = Colors.White,
    BackgroundColor = Colors.Magenta,
    CornerRadius = 10
} .Bold() .Paddings(50, 2, 50, 2) .CenterHorizontal();

#endregion
```


Nothing very complex at this stage:

* We set the image of the component `NightClubImage` with the “Source” property,

* And for the button, we set several base properties such as the text, the color, … but you might have noticed that we also apply some functions to the component `EnterButton`:

    * *Bold()* to set the text of the button in bold,

    * *Paddings()* to add a bit of space all around this text, especially to the left and to the right of the text,

    * And then, *CenterHorizontal()* to align the text at the center of the button.




{{< admonition type=info title="‎ " open=true >}}
These methods follow a particular programmation technique named "Declarative UI" or "Fluent UI". Finally, this page is coded in C#, as for the rest of the app!
{{< /admonition >}}
___

{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Alright! And shouldn’t we declare an action to this button?
{{< /admonition >}}


Correct! And we’ll see how to trigger code execution by clicking on this button, but not in any way. Indeed, we will apply this wonderful design pattern called <a href="../4-mvvm-for-successful-apps/">Model-View-ViewModel (MVVM)</a>.

___
More articles in the series:
{{< series "My first app" >}}
