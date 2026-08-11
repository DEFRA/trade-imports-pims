const MANUAL_POST_IMPORT_CHECK_BLANK_ERROR_MSG =
  "'Manual Post Import Check Decision' must be populated.";
type FormType = ReturnType<Xrm.Ui["getFormType"]>;

const ManualDecision = {
  Other: 714100000,
  DoNotCheck: 714100001,
  TB: 714100002,
  Quarantine: 714100003,
  System: 714100004,
} as const;
const InspectionRequired = { Yes: 714100000, No: 714100001 } as const;
const InspectionReason = {
  NoInspectionRequired: 714100004,
  Quarantine: 714100010,
  TB: 714100011,
  ManuallyRequested: 714100013,
} as const;
const ImportApplicationType = {
  ITAHC: 714100000,
  Notification: 714100001,
  CHEDA: 714100002,
  CHEDP: 714100003,
  DOCOM: 714100004,
  ITAHCLandbridge: 714100005,
} as const;
const FORM_TYPE_CREATE = 1 as FormType;
const nonComplianceOtherType = 714100005;
const manualDecisionWasSetByForm = new WeakMap<Xrm.FormContext, boolean>();
const errorDialogOpenByForm = new WeakSet<Xrm.FormContext>();

export function OnLoadQuickCreateForm(
  executionContext: Xrm.Events.EventContext
): void {
  const formContext = executionContext.getFormContext();
  if (formContext.ui.getFormType() === FORM_TYPE_CREATE)
    formContext
      .getAttribute<Xrm.Attributes.LookupAttribute>("ownerid")!
      .setValue(null);
}

export function onLoad(executionContext: Xrm.Events.EventContext): void {
  const formContext = executionContext.getFormContext();
  storeWasManualPostImportCheckSet(formContext);
  showOrHideNonComplianceTab(
    formContext,
    booleanValue(formContext, "defraimp_isnoncompliantcalculated")
  );
  showOrHideNonComplianceOther(executionContext);
}

export function onSave(executionContext: Xrm.Events.SaveEventContext): void {
  const formContext = executionContext.getFormContext();
  preventSaveIfPostImportChecksIsUpdatedToBlank(executionContext, formContext);
  storeWasManualPostImportCheckSet(formContext);
}

export function onChangeOfManualPostImportCheckDecision(
  executionContext: Xrm.Events.EventContext
): void {
  setSystemDeterminedInspectionValues(executionContext.getFormContext());
}

export function showOrHideNonComplianceOther(
  executionContext: Xrm.Events.EventContext
): void {
  const formContext = executionContext.getFormContext();
  const values = formContext
    .getAttribute<Xrm.Attributes.MultiSelectOptionSetAttribute>(
      "defraimp_typesofnoncompliance"
    )!
    .getValue();
  formContext
    .getControl<Xrm.Controls.StandardControl>(
      "defraimp_noncomplianceothercomments"
    )!
    .setVisible(values?.includes(nonComplianceOtherType) === true);
}

export function onChangeOfMoveToCompletion(
  executionContext: Xrm.Events.EventContext
): void {
  const formContext = executionContext.getFormContext();
  const move = booleanValue(formContext, "defraimp_movetocompletion");
  formContext
    .getAttribute<Xrm.Attributes.DateAttribute>(
      "defraimp_movedtocompletiondate"
    )!
    .setValue(move ? new Date() : null);
}

export function showRelevantSections(
  executionContext: Xrm.Events.EventContext
): void {
  const formContext = executionContext.getFormContext();
  formContext.ui.tabs
    .get("Charity_Tab")!
    .setVisible(booleanValue(formContext, "defraimp_importingfromcharity"));
  const type = formContext
    .getAttribute<Xrm.Attributes.OptionSetAttribute>(
      "defraimp_importapplicationtype"
    )!
    .getValue();
  const isItahc =
    type === ImportApplicationType.ITAHC ||
    type === ImportApplicationType.ITAHCLandbridge;
  formContext.ui.tabs.get("AdditionalITAHC_Tab")!.setVisible(isItahc);
  formContext.ui.tabs
    .get("Summary")!
    .sections.get("iv66_section")!
    .setVisible(isItahc || type === ImportApplicationType.Notification);
  formContext.ui.tabs
    .get("Summary")!
    .sections.get("cheda_section")!
    .setVisible(type === ImportApplicationType.CHEDA);
  formContext.ui.tabs
    .get("Summary")!
    .sections.get("chedp_section")!
    .setVisible(type === ImportApplicationType.CHEDP);
  formContext.ui.tabs
    .get("Summary")!
    .sections.get("chedp_controls_section")!
    .setVisible(type === ImportApplicationType.CHEDP);
  formContext.ui.tabs
    .get("Transporter_Tab")!
    .sections.get("transport_information_section")!
    .setVisible(
      type === ImportApplicationType.CHEDA ||
        type === ImportApplicationType.CHEDP
    );
}

