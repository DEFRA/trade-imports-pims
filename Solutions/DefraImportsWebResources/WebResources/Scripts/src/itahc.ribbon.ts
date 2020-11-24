namespace DefraImports.Itahc {
  class TRACESConfigParamaterConstants
  {
    static entityName = "defraexp_configurationparameter";
    static entityId = "{2bb103d9-b629-eb11-a813-000d3ad82cac}"
  }

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
    checkTracesEnabled(primaryControl);
  }

  function checkTracesEnabled(primaryControl: Form.defraimp_itahc.Main.Information): void {
    Xrm.WebApi.retrieveRecord(TRACESConfigParamaterConstants.entityName, TRACESConfigParamaterConstants.entityId, "?$select=defraexp_value").then(
      (result) => {
        const tracesEnbaled = (/true/i).test(result.defraexp_value);
        if (tracesEnbaled) {
          Xrm.Utility.showProgressIndicator("Loading");
          callCreateImportRecordFromItahcAction(primaryControl);
        }
        else {
          alert("Access TRACES is not enabled. Please create Import Records from Importer Notifications instead");
        }
      },
      (error) => {
        alert(error.message);
      }
    );
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