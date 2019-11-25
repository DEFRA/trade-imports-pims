var DefraImports;
(function (DefraImports) {
    var PostImportCheck;
    (function (PostImportCheck) {
        var formContext;
        function onLoad(executionObj) {
            formContext = executionObj.getFormContext();
            showHideSampleTestsBasedOnSamplingRequired();
        }
        PostImportCheck.onLoad = onLoad;
        function onChangeSamplingRequired() {
            showHideSampleTestsBasedOnSamplingRequired();
        }
        PostImportCheck.onChangeSamplingRequired = onChangeSamplingRequired;
        function showHideSampleTestsBasedOnSamplingRequired() {
            var isSamplingRequired = formContext.getAttribute("defraimp_samplingrequired").getValue();
            var sampleTestsRequiredSubgrid = formContext.getControl("SamplesTestsRequired");
            if (isSamplingRequired) {
                sampleTestsRequiredSubgrid.setVisible(true);
            }
            else {
                sampleTestsRequiredSubgrid.setVisible(false);
            }
        }
    })(PostImportCheck = DefraImports.PostImportCheck || (DefraImports.PostImportCheck = {}));
})(DefraImports || (DefraImports = {}));
//# sourceMappingURL=postimportcheck.form.js.map