function storeWasManualPostImportCheckSet(formContext: Xrm.FormContext): void {
  const decision = formContext
    .getAttribute<Xrm.Attributes.OptionSetAttribute>(
      "defraimp_manualpostimportcheckdecision"
    )!
    .getValue();

  if (decision !== null) {
    manualDecisionWasSetByForm.set(formContext, true);
  }
}

function setSystemDeterminedInspectionValues(
  formContext: Xrm.FormContext
): void {
  const decision = formContext
    .getAttribute<Xrm.Attributes.OptionSetAttribute>(
      "defraimp_manualpostimportcheckdecision"
    )!
    .getValue();

  switch (decision) {
    case ManualDecision.System:
      setOption(
        formContext,
        "defraimp_inspectionrequired",
        optionValue(formContext, "defraimp_inspectionrequiredoriginalvalue")
      );
      setOption(
        formContext,
        "defraimp_inspectionrequiredreason",
        optionValue(
          formContext,
          "defraimp_inspectionrequiredreasonoriginalvalue"
        )
      );
      return;
    case ManualDecision.Other:
      setInspection(
        formContext,
        InspectionRequired.Yes,
        InspectionReason.ManuallyRequested
      );
      return;
    case ManualDecision.Quarantine:
      setInspection(
        formContext,
        InspectionRequired.Yes,
        InspectionReason.Quarantine
      );
      return;
    case ManualDecision.TB:
      setInspection(
        formContext,
        InspectionRequired.No,
        InspectionReason.NoInspectionRequired
      );
      return;
    case ManualDecision.DoNotCheck: {
      setInspection(
        formContext,
        InspectionRequired.No,
        InspectionReason.NoInspectionRequired
      );
      const declined = formContext.getAttribute<Xrm.Attributes.StringAttribute>(
        "defraimp_inspectiondeclinedreason"
      )!;
      if (declined.getValue() === null || declined.getValue() === "") {
        declined.setValue("System Required Post Import Check Skipped");
      }
      return;
    }
    default:
      return;
  }
}

function preventSaveIfPostImportChecksIsUpdatedToBlank(
  executionContext: Xrm.Events.SaveEventContext,
  formContext: Xrm.FormContext
): void {
  const wasManualPostImportCheckSet =
    manualDecisionWasSetByForm.get(formContext) === true;

  if (
    wasManualPostImportCheckSet &&
    formContext
      .getAttribute<Xrm.Attributes.OptionSetAttribute>(
        "defraimp_manualpostimportcheckdecision"
      )!
      .getValue() === null
  ) {
    executionContext.getEventArgs().preventDefault();
    if (!errorDialogOpenByForm.has(formContext)) {
      void displayManualPostImportCheckDecisionErrorMessage(formContext);
    }
  }
}

async function displayManualPostImportCheckDecisionErrorMessage(
  formContext: Xrm.FormContext
): Promise<void> {
  errorDialogOpenByForm.add(formContext);
  try {
    await Xrm.Navigation.openErrorDialog({
      message: MANUAL_POST_IMPORT_CHECK_BLANK_ERROR_MSG,
    });
  } finally {
    errorDialogOpenByForm.delete(formContext);
  }
}

function setInspection(
  formContext: Xrm.FormContext,
  required: number,
  reason: number
): void {
  setOption(formContext, "defraimp_inspectionrequired", required);
  setOption(formContext, "defraimp_inspectionrequiredreason", reason);
}

function setOption(
  formContext: Xrm.FormContext,
  name: string,
  value: number | null
): void {
  formContext
    .getAttribute<Xrm.Attributes.OptionSetAttribute>(name)!
    .setValue(value);
}

function optionValue(
  formContext: Xrm.FormContext,
  name: string
): number | null {
  return formContext
    .getAttribute<Xrm.Attributes.NumberAttribute>(name)!
    .getValue();
}

function booleanValue(formContext: Xrm.FormContext, name: string): boolean {
  return (
    formContext
      .getAttribute<Xrm.Attributes.BooleanAttribute>(name)!
      .getValue() === true
  );
}

function showOrHideNonComplianceTab(
  formContext: Xrm.FormContext,
  visible: boolean
): void {
  formContext.ui.tabs.get("NonCompliance_Tab")!.setVisible(visible);
}
