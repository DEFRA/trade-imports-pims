const FORM_TYPE_CREATE = 1;
const RELATED_IMPORTER_NOTIFICATION_ATTRIBUTE = 'defraimp_relatedimporternotification';
const RELATED_IMPORT_RECORD_ATTRIBUTE = 'defraimp_relatedimportapplication';
const MISSING_RELATED_RECORD_NOTIFICATION_ID = 'missingRelatedRecordError';
const MISSING_RELATED_RECORD_MESSAGE =
  "Please complete either the 'Related Importer Notification' or 'Related Import Record' field";

let currentFormContext: Xrm.FormContext | undefined;

export function onLoad(executionContext: Xrm.Events.EventContext): void {
  currentFormContext = executionContext.getFormContext();
  showHideSampleTestsBasedOnSamplingRequired(currentFormContext);
  registerRelatedRecordValidation(currentFormContext);
}

// Quick create forms cannot host the samples grid, so they register only the save validation.
export function onLoadQuickCreate(executionContext: Xrm.Events.EventContext): void {
  registerRelatedRecordValidation(executionContext.getFormContext());
}

function registerRelatedRecordValidation(formContext: Xrm.FormContext): void {
  if (formContext.ui.getFormType() === FORM_TYPE_CREATE) {
    formContext.data.entity.addOnSave(onSaveRequireRelatedRecord);
  }
}

function onSaveRequireRelatedRecord(executionContext: Xrm.Events.SaveEventContext): void {
  const formContext = executionContext.getFormContext();
  const hasImporterNotification = hasLookupValue(formContext, RELATED_IMPORTER_NOTIFICATION_ATTRIBUTE);
  const hasImportRecord = hasLookupValue(formContext, RELATED_IMPORT_RECORD_ATTRIBUTE);

  if (hasImporterNotification || hasImportRecord) {
    formContext.ui.clearFormNotification(MISSING_RELATED_RECORD_NOTIFICATION_ID);
    return;
  }

  executionContext.getEventArgs().preventDefault();
  formContext.ui.setFormNotification(
    MISSING_RELATED_RECORD_MESSAGE,
    'ERROR',
    MISSING_RELATED_RECORD_NOTIFICATION_ID
  );
}

function hasLookupValue(formContext: Xrm.FormContext, name: string): boolean {
  const value = requiredAttribute<Xrm.Attributes.LookupAttribute>(formContext, name).getValue();
  return value !== null && value.length > 0;
}

// The solution registration intentionally does not pass execution context to this handler.
export function onChangeSamplingRequired(): void {
  if (!currentFormContext) {
    return;
  }

  showHideSampleTestsBasedOnSamplingRequired(currentFormContext);
}

function showHideSampleTestsBasedOnSamplingRequired(context: Xrm.FormContext): void {
  const isSamplingRequired = requiredAttribute<Xrm.Attributes.BooleanAttribute>(
    context,
    'defraimp_samplingrequired'
  ).getValue();
  requiredGridControl(context, 'SamplesTestsRequired').setVisible(isSamplingRequired === true);
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

function requiredGridControl(formContext: Xrm.FormContext, name: string): Xrm.Controls.GridControl {
  const control = formContext.getControl<Xrm.Controls.GridControl>(name);
  if (!control) {
    throw new Error(`Required grid control '${name}' was not found.`);
  }

  return control;
}
