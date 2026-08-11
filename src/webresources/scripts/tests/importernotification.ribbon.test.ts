import { makeFormContext, setupXrm } from "./xrm-mock.helper";
import { onCreateImportRecordFromNotification } from "../src/importernotification.ribbon";

let xrm: ReturnType<typeof setupXrm>;

function firstMockCallArg<T>(mockFn: jest.Mock): T {
  return (mockFn.mock.calls as Array<[T]>)[0][0];
}

beforeEach(() => {
  xrm = setupXrm();
});

function buildPrimaryControl() {
  const fc = makeFormContext();
  fc.data.entity.getEntityReference.mockReturnValue({
    id: "notif-1",
    name: "Notification 1",
    entityType: "defraimp_importernotification",
  });
  return fc;
}

// ---------------------------------------------------------------------------
// onCreateImportRecordFromNotification
// ---------------------------------------------------------------------------

describe("onCreateImportRecordFromNotification", () => {
  test("retrieves the traces configuration parameter record", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "true" });
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromNotification(pc as unknown as Xrm.FormContext);
    expect(xrm.WebApi.retrieveRecord).toHaveBeenCalledWith(
      "defraexp_configurationparameter",
      "{2bb103d9-b629-eb11-a813-000d3ad82cac}",
      "?$select=defraexp_value"
    );
  });

  // The variable `tracesEnabled` is set via `/false/i.test(value)`:
  // when defraexp_value is "false", tracesEnabled = true → proceeds to create record.
  test("shows progress and creates record when defraexp_value is 'false'", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "false" });
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromNotification(pc as unknown as Xrm.FormContext);
    expect(xrm.Utility.showProgressIndicator).toHaveBeenCalledWith("Loading");
    expect(xrm.WebApi.online.execute).toHaveBeenCalledTimes(1);
  });

  test("shows progress and creates record when defraexp_value is 'False' (case-insensitive)", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "False" });
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromNotification(pc as unknown as Xrm.FormContext);
    expect(xrm.Utility.showProgressIndicator).toHaveBeenCalledWith("Loading");
    expect(xrm.WebApi.online.execute).toHaveBeenCalledTimes(1);
  });

  test("shows alert dialog (not record creation) when defraexp_value is 'true'", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "true" });
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromNotification(pc as unknown as Xrm.FormContext);
    expect(xrm.WebApi.online.execute).not.toHaveBeenCalled();
    expect(xrm.Navigation.openAlertDialog).toHaveBeenCalledWith({
      text: "Access to TRACES is still enabled. Please create Import Records from ITAHCs.",
    });
  });

  test("shows alert dialog when defraexp_value is 'TRUE' (case-insensitive, does not match /false/i)", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "TRUE" });
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromNotification(pc as unknown as Xrm.FormContext);
    expect(xrm.WebApi.online.execute).not.toHaveBeenCalled();
    expect(xrm.Navigation.openAlertDialog).toHaveBeenCalled();
  });

  test("uses the entity reference from primaryControl when executing", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "false" });
    const entityRef = { id: "notif-42", name: "Notif 42", entityType: "defraimp_importernotification" };
    const pc = buildPrimaryControl();
    pc.data.entity.getEntityReference.mockReturnValue(entityRef);
    await onCreateImportRecordFromNotification(pc as unknown as Xrm.FormContext);
    const request = firstMockCallArg<{ entity: typeof entityRef }>(
      xrm.WebApi.online.execute
    );
    expect(request.entity).toEqual(entityRef);
  });

  test("uses defraimp_CreateImportRecordFromNotification operation name", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "false" });
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromNotification(pc as unknown as Xrm.FormContext);
    const request = firstMockCallArg<{ getMetadata(): { operationName: string } }>(
      xrm.WebApi.online.execute
    );
    expect(request.getMetadata().operationName).toBe("defraimp_CreateImportRecordFromNotification");
  });

  test("closes progress and refreshes form after successful execute", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "false" });
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromNotification(pc as unknown as Xrm.FormContext);
    expect(xrm.Utility.closeProgressIndicator).toHaveBeenCalled();
    expect(pc.data.refresh).toHaveBeenCalledWith(false);
  });

  test("closes progress and opens error dialog when inner execute fails", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "false" });
    xrm.WebApi.online.execute.mockRejectedValue(new Error("Execute error"));
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromNotification(pc as unknown as Xrm.FormContext);
    expect(xrm.Utility.closeProgressIndicator).toHaveBeenCalled();
    expect(xrm.Navigation.openErrorDialog).toHaveBeenCalledWith({ message: "Execute error" });
  });

  test("opens alert dialog when retrieveRecord throws", async () => {
    xrm.WebApi.retrieveRecord.mockRejectedValue(new Error("Network error"));
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromNotification(pc as unknown as Xrm.FormContext);
    expect(xrm.Navigation.openAlertDialog).toHaveBeenCalledWith({ text: "Network error" });
  });

  test("passes non-Error thrown value as string to alert dialog", async () => {
    xrm.WebApi.retrieveRecord.mockRejectedValue("raw error value");
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromNotification(pc as unknown as Xrm.FormContext);
    expect(xrm.Navigation.openAlertDialog).toHaveBeenCalledWith({ text: "raw error value" });
  });
});
