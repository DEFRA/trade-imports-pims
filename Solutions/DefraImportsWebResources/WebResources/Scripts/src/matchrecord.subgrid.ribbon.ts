namespace DefraImports.MatchRecord
{
    class AppendItahcToImportRecordRequest {
        public entity: Xrm.Lookup
        public importRecord: Xrm.Lookup;
        public itahc: Xrm.Lookup;


        constructor(matchRecord: Xrm.Lookup, importRecord: Xrm.Lookup, itahc: Xrm.Lookup) {
            this.entity = matchRecord;
            this.importRecord = importRecord;
            this.itahc = itahc;
        }

        public getMetadata() {
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
        }
    }
  }

  class AppendImporterNotificationToImportRecordRequest {
    public entity: Xrm.Lookup
    public importRecord: Xrm.Lookup;
    public importerNotification: Xrm.Lookup;


    constructor(matchRecord: Xrm.Lookup, importRecord: Xrm.Lookup, importerNotification: Xrm.Lookup) {
        this.entity = matchRecord;
        this.importRecord = importRecord;
        this.importerNotification = importerNotification;
    }

    public getMetadata() {
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
      }
    }
  }

  class CreateImportRecordFromITAHCRequest {
    public entity: Xrm.Lookup
    public itahc: Xrm.Lookup

    constructor(matchRecord: Xrm.Lookup, itahcRecord: Xrm.Lookup) {
        this.entity = matchRecord;
        this.itahc = itahcRecord;
    }

    public getMetadata() {
        return {
          boundParameter: "entity",
          operationType: 0,
          operationName: "defraimp_MatchRecordCreateImportRecordfromITAHC",
          parameterTypes: {
            entity: {
              typeName: "mscrm.defraimp_matchrecord",
              structuralProperty: 5
            },
            itahc: {
              typeName: "mscrm.defraimp_itahc",
              structuralProperty: 5
            },
        }
      }
    }
  }

export function onAppendItahc(primaryControl: Form.defraimp_matchrecord.Main.Information): void {
    Xrm.Utility.showProgressIndicator("Appending ITAHCs");
    var selectedImportRecords = getSelectedImportRecords(primaryControl);
    var itahc = primaryControl.getAttribute("defraimp_itahc").getValue()[0];
    const matchRecord : Xrm.Lookup = primaryControl.data.entity.getEntityReference();

    var requests: Array<AppendItahcToImportRecordRequest> = [];

    selectedImportRecords.forEach(importRecord => {
        requests.push(generateAppendItahcRequest(matchRecord,itahc,importRecord));
        
    });

    executeMultipleRequests(primaryControl, requests)
  }
  
  export function onAppendImporterNotification(primaryControl: Form.defraimp_matchrecord.Main.Information): void {
    Xrm.Utility.showProgressIndicator("Appending Importer Notifications");
    var selectedImportRecords = getSelectedImportRecords(primaryControl);
    var importerNotification = primaryControl.getAttribute("defraimp_importernotification").getValue()[0];
    const matchRecord : Xrm.Lookup = primaryControl.data.entity.getEntityReference();

    var requests: Array<AppendImporterNotificationToImportRecordRequest> = [];

    selectedImportRecords.forEach(importRecord => {
        requests.push(generateAppendImporterNotificationRequest(matchRecord,importerNotification,importRecord));
        
    });

    executeMultipleRequests(primaryControl, requests)
  }

  export function onCreateImportRecordFromITAHC(primaryControl: Form.defraimp_matchrecord.Main.Information): void {
    Xrm.Utility.showProgressIndicator("Creating Import Record");
    createImportRecordFromItahc(primaryControl);
  }
  
  function getSelectedImportRecords(primaryControl: Form.defraimp_matchrecord.Main.Information): Xrm.EntityReference<any>[] {
      var selectedRows = primaryControl.getControl("RelatedImportRecords").getGrid().getSelectedRows();
      var selectedImportRecords = [];

      selectedRows.forEach(element => {
          selectedImportRecords.push(element.data.entity.getEntityReference());
      });

      return selectedImportRecords;
  }
  
    function generateAppendItahcRequest(matchRecord: Xrm.Lookup, itahc: Xrm.Lookup, importRecord: Xrm.Lookup): AppendItahcToImportRecordRequest {
        return new AppendItahcToImportRecordRequest(matchRecord,importRecord,itahc);
    }

    function generateAppendImporterNotificationRequest(matchRecord: Xrm.Lookup, importerNotification: Xrm.Lookup, importRecord: Xrm.Lookup): AppendImporterNotificationToImportRecordRequest {
      return new AppendImporterNotificationToImportRecordRequest(matchRecord,importRecord,importerNotification);
  }


  function createImportRecordFromItahc(primaryControl: Form.defraimp_matchrecord.Main.Information): void {
    var itahc = primaryControl.getAttribute("defraimp_itahc").getValue()[0];
    const matchRecord : Xrm.Lookup = primaryControl.data.entity.getEntityReference();
    var request = new CreateImportRecordFromITAHCRequest(matchRecord, itahc);

    Xrm.WebApi.online
      .execute(request)
      .then(
        function (success: Xrm.WebApiResponse) {
          executeSuccess(primaryControl);
        },
        executeErrorCallback
      );
  }  

    function executeMultipleRequests(primaryControl: Form.defraimp_matchrecord.Main.Information, requests: Array<any>)
    {
      Xrm.WebApi.online
      .executeMultiple(requests)
      .then(
        (result) => {
            executeSuccess(primaryControl);
          }
        ,
        executeErrorCallback    
      );
    }

    function executeSuccess(primaryControl: Form.defraimp_matchrecord.Main.Information) {
        Xrm.Utility.closeProgressIndicator();
        primaryControl.data.refresh(false);
      }
    
      function executeErrorCallback(error: Xrm.ErrorCallbackObject) {
        Xrm.Utility.closeProgressIndicator();
        const errorOptions: Xrm.ErrorOptions = {
          errorCode: error.errorCode,
          message: error.message,
        }
        Xrm.Navigation.openErrorDialog(errorOptions);
      }
}