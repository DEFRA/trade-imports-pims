namespace DefraImports.Itahc {

  class CreateImportRecordFromItahcRequest {
    public entity: Xrm.Lookup;

    constructor(entity: Xrm.Lookup) {
      this.entity = entity;
    }

    public getMetadata() {
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
    }
  }

  export function onCreateImportRecordFromItahc(primaryControl: Form.defraimp_itahc.Main.Information): void {
    Xrm.Utility.showProgressIndicator("Loading");
    callCreateImportRecordFromItahcAction(primaryControl);
  }

  function callCreateImportRecordFromItahcAction(primaryControl: Form.defraimp_itahc.Main.Information): void {
    const itahc: Xrm.Lookup = primaryControl.data.entity.getEntityReference();
    const requestObject: CreateImportRecordFromItahcRequest = new CreateImportRecordFromItahcRequest(itahc);

    Xrm.WebApi.online
      .execute(requestObject)
      .then(
        function (success: Xrm.WebApiResponse) {
          executeSuccess(primaryControl);
        },
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