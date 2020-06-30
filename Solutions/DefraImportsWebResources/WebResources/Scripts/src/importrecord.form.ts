namespace DefraImports.ImportRecord {

  const MANUAL_POST_IMPORT_CHECK_BLANK_ERROR_MSG: string = "'Manual Post Import Check Decision' must be populated.";

  let wasManualPostImportCheckSet: boolean = false;
  let isErrorDialogDisplaying: boolean = false

  export function OnLoadQuickCreateForm(executionObj: Xrm.ExecutionContext<any>) {
    let formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Quick.Information;

    if (formContext.ui.getFormType() === Xrm.FormType.Create) {
      formContext.getAttribute("ownerid").setValue(null);
    }
  }

  export function onLoad(executionObj: Xrm.ExecutionContext<any>) {
    let formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Main.Information;

    storeWasManualPostImportCheckSet(formContext);
  }

  export function onSave(executionObj: Xrm.ExecutionContext<any>) {
    const formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Main.Information;

    preventSaveIfPostImportChecksIsUpdatedToBlank(executionObj);
    storeWasManualPostImportCheckSet(formContext);
  }

  function storeWasManualPostImportCheckSet(formContext: Form.defraimp_importapplication.Main.Information) {
    let manualPostImportCheckAttr = formContext.getAttribute("defraimp_manualpostimportcheckdecision");

    if (manualPostImportCheckAttr.getValue() !== null) {
      wasManualPostImportCheckSet = true;
    }
  }

  function preventSaveIfPostImportChecksIsUpdatedToBlank(executionObj: Xrm.ExecutionContext<any>) {
    let formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Main.Information;
    let currentManualPostImportCheckAttr = formContext.getAttribute("defraimp_manualpostimportcheckdecision");

    if (wasManualPostImportCheckSet && currentManualPostImportCheckAttr.getValue() === null) {
      executionObj.getEventArgs().preventDefault();
      if (!isErrorDialogDisplaying) {
        displayManualPostImportCheckDecisionErrorMessage();
      }
    }
  }

  function displayManualPostImportCheckDecisionErrorMessage() {
    let errorMessage: string = MANUAL_POST_IMPORT_CHECK_BLANK_ERROR_MSG;
    isErrorDialogDisplaying = true;
    Xrm.Navigation.openErrorDialog({ message: errorMessage }).then(
      function (success) {
        isErrorDialogDisplaying = false;
      },
      function (error) {

      });
  }

  export function onChangeOfMoveToCompletion(executionObj: Xrm.ExecutionContext<any>): void {
    let formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Main.Information;
    populateMoveToCompletionDate(formContext);
  }

  function populateMoveToCompletionDate(formContext: Form.defraimp_importapplication.Main.Information): void {
    const moveToCompletionVal = formContext.getAttribute("defraimp_movetocompletion").getValue();
    const moveCompletionDateAttr = formContext.getAttribute("defraimp_movedtocompletiondate");
    if (moveToCompletionVal) {
      const currentDate = new Date();
      moveCompletionDateAttr.setValue(currentDate);
    }
    else {
      moveCompletionDateAttr.setValue(null);
    }
  }

  export function showRelevantSections(executionObj: Xrm.ExecutionContext<any>)
  {
    let formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Main.Information;
    const importApplicationType = formContext.getAttribute("defraimp_importapplicationtype").getValue();

    if (importApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC)
    {
      //Hide any existing sections first
        hideCHEDASections(formContext);
        hideCHEDPSections(formContext);

        //Show the ITAHC section
        showITAHCSections(formContext);
    }
    else if (importApplicationType == defraimp_importapplication_defraimp_importapplicationtype.CHEDA)
    {
      //Hide any existing sections first
      hideITAHCSections(formContext);
      hideCHEDPSections(formContext);

      //Show the CHEDA section
      showCHEDASections(formContext);
    }
    else if (importApplicationType == defraimp_importapplication_defraimp_importapplicationtype.CHEDP)
    {
      //Hide any existing sections first
      hideITAHCSections(formContext);
      hideCHEDASections(formContext);

      //Show the CHEDP section
      showCHEDPSections(formContext);
    }
    else
    {
      //Hide all sections
      hideITAHCSections(formContext);
      hideCHEDASections(formContext);
      hideCHEDPSections(formContext);
    }
  }

  function showITAHCSections(formContext: Form.defraimp_importapplication.Main.Information)
  {
    formContext.ui.tabs.get("Summary").sections.get("iv66_section").setVisible(true);
    formContext.ui.tabs.get("AdditionalITAHC_Tab").setVisible(true);
  }

  function hideITAHCSections(formContext: Form.defraimp_importapplication.Main.Information)
  {
    formContext.ui.tabs.get("Summary").sections.get("iv66_section").setVisible(false);
    formContext.ui.tabs.get("AdditionalITAHC_Tab").setVisible(false);
  }

  function showCHEDASections(formContext: Form.defraimp_importapplication.Main.Information)
  {
    formContext.ui.tabs.get("Summary").sections.get("cheda_section").setVisible(true);
    formContext.ui.tabs.get("Transporter_Tab").sections.get("transport_information_section").setVisible(true);
  }

  function hideCHEDASections(formContext: Form.defraimp_importapplication.Main.Information)
  {
    formContext.ui.tabs.get("Summary").sections.get("cheda_section").setVisible(false);
    formContext.ui.tabs.get("Transporter_Tab").sections.get("transport_information_section").setVisible(false);
  }

  function showCHEDPSections(formContext: Form.defraimp_importapplication.Main.Information)
  {
    formContext.ui.tabs.get("Summary").sections.get("chedp_section").setVisible(true);
    formContext.ui.tabs.get("Summary").sections.get("chedp_controls_section").setVisible(true);
    formContext.ui.tabs.get("Transporter_Tab").sections.get("transport_information_section").setVisible(true);
  }

  function hideCHEDPSections(formContext: Form.defraimp_importapplication.Main.Information)
  {
    formContext.ui.tabs.get("Summary").sections.get("chedp_section").setVisible(false);
    formContext.ui.tabs.get("Summary").sections.get("chedp_controls_section").setVisible(false);
    formContext.ui.tabs.get("Transporter_Tab").sections.get("transport_information_section").setVisible(false);
  }
}