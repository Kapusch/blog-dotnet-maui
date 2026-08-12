import { getCollection, type CollectionEntry } from "astro:content";
import type { Locale } from "../config";

export type Article = CollectionEntry<"articles"> | CollectionEntry<"nativeArticles">;

export async function getPublishedArticles(locale?: Locale) {
  const now = new Date();
  const isPublished = ({ data }: Article) =>
    data.status === "published" && data.publishedAt <= now && (!locale || data.locale === locale);
  const [legacyArticles, nativeArticles] = await Promise.all([
    getCollection("articles"),
    getCollection("nativeArticles"),
  ]);
  const articles: Article[] = [...legacyArticles, ...nativeArticles].filter(isPublished);

  return articles.sort((left, right) =>
    right.data.publishedAt.getTime() - left.data.publishedAt.getTime(),
  );
}

export async function getArticleTranslation(article: Article) {
  const targetLocale = article.data.locale === "fr" ? "en" : "fr";
  const articles = await getPublishedArticles(targetLocale);
  return articles.find((candidate) => candidate.data.translationKey === article.data.translationKey);
}

export async function getSeriesArticles(article: Article) {
  if (!article.data.series) return [];
  const articles = await getPublishedArticles(article.data.locale);
  return articles
    .filter((candidate) => candidate.data.series === article.data.series)
    .sort((left, right) => (left.data.seriesOrder ?? 0) - (right.data.seriesOrder ?? 0));
}

export function readingMinutes(article: Article) {
  const words = (article.body ?? "")
    .replace(/```[\s\S]*?```/g, " ")
    .replace(/<[^>]+>/g, " ")
    .trim()
    .split(/\s+/)
    .filter(Boolean).length;
  const wordsPerMinute = article.data.locale === "fr" ? 220 : 240;
  return Math.max(1, Math.ceil(words / wordsPerMinute));
}

export function formatDate(date: Date, locale: Locale) {
  return new Intl.DateTimeFormat(locale === "fr" ? "fr-FR" : "en-GB", {
    day: "numeric",
    month: "long",
    year: "numeric",
  }).format(date);
}

export function articleKindLabel(article: Article) {
  if (article.data.locale === "fr") {
    return article.data.type === "tutorial" ? "Cours" : article.data.type === "tip" ? "Astuce" : "Article";
  }
  return article.data.type === "tutorial" ? "Tutorial" : article.data.type === "tip" ? "Tip" : "Article";
}

export function taxonomySlug(value: string) {
  return value
    .normalize("NFKD")
    .replace(/[\u0300-\u036f]/g, "")
    .toLowerCase()
    .trim()
    .replace(/[^a-z0-9.#-]+/g, "-")
    .replace(/-{2,}/g, "-")
    .replace(/^-|-$/g, "");
}
