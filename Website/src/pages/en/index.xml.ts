import rss from "@astrojs/rss";
import { getPublishedArticles } from "../../lib/content";

export async function GET(context: { site?: URL }) {
  const articles = await getPublishedArticles("en");
  return rss({
    title: "MAUI, c’est clair !",
    description: ".NET MAUI tutorials and articles in English.",
    site: context.site!,
    customData: "<language>en</language>",
    items: articles.map(({ data }) => ({ title: data.title, description: data.description, pubDate: data.publishedAt, link: data.url })),
  });
}
