import { pluginLineNumbers } from "@expressive-code/plugin-line-numbers";
import { defineEcConfig } from "astro-expressive-code";

export default defineEcConfig({
  themes: ["github-light", "github-dark"],
  plugins: [pluginLineNumbers()],
  defaultProps: {
    wrap: false,
    overridesByLang: {
      bash: { showLineNumbers: false },
      console: { showLineNumbers: false },
      shell: { showLineNumbers: false },
      text: { showLineNumbers: false },
    },
  },
  styleOverrides: {
    borderRadius: "0.85rem",
    frames: {
      frameBoxShadowCssValue: "0 18px 48px rgba(20, 27, 45, 0.12)",
    },
  },
});
