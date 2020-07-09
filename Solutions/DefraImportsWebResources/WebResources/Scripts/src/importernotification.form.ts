namespace DefraImports.ImporterNotification {

  let formContext: Form.defraimp_importernotification.Main.Information;

  export function showHideCharity(executionObj: Xrm.ExecutionContext<any>) {
    formContext = executionObj.getFormContext() as Form.defraimp_importernotification.Main.Information;

    const importingFromCharity = formContext.getAttribute("defraimp_importingfromcharity").getValue();
   
    //Set visibility to whatever value Importing from Charity is
    formContext.ui.tabs.get("Charity_Tab").setVisible(importingFromCharity);
  }
}