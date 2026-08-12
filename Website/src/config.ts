export const site = {
  name: "MAUI, c’est clair !",
  url: "https://www.mauicestclair.fr",
  author: "Jean-Emmanuel Baillat",
  authorUrl: "https://github.com/Kapusch",
  repository: "https://github.com/Kapusch/blog-dotnet-maui",
} as const;

export type Locale = "fr" | "en";

export const labels = {
  fr: {
    localeName: "Français",
    alternateLocale: "English",
    nav: {
      articles: "Articles",
      tutorials: "Cours",
      tips: "Astuces",
      tags: "Tags",
      about: "À propos",
      search: "Rechercher",
    },
    published: "Publié le",
    readingTime: "min de lecture",
    chapter: "chapitre",
    chapters: "chapitres",
    series: "Dans cette série",
    previous: "Chapitre précédent",
    next: "Chapitre suivant",
    contents: "Dans cet article",
    allArticles: "Tous les articles",
    source: "Code source",
  },
  en: {
    localeName: "English",
    alternateLocale: "Français",
    nav: {
      articles: "Articles",
      tutorials: "Tutorials",
      tips: "Tips",
      tags: "Tags",
      about: "About",
      search: "Search",
    },
    published: "Published on",
    readingTime: "min read",
    chapter: "chapter",
    chapters: "chapters",
    series: "In this series",
    previous: "Previous chapter",
    next: "Next chapter",
    contents: "On this page",
    allArticles: "All articles",
    source: "Source code",
  },
} as const;

export function localePath(locale: Locale, path = "/") {
  const normalized = path.startsWith("/") ? path : `/${path}`;
  if (locale === "fr") return normalized;
  if (normalized === "/") return "/en/";
  return `/en${normalized}`;
}
