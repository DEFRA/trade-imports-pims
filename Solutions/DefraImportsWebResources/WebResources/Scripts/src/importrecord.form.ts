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
}