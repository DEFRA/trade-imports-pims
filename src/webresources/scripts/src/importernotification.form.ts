const MULTIPLE_COMMODITY_ERROR_ID = "multipleCommodityError";
const MULTIPLE_COMMODITY_NOTIFICATION_ID = "multipleCommodityNotification";

type SaveMode = ReturnType<Xrm.Events.SaveEventArguments["getSaveMode"]>;

const TYPE_CVEDA = 714100000;
const PURPOSE_OF_CONSIGNMENT_RE_ENTRY = 714100009;
const SAVE_MODE_SAVE_AND_CLOSE = 2 as SaveMode;

export function showHideCharity(
  executionContext: Xrm.Events.EventContext
): void {
  const formContext = executionContext.getFormContext();

  const type = formContext
    .getAttribute<Xrm.Attributes.OptionSetAttribute>("defraimp_type")
    ?.getValue();

  if (type === TYPE_CVEDA) {
    formContext.data.entity.addOnSave(onSaveTriggerFunction);
    formContext.data.entity.addOnPostSave(onPostSaveTriggerFunction);
  }

  const importingFromCharity = formContext
    .getAttribute<Xrm.Attributes.BooleanAttribute>(
      "defraimp_importingfromcharity"
    )!
    .getValue();

  formContext.ui.tabs
    .get("Charity_Tab")!
    .setVisible(importingFromCharity ?? false);
}

export function checkForMultipleCommodities(
  executionContext: Xrm.Events.EventContext
): void {
  const formContext = executionContext.getFormContext();
  const hasMultipleCommodities = formContext
    .getAttribute<Xrm.Attributes.BooleanAttribute>(
      "defraimp_hasmultiplecommoditycodes"
    )!
    .getValue();
  if (hasMultipleCommodities === true) showCommodityWarning(formContext);
  else hideCommodityWarning(formContext);
}

function onSaveTriggerFunction(
  executionContext: Xrm.Events.SaveEventContext
): void {
  const eventArgs = executionContext.getEventArgs();
  const saveMode = eventArgs.getSaveMode();
  const formContext = executionContext.getFormContext();

  // Save & Close
  if (saveMode === SAVE_MODE_SAVE_AND_CLOSE) {
    formContext.data.entity.removeOnPostSave(onPostSaveTriggerFunction);
  }
}

function onPostSaveTriggerFunction(
  executionContext: Xrm.Events.EventContext
): void {
  const formContext = executionContext.getFormContext();

  const recordId = formContext.data.entity.getId();

  const type = formContext
    .getAttribute<Xrm.Attributes.OptionSetAttribute>("defraimp_type")
    ?.getValue();

  const purposeOfConsignment = formContext
    .getAttribute<Xrm.Attributes.OptionSetAttribute>(
      "defraimp_purposeofconsignment"
    )
    ?.getValue();

  if (
    type === TYPE_CVEDA &&
    purposeOfConsignment === PURPOSE_OF_CONSIGNMENT_RE_ENTRY
  ) {
    const entityFormOptions: Xrm.Navigation.EntityFormOptions = {
      entityName: "defraimp_importernotification",
      entityId: recordId,
    };

    void Xrm.Navigation.openForm(entityFormOptions);
  }
}

function showCommodityWarning(formContext: Xrm.FormContext): void {
  formContext.ui.tabs
    .get("details_tab")!
    .sections.get("caseworker_intervention_section")!
    .setVisible(true);
  const hasCaseworkerIntervened = formContext
    .getAttribute<Xrm.Attributes.BooleanAttribute>(
      "defraimp_caseworkerintervention"
    )!
    .getValue();
  // TODO: Centralise notification messages and identifiers when the form UX is next reviewed.
  if (hasCaseworkerIntervened !== true) {
    formContext.ui.clearFormNotification(MULTIPLE_COMMODITY_NOTIFICATION_ID);
    formContext.ui.setFormNotification(
      "More than 1 Commodity Code - No caseworker intervention",
      "ERROR",
      MULTIPLE_COMMODITY_ERROR_ID
    );
  } else {
    formContext.ui.clearFormNotification(MULTIPLE_COMMODITY_ERROR_ID);
    formContext.ui.setFormNotification(
      "More than 1 Commodity Code - caseworker has intervened",
      "INFO",
      MULTIPLE_COMMODITY_NOTIFICATION_ID
    );
  }
}

function hideCommodityWarning(formContext: Xrm.FormContext): void {
  formContext.ui.tabs
    .get("details_tab")!
    .sections.get("caseworker_intervention_section")!
    .setVisible(false);
  formContext.ui.clearFormNotification(MULTIPLE_COMMODITY_ERROR_ID);
  formContext.ui.clearFormNotification(MULTIPLE_COMMODITY_NOTIFICATION_ID);
}
