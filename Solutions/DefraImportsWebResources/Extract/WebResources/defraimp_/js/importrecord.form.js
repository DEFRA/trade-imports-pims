var DefraImports;
(function (DefraImports) {
    var ImportRecord;
    (function (ImportRecord) {
        var MANUAL_POST_IMPORT_CHECK_BLANK_ERROR_MSG = "'Manual Post Import Check Decision' must be populated.";
        var wasManualPostImportCheckSet = false;
        var isErrorDialogDisplaying = false;
        function OnLoadQuickCreateForm(executionObj) {
            var formContext = executionObj.getFormContext();
            if (formContext.ui.getFormType() === 1 /* Create */) {
                formContext.getAttribute("ownerid").setValue(null);
            }
        }
        ImportRecord.OnLoadQuickCreateForm = OnLoadQuickCreateForm;
        function onLoad(executionObj) {
            var formContext = executionObj.getFormContext();
            storeWasManualPostImportCheckSet(formContext);
        }
        ImportRecord.onLoad = onLoad;
        function onSave(executionObj) {
            var formContext = executionObj.getFormContext();
            preventSaveIfPostImportChecksIsUpdatedToBlank(executionObj);
            storeWasManualPostImportCheckSet(formContext);
        }
        ImportRecord.onSave = onSave;
        function storeWasManualPostImportCheckSet(formContext) {
            var manualPostImportCheckAttr = formContext.getAttribute("defraimp_manualpostimportcheckdecision");
            if (manualPostImportCheckAttr.getValue() !== null) {
                wasManualPostImportCheckSet = true;
            }
        }
        function preventSaveIfPostImportChecksIsUpdatedToBlank(executionObj) {
            var formContext = executionObj.getFormContext();
            var currentManualPostImportCheckAttr = formContext.getAttribute("defraimp_manualpostimportcheckdecision");
            if (wasManualPostImportCheckSet && currentManualPostImportCheckAttr.getValue() === null) {
                executionObj.getEventArgs().preventDefault();
                if (!isErrorDialogDisplaying) {
                    displayManualPostImportCheckDecisionErrorMessage();
                }
            }
        }
        function displayManualPostImportCheckDecisionErrorMessage() {
            var errorMessage = MANUAL_POST_IMPORT_CHECK_BLANK_ERROR_MSG;
            isErrorDialogDisplaying = true;
            Xrm.Navigation.openErrorDialog({ message: errorMessage }).then(function (success) {
                isErrorDialogDisplaying = false;
            }, function (error) {
            });
        }
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