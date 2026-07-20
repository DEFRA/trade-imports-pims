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
    showOrHideNonComplianceTab(formContext, formContext.getAttribute("defraimp_isnoncompliantcalculated").getValue()!);
    showOrHideNonComplianceOther(executionObj);
  }

  export function onSave(executionObj: Xrm.ExecutionContext<any>) {
    const formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Main.Information;

    preventSaveIfPostImportChecksIsUpdatedToBlank(executionObj);
    storeWasManualPostImportCheckSet(formContext);
  }

  export function onChangeOfManualPostImportCheckDecision(executionObj: Xrm.ExecutionContext<any>) {
    const formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Main.Information;
    setSystemDeterminedInspectionValues(formContext);
  }

  export function showOrHideNonComplianceOther(executionObj: Xrm.ExecutionContext<any>) {
    const formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Main.Information;
    if ((formContext.getAttribute("defraimp_typesofnoncompliance").getValue() as any)?.includes(714100005 /* Other */)) {
      formContext.getControl("defraimp_noncomplianceothercomments").setVisible(true);
    } else {
      formContext.getControl("defraimp_noncomplianceothercomments").setVisible(false);
    }
  }

  function storeWasManualPostImportCheckSet(formContext: Form.defraimp_importapplication.Main.Information) {
    let manualPostImportCheckAttr = formContext.getAttribute("defraimp_manualpostimportcheckdecision");

    if (manualPostImportCheckAttr.getValue() !== null) {
      wasManualPostImportCheckSet = true;
    }
  }

  function setSystemDeterminedInspectionValues(formContext: Form.defraimp_importapplication.Main.Information) {
    let currentManualPostImportCheckAttr = formContext.getAttribute("defraimp_manualpostimportcheckdecision");

    if (currentManualPostImportCheckAttr.getValue() == defraimp_importapplication_defraimp_manualpostimportcheckdecision.UseSystemDecision) {
      var originalInspectionRequiredValue = formContext.getAttribute("defraimp_inspectionrequiredoriginalvalue").getValue();
      var originalInspectionRequiredReasonValue = formContext.getAttribute("defraimp_inspectionrequiredreasonoriginalvalue").getValue();
      formContext.getAttribute("defraimp_inspectionrequired").setValue(originalInspectionRequiredValue);
      formContext.getAttribute("defraimp_inspectionrequiredreason").setValue(originalInspectionRequiredReasonValue);
    }
    else if (currentManualPostImportCheckAttr.getValue() == defraimp_importapplication_defraimp_manualpostimportcheckdecision.ManualCheckOther) {
      formContext.getAttribute("defraimp_inspectionrequired").setValue(defraimp_importapplication_defraimp_inspectionrequired.Yes);
      formContext.getAttribute("defraimp_inspectionrequiredreason").setValue(defraimp_importapplication_defraimp_inspectionrequiredreason.ManuallyRequestedPostImportCheck);
    }
    else if (currentManualPostImportCheckAttr.getValue() == defraimp_importapplication_defraimp_manualpostimportcheckdecision.ManualCheckQuarantine) {
      formContext.getAttribute("defraimp_inspectionrequired").setValue(defraimp_importapplication_defraimp_inspectionrequired.Yes);
      formContext.getAttribute("defraimp_inspectionrequiredreason").setValue(defraimp_importapplication_defraimp_inspectionrequiredreason.Quarantine);
    }
    else if (currentManualPostImportCheckAttr.getValue() == defraimp_importapplication_defraimp_manualpostimportcheckdecision.ManualCheckTB) {
      formContext.getAttribute("defraimp_inspectionrequired").setValue(defraimp_importapplication_defraimp_inspectionrequired.Yes);
      formContext.getAttribute("defraimp_inspectionrequiredreason").setValue(defraimp_importapplication_defraimp_inspectionrequiredreason.TB);
    }
    else if (currentManualPostImportCheckAttr.getValue() == defraimp_importapplication_defraimp_manualpostimportcheckdecision.DoNotPostImportCheck) {
      formContext.getAttribute("defraimp_inspectionrequired").setValue(defraimp_importapplication_defraimp_inspectionrequired.No);
      formContext.getAttribute("defraimp_inspectionrequiredreason").setValue(defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired);

      if (formContext.getAttribute("defraimp_inspectiondeclinedreason").getValue() === null || formContext.getAttribute("defraimp_inspectiondeclinedreason").getValue() === "") {
        formContext.getAttribute("defraimp_inspectiondeclinedreason").setValue("System Required Post Import Check Skipped");
      }
    }
  }

  function preventSaveIfPostImportChecksIsUpdatedToBlank(executionObj: Xrm.ExecutionContext<any>) {
    let formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Main.Information;
    let currentManualPostImportCheckAttr = formContext.getAttribute("defraimp_manualpostimportcheckdecision");

    if (wasManualPostImportCheckSet && currentManualPostImportCheckAttr.getValue() === null) {
      (executionObj as any)?.getEventArgs()?.preventDefault();
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

  export function showRelevantSections(executionObj: Xrm.ExecutionContext<any>) {
    let formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Main.Information;
    const importApplicationType = formContext.getAttribute("defraimp_importapplicationtype").getValue();

    //Check if we are importing from a charity and show the relevant section
    showHideCharitySection(formContext);

    if (importApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC
      || importApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHCLandbridge) {
      //Hide any existing sections first
      hideCHEDASections(formContext);
      hideCHEDPSections(formContext);
      hideIMPSections(formContext);
      //Show the ITAHC section
      showITAHCSections(formContext);
    }
    else if (importApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ImportNotification) {
      hideCHEDASections(formContext);
      hideCHEDPSections(formContext);
      hideITAHCSections(formContext);
      showIMPSections(formContext);
    }
    else if (importApplicationType == defraimp_importapplication_defraimp_importapplicationtype.DOCOM) {
      hideCHEDASections(formContext);
      hideCHEDPSections(formContext);
      hideITAHCSections(formContext);
      hideIMPSections(formContext);
    }
    else if (importApplicationType == defraimp_importapplication_defraimp_importapplicationtype.CHEDA) {
      //Hide any existing sections first
      hideITAHCSections(formContext);
      hideIMPSections(formContext);
      hideCHEDPSections(formContext);

      //Show the CHEDA section
      showCHEDASections(formContext);
    }
    else if (importApplicationType == defraimp_importapplication_defraimp_importapplicationtype.CHEDP) {
      //Hide any existing sections first
      hideITAHCSections(formContext);
      hideIMPSections(formContext);
      hideCHEDASections(formContext);

      //Show the CHEDP section
      showCHEDPSections(formContext);
    }
    else {
      //Hide all sections
      hideITAHCSections(formContext);
      hideIMPSections(formContext);
      hideCHEDASections(formContext);
      hideCHEDPSections(formContext);
    }
  }

  function showITAHCSections(formContext: Form.defraimp_importapplication.Main.Information) {
    formContext.ui.tabs.get("Summary").sections.get("iv66_section").setVisible(true);
    formContext.ui.tabs.get("AdditionalITAHC_Tab").setVisible(true);
  }

  function showIMPSections(formContext: Form.defraimp_importapplication.Main.Information) {
    formContext.ui.tabs.get("Summary").sections.get("iv66_section").setVisible(true);
  }

  function hideITAHCSections(formContext: Form.defraimp_importapplication.Main.Information) {
    formContext.ui.tabs.get("Summary").sections.get("iv66_section").setVisible(false);
    formContext.ui.tabs.get("AdditionalITAHC_Tab").setVisible(false);
  }

  function hideIMPSections(formContext: Form.defraimp_importapplication.Main.Information) {
    formContext.ui.tabs.get("Summary").sections.get("iv66_section").setVisible(false);
  }

  function showCHEDASections(formContext: Form.defraimp_importapplication.Main.Information) {
    formContext.ui.tabs.get("Summary").sections.get("cheda_section").setVisible(true);
    formContext.ui.tabs.get("Transporter_Tab").sections.get("transport_information_section").setVisible(true);
  }

  function hideCHEDASections(formContext: Form.defraimp_importapplication.Main.Information) {
    formContext.ui.tabs.get("Summary").sections.get("cheda_section").setVisible(false);
    formContext.ui.tabs.get("Transporter_Tab").sections.get("transport_information_section").setVisible(false);
  }

  function showCHEDPSections(formContext: Form.defraimp_importapplication.Main.Information) {
    formContext.ui.tabs.get("Summary").sections.get("chedp_section").setVisible(true);
    formContext.ui.tabs.get("Summary").sections.get("chedp_controls_section").setVisible(true);
    formContext.ui.tabs.get("Transporter_Tab").sections.get("transport_information_section").setVisible(true);
  }

  function hideCHEDPSections(formContext: Form.defraimp_importapplication.Main.Information) {
    formContext.ui.tabs.get("Summary").sections.get("chedp_section").setVisible(false);
    formContext.ui.tabs.get("Summary").sections.get("chedp_controls_section").setVisible(false);
    formContext.ui.tabs.get("Transporter_Tab").sections.get("transport_information_section").setVisible(false);
  }

  function showHideCharitySection(formContext: Form.defraimp_importapplication.Main.Information) {
    const importingFromCharity = formContext.getAttribute("defraimp_importingfromcharity").getValue();

    //Set visibility to whatever value Importing from Charity is
    formContext.ui.tabs.get("Charity_Tab").setVisible(importingFromCharity!);
  }

  function showDOCOMTab(formContext: Form.defraimp_importapplication.Main.Information) {
    formContext.ui.tabs.get("DOCOM_Tab").setVisible(true);
  }

  function hideDOCOMTab(formContext: Form.defraimp_importapplication.Main.Information) {
    formContext.ui.tabs.get("DOCOM_Tab").setVisible(false);
  }

  function showOrHideNonComplianceTab(formContext: Form.defraimp_importapplication.Main.Information, showOrHide: boolean) {
    formContext.ui.tabs.get("NonCompliance_Tab").setVisible(showOrHide);
  }
}