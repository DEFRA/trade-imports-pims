namespace DefraImports.ImporterNotification {
  class TRACESConfigParamaterConstants
  {
    static entityName = "defraexp_configurationparameter";
    static entityId = "{2bb103d9-b629-eb11-a813-000d3ad82cac}"
  }

  class CreateImportRecordFromNotificationRequest {
    public entity: Xrm.Lookup;

    constructor(entity: Xrm.Lookup) {
      this.entity = entity;
    }

    public getMetadata() {
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
    }
  }

  export function onCreateImportRecordFromNotification(primaryControl: Form.defraimp_importernotification.Main.Information) {
    checkTracesEnabled(primaryControl);
  }

  function checkTracesEnabled(primaryControl: Form.defraimp_importernotification.Main.Information): void {
    Xrm.WebApi.retrieveRecord(TRACESConfigParamaterConstants.entityName, TRACESConfigParamaterConstants.entityId, "?$select=defraexp_value").then(
      (result) => {
        const tracesEnbaled = (/false/i).test(result.defraexp_value);
        if (tracesEnbaled) {
          Xrm.Utility.showProgressIndicator("Loading");
          executeCreateImportRecordFromNotification(primaryControl);
        }
        else {
          alert("Access to TRACES is still enabled. Please create Import Records from ITAHCs.");
        }
      },
      (error) => {
        alert(error.message);
      }
    );
  }

  function executeCreateImportRecordFromNotification(primaryControl: Form.defraimp_importernotification.Main.Information) {
    const notification: Xrm.Lookup = primaryControl.data.entity.getEntityReference();
    const requestObject: CreateImportRecordFromNotificationRequest = new CreateImportRecordFromNotificationRequest(notification);
    
    Xrm.WebApi.online
      .execute(requestObject)
      .then(
        () => { createImportRecordFromNotificationSuccess(primaryControl); },
        createImportRecordFromNotificationError
      );
  }

  function createImportRecordFromNotificationSuccess(primaryControl: Form.defraimp_importernotification.Main.Information) {
    Xrm.Utility.closeProgressIndicator();
    primaryControl.data.refresh(false);
  }

  function createImportRecordFromNotificationError(error: Xrm.ErrorCallbackObject) {
    Xrm.Utility.closeProgressIndicator();
    const errorOptions: Xrm.ErrorOptions = {
      errorCode: error.errorCode,
      message: error.message,
    }
    Xrm.Navigation.openErrorDialog(errorOptions);
  }
}