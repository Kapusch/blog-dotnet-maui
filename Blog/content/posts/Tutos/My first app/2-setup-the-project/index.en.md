---
PostURL: "setup-the-project"
Title: "Setup the project .NET MAUI"
Category: "Tutos"
Subcategory: "My first app"
series: ["My first app"]
Index: "2"
PublishDate: "2023-01-02 00:00:02Z"
Language: "English"
Description: "Today we'll start building our first application! Let’s first set up your working environment, and then you’ll have something concrete soon. Let's get started!"
Tags: ["Visual Studio","Setup","New Project"]
featuredImagePreview: 'featured-image-preview-en'
resources:
- name: 'featured-image-preview-en'
  src: 'featured-image-preview-en.png'
draft: false
---

<!--more-->

Today we'll start building our first application! Let’s first set up your working environment, and then you’ll have something concrete soon. Let's get started!

## Which tools for development ?
Let’s first see the tools you will need to develop your app.




{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ By the way, do I need to be on Windows, MacOS or Linux?
{{< /admonition >}}


Actually, there is no particular pre-requisite to develop an app with .NET MAUI, unless you own a very old fashion computer and you never did update anything… 🤔 Otherwise, it's mostly a matter of preference! Personally, I develop on macOS because it's an operating system I like, and the built in iPhone simulator is very powerful and quickly gives me a realistic render of what I'm programming.

But you can also develop under Windows or Linux! More concretely:

* for Windows or MacOS, it's easy, Microsoft offers a very powerful integrated development environment:

    * [Visual Studio](https://visualstudio.microsoft.com/vs/) for Windows,

    * and [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/) for… well for MacOS 😄

* and if you're on Linux, you'll have to be a bit more resourceful (and that's why you're on Linux, isn't it? 😛). In that case, I suggest you use the excellent source code editor [Visual Studio Code](https://code.visualstudio.com/).




{{< admonition type=info title="‎ " open=true >}}
For the purposes of this blog, I will only focus on mobile application development using Visual Studio. It is much more intuitive to use and that is what appeals to any true novice.
{{< /admonition >}}
## Install your environment
Let's move on to the installation of the working environment:

1. First, download Visual Studio from the official Microsoft website, choosing the appropriate version for your operating system ([Windows](https://visualstudio.microsoft.com/vs/) or [MacOS](https://visualstudio.microsoft.com/vs/mac/)),

1. Then comes the time to install Visual Studio and its development environment for .NET MAUI. In order to improve the reading of this blog, I suggest you directly follow the installation steps on the official website:

    1. [follow the installation steps for Windows](https://learn.microsoft.com/en-us/dotnet/maui/get-started/installation?view=net-maui-8.0&tabs=vswin#installation-1)

    1. [follow the installation steps for MacOS](https://learn.microsoft.com/en-us/dotnet/maui/get-started/installation?view=net-maui-8.0&tabs=vsmac#installation-2) (since release of .NET 8 SDK, there might be one more step detailed [here](https://developercommunity.visualstudio.com/t/Net-8-not-working-on-VS-for-mac/10516623#T-N10517725))

1. Finally, download my class from GitHub. To do this, go to the [blog's code repository](https://github.com/Kapusch/blog-dotnet-maui), click on the "Code" button and download the ZIP version.

<p align="center"><img max-width="100%" max-height="100%" src="./images/52D99A38E16814B6261CC1610BCE2A9F.png" /></p>
<figure></figure>




{{< admonition type=info title="‎ " open=true >}}
Before you go further, if you are on MacOS, make sure you have [downloaded the latest version of Xcode](https://developer.apple.com/xcode/) which is required for the iPhone simulator. This is usually long to install, so I suggest you do it while reading.
{{< /admonition >}}
## Opening the project
Once you have unzipped the downloaded file, go to the Samples folder associated with our course (*Samples/NightClub*). The folders under it each correspond to a different part of the course:

<p align="center"><img max-width="100%" max-height="100%" src="./images/B33D3B1E775ECE6D8A9EC8F8A6E1F4B5.png" /></p>
<figure></figure>

For now, open the first folder (*0 - Get Started)* and double click on `NightClub.sln` to open the NightClub project in Visual Studio.

<p align="center"><img max-width="100%" max-height="100%" src="./images/D9933119C293F4DC96A394EB54E5C5C9.png" /></p>
<figure><figcaption class="image-caption">The NightClub project structure from Visual Studio.</figcaption></figure>




{{< admonition type=comment title="‎ " open=true >}}
🐒‎ ‎ Ok! Opening the project… loading... But there are already many things in this project, can't we start from scratch?
{{< /admonition >}}


Actually, it already is! The project you just opened is brand new, but it includes several base files that are necessary for the proper functioning of a .NET MAUI application:

* All the libraries that are required for the proper functioning of the project are grouped in the **Dependencies** folder,

* In **Platforms**, you will find all the files needed to run the application, for each target platform,

* As for the **Properties** folder, it generally contains configuration files (and besides, there is one created by default for Windows),

* And then, you will be able to configure an icon and a loading screen for each application! For this, we usually use the **Resources** folder to store all our media (icons, images, audio and video tracks, ...),

* Finally, we have to display something when the app opens! And for that, we have the **Views** folder which at the moment contains our first page, the famous home page…




{{< admonition type=tip title="‎ " open=true >}}
Going further with the [basic structure of a Visual Studio project](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/single-project)
{{< /admonition >}}
And that's it, how about if we <a href="../3-first-run-of-the-project/">launch that app</a> ? 🙂




___
More articles in the series:
{{< series "My first app" >}}
