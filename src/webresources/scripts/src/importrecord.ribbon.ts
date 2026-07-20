namespace DefraImports.ImportRecord {

  class UpdateImportRecordWithItahcRequest {
    public entity: Xrm.Lookup;
    public overwriteExistingData: boolean;

    constructor(entity: Xrm.Lookup, overwriteExistingData: boolean) {
      this.entity = entity;
      this.overwriteExistingData = overwriteExistingData;
    }

    public getMetadata() {
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
    }

  }

  export function onFillEmptyDataWithItahc(primaryControl: Form.defraimp_importapplication.Main.Information): void {
    Xrm.Utility.showProgressIndicator("Loading");
    callUpdateImportRecordWithItahcAction(primaryControl, false);
  }

  export function onOverwriteDataWithItahc(primaryControl: Form.defraimp_importapplication.Main.Information): void {
    Xrm.Utility.showProgressIndicator("Loading");
    callUpdateImportRecordWithItahcAction(primaryControl, true);
  }

  function callUpdateImportRecordWithItahcAction(primaryControl: Form.defraimp_importapplication.Main.Information, overwriteExistingData: boolean) {
    const importApplication: Xrm.Lookup = primaryControl.data.entity.getEntityReference();
    const requestObject: UpdateImportRecordWithItahcRequest = new UpdateImportRecordWithItahcRequest(importApplication, overwriteExistingData);

    Xrm.WebApi.online
      .execute(requestObject)
      .then(
        function (success: Xrm.WebApiResponse) {
          executeSuccess(primaryControl);
        },
        executeErrorCallback

      );
  }

  class UpdateImportRecordWithNotificationRequest {
    public entity: Xrm.Lookup;
    public overwriteExistingData: boolean;

    constructor(entity: Xrm.Lookup, overwriteExistingData: boolean) {
      this.entity = entity;
      this.overwriteExistingData = overwriteExistingData;
    }

    public getMetadata() {
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
    }

  }

  export function onFillEmptyDataWithNotification(primaryControl: Form.defraimp_importapplication.Main.Information): void {
    Xrm.Utility.showProgressIndicator("Loading");
    callUpdateImportRecordWithNotificationAction(primaryControl, false);
  }

  export function onOverwriteDataWithNotification(primaryControl: Form.defraimp_importapplication.Main.Information): void {
    Xrm.Utility.showProgressIndicator("Loading");
    callUpdateImportRecordWithNotificationAction(primaryControl, true);
  }

  function callUpdateImportRecordWithNotificationAction(primaryControl: Form.defraimp_importapplication.Main.Information, overwriteExistingData: boolean) {
    const importApplication: Xrm.Lookup = primaryControl.data.entity.getEntityReference();
    const requestObject: UpdateImportRecordWithNotificationRequest = new UpdateImportRecordWithNotificationRequest(importApplication, overwriteExistingData);

    Xrm.WebApi.online
      .execute(requestObject)
      .then(
        (success: Xrm.WebApiResponse) => { executeSuccess(primaryControl); },
        executeErrorCallback
      );
  }

  function executeSuccess(primaryControl: Form.defraimp_importapplication.Main.Information) {
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