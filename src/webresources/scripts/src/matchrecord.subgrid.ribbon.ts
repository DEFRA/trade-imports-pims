type ActionRequest = { getMetadata(): object };
type Reference = Xrm.LookupValue;
type MetadataParameter = { typeName: string; structuralProperty: number };
class AppendItahcToImportRecordRequest implements ActionRequest {
  public constructor(
    public entity: Reference,
    public importRecord: Reference,
    public itahc: Reference
  ) {}
  public getMetadata() {
    return metadata("defraimp_AppendITAHCtoImportRecord", {
      itahc: "mscrm.defraimp_itahc",
      importRecord: "mscrm.defraimp_importapplication",
    });
  }
}
class AppendImporterNotificationToImportRecordRequest implements ActionRequest {
  public constructor(
    public entity: Reference,
    public importRecord: Reference,
    public importerNotification: Reference
  ) {}
  public getMetadata() {
    return metadata("defraimp_AppendImporterNotificationtoImportRecord", {
      importerNotification: "mscrm.defraimp_importernotification",
      importRecord: "mscrm.defraimp_importapplication",
    });
  }
}
class CreateImportRecordFromITAHCRequest implements ActionRequest {
  public constructor(public entity: Reference, public itahc: Reference) {}
  public getMetadata() {
    return metadata("defraimp_MatchRecordCreateImportRecordfromITAHC", {
      itahc: "mscrm.defraimp_itahc",
    });
  }
}
export function onAppendItahc(primaryControl: Xrm.FormContext): void {
  void saveThen(primaryControl, "Appending ITAHCs", async () => {
    const match = primaryControl.data.entity.getEntityReference();
    const itahc = lookup(primaryControl, "defraimp_itahc");
    await executeMultiple(
      primaryControl,
      selected(primaryControl).map(
        (record) => new AppendItahcToImportRecordRequest(match, record, itahc)
      )
    );
  });
}
export function onAppendImporterNotification(
  primaryControl: Xrm.FormContext
): void {
  void saveThen(
    primaryControl,
    "Appending Importer Notifications",
    async () => {
      const match = primaryControl.data.entity.getEntityReference();
      const notification = lookup(
        primaryControl,
        "defraimp_importernotification"
      );
      await executeMultiple(
        primaryControl,
        selected(primaryControl).map(
          (record) =>
            new AppendImporterNotificationToImportRecordRequest(
              match,
              record,
              notification
            )
        )
      );
    }
  );
}
export function onCreateImportRecordFromITAHC(
  primaryControl: Xrm.FormContext
): void {
  void saveThen(primaryControl, "Creating Import Record", async () => {
    await executeOne(
      primaryControl,
      new CreateImportRecordFromITAHCRequest(
        primaryControl.data.entity.getEntityReference(),
        lookup(primaryControl, "defraimp_itahc")
      )
    );
  });
}
// TODO: Handle a rejected form save when the ribbon behavior is next reviewed.
async function saveThen(
  form: Xrm.FormContext,
  message: string,
  action: () => Promise<void>
): Promise<void> {
  await form.data.save();
  Xrm.Utility.showProgressIndicator(message);
  await action();
}
function selected(form: Xrm.FormContext): Reference[] {
  const relatedImportRecords = form.getControl<Xrm.Controls.GridControl>(
    "RelatedImportRecords"
  );
  if (!relatedImportRecords) {
    throw new Error("RelatedImportRecords grid was not found on the form.");
  }

  const rows = relatedImportRecords.getGrid().getSelectedRows();
  const result: Reference[] = [];
  rows.forEach((row) =>
    result.push(row.getData().getEntity().getEntityReference())
  );
  return result;
}
function lookup(form: Xrm.FormContext, name: string): Reference {
  const attribute = form.getAttribute<Xrm.Attributes.LookupAttribute>(name);
  if (!attribute) {
    throw new Error(`Lookup attribute '${name}' was not found.`);
  }

  const value = attribute.getValue();
  if (!value || value.length === 0) {
    throw new Error(`Lookup attribute '${name}' does not contain a value.`);
  }

  return value[0];
}
async function executeOne(
  form: Xrm.FormContext,
  request: ActionRequest
): Promise<void> {
  try {
    await Xrm.WebApi.online.execute(request);
    await success(form);
  } catch (error) {
    await failure(error);
  }
}
async function executeMultiple(
  form: Xrm.FormContext,
  requests: ActionRequest[]
): Promise<void> {
  try {
    await Xrm.WebApi.online.executeMultiple(requests);
    await success(form);
  } catch (error) {
    await failure(error);
  }
}
async function success(form: Xrm.FormContext): Promise<void> {
  Xrm.Utility.closeProgressIndicator();
  await form.data.refresh(false);
}
async function failure(error: unknown): Promise<void> {
  Xrm.Utility.closeProgressIndicator();
  await Xrm.Navigation.openErrorDialog({
    message: error instanceof Error ? error.message : String(error),
  });
}
function metadata(operationName: string, parameters: Record<string, string>) {
  const parameterTypes: Record<string, MetadataParameter> = {
    entity: { typeName: "mscrm.defraimp_matchrecord", structuralProperty: 5 },
  };
  for (const name of Object.keys(parameters))
    parameterTypes[name] = { typeName: parameters[name], structuralProperty: 5 };
  return {
    boundParameter: "entity",
    operationType: 0,
    operationName,
    parameterTypes,
  };
}
