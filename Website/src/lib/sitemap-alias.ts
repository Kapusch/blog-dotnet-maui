import { site } from "../config";

export function sitemapAlias() {
  const body = `<?xml version="1.0" encoding="UTF-8"?>
<sitemapindex xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <sitemap><loc>${site.url}/sitemap-0.xml</loc></sitemap>
</sitemapindex>`;
  return new Response(body, { headers: { "Content-Type": "application/xml; charset=utf-8" } });
}
