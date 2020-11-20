namespace DefraImports.ImporterNotification {

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
    Xrm.Utility.showProgressIndicator("Loading");
    executeCreateImportRecordFromNotification(primaryControl);
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