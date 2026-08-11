const tracesConfigParameter = {
  entityName: "defraexp_configurationparameter",
  entityId: "{2bb103d9-b629-eb11-a813-000d3ad82cac}",
} as const;
class CreateImportRecordFromItahcRequest {
  public constructor(public entity: Xrm.LookupValue) {}
  public getMetadata() {
    return {
      boundParameter: "entity",
      operationType: 0,
      operationName: "defraimp_CreateImportRecordFromItahc",
      parameterTypes: {
        entity: { typeName: "mscrm.defraimp_itahc", structuralProperty: 5 },
      },
    };
  }
}
export async function onCreateImportRecordFromItahc(
  primaryControl: Xrm.FormContext
): Promise<void> {
  try {
    const result: { defraexp_value: string } = await Xrm.WebApi.retrieveRecord(
      tracesConfigParameter.entityName,
      tracesConfigParameter.entityId,
      "?$select=defraexp_value"
    );
    if (/true/i.test(result.defraexp_value)) {
      Xrm.Utility.showProgressIndicator("Loading");
      await createImportRecordFromItahc(primaryControl);
    } else
      await Xrm.Navigation.openAlertDialog({
        text: "Access TRACES is not enabled. Please create Import Records from Importer Notifications instead",
      });
  } catch (error) {
    await Xrm.Navigation.openAlertDialog({ text: getErrorMessage(error) });
  }
}
async function createImportRecordFromItahc(
  primaryControl: Xrm.FormContext
): Promise<void> {
  try {
    await Xrm.WebApi.online.execute(
      new CreateImportRecordFromItahcRequest(
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
