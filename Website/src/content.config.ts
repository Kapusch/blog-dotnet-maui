import { defineCollection } from "astro:content";
import { glob } from "astro/loaders";
import { z } from "astro/zod";

const articleSchema = z.object({
  title: z.string(),
  description: z.string(),
  locale: z.enum(["fr", "en"]),
  translationKey: z.string(),
  url: z.string().startsWith("/"),
  type: z.enum(["tutorial", "tip", "article"]),
  publishedAt: z.coerce.date(),
  updatedAt: z.coerce.date().optional(),
  tags: z.array(z.string()).default([]),
  featuredImage: z.string().optional(),
  series: z.string().optional(),
  seriesTitle: z.string().optional(),
  seriesOrder: z.number().int().positive().optional(),
  sourcePath: z.string(),
  status: z.enum(["draft", "review", "scheduled", "published"]),
  legacy: z.boolean().default(false),
});

const articles = defineCollection({
  loader: glob({
    pattern: "**/*.{md,mdx}",
    base: "./.generated/content/articles",
    retainBody: true,
  }),
  schema: articleSchema,
});

const nativeArticles = defineCollection({
  loader: glob({
    pattern: "**/*.{md,mdx}",
    base: "./content/articles",
    retainBody: true,
  }),
  schema: articleSchema,
});

const legal = defineCollection({
  loader: glob({
    pattern: "**/*.{md,mdx}",
    base: "./.generated/content/legal",
    retainBody: true,
  }),
  schema: z.object({
    title: z.string(),
    description: z.string(),
    path: z.string().startsWith("/"),
    sourcePath: z.string(),
  }),
});

export const collections = { articles, nativeArticles, legal };
