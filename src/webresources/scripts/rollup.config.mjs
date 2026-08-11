import path from "node:path";
import { globSync } from "glob";
import { defineConfig } from "rollup";
import commonjs from "@rollup/plugin-commonjs";

const namespacesByFileName = {
  "importernotification.form.js": "DefraImports.ImporterNotification",
  "importernotification.ribbon.js": "DefraImports.ImporterNotification",
  "importquery.ribbon.js": "DefraImports.ImportQuery",
  "importrecord.form.js": "DefraImports.ImportRecord",
  "importrecord.ribbon.js": "DefraImports.ImportRecord",
  "itahc.ribbon.js": "DefraImports.Itahc",
  "matchrecord.subgrid.ribbon.js": "DefraImports.MatchRecord",
  "postimportcheck.form.js": "DefraImports.PostImportCheck",
};

export default globSync("out/src/*.js").flatMap((entry) => {
  const fileName = path.basename(entry);
  const namespace = namespacesByFileName[fileName];
  if (namespace === undefined) {
    return [];
  }

  return defineConfig({
    input: entry,
    context: "window",
    plugins: [commonjs()],
    output: {
      file: `dist/${fileName}`,
      format: "iife",
      name: namespace,
      extend: true,
      sourcemap: true,
    },
  });
});
