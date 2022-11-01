var DefraImports;
(function (DefraImports) {
    var ImporterNotification;
    (function (ImporterNotification) {
        var formContext;
        function showHideCharity(executionObj) {
            formContext = executionObj.getFormContext();
            formContext.data.entity.addOnPostSave(PostSaveTriggerFunction);
            var importingFromCharity = formContext.getAttribute("defraimp_importingfromcharity").getValue();
            //Set visibility to whatever value Importing from Charity is
            formContext.ui.tabs.get("Charity_Tab").setVisible(importingFromCharity);
        }
        ImporterNotification.showHideCharity = showHideCharity;
        function checkForMultipleCommodities(executionObj) {
            formContext = executionObj.getFormContext();
            //Check if Importer Notification hasMultipleCommodities field is set to true
            var hasMultipleCommodities = formContext.getAttribute("defraimp_hasmultiplecommoditycodes").getValue();
            if (hasMultipleCommodities == true) {
                //If true, we need to show a warning notification
                showCommodityWarning(formContext);
            }
            else if (hasMultipleCommodities == false || hasMultipleCommodities == null) {
                //If not true, we should not show a warning notification
                hideCommodityWarning(formContext);
            }
        }
        ImporterNotification.checkForMultipleCommodities = checkForMultipleCommodities;


        function showCommodityWarning(formContext) {
            //Show the caseworker Intervention section on the form
            formContext.ui.tabs.get("details_tab").sections.get("caseworker_intervention_section").setVisible(true);
            var hasCaseworkerIntervened = formContext.getAttribute("defraimp_caseworkerintervention").getValue();
            if (hasCaseworkerIntervened == false || hasCaseworkerIntervened == null) {
                formContext.ui.clearFormNotification("multipleCommodityNotification");
                formContext.ui.setFormNotification("More than 1 Commodity Code - No caseworker intervention", "ERROR", "multipleCommodityError");
            }
            else if (hasCaseworkerIntervened == true) {
                formContext.ui.clearFormNotification("multipleCommodityError");
                formContext.ui.setFormNotification("More than 1 Commodity Code - caseworker has intervened", "INFO", "multipleCommodityNotification");
            }
        }
        function hideCommodityWarning(formContext) {
            //Hide the caseworker Intervention section on the form
            formContext.ui.tabs.get("details_tab").sections.get("caseworker_intervention_section").setVisible(false);
            formContext.ui.clearFormNotification("multipleCommodityError");
            formContext.ui.clearFormNotification("multipleCommodityNotification");
        }

        function PostSaveTriggerFunction(executionContext) {
            var formContext = executionContext.getFormContext();
            var recordId = formContext.data.entity.getId();
            entityFormOptions = {};
            entityFormOptions["entityName"] = "defraimp_importernotification";
            entityFormOptions["entityId"] = recordId;
            Xrm.Navigation.openForm(entityFormOptions).then(
                function (success) {
                    console.log(success);
                },
                function (error) {
                    console.log(error);
                });
        }


    })(ImporterNotification = DefraImports.ImporterNotification || (DefraImports.ImporterNotification = {}));
})(DefraImports || (DefraImports = {}));
//# sourceMappingURL=importernotification.form.js.map