# Audit et direction de refonte

Audit réalisé le 12 août 2026 à partir du site public, de ce dépôt, de l’ancien exporteur Notion et de la fondation récente de BloomFlyWebsite.

## Ce que l’existant nous apprend

Le site publié est un Hugo 0.122 avec le thème FixIt 0.2.18. Le dépôt contient 17 articles traduits en français et en anglais, soit 34 fichiers localisés, une série de 15 chapitres et deux astuces. Ce corpus représente près de 10 000 lignes, 230 blocs de code, 232 admonitions et environ 28 Mo de médias.

Les forces à préserver sont le niveau de détail des tutoriels, les paires de traduction, la continuité de la série, les fichiers téléchargeables et les nombreuses illustrations. La lecture d’un article sur grand écran est déjà convenable grâce au sommaire, aux callouts et aux blocs de code.

Les faiblesses structurantes sont les suivantes :

- le rendu dépend de shortcodes et de conventions propres au thème ;
- l’exporteur Notion produit du HTML peu accessible, notamment des images sans texte alternatif, et demande des manipulations manuelles ;
- le mobile présente un débordement horizontal qui tronque navigation, cartes, titres et contenu ;
- le `robots.txt` publié bloque les images, y compris pour Googlebot-Image ;
- les métadonnées sociales sont incomplètes et certains chemins d’images sont invalides ;
- Google Analytics est chargé immédiatement malgré la bannière de consentement ;
- la publication ne se déclenche pas sur un changement de contenu, uniquement chaque lundi ou manuellement ;
- le thème est un sous-module non initialisé par défaut et le Hugo local ne correspond pas à la version CI.

L’ancien exporteur Notion est une application .NET 6 très couplée à FixIt. Il ne couvre qu’une partie des blocs Notion, transmet le jeton en argument de ligne de commande, bloque en fin d’exécution et n’a ni test ni CI. Le prolonger déplacerait la dette au lieu de simplifier l’écriture.

## Direction retenue

La nouvelle fondation est un site statique Astro 7 sur mesure, sans framework client ni thème externe. Astro garde le résultat léger tout en donnant des contrats de contenu typés et des composants MDX adaptés aux tutoriels. Expressive Code prend en charge les fichiers, lignes importantes, copie et repli de longues portions. Pagefind construit après compilation un index local multilingue sans service tiers.

La stratégie de migration est additive : `Website/` vit à côté de `Blog/`, le déploiement Hugo reste intact et un workflow séparé ne fait que valider la prévisualisation. Le convertisseur temporaire relit le corpus Hugo à chaque build, conserve toutes les routes d’articles et copie les médias aux chemins français et anglais historiques. Les nouveaux contenus sont écrits directement en MDX typé et ne dépendent plus de Notion.

La première fondation couvre les 98 pages déclarées par les deux sitemaps publics historiques : accueil, sections, 34 articles, série, 46 pages de tags, pages À propos et documents légaux. Les flux principaux `/index.xml` et `/en/index.xml` sont également conservés. Une validation locale échoue si l’une de ces routes ou une ressource locale appelée par un article disparaît.

## Compromis assumés

- Astro demande un peu plus de code local qu’un thème Hugo, mais évite le verrouillage visuel et rend les évolutions explicites pour les contributeurs comme pour les agents IA.
- MDX est réservé aux composants pédagogiques ; le Markdown standard reste recommandé pour limiter la complexité.
- Les médias historiques sont temporairement dupliqués sous les routes FR et EN afin de préserver leurs URL. Une optimisation d’images pourra être conduite après l’inventaire des liens externes.
- L’analytics n’est pas inclus dans la prévisualisation. Une mesure respectueuse du consentement et une mise à jour juridique doivent être décidées ensemble avant la bascule.
- Les pages légales existantes sont rendues sans réécriture. Leur contenu décrit l’ancien fonctionnement et nécessite une revue juridique avant publication du nouveau site.

## Étapes avant bascule

1. Comparer visuellement les pages clés et corriger accessibilité, responsive et performance.
2. Compléter les compatibilités hors sitemap si les journaux de trafic les justifient : pagination historique et flux RSS de taxonomies.
3. Définir le cycle éditorial final, les contrôles de traduction et les aperçus de pull request.
4. Réviser confidentialité, cookies, analytics et mentions légales.
5. Ajouter les redirections/alias nécessaires, mesurer la parité, puis seulement remplacer le workflow de production.
