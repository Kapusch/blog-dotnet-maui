import fs from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";

const websiteRoot = path.resolve(path.dirname(fileURLToPath(import.meta.url)), "..");
const manifest = JSON.parse(await fs.readFile(path.join(websiteRoot, ".generated", "manifest.json"), "utf8"));
const distRoot = path.join(websiteRoot, "dist");
const failures = [];
const legacyPageRoutes = new Set([
  "/", "/posts/", "/posts/tutos/", "/posts/tips/", "/series/", "/series/my-first-app/", "/tags/", "/about/", "/privacy-policy/", "/terms-of-use/",
  "/en/", "/en/posts/", "/en/posts/tutos/", "/en/posts/tips/", "/en/series/", "/en/series/my-first-app/", "/en/tags/", "/en/about/",
]);

for (const article of manifest) {
  legacyPageRoutes.add(article.url);
  for (const tag of article.tags) {
    const prefix = article.locale === "en" ? "/en" : "";
    legacyPageRoutes.add(`${prefix}/tags/${encodeURIComponent(taxonomySlug(tag))}/`);
  }
  const articleHtml = path.join(distRoot, ...article.url.split("/").filter(Boolean), "index.html");
  try {
    const html = await fs.readFile(articleHtml, "utf8");
    if (html.includes("{{<")) failures.push(`${article.url}: unresolved Hugo shortcode`);
    if (!html.includes(`<html lang=\"${article.locale}\"`)) failures.push(`${article.url}: incorrect document language`);
    if (!html.includes(article.title.replaceAll("&", "&amp;"))) failures.push(`${article.url}: title not rendered`);
    for (const reference of html.matchAll(/(?:href|src)="([^"]+)"/g)) {
      const value = reference[1].replaceAll("&amp;", "&");
      if (/^(?:https?:|mailto:|tel:|data:|#)/.test(value)) continue;
      const resolved = new URL(value, `https://www.mauicestclair.fr${article.url}`);
      await verifyLocalReference(resolved.pathname, `${article.url}: broken reference ${value}`);
    }
  } catch {
    failures.push(`${article.url}: missing generated page`);
  }
}

for (const route of legacyPageRoutes) {
  const output = route === "/" ? "index.html" : path.join(...route.split("/").filter(Boolean), "index.html");
  try {
    await fs.access(path.join(distRoot, output));
  } catch {
    failures.push(`Missing legacy public route: ${route}`);
  }
}

for (const requiredPath of [
  "index.html",
  "en/index.html",
  "posts/index.html",
  "en/posts/index.html",
  "tags/index.html",
  "en/tags/index.html",
  "series/my-first-app/index.html",
  "en/series/my-first-app/index.html",
  "index.xml",
  "en/index.xml",
  "posts/tutos/my-first-app/index.html",
  "en/posts/tutos/my-first-app/index.html",
  "search/index.html",
  "en/search/index.html",
  "privacy-policy/index.html",
  "terms-of-use/index.html",
  "sitemap-index.xml",
  "sitemap.xml",
  "fr/sitemap.xml",
  "en/sitemap.xml",
  "CNAME",
  "pagefind/pagefind.js",
  "pagefind/pagefind-component-ui.js",
  "pagefind/pagefind-component-ui.css",
]) {
  try {
    await fs.access(path.join(distRoot, requiredPath));
  } catch {
    failures.push(`Missing build artifact: ${requiredPath}`);
  }
}

if (failures.length) {
  console.error(failures.map((failure) => `- ${failure}`).join("\n"));
  process.exitCode = 1;
} else {
  console.log(`Validated ${manifest.length} localized articles, ${legacyPageRoutes.size} legacy routes and core build artifacts.`);
}

async function verifyLocalReference(pathname, failure) {
  const decoded = decodeURIComponent(pathname);
  const relative = decoded.replace(/^\//, "");
  const candidates = path.extname(relative)
    ? [relative]
    : [relative, path.join(relative, "index.html")];
  for (const candidate of candidates) {
    try {
      await fs.access(path.join(distRoot, candidate));
      return;
    } catch {
      // Try the next static-output form.
    }
  }
  failures.push(failure);
}

function taxonomySlug(value) {
  return value
    .normalize("NFKD")
    .replace(/[\u0300-\u036f]/g, "")
    .toLowerCase()
    .trim()
    .replace(/[^a-z0-9.#-]+/g, "-")
    .replace(/-{2,}/g, "-")
    .replace(/^-|-$/g, "");
}
