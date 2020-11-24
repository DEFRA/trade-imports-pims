var DefraImports;
(function (DefraImports) {
    var ImporterNotification;
    (function (ImporterNotification) {
        var TRACESConfigParamaterConstants = /** @class */ (function () {
            function TRACESConfigParamaterConstants() {
            }
            TRACESConfigParamaterConstants.entityName = "defraexp_configurationparameter";
            TRACESConfigParamaterConstants.entityId = "{2bb103d9-b629-eb11-a813-000d3ad82cac}";
            return TRACESConfigParamaterConstants;
        }());
        var CreateImportRecordFromNotificationRequest = /** @class */ (function () {
            function CreateImportRecordFromNotificationRequest(entity) {
                this.entity = entity;
            }
            CreateImportRecordFromNotificationRequest.prototype.getMetadata = function () {
                return {
                    boundParameter: "entity",
                    operationType: 0,
                    operationName: "defraimp_CreateImportRecordFromNotification",
                    parameterTypes: {
                        entity: {
                            typeName: "mscrm.defraimp_importernotification",
                            structuralProperty: 5
                        }
                    }
                };
            };
            return CreateImportRecordFromNotificationRequest;
        }());
        function onCreateImportRecordFromNotification(primaryControl) {
            checkTracesEnabled(primaryControl);
        }
        ImporterNotification.onCreateImportRecordFromNotification = onCreateImportRecordFromNotification;
        function checkTracesEnabled(primaryControl) {
            Xrm.WebApi.retrieveRecord(TRACESConfigParamaterConstants.entityName, TRACESConfigParamaterConstants.entityId, "?$select=defraexp_value").then(function (result) {
                var tracesEnbaled = (/false/i).test(result.defraexp_value);
                if (tracesEnbaled) {
                    Xrm.Utility.showProgressIndicator("Loading");
                    executeCreateImportRecordFromNotification(primaryControl);
                }
                else {
                    alert("Access to TRACES is still enabled. Please create Import Records from ITAHCs.");
                }
            }, function (error) {
                alert(error.message);
            });
        }
        function executeCreateImportRecordFromNotification(primaryControl) {
            var notification = primaryControl.data.entity.getEntityReference();
            var requestObject = new CreateImportRecordFromNotificationRequest(notification);
            Xrm.WebApi.online
                .execute(requestObject)
                .then(function () { createImportRecordFromNotificationSuccess(primaryControl); }, createImportRecordFromNotificationError);
        }
        function createImportRecordFromNotificationSuccess(primaryControl) {
            Xrm.Utility.closeProgressIndicator();
            primaryControl.data.refresh(false);
        }
        function createImportRecordFromNotificationError(error) {
            Xrm.Utility.closeProgressIndicator();
            var errorOptions = {
                errorCode: error.errorCode,
                message: error.message,
            };
            Xrm.Navigation.openErrorDialog(errorOptions);
        }
    })(ImporterNotification = DefraImports.ImporterNotification || (DefraImports.ImporterNotification = {}));
})(DefraImports || (DefraImports = {}));
//# sourceMappingURL=importernotification.ribbon.js.map