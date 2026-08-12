# MAUI, c’est clair ! — nouvelle fondation

Cette application Astro est développée en parallèle du site Hugo situé dans `Blog/`. Elle ne remplace pas encore le site publié et le workflow de déploiement historique reste intact.

## Démarrer

Prérequis : Node.js 22.12 ou plus récent.

```bash
npm install
npm run dev
```

La préparation copie les médias historiques dans `public/posts` et `public/en/posts`, puis convertit les 34 fichiers Markdown Hugo vers MDX dans `.generated/`. Ces dossiers sont temporaires et ne doivent pas être modifiés.

La validation complète s’exécute avec :

```bash
npm test
```

Elle contrôle les types Astro, compile le site, construit l’index Pagefind et vérifie chaque URL d’article FR/EN.

## Écrire un nouvel article

1. Copier les modèles de `content/articles/_template/` vers un dossier nommé d’après l’article.
2. Conserver la même `translationKey` dans les versions française et anglaise.
3. Choisir explicitement une `url` publique dans chaque langue.
4. Commencer avec `status: draft`, puis passer par `review` et `published`.
5. Placer les médias pérennes sous `public/media/<slug>/` et utiliser des chemins absolus.

Le Markdown standard reste le chemin principal. MDX n’est utile que pour les composants éditoriaux fournis automatiquement au rendu :

- `<Callout kind="info|warning|tip|danger" title="…">` pour une information visible ;
- `<DeepDive title="…">` pour un approfondissement repliable ;
- `<Figure src="…" alt="…" caption="…" />` pour une image accessible ;
- `<DownloadLink href="…" label="…" />` pour un fichier complémentaire.

Les blocs de code acceptent notamment `title="Fichier.cs"`, la mise en évidence `{3,5-7}` et `collapse={20-60}` via Expressive Code.

## Limites de cette étape

Les contenus historiques restent canoniques dans `Blog/content` jusqu’à la bascule. Le convertisseur est volontairement transitoire : les nouveaux articles sont écrits directement dans `Website/content/articles`, sans Notion ni exporteur spécifique au thème.

Le déploiement, le domaine, les redirections secondaires, l’analytics et la révision juridique sont hors de cette première fondation.
