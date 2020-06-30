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
        function showRelevantSections(executionObj) {
            var formContext = executionObj.getFormContext();
            var importApplicationType = formContext.getAttribute("defraimp_importapplicationtype").getValue();
            if (importApplicationType == 714100000 /* ITAHC */) {
                //Hide any existing sections first
                hideCHEDASections(formContext);
                hideCHEDPSections(formContext);
                //Show the ITAHC section
                showITAHCSections(formContext);
            }
            else if (importApplicationType == 714100002 /* CHEDA */) {
                //Hide any existing sections first
                hideITAHCSections(formContext);
                hideCHEDPSections(formContext);
                //Show the CHEDA section
                showCHEDASections(formContext);
            }
            else if (importApplicationType == 714100003 /* CHEDP */) {
                //Hide any existing sections first
                hideITAHCSections(formContext);
                hideCHEDASections(formContext);
                //Show the CHEDP section
                showCHEDPSections(formContext);
            }
            else {
                //Hide all sections
                hideITAHCSections(formContext);
                hideCHEDASections(formContext);
                hideCHEDPSections(formContext);
            }
        }
        ImportRecord.showRelevantSections = showRelevantSections;
        function showITAHCSections(formContext) {
            formContext.ui.tabs.get("Summary").sections.get("iv66_section").setVisible(true);
            formContext.ui.tabs.get("AdditionalITAHC_Tab").setVisible(true);
        }
        function hideITAHCSections(formContext) {
            formContext.ui.tabs.get("Summary").sections.get("iv66_section").setVisible(false);
            formContext.ui.tabs.get("AdditionalITAHC_Tab").setVisible(false);
        }
        function showCHEDASections(formContext) {
            formContext.ui.tabs.get("Summary").sections.get("cheda_section").setVisible(true);
            formContext.ui.tabs.get("Transporter_Tab").sections.get("transport_information_section").setVisible(true);
        }
        function hideCHEDASections(formContext) {
            formContext.ui.tabs.get("Summary").sections.get("cheda_section").setVisible(false);
            formContext.ui.tabs.get("Transporter_Tab").sections.get("transport_information_section").setVisible(false);
        }
        function showCHEDPSections(formContext) {
            formContext.ui.tabs.get("Summary").sections.get("chedp_section").setVisible(true);
            formContext.ui.tabs.get("Summary").sections.get("chedp_controls_section").setVisible(true);
            formContext.ui.tabs.get("Transporter_Tab").sections.get("transport_information_section").setVisible(true);
        }
        function hideCHEDPSections(formContext) {
            formContext.ui.tabs.get("Summary").sections.get("chedp_section").setVisible(false);
            formContext.ui.tabs.get("Summary").sections.get("chedp_controls_section").setVisible(false);
            formContext.ui.tabs.get("Transporter_Tab").sections.get("transport_information_section").setVisible(false);
        }
    })(ImportRecord = DefraImports.ImportRecord || (DefraImports.ImportRecord = {}));
})(DefraImports || (DefraImports = {}));
//# sourceMappingURL=importrecord.form.js.map