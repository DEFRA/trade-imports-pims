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
        function onChangeOfMoveToCompletion(executionObj) {
            var formContext = executionObj.getFormContext();
            populateMoveToCompletionDate(formContext);
        }
        ImportRecord.onChangeOfMoveToCompletion = onChangeOfMoveToCompletion;
        function populateMoveToCompletionDate(formContext) {
            var moveToCompletionVal = formContext.getAttribute("defraimp_movetocompletion").getValue();
            var moveCompletionDateAttr = formContext.getAttribute("defraimp_movedtocompletiondate");
            if (moveToCompletionVal) {
                var currentDate = new Date();
                moveCompletionDateAttr.setValue(currentDate);
            }
            else {
                moveCompletionDateAttr.setValue(null);
            }
        }
    })(ImportRecord = DefraImports.ImportRecord || (DefraImports.ImportRecord = {}));
})(DefraImports || (DefraImports = {}));
//# sourceMappingURL=importrecord.form.js.map