import { readFileSync } from "node:fs";
import { runInNewContext } from "node:vm";

const bundles = [
  [
    "importrecord.form.js",
    "ImportRecord",
    [
      "OnLoadQuickCreateForm",
      "onLoad",
      "onSave",
      "onChangeOfManualPostImportCheckDecision",
      "showOrHideNonComplianceOther",
      "onChangeOfMoveToCompletion",
      "showRelevantSections"
    ],
  ],
  [
    "importrecord.ribbon.js",
    "ImportRecord",
    [
      "onFillEmptyDataWithItahc",
      "onOverwriteDataWithItahc",
      "onFillEmptyDataWithNotification",
      "onOverwriteDataWithNotification",
      "openUrlFromRibbon",
    ],
  ],
  [
    "importernotification.form.js",
    "ImporterNotification",
    ["showHideCharity", "checkForMultipleCommodities"],
  ],
  [
    "importernotification.ribbon.js",
    "ImporterNotification",
    ["onCreateImportRecordFromNotification"],
  ],
  ["importquery.ribbon.js", "ImportQuery", ["CloneImportQueryButton"]],
  ["itahc.ribbon.js", "Itahc", ["onCreateImportRecordFromItahc"]],
  [
    "matchrecord.subgrid.ribbon.js",
    "MatchRecord",
    [
      "onAppendItahc",
      "onAppendImporterNotification",
      "onCreateImportRecordFromITAHC",
    ],
  ],
  [
    "postimportcheck.form.js",
    "PostImportCheck",
    ["onLoad", "onChangeSamplingRequired"],
  ],
] as const;

test("bundles expose every Dynamics handler on DefraImports", () => {
  const context: {
    window?: unknown;
    DefraImports?: Record<string, Record<string, unknown>>;
  } = {};
  context.window = context;
  for (const [fileName] of bundles)
    runInNewContext(readFileSync(`dist/${fileName}`, "utf8"), context);
  for (const [, namespace, handlers] of bundles)
    for (const handler of handlers)
      expect(typeof context.DefraImports?.[namespace]?.[handler]).toBe(
        "function"
      );
});
