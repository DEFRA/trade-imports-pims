# Web resource script templates

A data-driven form script, shown in the order you build it: tests first, then the smallest code that passes them. The code was run against this repository's ESLint, Prettier, Jest and rollup bundle contract test with no new errors.

The capability is "show or hide controls according to rules". It is named for that capability, and the rules are JSON that the form's handler registration supplies. The first requirement (for example, hide a section when a status is closed) is one rule, and the next similar requirement is another handler registration, not another script.

Adapt the shape, not the behaviour. A handler that takes configuration is registered with "Pass execution context as first parameter" and the JSON string in the handler's comma-separated parameters.

## Order of work

1. Test: the simplest case, then a second configuration, then invalid input.
2. Script: the smallest code that passes each test.
3. Bundle registration, so that the script is built and its handler checked.

Run each test and confirm that it fails for the right reason before writing the code for it. For TypeScript, a missing module is the first red; add the smallest export so that the test fails on behaviour.

## 1. Test

`tests/visibility.form.test.ts`. The helpers in `xrm-mock.helper.ts` build the form context, the execution context, attributes and controls, and install a global `Xrm`. The third test is the proof that nothing is hard-coded; keep one in every data-driven script.

```ts
import {
  makeAttr,
  makeControl,
  makeExecutionContext,
  makeFormContext,
  setupXrm,
} from "./xrm-mock.helper";
import { applyVisibilityRules } from "../src/visibility.form";

beforeEach(() => {
  setupXrm();
});

function buildContext(value: unknown) {
  const first = makeControl();
  const second = makeControl();
  const formContext = makeFormContext(
    { status: makeAttr(value) },
    { first, second }
  );
  return {
    first,
    second,
    executionContext: makeExecutionContext(
      formContext
    ) as unknown as Xrm.Events.EventContext,
  };
}

const rules = JSON.stringify([
  { attribute: "status", equals: "open", controls: ["first", "second"] },
]);

describe("applyVisibilityRules", () => {
  test("shows the controls when the attribute equals the configured value", () => {
    const { first, second, executionContext } = buildContext("open");
    applyVisibilityRules(executionContext, rules);
    expect(first.setVisible).toHaveBeenCalledWith(true);
    expect(second.setVisible).toHaveBeenCalledWith(true);
  });

  test("hides the controls when it does not", () => {
    const { first, executionContext } = buildContext("closed");
    applyVisibilityRules(executionContext, rules);
    expect(first.setVisible).toHaveBeenCalledWith(false);
  });

  test("a different configuration drives different behaviour with no code change", () => {
    const { first, second, executionContext } = buildContext(2);
    applyVisibilityRules(
      executionContext,
      JSON.stringify([{ attribute: "status", equals: 2, controls: ["second"] }])
    );
    expect(second.setVisible).toHaveBeenCalledWith(true);
    expect(first.setVisible).not.toHaveBeenCalled();
  });

  test.each(["", "not json", "{}", "[{}]"])(
    "rejects invalid configuration %p",
    (config) => {
      const { executionContext } = buildContext("open");
      expect(() => applyVisibilityRules(executionContext, config)).toThrow();
    }
  );

  test("throws a clear error when a configured control is missing", () => {
    const { executionContext } = buildContext("open");
    expect(() =>
      applyVisibilityRules(
        executionContext,
        JSON.stringify([
          { attribute: "status", equals: "open", controls: ["missing"] },
        ])
      )
    ).toThrow("Required control 'missing' was not found.");
  });
});
```

## 2. Script

`src/visibility.form.ts`. Exported functions are the handlers. The configuration is parsed and validated in one place, and a bad configuration fails with a clear message instead of doing nothing.

```ts
export interface VisibilityRule {
  attribute: string;
  equals: string | number | boolean | null;
  controls: string[];
}

/**
 * Shows or hides controls according to rules supplied as JSON when the handler is registered,
 * for example [{"attribute":"status","equals":"open","controls":["first","second"]}].
 * The rules are configuration, so a form needs no code change to use a different set.
 */
export function applyVisibilityRules(
  executionContext: Xrm.Events.EventContext,
  rulesJson: string
): void {
  const formContext = executionContext.getFormContext();
  for (const rule of parseRules(rulesJson)) {
    const value = requiredAttribute(formContext, rule.attribute).getValue();
    const visible = value === rule.equals;
    for (const controlName of rule.controls) {
      requiredControl(formContext, controlName).setVisible(visible);
    }
  }
}

function parseRules(rulesJson: string): VisibilityRule[] {
  let parsed: unknown;
  try {
    parsed = JSON.parse(rulesJson);
  } catch {
    throw new Error("The visibility rules are not valid JSON.");
  }

  if (!Array.isArray(parsed) || parsed.length === 0) {
    throw new Error("The visibility rules must be a non-empty array.");
  }

  return parsed.map((item: unknown) => {
    const rule = item as Partial<VisibilityRule>;
    if (
      typeof rule.attribute !== "string" ||
      !Array.isArray(rule.controls) ||
      rule.controls.length === 0 ||
      !("equals" in rule)
    ) {
      throw new Error(
        "Each visibility rule needs an attribute, an equals value and controls."
      );
    }

    return rule as VisibilityRule;
  });
}

function requiredAttribute(
  formContext: Xrm.FormContext,
  name: string
): Xrm.Attributes.Attribute {
  const attribute = formContext.getAttribute(name);
  if (!attribute) {
    throw new Error(`Required attribute '${name}' was not found.`);
  }

  return attribute;
}

function requiredControl(
  formContext: Xrm.FormContext,
  name: string
): Xrm.Controls.StandardControl {
  const control = formContext.getControl<Xrm.Controls.StandardControl>(name);
  if (!control) {
    throw new Error(`Required control '${name}' was not found.`);
  }

  return control;
}
```

## 3. Bundle registration

Add the file to `namespacesByFileName` in `rollup.config.mjs`. A file that is not listed is silently skipped. A namespace can be shared by several files, because the bundle extends the global object.

```js
  "visibility.form.js": "DefraImports.Visibility",
```

Add the handlers to `bundles` in `tests/bundle.contract.test.ts`.

```ts
  ["visibility.form.js", "Visibility", ["applyVisibilityRules"]],
```

The handler is then called from the solution as `DefraImports.Visibility.applyVisibilityRules` in the web resource `defraimp_/js/visibility.form.js`, with the rules JSON as its parameter.

## Other shapes

For a ribbon button that calls an action, export a function that takes `primaryControl: Xrm.FormContext` and follow `itahc.ribbon.ts`: wrap the work in `try`, `catch` and `finally`, report errors with `Xrm.Navigation.openErrorDialog`, close the progress indicator in `finally`, and take the action name or any record GUID as a parameter or read it from a configuration record rather than hard-coding it. For async handlers, `await` the call and use `mockResolvedValue` and `mockRejectedValue` on `xrm.WebApi.*`, as `itahc.ribbon.test.ts` does. Where a script keeps module-level state, reset modules before each test and `require` the script afresh, as `postimportcheck.form.test.ts` does.
