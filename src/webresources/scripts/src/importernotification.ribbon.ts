const tracesConfigParameter = {
  entityName: "defraexp_configurationparameter",
  entityId: "{2bb103d9-b629-eb11-a813-000d3ad82cac}",
} as const;

class CreateImportRecordFromNotificationRequest {
  public constructor(public entity: Xrm.LookupValue) {}
  public getMetadata() {
    return {
      boundParameter: "entity",
      operationType: 0,
      operationName: "defraimp_CreateImportRecordFromNotification",
      parameterTypes: {
        entity: {
          typeName: "mscrm.defraimp_importernotification",
          structuralProperty: 5,
        },
      },
    };
  }
}

export async function onCreateImportRecordFromNotification(
  primaryControl: Xrm.FormContext
): Promise<void> {
  try {
    const result: { defraexp_value: string } = await Xrm.WebApi.retrieveRecord(
      tracesConfigParameter.entityName,
      tracesConfigParameter.entityId,
      "?$select=defraexp_value"
    );
    const isTracesDisabled = /false/i.test(result.defraexp_value);
    if (isTracesDisabled) {
      Xrm.Utility.showProgressIndicator("Loading");
      await executeCreateImportRecordFromNotification(primaryControl);
    } else
      await Xrm.Navigation.openAlertDialog({
        text: "Access to TRACES is still enabled. Please create Import Records from ITAHCs.",
      });
  } catch (error) {
    await Xrm.Navigation.openAlertDialog({ text: getErrorMessage(error) });
  }
}

async function executeCreateImportRecordFromNotification(
  primaryControl: Xrm.FormContext
): Promise<void> {
  try {
    await Xrm.WebApi.online.execute(
      new CreateImportRecordFromNotificationRequest(
        primaryControl.data.entity.getEntityReference()
      )
    );
    await primaryControl.data.refresh(false);
  } catch (error) {
    await Xrm.Navigation.openErrorDialog({ message: getErrorMessage(error) });
  } finally {
    Xrm.Utility.closeProgressIndicator();
  }
}

function getErrorMessage(error: unknown): string {
  return error instanceof Error ? error.message : String(error);
}
