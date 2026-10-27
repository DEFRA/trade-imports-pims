---
name: web-resource-script-authoring
description: Write, change or review the TypeScript web resource scripts in src/webresources/scripts (model-driven app form, ribbon and subgrid handlers) and their Jest tests. Covers the file and bundle conventions, the rollup namespace mapping and bundle contract test, Xrm typing and mocking, the lint and test commands, and the hand-off for registering the web resource and wiring the handler in a development environment. Use whenever a request adds or changes a form, ribbon or subgrid script. Owned by the Developer agent.
---

# Web resource script authoring

The scripts are TypeScript, bundled by rollup into one IIFE per file in `dist/`, and packed into the `defra_Imports` solution. Code to adapt is in [templates.md](./references/templates.md).

## Layout

All paths are under `src/webresources/scripts`.

| Path | Purpose |
| --- | --- |
| `src/<table>.<form\|ribbon\|subgrid.ribbon>.ts` | One script per web resource. Exported functions are the handlers that forms and ribbons call |
| `tests/<same name>.test.ts` | Jest tests for that script |
| `tests/xrm-mock.helper.ts` | Shared Xrm mocks (`makeFormContext`, `makeExecutionContext`, `makeAttr`, `makeControl`, `setupXrm` and others) |
| `tests/bundle.contract.test.ts` | Checks that every built bundle exposes the handlers that the solution calls |
| `rollup.config.mjs` | `namespacesByFileName` maps each bundle to its `DefraImports.<Name>` namespace |
| `out/`, `dist/`, `coverage/`, `test-results/` | Build and test output. Never edit or commit |

A handler is called from the solution as `DefraImports.<Namespace>.<functionName>` in the web resource `defraimp_/js/<file>.js`. The solution packs the files from `dist/` through its mapping file.

## Commands

Run these from `src/webresources/scripts`. Restore with `npm ci --legacy-peer-deps` if `node_modules` is missing.

| Command | Use |
| --- | --- |
| `npm run lint` | ESLint over TypeScript. It passes today, so it must stay clean |
| `npm test` | Builds (`tsc` then rollup) and runs Jest with coverage, including the bundle contract test. This is what the pipeline runs |
| `npx prettier --check <files> --end-of-line auto` | Format check for the files you touched |

`npm run format:check` is not clean at baseline, because of line endings in Windows checkouts and a few existing files. Do not use it as the gate, and never run `npm run format` over the whole folder. Check only your own files, and match the surrounding code (Prettier defaults: double quotes, two-space indent, semicolons).

## Procedure

1. **Capture the contract.** Which form, ribbon button or subgrid calls the handler, from which event, with what parameters (`executionContext` or `primaryControl`), and what must happen on failure. Ask for what is missing.
2. **Decide whether script is needed.** Prefer declarative options first: business rules, form configuration, and workflows, actions or flows for server-side work. Add a script only when those cannot do it. If the request describes a business process, say so in the design proposal.
3. **Find the neighbours.** Read the script that is closest to the target (`postimportcheck.form.ts` for a form, `itahc.ribbon.ts` for a ribbon button calling an action) and its test.
4. **Propose the design and wait for approval.** State the files, the handler names and signatures, the namespace, and what the user must register.
5. **Implement.**
   - Add or change `src/<file>.ts`, exporting each handler.
   - For a new file, add it to `namespacesByFileName` in `rollup.config.mjs`. A file that is not listed is silently skipped and produces no bundle.
   - Add the file and its handler names to `bundles` in `tests/bundle.contract.test.ts`.
6. **Write the tests** in `tests/<file>.test.ts`. Cover the checklist below. Extend `xrm-mock.helper.ts` only when a test needs a mock it lacks, and keep existing helpers backwards compatible.
7. **Verify.** From `src/webresources/scripts`, run `npm run lint` and `npm test`. Both must pass, and `npx prettier --check` must pass for your files.
8. **Report,** including the hand-off below.

## Registration hand-off

Web resource and handler registration is solution metadata. Do not edit `src/solutions/defra_Imports/src/WebResources` or form and ribbon XML (except the form, ribbon and site map components that the Developer agent may edit by hand). Tell the user, in the report:

- the web resource name to create or update, for example `defraimp_/js/<file>.js`;
- for each handler, the library, the function in the form `DefraImports.<Namespace>.<function>`, the event it is bound to, and whether to pass the execution context (a handler that takes `executionContext` must be registered with "Pass execution context as first parameter");
- to register and wire these in their development environment, and to synchronise the solution metadata with `pipelines/sync-solution-metadata.yml`;
- that renaming or removing an exported function, or changing its signature, breaks forms and ribbons that already call it;
- that script changes outside the solution metadata folder need `+solutionVer(defra_Imports): <major|minor|patch>` in the commit body.

## Coding rules

- Use the Xrm typings (`@types/xrm`): `Xrm.FormContext`, `Xrm.Events.EventContext`, `Xrm.Attributes.*`, `Xrm.Controls.*`. Avoid `any` and non-null assertions; ESLint with type-aware rules is on.
- Get the form context from `executionContext.getFormContext()`. Never use the deprecated `Xrm.Page`.
- Resolve attributes and controls by logical name, and fail with a clear error when a required one is missing (see `requiredAttribute` in the templates).
- Use `Xrm.WebApi` for data access, selecting only the columns you need. Use `async` and `await`, and handle rejection: surface it with `Xrm.Navigation.openErrorDialog` or `openAlertDialog`. Do not leave a promise unhandled.
- Close any progress indicator you open in a `finally` block.
- Avoid module-level state. Where it is unavoidable, document why, and reset modules between tests (see `postimportcheck.form.test.ts`).
- Do not hard-code environment URLs, record GUIDs that differ between environments, secrets or personal data. A GUID for a configuration record that the solution ships is acceptable only where existing code already does so.
- Add no dependencies, and do not change `package.json` or `package-lock.json`, unless the user asks. The pipeline installs with `npm ci`, so the lock file must stay consistent.
- Add no suppressions (`eslint-disable`, `@ts-ignore`, `@ts-expect-error`) without the user's approval. Existing tests use a narrow `eslint-disable-next-line` only for `require` of a freshly reset module.
- Write comments in British English, and only where the code needs clarifying.

## Test checklist

- Each exported handler has a test for its main path and for each branch.
- A missing attribute, control or form type is handled as designed.
- Every `Xrm.WebApi`, navigation and utility call is asserted with its arguments, and a rejected call is tested.
- UI side effects are asserted (`setVisible`, `setValue`, notifications, dialogs), and so is the progress indicator being closed on failure.
- The bundle contract test lists every handler that the solution calls.
- The new tests do not depend on test order or on state from another test.
