import fs from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";
import YAML from "yaml";

const scriptDirectory = path.dirname(fileURLToPath(import.meta.url));
const websiteRoot = path.resolve(scriptDirectory, "..");
const repositoryRoot = path.resolve(websiteRoot, "..");
const legacyContentRoot = path.join(repositoryRoot, "Blog", "content");
const legacyPostsRoot = path.join(legacyContentRoot, "posts");
const generatedRoot = path.join(websiteRoot, ".generated");
const generatedContentRoot = path.join(generatedRoot, "content", "articles");
const generatedLegalRoot = path.join(generatedRoot, "content", "legal");
const generatedAssetRoot = path.join(websiteRoot, "public", "posts");
const generatedEnglishAssetRoot = path.join(websiteRoot, "public", "en", "posts");

assertGeneratedPath(generatedRoot);
assertGeneratedPath(generatedAssetRoot);
assertGeneratedPath(generatedEnglishAssetRoot);

await fs.rm(generatedRoot, { recursive: true, force: true });
await fs.rm(generatedAssetRoot, { recursive: true, force: true });
await fs.rm(generatedEnglishAssetRoot, { recursive: true, force: true });
await fs.mkdir(generatedContentRoot, { recursive: true });
await fs.mkdir(generatedAssetRoot, { recursive: true });
await fs.mkdir(generatedEnglishAssetRoot, { recursive: true });

const sourceFiles = (await walkFiles(legacyPostsRoot))
  .filter((file) => ["index.fr.md", "index.en.md"].includes(path.basename(file).toLowerCase()))
  .sort();

const manifest = [];

for (const sourceFile of sourceFiles) {
  const source = await fs.readFile(sourceFile, "utf8");
  const { data: legacy, body } = parseFrontmatter(source, sourceFile);
  const locale = sourceFile.endsWith(".en.md") ? "en" : "fr";
  const bundleDirectory = path.dirname(sourceFile);
  const relativeBundle = path.relative(legacyContentRoot, bundleDirectory);
  const publicSegments = relativeBundle.split(path.sep).map(toPublicSegment);
  const translationKey = publicSegments.join("/");
  const localizedSegments = locale === "en" ? ["en", ...publicSegments] : publicSegments;
  const url = `/${localizedSegments.join("/")}/`;
  const publishedAt = normalizeDate(legacy.PublishDate, sourceFile);
  const featuredFile = legacy.resources?.[0]?.src;
  const seriesName = Array.isArray(legacy.series) ? legacy.series[0] : undefined;
  const series = seriesName ? toPublicSegment(seriesName) : undefined;
  const type = legacy.Category === "Tutos" ? "tutorial" : legacy.Category === "Tips" ? "tip" : "article";

  const normalized = {
    title: String(legacy.Title ?? "").trim(),
    description: String(legacy.Description ?? "").trim(),
    locale,
    translationKey,
    url,
    type,
    publishedAt,
    tags: Array.isArray(legacy.Tags) ? legacy.Tags : [],
    ...(featuredFile ? { featuredImage: `${url}${featuredFile}` } : {}),
    ...(series ? {
      series,
      seriesTitle: locale === "fr" && series === "my-first-app" ? "Ma première app" : seriesName,
      seriesOrder: Number.parseInt(legacy.Index, 10),
    } : {}),
    sourcePath: path.relative(repositoryRoot, sourceFile).split(path.sep).join("/"),
    status: legacy.draft ? "draft" : "published",
    legacy: true,
  };

  if (!normalized.title || !normalized.description) {
    throw new Error(`Missing required title or description in ${sourceFile}`);
  }

  const transformedBody = transformLegacyMarkdown(body);
  const outputDirectory = path.join(generatedContentRoot, locale, ...publicSegments);
  await fs.mkdir(outputDirectory, { recursive: true });
  await fs.writeFile(
    path.join(outputDirectory, "index.mdx"),
    `---\n${YAML.stringify(normalized, { lineWidth: 0 })}---\n\n${transformedBody.trim()}\n`,
    "utf8",
  );

  const publicBundleDirectory = path.join(websiteRoot, "public", ...localizedSegments);
  await copyBundleAssets(bundleDirectory, publicBundleDirectory);

  manifest.push({
    locale,
    translationKey,
    url,
    title: normalized.title,
    tags: normalized.tags,
    sourcePath: normalized.sourcePath,
  });
}

