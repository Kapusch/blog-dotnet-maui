import rss from "@astrojs/rss";
import { getPublishedArticles } from "../lib/content";

export async function GET(context: { site?: URL }) {
  const articles = await getPublishedArticles("fr");
  return rss({
    title: "MAUI, c’est clair !",
    description: "Tutoriels et articles .NET MAUI en français.",
    site: context.site!,
    customData: "<language>fr</language>",
    items: articles.map(({ data }) => ({ title: data.title, description: data.description, pubDate: data.publishedAt, link: data.url })),
  });
}
