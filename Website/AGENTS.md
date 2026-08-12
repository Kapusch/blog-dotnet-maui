# Working agreements for Website

This directory is the candidate replacement for the published Hugo site in `../Blog`. Treat the two implementations as parallel until the migration is explicitly approved.

## Safety and compatibility

- Do not modify or remove the Hugo deployment workflow while this site is a preview.
- Do not deploy, change DNS, or alter the production `CNAME` from work in this directory.
- Public paths are product contracts. Preserve `data.url` values and verify both locales before changing a route.
- Keep French at `/` and English at `/en/`; every published article should have a translation peer with the same `translationKey`.
- Never edit `.generated/`, `public/posts/`, or `public/en/posts/`. They are recreated by `scripts/prepare-content.mjs`.
- Preserve legal text verbatim until an explicit legal review approves changes.

## Architecture

- Prefer Astro components, semantic HTML, and scoped CSS. Add client JavaScript only for an interaction that cannot work as HTML/CSS.
- Do not introduce a UI framework, global state library, CMS, or analytics SDK without documenting the concrete need and tradeoff.
- New editorial content belongs in `content/articles/`; legacy Hugo content remains read-only input during migration.
- Keep tutorial components accessible: images need meaningful alt text, controls need labels, and expandable detail must remain usable without animation.
- Use `src/config.ts` for shared routes and labels, and content schemas for metadata contracts.

## Verification

Use Node.js 22 and run `npm test` after changes. For layout work, inspect at least one home page and one long article at desktop and 390px mobile widths. A page must not create horizontal document overflow.
