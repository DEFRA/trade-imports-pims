# Web resource script templates

Adapt these to the approved design. Replace the file name, namespace, attribute and handler names with the real ones. They are illustrative: copy the structure, not the behaviour. A version of these files was run against the repository's lint and Jest configuration.

## Script

`src/itahc.form.ts`. Exported functions are the handlers. Helpers that are not exported stay private to the bundle.

```ts
export function onLoad(executionContext: Xrm.Events.EventContext): void {
  const formContext = executionContext.getFormContext();
  const name = requiredAttribute<Xrm.Attributes.StringAttribute>(
    formContext,
    "defraimp_name"
  );
  requiredControl<Xrm.Controls.StringControl>(
    formContext,
    "defraimp_name"
  ).setVisible(name.getValue() !== null);
}

function requiredAttribute<T extends Xrm.Attributes.Attribute>(
  formContext: Xrm.FormContext,
  name: string
): T {
  const attribute = formContext.getAttribute<T>(name);
  if (!attribute) {
    throw new Error(`Required attribute '${name}' was not found.`);
  }

  return attribute;
}

function requiredControl<T extends Xrm.Controls.Control>(
  formContext: Xrm.FormContext,
  name: string
): T {
  const control = formContext.getControl<T>(name);
  if (!control) {
    throw new Error(`Required control '${name}' was not found.`);
  }

  return control;
}
```

For a ribbon button, export a function that takes `primaryControl: Xrm.FormContext` (or the selected rows for a subgrid) and follow `itahc.ribbon.ts`: wrap the work in `try`, `catch` and `finally`, report errors with `Xrm.Navigation.openErrorDialog`, and close the progress indicator in `finally`.

## Bundle registration

Add the file to `namespacesByFileName` in `rollup.config.mjs`. A namespace can be shared by several files, because the bundle extends the global object.

```js
  "itahc.form.js": "DefraImports.Itahc",
```

Add the handlers to `bundles` in `tests/bundle.contract.test.ts`.

```ts
  ["itahc.form.js", "Itahc", ["onLoad"]],
```

The handler is then called from the solution as `DefraImports.Itahc.onLoad` in the web resource `defraimp_/js/itahc.form.js`.

## Test

`tests/itahc.form.test.ts`. The helpers in `xrm-mock.helper.ts` build the form context, the execution context, attributes and controls, and install a global `Xrm`.

```ts
import {
  makeAttr,
  makeControl,
  makeExecutionContext,
  makeFormContext,
  setupXrm,
} from "./xrm-mock.helper";
import { onLoad } from "../src/itahc.form";

beforeEach(() => {
  setupXrm();
});

function buildContext(name: string | null) {
  const control = makeControl();
  const formContext = makeFormContext(
    { defraimp_name: makeAttr(name) },
    { defraimp_name: control }
  );
  return {
    control,
    executionContext: makeExecutionContext(
      formContext
    ) as unknown as Xrm.Events.EventContext,
  };
}

describe("onLoad", () => {
  test("shows the name control when a name is set", () => {
    const { control, executionContext } = buildContext("CHEDA.GB.2026.0000001");
    onLoad(executionContext);
    expect(control.setVisible).toHaveBeenCalledWith(true);
  });

  test("hides the name control when no name is set", () => {
    const { control, executionContext } = buildContext(null);
    onLoad(executionContext);
    expect(control.setVisible).toHaveBeenCalledWith(false);
  });

  test("throws when the name attribute is missing", () => {
    const formContext = makeFormContext();
    const executionContext = makeExecutionContext(
      formContext
    ) as unknown as Xrm.Events.EventContext;
    expect(() => onLoad(executionContext)).toThrow(
      "Required attribute 'defraimp_name' was not found."
    );
  });
});
```

For async handlers, `await` the call and use `mockResolvedValue` and `mockRejectedValue` on `xrm.WebApi.*`, as `itahc.ribbon.test.ts` does. Where the script keeps module-level state, reset modules before each test and `require` the script afresh, as `postimportcheck.form.test.ts` does.
