---
Topic: "Setup the project"
Title: "Installer le projet"
Category: "Tutos"
Subcategory: "My first app"
Index: "2"
PublishDate: "2023-01-01 00:00:02Z"
Language: "French"
Description: "Aujourd'hui on attaque la création de notre première application ! Juste le temps d'installer ton environnement de travail et on aura rapidement quelque chose de concret. Allez c'est parti !"
Tags: ["Visual Studio","Setup","New Project"]
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
Salut les Dev•e•s ! 🤓



Tu vas bientôt pouvoir créer ta première application mobile ! Juste le temps d'installer ton environnement de travail et on aura rapidement quelque chose de concret. Allez c'est parti !


{{< callout emoji="🐒" text="Mais au fait, il faut être sous Windows, MacOS ou Linux ?" >}}




En fait, il n’y a pas de pré-requis particulier pour développer une app avec .NET MAUI, à moins que tu ne traînes un ordinateur de l’avant-guerre et que tu ne mettes rien à jour… 🤔 Sinon, c’est surtout une question de préférence ! Personnellement, je développe sous MacOS car c’est un système que j’affectionne, et le simulateur d’iPhone qui y est intégré est très performant et me permet d’avoir rapidement un rendu de ce que je programme.

Mais tu peux tout aussi bien développer sous Windows ou Linux ! Plus concrètement :

* sous Windows ou MacOS, c’est facile, Microsoft propose un environnement de développement intégré très performant:
    * [Visual Studio](https://visualstudio.microsoft.com/vs/) pour Windows,
    * et [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/) pour… eh bien pour MacOS 😄
* et si tu es sous Linux, il te faudra recourir à un peu plus d’ingéniosité (et c’est bien pour cela que tu es sous Linux, n’est-ce pas ? 😛). Dans ce cas-là, je te conseille d’utiliser l’excellent éditeur de code source [Visual Studio Code](https://code.visualstudio.com).



{{< callout emoji="ℹ️" text="Dans le cadre de ce blog, je me concentrerais uniquement sur le développement d’application mobile à l’aide de Visual Studio. En effet, son utilisation est bien plus intuitive et c’est justement cela qui plaît à tout vrai néophyte." >}}




Passons maintenant à l’installation de l’environnement de travail:

1. Dans un premier temps, télécharge Visual Studio depuis le site officiel de Microsoft, en choisissant la version appropriée à ton système ([Windows](https://visualstudio.microsoft.com/vs/) ou [MacOS](https://visualstudio.microsoft.com/vs/mac/)),
1. Puis vient le moment d’installer Visual Studio et son environnement de développement .NET MAUI. Pour améliorer la lecture de ce blog, je te propose de directement suivre les étapes d’installation sur le site officiel:
    1. [suivre les étapes d’installation sous Windows](https://learn.microsoft.com/fr-fr/dotnet/maui/get-started/installation?view=net-maui-7.0&tabs=vswin#installation-1)
    1. [suivre les étapes d’installation sous MacOS](https://learn.microsoft.com/fr-fr/dotnet/maui/get-started/installation?view=net-maui-7.0&tabs=vsmac#installation-2)
1. Enfin, télécharge le projet depuis GitHub. Pour cela, rends-toi sur [le dépôt de code du blog](https://github.com/Kapusch/blog-dotnet-maui), clique sur le bouton “Code” et télécharge le tout au format .ZIP
<p align="center"><img class="img-sizes" src="./images/7B86AE88DAB3362A9B47A4007B949027.png"></p>




{{< callout emoji="ℹ️" text="Avant d’aller plus loin, si tu es sous MacOS, il te faudra t’assurer d’avoir [téléchargé la dernière version d’Xcode](https://developer.apple.com/xcode) qui est requise pour le simulateur d’iPhone. Son installation peut être très longue, aussi, je te conseille de le faire en parallèle de ta lecture." >}}




Une fois le fichier téléchargé décompressé, rends-toi dans le dossier des exemples associés à notre cours (*Samples/NightClub*). Les dossiers qui y sont entreposés correspondent chacun à une partie différente du cours:

<p align="center"><img class="img-sizes" src="./images/AB6D4CEADA0C31BD182CB28EA1C158C1.png"></p>

Pour le moment, ouvre le premier dossier (*0 - Get Started)* et double-clique sur `NightClub.sln` pour ouvrir le projet NightClub dans Visual Studio.

<p align="center"><img class="img-sizes" src="./images/1D187B2CC26417B658FD450BB0D7B3B3.png"></p>




{{< callout emoji="🐒" text="Ok ! J’ouvre le projet, ça charge… Mais il y a déjà plein de choses dans ce projet, on ne peut pas partir de zéro ?" >}}




En fait, c’est déjà le cas ! Le projet que tu as sous les yeux est tout neuf, mais il embarque plusieurs fichiers de base nécessaires au bon fonctionnement d’une application .NET MAUI:

* Toutes les librairies nécessaires au bon fonctionnement du projet sont regroupées dans le dossier **Dependencies**,
* Dans **Platforms**, tu retrouveras tous les fichiers nécessaires à l’exécution de l’application, et ce pour chaque plateforme cible,
* Quant au dossier **Properties**, on y retrouve généralement des fichiers de configuration (il y en a d’ailleurs une créée par défaut pour Windows),
* Et puis, tu auras la possibilité pour chaque application de lui configurer une icône et un écran de chargement ! Pour cela, on recoure généralement au dossier **Resources** pour stocker tous nos médias (icônes, images, pistes audio et vidéo, …),
* Enfin, il faudra bien pouvoir afficher quelque chose à l’ouverture de l’app ! Et pour ça, on a le dossier **Views** qui contient pour le moment notre première page, la fameuse page d’accueil…


Et voilà c’est tout, et si on <a href="../3-first-run-of-the-project/">lançait cette app</a> ? 🙂


{{< callout emoji="💡" text="Aller plus loin avec la [structure de base des projets Visual Studio](https://learn.microsoft.com/fr-fr/dotnet/maui/fundamentals/single-project?view=net-maui-7.0)" >}}



