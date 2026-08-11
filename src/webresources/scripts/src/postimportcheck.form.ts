let currentFormContext: Xrm.FormContext | undefined;

export function onLoad(executionContext: Xrm.Events.EventContext): void {
  currentFormContext = executionContext.getFormContext();
  showHideSampleTestsBasedOnSamplingRequired(currentFormContext);
}

// The solution registration intentionally does not pass execution context to this handler.
export function onChangeSamplingRequired(): void {
  if (!currentFormContext) {
    return;
  }

  showHideSampleTestsBasedOnSamplingRequired(currentFormContext);
}

function showHideSampleTestsBasedOnSamplingRequired(
  context: Xrm.FormContext
): void {
  const isSamplingRequired = requiredAttribute<Xrm.Attributes.BooleanAttribute>(
    context,
    "defraimp_samplingrequired"
  ).getValue();
  requiredGridControl(context, "SamplesTestsRequired").setVisible(
    isSamplingRequired === true
  );
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

function requiredGridControl(
  formContext: Xrm.FormContext,
  name: string
): Xrm.Controls.GridControl {
  const control = formContext.getControl<Xrm.Controls.GridControl>(name);
  if (!control) {
    throw new Error(`Required grid control '${name}' was not found.`);
  }

  return control;
}
