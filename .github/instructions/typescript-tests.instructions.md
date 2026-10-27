---
description: "Use when writing, reviewing or updating Jest tests under src/webresources/scripts/tests for the TypeScript web resource scripts. Covers test-driven development, the Xrm mock helpers, test structure and naming, mocking Xrm.WebApi and the form context, testing data-driven handlers, module-level state, the bundle contract test and how to run the tests."
applyTo: "src/webresources/scripts/tests/**/*.ts"
---

# TypeScript Test Standards

The tests use Jest 30 with `ts-jest` in a Node environment. Xrm is not available, so each test installs a mock. They compile with `tsconfig.jest.json`, which extends `tsconfig.json` (strict) and adds the `jest`, `node` and `xrm` types. Jest collects coverage for `src/**/*.ts` and writes a JUnit report to `test-results/junit.xml`, which the pipeline publishes.

The production code conventions are in [typescript-source.instructions.md](./typescript-source.instructions.md).

## Test-driven development

Tests drive the design. Work red, green, refactor, one behaviour at a time:

1. **Red.** Write one test that states a behaviour, run it and confirm that it fails for the right reason. For a new script, the first red is a missing module; add the smallest export so that the test then fails on the behaviour.
2. **Green.** Write the smallest production code that makes it pass. Run the tests again.
3. **Refactor.** Remove duplication and improve names while the tests stay green. Do not change behaviour while refactoring.

Rules:

- Test behaviour through the exported handler, not private functions or call order, unless the call is the behaviour (a `WebApi` request that must or must not happen).
- Order the first tests from the simplest case to the most general: the no-op case, one configured rule, then several.
- Every data-driven handler has a test that supplies a second, different configuration and shows that the behaviour changes with no code change. This proves that nothing is hard-coded.
- Cover invalid configuration with `test.each`: empty, malformed and incomplete input must fail with a clear error.
- Hard-to-test code is a design signal: pass the dependency in or extract a function instead of adding a workaround.
- A bug fix starts with a test that reproduces the bug and fails.

## Conventions

- One test file per script: `tests/<script name>.test.ts` for `src/<script name>.ts`, importing the script by its relative path (`../src/<script name>`).
- Group with `describe` named for the exported handler. Name each `test` as a sentence that states the behaviour, for example `"shows the controls when the attribute equals the configured value"`.
- Structure each test as arrange, act, assert. Keep one behaviour in each test.
- Use `beforeEach` to install a fresh `Xrm` with `setupXrm()`, and build test data in small helper functions at the top of the file, as the existing tests do. Do not share mutable state between tests, and do not depend on test order.
- Type the tests. Cast a hand-built mock with `as unknown as Xrm.FormContext` (or the relevant type) at the call, not with `any`.
- Do not add suppressions (`eslint-disable`, `@ts-ignore`) without the user's approval. The one accepted exception is the narrow `eslint-disable-next-line @typescript-eslint/no-require-imports, @typescript-eslint/no-unsafe-assignment` on a `require` of a script after `jest.resetModules()`, as in `postimportcheck.form.test.ts`.
- Format with Prettier defaults, as in the source (`npx prettier --check <files> --end-of-line auto`). Write test names in British English.

## Mocks and test data

- Use the helpers in `tests/xrm-mock.helper.ts`: `setupXrm`, `makeFormContext`, `makeExecutionContext`, `makeAttr`, `makeControl`, `makeTab`, `makeSection`, `makeSelectedRows` and `flushPromises`. Extend the file only when a test needs a mock it lacks, and keep existing helpers backwards compatible. Add a helper there when two test files need it.
- `setupXrm()` returns the mock and installs it on `global.Xrm`. Set results with `mockResolvedValue` and `mockRejectedValue` on `xrm.WebApi.*`, `xrm.Navigation.*` and `xrm.Utility.*`.
- Assert every `Xrm.WebApi`, navigation and utility call with its arguments (`toHaveBeenCalledWith`), and assert the calls that must not happen (`not.toHaveBeenCalled`).
- Assert UI effects (`setVisible`, `setValue`, notifications, dialogs) and that a progress indicator is closed on failure as well as on success.
- For async handlers, `await` the call, and test the rejected path as well as the resolved one.
- Do not use real timers, the network or the file system, apart from the bundle contract test, which reads `dist/`.
- Use only constants, generated names and values that the test owns. Do not copy production GUIDs unless the script hard-codes the same record and the test pins it.

## Module-level state

A script that keeps module-level state, such as `postimportcheck.form.ts`, leaks it between tests. Reset the modules in `beforeEach` and `require` the script afresh:

```ts
let subject: typeof import("../src/postimportcheck.form");

beforeEach(() => {
  jest.resetModules();
  // eslint-disable-next-line @typescript-eslint/no-require-imports, @typescript-eslint/no-unsafe-assignment
  subject = require("../src/postimportcheck.form");
});
```

Prefer removing the module state from the script over adding this to a new one.

## The bundle contract test

`tests/bundle.contract.test.ts` runs each built bundle in `dist/` in a fresh context and checks that every handler the solution calls is exposed on `DefraImports.<Namespace>` as a function. It needs `npm run build`, which `npm test` runs first.

- Add every new script to `bundles`, with its file name, namespace and handler names. Keep it in step with `namespacesByFileName` in `rollup.config.mjs`.
- Removing or renaming an exported handler breaks forms and ribbons that call it, and this test is what catches it. Do not edit an entry to make the test pass without confirming that the solution no longer calls that handler.

## What to test

- **Form handler:** each branch of the logic; a missing attribute, control or form type; the attributes and controls that are changed.
- **Ribbon or subgrid handler:** the main path, the rejected path, the progress indicator closing in `finally`, and the arguments passed to `Xrm.WebApi` or the action.
- **Data-driven handler:** a valid configuration, a second configuration, and each kind of invalid configuration.
- **Pure helper:** each case, with `test.each` for a set of equivalent inputs.

## Running the tests

From `src/webresources/scripts`:

- One file while iterating: `npx jest tests/<file>.test.ts --coverage=false`. It does not build the bundles.
- The full run, as the pipeline does it: `npm test`. It builds first, so it also compiles the source and runs the bundle contract test.
- Lint: `npm run lint`. It covers the test files too, because `tsconfig.jest.json` is one of its projects.
