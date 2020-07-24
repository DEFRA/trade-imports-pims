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
        function onChangeOfManualPostImportCheckDecision(executionObj) {
            var formContext = executionObj.getFormContext();
            setSystemDeterminedInspectionValues(formContext);
        }
        ImportRecord.onChangeOfManualPostImportCheckDecision = onChangeOfManualPostImportCheckDecision;
        function storeWasManualPostImportCheckSet(formContext) {
            var manualPostImportCheckAttr = formContext.getAttribute("defraimp_manualpostimportcheckdecision");
            if (manualPostImportCheckAttr.getValue() !== null) {
                wasManualPostImportCheckSet = true;
            }
        }
        function setSystemDeterminedInspectionValues(formContext) {
            var currentManualPostImportCheckAttr = formContext.getAttribute("defraimp_manualpostimportcheckdecision");
            if (currentManualPostImportCheckAttr.getValue() == 714100004 /* UseSystemDecision */) {
                var originalInspectionRequiredValue = formContext.getAttribute("defraimp_inspectionrequiredoriginalvalue").getValue();
                var originalInspectionRequiredReasonValue = formContext.getAttribute("defraimp_inspectionrequiredreasonoriginalvalue").getValue();
                formContext.getAttribute("defraimp_inspectionrequired").setValue(originalInspectionRequiredValue);
                formContext.getAttribute("defraimp_inspectionrequiredreason").setValue(originalInspectionRequiredReasonValue);
            }
            else if (currentManualPostImportCheckAttr.getValue() == 714100000 /* ManualCheckOther */) {
                formContext.getAttribute("defraimp_inspectionrequired").setValue(714100000 /* Yes */);
                formContext.getAttribute("defraimp_inspectionrequiredreason").setValue(714100013 /* ManuallyRequestedPostImportCheck */);
            }
            else if (currentManualPostImportCheckAttr.getValue() == 714100003 /* ManualCheckQuarantine */) {
                formContext.getAttribute("defraimp_inspectionrequired").setValue(714100000 /* Yes */);
                formContext.getAttribute("defraimp_inspectionrequiredreason").setValue(714100010 /* Quarantine */);
            }
            else if (currentManualPostImportCheckAttr.getValue() == 714100002 /* ManualCheckTB */) {
                formContext.getAttribute("defraimp_inspectionrequired").setValue(714100000 /* Yes */);
                formContext.getAttribute("defraimp_inspectionrequiredreason").setValue(714100011 /* TB */);
            }
            else if (currentManualPostImportCheckAttr.getValue() == 714100001 /* DoNotPostImportCheck */) {
                formContext.getAttribute("defraimp_inspectionrequired").setValue(714100001 /* No */);
                formContext.getAttribute("defraimp_inspectionrequiredreason").setValue(714100004 /* NoInspectionRequired */);
                if (formContext.getAttribute("defraimp_inspectiondeclinedreason").getValue() === null || formContext.getAttribute("defraimp_inspectiondeclinedreason").getValue() === "") {
                    formContext.getAttribute("defraimp_inspectiondeclinedreason").setValue("System Required Post Import Check Skipped");
                }
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
            //Check if we are importing from a charity and show the relevant section
            showHideCharitySection(formContext);
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
        function showHideCharitySection(formContext) {
            var importingFromCharity = formContext.getAttribute("defraimp_importingfromcharity").getValue();
            //Set visibility to whatever value Importing from Charity is
            formContext.ui.tabs.get("Charity_Tab").setVisible(importingFromCharity);
        }
    })(ImportRecord = DefraImports.ImportRecord || (DefraImports.ImportRecord = {}));
})(DefraImports || (DefraImports = {}));
//# sourceMappingURL=importrecord.form.js.map