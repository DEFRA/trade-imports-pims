var DefraImports;
(function (DefraImports) {
    var ImporterNotification;
    (function (ImporterNotification) {
        var formContext;
        function showHideCharity(executionObj) {
            formContext = executionObj.getFormContext();
            var importingFromCharity = formContext.getAttribute("defraimp_importingfromcharity").getValue();
            //Set visibility to whatever value Importing from Charity is
            formContext.ui.tabs.get("Charity_Tab").setVisible(importingFromCharity);
        }
        ImporterNotification.showHideCharity = showHideCharity;
    })(ImporterNotification = DefraImports.ImporterNotification || (DefraImports.ImporterNotification = {}));
})(DefraImports || (DefraImports = {}));
//# sourceMappingURL=importernotification.form.js.map