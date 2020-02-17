namespace DefraImports.ImportRecord {

  let wasManualPostImportCheckSet: boolean = false;

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

  function storeWasManualPostImportCheckSet(formContext: Form.defraimp_importapplication.Main.Information) {
    let manualPostImportCheckAttr = formContext.getAttribute("defraimp_manualpostimportcheckdecision");

    if (manualPostImportCheckAttr.getValue() !== null) {
      wasManualPostImportCheckSet = true;
    }
  }


  export function onSave(executionObj: Xrm.ExecutionContext<any>) {

    preventSaveIfPostImportChecksIsUpdatedToBlank(executionObj);
  }

  function preventSaveIfPostImportChecksIsUpdatedToBlank(executionObj: Xrm.ExecutionContext<any>) {
    let formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Main.Information;
    let currentManualPostImportCheckAttr = formContext.getAttribute("defraimp_manualpostimportcheckdecision");

    if (wasManualPostImportCheckSet && currentManualPostImportCheckAttr.getValue() === null) {
      executionObj.getEventArgs().preventDefault();
      displayManualPostImportCheckDecisionErrorMessage();
    }
    else {
      wasManualPostImportCheckSet = true;
    }
  }

  function displayManualPostImportCheckDecisionErrorMessage() {
    let errorMessage: string = "'Manual Post Import Check Decision' must be populated.";
    Xrm.Navigation.openErrorDialog({ message: errorMessage }).then(
      function (success) {

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