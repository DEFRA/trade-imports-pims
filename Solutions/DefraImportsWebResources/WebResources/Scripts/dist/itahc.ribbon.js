var DefraImports;
(function (DefraImports) {
    var Itahc;
    (function (Itahc) {
        var TRACESConfigParamaterConstants = /** @class */ (function () {
            function TRACESConfigParamaterConstants() {
            }
            TRACESConfigParamaterConstants.entityName = "defraexp_configurationparameter";
            TRACESConfigParamaterConstants.entityId = "{2bb103d9-b629-eb11-a813-000d3ad82cac}";
            return TRACESConfigParamaterConstants;
        }());
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
            checkTracesEnabled(primaryControl);
        }
        Itahc.onCreateImportRecordFromItahc = onCreateImportRecordFromItahc;
        function checkTracesEnabled(primaryControl) {
            Xrm.WebApi.retrieveRecord(TRACESConfigParamaterConstants.entityName, TRACESConfigParamaterConstants.entityId, "?$select=defraexp_value").then(function (result) {
                var tracesEnbaled = (/true/i).test(result.defraexp_value);
                if (tracesEnbaled) {
                    Xrm.Utility.showProgressIndicator("Loading");
                    callCreateImportRecordFromItahcAction(primaryControl);
                }
                else {
                    alert("Access TRACES is not enabled. Please create Import Records from Importer Notifications instead");
                }
            }, function (error) {
                alert(error.message);
            });
        }
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