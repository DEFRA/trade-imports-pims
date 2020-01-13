namespace DefraImports.ImportRecord {

  export function OnLoadQuickCreateForm(executionObj: Xrm.ExecutionContext<any>) {

    let formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Quick.Information;

    if (formContext.ui.getFormType() === Xrm.FormType.Create) {
      formContext.getAttribute("ownerid").setValue(null);
    }
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