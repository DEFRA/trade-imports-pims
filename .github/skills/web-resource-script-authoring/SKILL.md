---
name: web-resource-script-authoring
description: Write, change or review the TypeScript web resource scripts in src/webresources/scripts (model-driven app form, ribbon and subgrid handlers) and their Jest tests. Covers generalising the requirement to a capability and naming handlers for it, passing what varies as registration data, building test-first with small single-purpose functions, the file and bundle conventions, the rollup namespace mapping and bundle contract test, Xrm typing and mocking, the lint and test commands, and the hand-off for registering the web resource and wiring the handler in a development environment. Use whenever a request adds or changes a form, ribbon or subgrid script. Owned by the Developer agent.
---

# Web resource script authoring

The scripts are TypeScript, bundled by rollup into one IIFE per file in `dist/`, and packed into the `defra_Imports` solution. Code standards are in [typescript-source.instructions.md](../../instructions/typescript-source.instructions.md) and [typescript-tests.instructions.md](../../instructions/typescript-tests.instructions.md). Code to adapt is in [templates.md](./references/templates.md).

## Layout

All paths are under `src/webresources/scripts`.

| Path | Purpose |
| --- | --- |
| `src/<table or capability>.<form\|ribbon\|subgrid.ribbon>.ts` | One script per web resource. Exported functions are the handlers that forms and ribbons call |
| `tests/<same name>.test.ts` | Jest tests for that script |
| `tests/xrm-mock.helper.ts` | Shared Xrm mocks (`makeFormContext`, `makeExecutionContext`, `makeAttr`, `makeControl`, `setupXrm` and others) |
| `tests/bundle.contract.test.ts` | Checks that every built bundle exposes the handlers that the solution calls |
| `rollup.config.mjs` | `namespacesByFileName` maps each bundle to its `DefraImports.<Name>` namespace |
| `out/`, `dist/`, `coverage/`, `test-results/` | Build and test output. Never edit or commit |

A handler is called from the solution as `DefraImports.<Namespace>.<functionName>` in the web resource `defraimp_/js/<file>.js`. The solution packs the files from `dist/` through its mapping file.

## Commands

Run these from `src/webresources/scripts`; the full list, including iterating on one file, is in [typescript-source.instructions.md](../../instructions/typescript-source.instructions.md#running-and-verifying).

| Command | Use |
| --- | --- |
| `npm run lint` | ESLint over TypeScript. It passes today, so it must stay clean |
| `npm test` | Builds (`tsc` then rollup) and runs Jest with coverage, including the bundle contract test. This is what the pipeline runs |
| `npm run format:check` | Prettier check over the project. It is clean, so it must stay clean. Fix with `npm run format` |

## Design principles for this skill

The Developer agent's Design Principles apply in full, and [typescript-source.instructions.md](../../instructions/typescript-source.instructions.md#design-principles) states them for scripts. In short: name handlers and files for the capability, pass what varies in as registration data, keep handlers thin over small pure functions, and let a failing test drive each behaviour.

## Procedure

1. **Generalise first.** Restate the requirement as the general capability it is an instance of, and write down the handler name. Search `src` for an existing generic handler or helper to reuse or extend before adding one. Ask: "If a second, similar form needed this next month, would a handler registration be enough?"
2. **Capture the contract.** Which form, ribbon button or subgrid calls the handler, from which event, with what parameters (`executionContext`, `primaryControl` and any registration parameters, which are the configuration), and what must happen on failure. Ask for what is missing.
3. **Decide whether script is needed.** Prefer declarative options first: business rules, form configuration, and workflows, actions or flows for server-side work. Add a script only when those cannot do it. If the request describes a business process, say so in the design proposal.
4. **Find the neighbours.** Read the script that is closest to the target (`postimportcheck.form.ts` for a form, `itahc.ribbon.ts` for a ribbon button calling an action) and its test.
5. **Propose the design and wait for approval.** State the capability, the files, the handler names and signatures, the namespace, what is configuration and its format, the ordered test list (simplest first, with a second-configuration test), what the user must register, and how the design meets each principle.
6. **Build test-first, one behaviour at a time.**
   - Write the test in `tests/<file>.test.ts`, run it and confirm that it fails for the right reason (red). Write the smallest code in `src/<file>.ts` that passes it (green), then refactor while the tests stay green.
   - For a new file, add it to `namespacesByFileName` in `rollup.config.mjs`. A file that is not listed is silently skipped and produces no bundle. Add the file and its handler names to `bundles` in `tests/bundle.contract.test.ts`.
   - Extend `xrm-mock.helper.ts` only when a test needs a mock it lacks, and keep existing helpers backwards compatible.
7. **Verify.** From `src/webresources/scripts`, run `npm run format`, then `npm run lint`, `npm run format:check` and `npm test`. All must pass. Cover the checklist below.
8. **Report,** including how the design meets the principles, the parameters to supply, and the hand-off below.

## Registration hand-off

Web resource and handler registration is solution metadata. Do not edit `src/solutions/defra_Imports/src/WebResources` or form and ribbon XML (except the form, ribbon and site map components that the Developer agent may edit by hand). Tell the user, in the report:

- the web resource name to create or update, for example `defraimp_/js/<file>.js`;
- for each handler, the library, the function in the form `DefraImports.<Namespace>.<function>`, the event it is bound to, the exact parameters (the configuration) to enter, and whether to pass the execution context (a handler that takes `executionContext` must be registered with "Pass execution context as first parameter"). Reusing the handler on another form is another registration with its own parameters;
- to register and wire these in their development environment, and to synchronise the solution metadata with `pipelines/sync-solution-metadata.yml`;
- that renaming or removing an exported function, or changing its signature, breaks forms and ribbons that already call it;
- that script changes outside the solution metadata folder need `+solutionVer(defra_Imports): <major|minor|patch>` in the commit body.

## Coding rules

Follow [typescript-source.instructions.md](../../instructions/typescript-source.instructions.md) for the script and [typescript-tests.instructions.md](../../instructions/typescript-tests.instructions.md) for the tests. They cover typing, Xrm usage, error handling, data-driven design, linting, formatting, suppressions and dependencies. This skill is the procedure and does not repeat them.

## Test checklist

- Tests were written first, and each was seen to fail before its code existed.
- A test supplies a second, different configuration and shows that the behaviour changes with no code change.
- Missing, malformed or incomplete configuration fails with a clear error.
- Each exported handler has a test for its main path and for each branch.
- A missing attribute, control or form type is handled as designed.
- Every `Xrm.WebApi`, navigation and utility call is asserted with its arguments, and a rejected call is tested.
- UI side effects are asserted (`setVisible`, `setValue`, notifications, dialogs), and so is the progress indicator being closed on failure.
- The bundle contract test lists every handler that the solution calls.
- The new tests do not depend on test order or on state from another test.