await fs.writeFile(
  path.join(generatedRoot, "manifest.json"),
  `${JSON.stringify(manifest, null, 2)}\n`,
  "utf8",
);

const translations = new Map();
for (const entry of manifest) {
  const locales = translations.get(entry.translationKey) ?? new Set();
  locales.add(entry.locale);
  translations.set(entry.translationKey, locales);
}
for (const [translationKey, locales] of translations) {
  if (!locales.has("fr") || !locales.has("en")) {
    throw new Error(`Missing FR/EN translation pair for ${translationKey}`);
  }
}

for (const page of [
  {
    source: "privacy-policy.md",
    slug: "privacy-policy",
    title: "Privacy Policy",
    description: "Privacy policy for MAUI, c’est clair !",
  },
  {
    source: "terms-of-use.md",
    slug: "terms-of-use",
    title: "Terms and Conditions",
    description: "Terms and conditions for MAUI, c’est clair !",
  },
]) {
  const sourceFile = path.join(legacyContentRoot, page.source);
  const source = await fs.readFile(sourceFile, "utf8");
  const { body } = parseFrontmatter(source, sourceFile);
  const frontmatter = {
    title: page.title,
    description: page.description,
    path: `/${page.slug}/`,
    sourcePath: path.relative(repositoryRoot, sourceFile).split(path.sep).join("/"),
  };
  await fs.mkdir(generatedLegalRoot, { recursive: true });
  await fs.writeFile(
    path.join(generatedLegalRoot, `${page.slug}.mdx`),
    `---\n${YAML.stringify(frontmatter, { lineWidth: 0 })}---\n\n${body.trim()}\n`,
    "utf8",
  );
}

console.log(`Prepared ${manifest.length} localized articles (${translations.size} translation pairs).`);

function parseFrontmatter(source, sourceFile) {
  const match = source.match(/^---\r?\n([\s\S]*?)\r?\n---\r?\n([\s\S]*)$/);
  if (!match) throw new Error(`Invalid frontmatter in ${sourceFile}`);
  return { data: YAML.parse(repairLegacyYaml(match[1])), body: match[2] };
}

function repairLegacyYaml(frontmatter) {
  let multilineQuote = false;
  return frontmatter
    .split(/\r?\n/)
    .map((line) => {
      if (multilineQuote) {
        multilineQuote = countUnescapedDoubleQuotes(line) === 0;
        return `  ${line}`;
      }

      if (/^[A-Za-z][\w-]*:\s*"/.test(line) && hasUnpairedDoubleQuote(line)) {
        multilineQuote = true;
      }
      return line;
    })
    .join("\n");
}

function hasUnpairedDoubleQuote(value) {
  return countUnescapedDoubleQuotes(value) % 2 === 1;
}

function countUnescapedDoubleQuotes(value) {
  return value.match(/(?<!\\)"/g)?.length ?? 0;
}

function normalizeDate(value, sourceFile) {
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) throw new Error(`Invalid publication date in ${sourceFile}`);
  return date.toISOString();
}

