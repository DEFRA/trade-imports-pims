---
description: "Use when writing, reviewing or refactoring the TypeScript web resource scripts under src/webresources/scripts/src: model-driven app form, ribbon and subgrid handlers. Covers the project layout and build, strict typing and Xrm typings, handler design, data-driven configuration, error handling, behaviour preservation, formatting, linting and the rule against suppressions."
applyTo: "src/webresources/scripts/src/**/*.ts"
---

# TypeScript Source Standards

These rules apply to the web resource scripts in `src/webresources/scripts/src`. Follow the conventions of the neighbouring scripts where these rules are silent. The tests are covered in [typescript-tests.instructions.md](./typescript-tests.instructions.md).

## Project and build

- Each file in `src` is one web resource. `tsc` compiles it to `out/src`, and rollup bundles each listed file into an IIFE in `dist/`, exposed on `DefraImports.<Namespace>`. The solution packs `dist/` through its mapping file.
- Exported functions are the handlers that forms, ribbons and subgrids call. Anything not exported is private to the bundle.
- A new file is built only when it is listed in `namespacesByFileName` in `rollup.config.mjs`, and its handlers belong in `bundles` in `tests/bundle.contract.test.ts`. Otherwise it is silently skipped.
- Imports between `src` files are bundled into each bundle that uses them. `global-types.ts` only imports the Xrm typings.
- `out/`, `dist/`, `coverage/`, `test-results/` and `node_modules/` are output. Never edit or commit them.
- The code is compiled with TypeScript 5, target ES2022 and `strict` on. Do not weaken `tsconfig.json`.

## Scope

- Make the smallest change that delivers the request. Do not refactor unrelated scripts as a side effect of another change.
- When you change an existing handler, its behaviour is frozen unless a change is explicitly approved. Forms and ribbons in the solution call it by name, so do not rename or remove an exported function, change its parameters or change what it does for existing callers without approval. Flag a suspected bug in your report instead of fixing it.
- Prefer declarative options first: business rules, form configuration, and workflows, actions or flows. Script belongs here only when those cannot do the job. If the request describes a business process, say so before writing any.
- Add no dependencies, and do not change `package.json` or `package-lock.json`, unless the user asks. The pipeline installs with `npm ci`, so the lock file must stay consistent.

## Design principles

Apply these to new code and to code you change.

- **Single responsibility.** An exported handler reads its inputs, calls small functions that parse, decide and apply, and nothing more. Keep decision logic in pure functions that need no Xrm mock, and keep Xrm calls thin.
- **Open/closed.** Add behaviour by adding data, a registration or a small function, not by adding a branch to a working handler. Replace `if` chains on a name or key with a lookup that data chooses.
- **Interface segregation and dependency inversion.** Depend on the narrowest thing you need: a value, a control or an attribute, not the whole form context. Where a function needs a service such as `Xrm.WebApi`, take it as a parameter or reach it through one thin function, so that a test can substitute it.
- **Data-driven.** Anything that varies by form, field, control, action, key or threshold is data, not code. A registration can pass parameters to a handler (a JSON string, a field list, an action name). Parse and validate that data in one place, fail with a clear error on bad data, and cover it with tests. Hard-code only what is structural.
- **Capability naming.** Name every file, namespace, handler and type for the capability, not the first use case: `applyVisibilityRules`, not `hideSectionWhenStatusClosed`. A table name belongs in a file name only when the script is genuinely about that table.
- **No speculative generality.** Generalise to the capability that the requirement plainly implies and no further. Add no extension point that nothing uses.

## Typing

- Use the Xrm typings from `@types/xrm`: `Xrm.FormContext`, `Xrm.Events.EventContext`, `Xrm.Attributes.*`, `Xrm.Controls.*`, `Xrm.LookupValue`. They are ambient, so do not import them.
- Do not use `any`, non-null assertions (`!`) or blanket type assertions to silence the compiler. Narrow `unknown` with a type check, and write a type guard for parsed data (see the parsing function in the templates).
- Type every exported function's parameters and return value. Return `Promise<void>` from an async handler.
- Prefer `interface` or `type` for configuration shapes, and `as const` for fixed values.
- Do not use `Xrm.Page`, which is deprecated. Get the form context from `executionContext.getFormContext()` or the ribbon's `primaryControl`.

## Handlers and Xrm

- Resolve attributes, controls, tabs and sections by logical name, and fail with a clear error when a required one is missing, as `requiredAttribute` and `requiredControl` do in the neighbouring scripts. Do not let a missing control fail later with `undefined`.
- A handler that takes `executionContext` must be registered with "Pass execution context as first parameter". A handler that cannot receive it must not depend on it; where unavoidable, module-level state needs a comment that explains why, as in `postimportcheck.form.ts`.
- Use `Xrm.WebApi` for data access. Select only the columns you need (`?$select=`), and never build a query by concatenating unvalidated input.
- Use `async` and `await`. Never leave a promise unhandled: catch the rejection and surface it with `Xrm.Navigation.openErrorDialog` or `openAlertDialog`, as the existing ribbon scripts do.
- Close any progress indicator you open in a `finally` block.
- Do not hard-code environment URLs, tenant data, secrets or personal data. A record GUID is environment-specific unless the solution ships the record. Read a configuration value from a configuration record or a handler parameter instead.
- Guard against a handler running twice (a double click, a repeated `OnSave`): make it safe to repeat, or disable the control while it runs.

## Style and linting

- ESLint (`eslint.config.js`) is type-aware, with `@typescript-eslint/recommended-requiring-type-checking`, `unbound-method` and the Creedengo plugin. `npm run lint` is clean at baseline and must stay clean.
- Do not add suppressions (`eslint-disable`, `@ts-ignore`, `@ts-expect-error`) or change the ESLint or TypeScript configuration to reach a pass without the user's approval for that specific suppression.
- Format with Prettier, configured in `.prettierrc.js` (single quotes, two-space indent, semicolons, a 100-column print width, ES5 trailing commas, and `endOfLine: "auto"` so that CRLF and LF checkouts both pass). Do not hand-format. Run `npm run format` (or `npx prettier --write <files>`) after editing, and `npm run format:check` must pass. `package-lock.json`, `*.js` and build output are in `.prettierignore`.
- Write comments and messages in British English, and comment only what needs clarifying. Error messages are read by makers and users, so make them specific.

## Running and verifying

From `src/webresources/scripts`, restore with `npm ci --legacy-peer-deps` if `node_modules` is missing, then:

- `npm run lint` for linting;
- `npm run format:check` for formatting. It is clean, so it must stay clean;
- `npm test` to build (`tsc`, then rollup) and run Jest with coverage, including the bundle contract test. This is what the pipeline runs (`pipelines/templates/steps/run-typescript-unit-tests-steps.yml`);
- `npx jest tests/<file>.test.ts --coverage=false` while iterating on one file. It does not build the bundles, so finish with `npm test`.
