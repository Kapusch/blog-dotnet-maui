import mdx from "@astrojs/mdx";
import sitemap from "@astrojs/sitemap";
import expressiveCode from "astro-expressive-code";
import { defineConfig } from "astro/config";

export default defineConfig({
  site: "https://www.mauicestclair.fr",
  output: "static",
  trailingSlash: "always",
  integrations: [expressiveCode(), mdx(), sitemap()],
});
