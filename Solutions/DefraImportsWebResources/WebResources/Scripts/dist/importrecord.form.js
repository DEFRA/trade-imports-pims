var DefraImports;
(function (DefraImports) {
    var ImportRecord;
    (function (ImportRecord) {
        function OnLoadQuickCreateForm(executionObj) {
            var formContext = executionObj.getFormContext();
            if (formContext.ui.getFormType() === 1 /* Create */) {
                formContext.getAttribute("ownerid").setValue(null);
            }
        }
        ImportRecord.OnLoadQuickCreateForm = OnLoadQuickCreateForm;
    })(ImportRecord = DefraImports.ImportRecord || (DefraImports.ImportRecord = {}));
})(DefraImports || (DefraImports = {}));
//# sourceMappingURL=importrecord.form.js.map