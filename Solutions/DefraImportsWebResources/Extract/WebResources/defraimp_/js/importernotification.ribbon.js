var DefraImports;
(function (DefraImports) {
    var ImporterNotification;
    (function (ImporterNotification) {
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
            Xrm.Utility.showProgressIndicator("Loading");
            executeCreateImportRecordFromNotification(primaryControl);
        }
        ImporterNotification.onCreateImportRecordFromNotification = onCreateImportRecordFromNotification;
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