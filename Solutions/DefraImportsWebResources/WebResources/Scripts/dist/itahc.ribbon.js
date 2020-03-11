var DefraImports;
(function (DefraImports) {
    var Itahc;
    (function (Itahc) {
        var CreateImportRecordFromItahcRequest = /** @class */ (function () {
            function CreateImportRecordFromItahcRequest(entity) {
                this.entity = entity;
            }
            CreateImportRecordFromItahcRequest.prototype.getMetadata = function () {
                return {
                    boundParameter: "entity",
                    operationType: 0,
                    operationName: "defraimp_CreateImportRecordFromItahc",
                    parameterTypes: {
                        entity: {
                            typeName: "mscrm.defraimp_itahc",
                            structuralProperty: 5
                        }
                    }
                };
            };
            return CreateImportRecordFromItahcRequest;
        }());
        function onCreateImportRecordFromItahc(primaryControl) {
            Xrm.Utility.showProgressIndicator("Loading");
            callCreateImportRecordFromItahcAction(primaryControl);
        }
        Itahc.onCreateImportRecordFromItahc = onCreateImportRecordFromItahc;
        function callCreateImportRecordFromItahcAction(primaryControl) {
            var itahc = primaryControl.data.entity.getEntityReference();
            var requestObject = new CreateImportRecordFromItahcRequest(itahc);
            Xrm.WebApi.online
                .execute(requestObject)
                .then(function (success) {
                executeSuccess(primaryControl);
            }, executeErrorCallback);
        }
        function executeSuccess(primaryControl) {
            Xrm.Utility.closeProgressIndicator();
            primaryControl.data.refresh(false);
        }
        function executeErrorCallback(error) {
            Xrm.Utility.closeProgressIndicator();
            var errorOptions = {
                errorCode: error.errorCode,
                message: error.message,
            };
            Xrm.Navigation.openErrorDialog(errorOptions);
        }
    })(Itahc = DefraImports.Itahc || (DefraImports.Itahc = {}));
})(DefraImports || (DefraImports = {}));
//# sourceMappingURL=itahc.ribbon.js.map