function transformLegacyMarkdown(markdown) {
  let transformed = markdown
    .replace(/<!--more-->/g, "")
    .replace(/[\u200e\u200f]/g, "")
    .replace(/\n___\s*\n(?:Plus d'articles dans la même série:|More articles in the series:)\s*\n\{\{< series [^\n]+>\}\}\s*$/u, "")
    .replace(
      /<p align="center" style="margin-bottom:-10px"><strong>(?:Nom du fichier\s*|Filename)\s*:<\/strong><code>(.*?)<\/code><\/p>\s*\n\s*```([^\n]*)/g,
      (_, fileName, fenceMeta) => `\`\`\`${fenceMeta.trim()} title=${JSON.stringify(stripMarkup(fileName))}`,
    )
    .replace(
      /<p[^>]*align="center"[^>]*>\s*<img[^>]*src="([^"]+)"[^>]*\/?\s*>\s*<\/p>\s*<figure>(?:<figcaption[^>]*>([\s\S]*?)<\/figcaption>)?<\/figure>/g,
      (_, source, caption = "") => {
        const plainCaption = stripMarkup(caption);
        const props = [jsxAttribute("src", source), jsxAttribute("alt", plainCaption)];
        if (plainCaption) props.push(jsxAttribute("caption", plainCaption));
        return `<Figure ${props.join(" ")} />`;
      },
    );

  const output = [];
  const componentStack = [];

  for (const line of transformed.split(/\r?\n/)) {
    const opening = line.match(/^\s*\{\{< admonition\s+([^>]+)>\}\}\s*$/);
    if (opening) {
      const attributes = parseShortcodeAttributes(opening[1]);
      const title = stripMarkup(attributes.title ?? "").trim();
      const isDeepDive = attributes.open === "false";
      const component = isDeepDive ? "DeepDive" : "Callout";
      componentStack.push(component);
      if (isDeepDive) {
        output.push(`<DeepDive${title ? ` ${jsxAttribute("title", title)}` : ""}>`);
      } else {
        output.push(`<Callout ${jsxAttribute("kind", attributes.type ?? "note")}${title ? ` ${jsxAttribute("title", title)}` : ""}>`);
      }
      continue;
    }

    if (/^\s*\{\{< \/admonition >\}\}\s*$/.test(line)) {
      const component = componentStack.pop();
      if (!component) throw new Error("Unexpected closing admonition shortcode");
      output.push(`</${component}>`);
      continue;
    }

    const download = line.match(/^\s*\{\{< link\s+([^>]+)>\}\}\s*$/);
    if (download) {
      const attributes = parseShortcodeAttributes(download[1]);
      const label = attributes.content ?? attributes.download ?? attributes.href ?? "Download";
      output.push(`<DownloadLink ${jsxAttribute("href", attributes.href)} ${jsxAttribute("label", label)} />`);
      continue;
    }

    if (/^\s*\{\{< series [^>]+>\}\}\s*$/.test(line)) continue;
    output.push(line);
  }

  if (componentStack.length) throw new Error("Unclosed admonition shortcode");
  return output.join("\n");
}

function parseShortcodeAttributes(source) {
  const attributes = {};
  for (const match of source.matchAll(/([\w-]+)=(?:"([^"]*)"|'([^']*)'|([^\s]+))/g)) {
    attributes[match[1]] = match[2] ?? match[3] ?? match[4];
  }
  return attributes;
}

function stripMarkup(value) {
  return String(value)
    .replace(/<[^>]*>/g, "")
    .replace(/\[([^\]]+)]\([^)]*\)/g, "$1")
    .replace(/[*_`]/g, "")
    .replace(/\s+/g, " ")
    .trim();
}

function jsxAttribute(name, value) {
  return `${name}={${JSON.stringify(String(value ?? ""))}}`;
}

function toPublicSegment(segment) {
  return segment
    .normalize("NFKD")
    .replace(/[\u0300-\u036f]/g, "")
    .toLowerCase()
    .trim()
    .replace(/[^a-z0-9.#-]+/g, "-")
    .replace(/-{2,}/g, "-")
    .replace(/^-|-$/g, "");
}

async function walkFiles(directory) {
  const entries = await fs.readdir(directory, { withFileTypes: true });
  const files = [];
  for (const entry of entries) {
    const fullPath = path.join(directory, entry.name);
    if (entry.isDirectory()) files.push(...await walkFiles(fullPath));
    else if (entry.isFile()) files.push(fullPath);
  }
  return files;
}

async function copyBundleAssets(sourceDirectory, destinationDirectory) {
  for (const file of await walkFiles(sourceDirectory)) {
    if (/\.(?:md)$/i.test(file)) continue;
    const relative = path.relative(sourceDirectory, file);
    const destination = path.join(destinationDirectory, relative);
    await fs.mkdir(path.dirname(destination), { recursive: true });
    await fs.copyFile(file, destination);
  }
}

function assertGeneratedPath(target) {
  const relative = path.relative(websiteRoot, target);
  const isGeneratedRoot = relative === ".generated";
  const isGeneratedPosts = relative === path.join("public", "posts");
  const isGeneratedEnglishPosts = relative === path.join("public", "en", "posts");
  if ((!isGeneratedRoot && !isGeneratedPosts && !isGeneratedEnglishPosts) || relative.startsWith("..")) {
    throw new Error(`Refusing to clean unexpected path: ${target}`);
  }
}
