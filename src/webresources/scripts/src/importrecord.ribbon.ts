type ImportRecordRequest = {
  entity: Xrm.LookupValue;
  overwriteExistingData: boolean;
  getMetadata(): object;
};

class UpdateImportRecordWithItahcRequest implements ImportRecordRequest {
  public constructor(
    public entity: Xrm.LookupValue,
    public overwriteExistingData: boolean
  ) {}
  public getMetadata() {
    return {
      boundParameter: "entity",
      operationType: 0,
      operationName: "defraimp_UpdateImportRecordWithItahc",
      parameterTypes: {
        entity: {
          typeName: "mscrm.defraimp_importapplication",
          structuralProperty: 5,
        },
        overwriteExistingData: {
          typename: "Edm.Boolean",
          structuralProperty: 1,
        },
      },
    };
  }
}
class UpdateImportRecordWithNotificationRequest implements ImportRecordRequest {
  public constructor(
    public entity: Xrm.LookupValue,
    public overwriteExistingData: boolean
  ) {}
  // TODO: Verify the deployed custom-action metadata's existing `typename` spelling before changing it.
  public getMetadata() {
    return {
      boundParameter: "entity",
      operationType: 0,
      operationName: "defraimp_UpdateImportRecordWithNotification",
      parameterTypes: {
        entity: {
          typeName: "mscrm.defraimp_importapplication",
          structuralProperty: 5,
        },
        overwriteExistingData: {
          typename: "Edm.Boolean",
          structuralProperty: 1,
        },
      },
    };
  }
}
export function onFillEmptyDataWithItahc(
  primaryControl: Xrm.FormContext
): void {
  void execute(primaryControl, false, UpdateImportRecordWithItahcRequest);
}
export function onOverwriteDataWithItahc(
  primaryControl: Xrm.FormContext
): void {
  void execute(primaryControl, true, UpdateImportRecordWithItahcRequest);
}
export function onFillEmptyDataWithNotification(
  primaryControl: Xrm.FormContext
): void {
  void execute(
    primaryControl,
    false,
    UpdateImportRecordWithNotificationRequest
  );
}
export function onOverwriteDataWithNotification(
  primaryControl: Xrm.FormContext
): void {
  void execute(primaryControl, true, UpdateImportRecordWithNotificationRequest);
}

export function openUrlFromRibbon(
  formContext: Xrm.FormContext,
  schemaName: string
): void {
  ImportRibbon.openUrlField(formContext, schemaName);
}

async function execute(
  primaryControl: Xrm.FormContext,
  overwriteExistingData: boolean,
  Request: new (
    entity: Xrm.LookupValue,
    overwrite: boolean
  ) => ImportRecordRequest
): Promise<void> {
  Xrm.Utility.showProgressIndicator("Loading");
  try {
    await Xrm.WebApi.online.execute(
      new Request(
        primaryControl.data.entity.getEntityReference(),
        overwriteExistingData
      )
    );
    Xrm.Utility.closeProgressIndicator();
    await primaryControl.data.refresh(false);
  } catch (error) {
    Xrm.Utility.closeProgressIndicator();
    await Xrm.Navigation.openErrorDialog({ message: getErrorMessage(error) });
  }
}
function getErrorMessage(error: unknown): string {
  return error instanceof Error ? error.message : String(error);
}

class ImportRibbon {
  private static readonly Ids = {
    UrlErrorId: "ERROR_OPENURL",
    UrlMissingSchemaId: "ERROR_MISSINGURLSCHEMA",
  };

  private static readonly Errors = {
    UrlErrorMessage: "URL is missing for this Import Record.",
    UrlMissingSchema: "Notification id field not found on Import Record.",
  };

  private static readonly ConfigParameterConstants = {
    entityName: "defraexp_configurationparameter",
    decisionHubEntityId: "537cea15-01d5-ec11-a7b5-0022489ef71e",
  };

  public static openUrlField(
    formContext: Xrm.FormContext,
    schemaName: string
  ): void {
    formContext.ui.clearFormNotification(ImportRibbon.Ids.UrlErrorId);
    formContext.ui.clearFormNotification(ImportRibbon.Ids.UrlMissingSchemaId);

    const urlField =
      formContext.getAttribute<Xrm.Attributes.LookupAttribute>(schemaName);

    if (!urlField) {
      formContext.ui.setFormNotification(
        ImportRibbon.Errors.UrlMissingSchema,
        "ERROR",
        ImportRibbon.Ids.UrlMissingSchemaId
      );
      return;
    }

    const value = urlField.getValue();
    const notificationId = value?.[0]?.name;

    if (!notificationId || notificationId === "") {
      formContext.ui.setFormNotification(
        ImportRibbon.Errors.UrlErrorMessage,
        "ERROR",
        ImportRibbon.Ids.UrlErrorId
      );
      return;
    }

    void ImportRibbon.openDecisionHubNotificationUrl(formContext, notificationId);
  }

  private static async openDecisionHubNotificationUrl(
    formContext: Xrm.FormContext,
    notificationId: string
  ): Promise<void> {
    try {
      const result: { defraexp_value: string } = await Xrm.WebApi.retrieveRecord(
        ImportRibbon.ConfigParameterConstants.entityName,
        ImportRibbon.ConfigParameterConstants.decisionHubEntityId,
        "?$select=defraexp_value"
      );
      const url = result.defraexp_value.replace("#notificationid", notificationId);

      Xrm.Navigation.openUrl(url);
    } catch (error) {
      formContext.ui.setFormNotification(
        error instanceof Error ? error.message : String(error),
        "ERROR",
        ImportRibbon.Ids.UrlErrorId
      );
    }
  }
}
