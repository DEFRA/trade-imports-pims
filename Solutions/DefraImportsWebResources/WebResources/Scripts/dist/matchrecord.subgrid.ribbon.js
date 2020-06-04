var DefraImports;
(function (DefraImports) {
    var MatchRecord;
    (function (MatchRecord) {
        var AppendItahcToImportRecordRequest = /** @class */ (function () {
            function AppendItahcToImportRecordRequest(matchRecord, importRecord, itahc) {
                this.entity = matchRecord;
                this.importRecord = importRecord;
                this.itahc = itahc;
            }
            AppendItahcToImportRecordRequest.prototype.getMetadata = function () {
                return {
                    boundParameter: "entity",
                    operationType: 0,
                    operationName: "defraimp_AppendITAHCtoImportRecord",
                    parameterTypes: {
                        entity: {
                            typeName: "mscrm.defraimp_matchrecord",
                            structuralProperty: 5
                        },
                        itahc: {
                            typeName: "mscrm.defraimp_itahc",
                            structuralProperty: 5
                        },
                        importRecord: {
                            typeName: "mscrm.defraimp_importapplication",
                            structuralProperty: 5
                        },
                    }
                };
            };
            return AppendItahcToImportRecordRequest;
        }());
        var AppendImporterNotificationToImportRecordRequest = /** @class */ (function () {
            function AppendImporterNotificationToImportRecordRequest(matchRecord, importRecord, importerNotification) {
                this.entity = matchRecord;
                this.importRecord = importRecord;
                this.importerNotification = importerNotification;
            }
            AppendImporterNotificationToImportRecordRequest.prototype.getMetadata = function () {
                return {
                    boundParameter: "entity",
                    operationType: 0,
                    operationName: "defraimp_AppendImporterNotificationtoImportRecord",
                    parameterTypes: {
                        entity: {
                            typeName: "mscrm.defraimp_matchrecord",
                            structuralProperty: 5
                        },
                        importerNotification: {
                            typeName: "mscrm.defraimp_importernotification",
                            structuralProperty: 5
                        },
                        importRecord: {
                            typeName: "mscrm.defraimp_importapplication",
                            structuralProperty: 5
                        },
                    }
                };
            };
            return AppendImporterNotificationToImportRecordRequest;
        }());
        function onAppendItahc(primaryControl) {
            Xrm.Utility.showProgressIndicator("Appending ITAHCs");
            var selectedImportRecords = getSelectedImportRecords(primaryControl);
            var itahc = primaryControl.getAttribute("defraimp_itahc").getValue()[0];
            var matchRecord = primaryControl.data.entity.getEntityReference();
            var requests = [];
            selectedImportRecords.forEach(function (importRecord) {
                requests.push(generateAppendItahcRequest(matchRecord, itahc, importRecord));
            });
            executeMultipleRequests(primaryControl, requests);
        }
        MatchRecord.onAppendItahc = onAppendItahc;
        function onAppendImporterNotification(primaryControl) {
            Xrm.Utility.showProgressIndicator("Appending Importer Notifications");
            var selectedImportRecords = getSelectedImportRecords(primaryControl);
            var importerNotification = primaryControl.getAttribute("defraimp_importernotification").getValue()[0];
            var matchRecord = primaryControl.data.entity.getEntityReference();
            var requests = [];
            selectedImportRecords.forEach(function (importRecord) {
                requests.push(generateAppendImporterNotificationRequest(matchRecord, importerNotification, importRecord));
            });
            executeMultipleRequests(primaryControl, requests);
        }
        MatchRecord.onAppendImporterNotification = onAppendImporterNotification;
        function getSelectedImportRecords(primaryControl) {
            var selectedRows = primaryControl.getControl("RelatedImportRecords").getGrid().getSelectedRows();
            var selectedImportRecords = [];
            selectedRows.forEach(function (element) {
                selectedImportRecords.push(element.data.entity.getEntityReference());
            });
            return selectedImportRecords;
        }
        function generateAppendItahcRequest(matchRecord, itahc, importRecord) {
            return new AppendItahcToImportRecordRequest(matchRecord, importRecord, itahc);
        }
        function generateAppendImporterNotificationRequest(matchRecord, importerNotification, importRecord) {
            return new AppendImporterNotificationToImportRecordRequest(matchRecord, importRecord, importerNotification);
        }
        function executeMultipleRequests(primaryControl, requests) {
            Xrm.WebApi.online
                .executeMultiple(requests)
                .then(function (result) {
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
    })(MatchRecord = DefraImports.MatchRecord || (DefraImports.MatchRecord = {}));
})(DefraImports || (DefraImports = {}));
//# sourceMappingURL=matchrecord.subgrid.ribbon.js.map