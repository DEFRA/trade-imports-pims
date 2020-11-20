var DefraImports;
(function (DefraImports) {
    var ImportRecord;
    (function (ImportRecord) {
        var UpdateImportRecordWithItahcRequest = /** @class */ (function () {
            function UpdateImportRecordWithItahcRequest(entity, overwriteExistingData) {
                this.entity = entity;
                this.overwriteExistingData = overwriteExistingData;
            }
            UpdateImportRecordWithItahcRequest.prototype.getMetadata = function () {
                return {
                    boundParameter: "entity",
                    operationType: 0,
                    operationName: "defraimp_UpdateImportRecordWithItahc",
                    parameterTypes: {
                        entity: {
                            typeName: "mscrm.defraimp_importapplication",
                            structuralProperty: 5
                        },
                        overwriteExistingData: {
                            typename: "Edm.Boolean",
                            structuralProperty: 1
                        }
                    }
                };
            };
            return UpdateImportRecordWithItahcRequest;
        }());
        function onFillEmptyDataWithItahc(primaryControl) {
            Xrm.Utility.showProgressIndicator("Loading");
            callUpdateImportRecordWithItahcAction(primaryControl, false);
        }
        ImportRecord.onFillEmptyDataWithItahc = onFillEmptyDataWithItahc;
        function onOverwriteDataWithItahc(primaryControl) {
            Xrm.Utility.showProgressIndicator("Loading");
            callUpdateImportRecordWithItahcAction(primaryControl, true);
        }
        ImportRecord.onOverwriteDataWithItahc = onOverwriteDataWithItahc;
        function callUpdateImportRecordWithItahcAction(primaryControl, overwriteExistingData) {
            var importApplication = primaryControl.data.entity.getEntityReference();
            var requestObject = new UpdateImportRecordWithItahcRequest(importApplication, overwriteExistingData);
            Xrm.WebApi.online
                .execute(requestObject)
                .then(function (success) {
                executeSuccess(primaryControl);
            }, executeErrorCallback);
        }
        var UpdateImportRecordWithNotificationRequest = /** @class */ (function () {
            function UpdateImportRecordWithNotificationRequest(entity, overwriteExistingData) {
                this.entity = entity;
                this.overwriteExistingData = overwriteExistingData;
            }
            UpdateImportRecordWithNotificationRequest.prototype.getMetadata = function () {
                return {
                    boundParameter: "entity",
                    operationType: 0,
                    operationName: "defraimp_UpdateImportRecordWithNotification",
                    parameterTypes: {
                        entity: {
                            typeName: "mscrm.defraimp_importapplication",
                            structuralProperty: 5
                        },
                        overwriteExistingData: {
                            typename: "Edm.Boolean",
                            structuralProperty: 1
                        }
                    }
                };
            };
            return UpdateImportRecordWithNotificationRequest;
        }());
        function onFillEmptyDataWithNotification(primaryControl) {
            Xrm.Utility.showProgressIndicator("Loading");
            callUpdateImportRecordWithNotificationAction(primaryControl, false);
        }
        ImportRecord.onFillEmptyDataWithNotification = onFillEmptyDataWithNotification;
        function onOverwriteDataWithNotification(primaryControl) {
            Xrm.Utility.showProgressIndicator("Loading");
            callUpdateImportRecordWithNotificationAction(primaryControl, true);
        }
        ImportRecord.onOverwriteDataWithNotification = onOverwriteDataWithNotification;
        function callUpdateImportRecordWithNotificationAction(primaryControl, overwriteExistingData) {
            var importApplication = primaryControl.data.entity.getEntityReference();
            var requestObject = new UpdateImportRecordWithNotificationRequest(importApplication, overwriteExistingData);
            Xrm.WebApi.online
                .execute(requestObject)
                .then(function (success) { executeSuccess(primaryControl); }, executeErrorCallback);
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
    })(ImportRecord = DefraImports.ImportRecord || (DefraImports.ImportRecord = {}));
})(DefraImports || (DefraImports = {}));
//# sourceMappingURL=importrecord.ribbon.js.map