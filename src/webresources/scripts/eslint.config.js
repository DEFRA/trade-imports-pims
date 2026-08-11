const { defineConfig, globalIgnores } = require("eslint/config");

const tsParser = require("@typescript-eslint/parser");
const jest = require("eslint-plugin-jest");
const typescriptEslint = require("@typescript-eslint/eslint-plugin");
const creedengoEsLint = require("@creedengo/eslint-plugin");

const globals = require("globals");
const js = require("@eslint/js");
const espree = require("espree");

const { FlatCompat } = require("@eslint/eslintrc");

const compat = new FlatCompat({
  baseDirectory: __dirname,
  recommendedConfig: js.configs.recommended,
  allConfig: js.configs.all,
});

module.exports = defineConfig(creedengoEsLint.configs.recommended, [
  /**
   * JS / MJS / CJS files — use default JS parser (espree).
   * This prevents @typescript-eslint/parser (with project mode)
   * from trying to parse rollup.config.mjs and similar files.
   */
  {
    files: ["**/*.js", "**/*.mjs", "**/*.cjs"],
    languageOptions: {
      parser: espree,
      ecmaVersion: "latest",
      sourceType: "module",
      globals: {
        // Rollup configs usually run in Node, but they may reference browser globals.
        // Include both as needed; adjust if you want to be strict.
        ...globals.node,
        ...globals.browser,
      },
    },
    // Add any JS-specific rules here if you like.
    // rules: {}
  },

  /**
   * TypeScript files — keep type-aware linting.
   * Scoped to TS only so that non-TS files won’t trigger the project parser.
   */
  {
    files: ["**/*.ts", "**/*.tsx"],
    extends: compat.extends(
      "eslint:recommended",
      "plugin:@typescript-eslint/recommended",
      "plugin:@typescript-eslint/recommended-requiring-type-checking"
    ),
    languageOptions: {
      parser: tsParser,
      parserOptions: {
        tsconfigRootDir: __dirname,
        project: ["./tsconfig.json", "./tsconfig.jest.json"],
        sourceType: "module",
      },
      globals: {
        ...globals.browser,
      },
    },
    plugins: {
      "@typescript-eslint": typescriptEslint,
    },
    rules: {
      "@typescript-eslint/unbound-method": "error",
    },
  },

  /**
   * Jest override for specs.
   */
  {
    files: ["spec/**", "**/*.spec.ts", "**/*.spec.tsx"],
    plugins: {
      jest,
    },
    rules: {
      "@typescript-eslint/unbound-method": "off",
      "jest/unbound-method": "error",
    },
  },

  /**
   * Global ignores — keeping your original entries.
   * Note: *.mjs is NOT ignored so rollup.config.mjs will be linted.
   */
  globalIgnores([
    "**/*.js",
    "**/*.cjs",
    "**/node_modules/",
    "**/out/",
    "**/dist/",
    "**/coverage/",
    "**/typings/",
  ]),
]);
