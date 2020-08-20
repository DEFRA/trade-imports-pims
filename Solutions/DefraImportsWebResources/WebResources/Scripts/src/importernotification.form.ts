namespace DefraImports.ImporterNotification {

  let formContext: Form.defraimp_importernotification.Main.Information;

  export function showHideCharity(executionObj: Xrm.ExecutionContext<any>) {
    formContext = executionObj.getFormContext() as Form.defraimp_importernotification.Main.Information;

    const importingFromCharity = formContext.getAttribute("defraimp_importingfromcharity").getValue();
   
    //Set visibility to whatever value Importing from Charity is
    formContext.ui.tabs.get("Charity_Tab").setVisible(importingFromCharity);
  }

  export function checkForMultipleCommodities(executionObj: Xrm.ExecutionContext<any>)
  {
    formContext = executionObj.getFormContext() as Form.defraimp_importernotification.Main.Information;

    //Check if Importer Notification hasMultipleCommodities field is set to true
    var hasMultipleCommodities = formContext.getAttribute("defraimp_hasmultiplecommoditycodes").getValue();

    if (hasMultipleCommodities == true)
    {
      //If true, we need to show a warning notification
      showCommodityWarning(formContext);
    }
    else if (hasMultipleCommodities == false || hasMultipleCommodities == null)
    {
      //If not true, we should not show a warning notification
      hideCommodityWarning(formContext);
    }
  }

  function showCommodityWarning(formContext : Form.defraimp_importernotification.Main.Information)
  {
    //Show the caseworker Intervention section on the form
    formContext.ui.tabs.get("details_tab").sections.get("caseworker_intervention_section").setVisible(true);

    var hasCaseworkerIntervened = formContext.getAttribute("defraimp_caseworkerintervention").getValue();

    if (hasCaseworkerIntervened == false || hasCaseworkerIntervened == null)
    {
      formContext.ui.clearFormNotification("multipleCommodityNotification");
      formContext.ui.setFormNotification("More than 1 Commodity Code - No caseworker intervention", "ERROR","multipleCommodityError");
    }
    else if (hasCaseworkerIntervened == true)
    {
      formContext.ui.clearFormNotification("multipleCommodityError");
      formContext.ui.setFormNotification("More than 1 Commodity Code - caseworker has intervened", "INFO","multipleCommodityNotification");
    }

  }

  function hideCommodityWarning(formContext : Form.defraimp_importernotification.Main.Information)
  {
    //Hide the caseworker Intervention section on the form
    formContext.ui.tabs.get("details_tab").sections.get("caseworker_intervention_section").setVisible(false);
    formContext.ui.clearFormNotification("multipleCommodityError");
    formContext.ui.clearFormNotification("multipleCommodityNotification");
  }
}