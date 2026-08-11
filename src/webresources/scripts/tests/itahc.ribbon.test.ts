import { makeFormContext, setupXrm } from "./xrm-mock.helper";
import { onCreateImportRecordFromItahc } from "../src/itahc.ribbon";

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
    id: "itahc-1",
    name: "ITAHC 1",
    entityType: "defraimp_itahc",
  });
  return fc;
}

// ---------------------------------------------------------------------------
// onCreateImportRecordFromItahc
// ---------------------------------------------------------------------------

describe("onCreateImportRecordFromItahc", () => {
  test("retrieves the traces configuration parameter record", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "true" });
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromItahc(pc as unknown as Xrm.FormContext);
    expect(xrm.WebApi.retrieveRecord).toHaveBeenCalledWith(
      "defraexp_configurationparameter",
      "{2bb103d9-b629-eb11-a813-000d3ad82cac}",
      "?$select=defraexp_value"
    );
  });

  test("shows progress and creates record when defraexp_value is 'true'", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "true" });
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromItahc(pc as unknown as Xrm.FormContext);
    expect(xrm.Utility.showProgressIndicator).toHaveBeenCalledWith("Loading");
    expect(xrm.WebApi.online.execute).toHaveBeenCalledTimes(1);
  });

  test("shows progress and creates record when defraexp_value is 'True' (case-insensitive)", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "True" });
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromItahc(pc as unknown as Xrm.FormContext);
    expect(xrm.Utility.showProgressIndicator).toHaveBeenCalledWith("Loading");
    expect(xrm.WebApi.online.execute).toHaveBeenCalledTimes(1);
  });

  test("shows alert dialog (not record creation) when defraexp_value is 'false'", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "false" });
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromItahc(pc as unknown as Xrm.FormContext);
    expect(xrm.WebApi.online.execute).not.toHaveBeenCalled();
    expect(xrm.Navigation.openAlertDialog).toHaveBeenCalledWith({
      text: "Access TRACES is not enabled. Please create Import Records from Importer Notifications instead",
    });
  });

  test("uses the entity reference from primaryControl when executing", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "true" });
    const entityRef = { id: "itahc-99", name: "ITAHC 99", entityType: "defraimp_itahc" };
    const pc = buildPrimaryControl();
    pc.data.entity.getEntityReference.mockReturnValue(entityRef);
    await onCreateImportRecordFromItahc(pc as unknown as Xrm.FormContext);
    const request = firstMockCallArg<{ entity: typeof entityRef }>(
      xrm.WebApi.online.execute
    );
    expect(request.entity).toEqual(entityRef);
  });

  test("uses defraimp_CreateImportRecordFromItahc operation name", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "true" });
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromItahc(pc as unknown as Xrm.FormContext);
    const request = firstMockCallArg<{ getMetadata(): { operationName: string } }>(
      xrm.WebApi.online.execute
    );
    expect(request.getMetadata().operationName).toBe("defraimp_CreateImportRecordFromItahc");
  });

  test("closes progress and refreshes form after successful execute", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "true" });
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromItahc(pc as unknown as Xrm.FormContext);
    expect(xrm.Utility.closeProgressIndicator).toHaveBeenCalled();
    expect(pc.data.refresh).toHaveBeenCalledWith(false);
  });

  test("closes progress and opens error dialog when inner execute fails", async () => {
    xrm.WebApi.retrieveRecord.mockResolvedValue({ defraexp_value: "true" });
    xrm.WebApi.online.execute.mockRejectedValue(new Error("ITAHC execute error"));
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromItahc(pc as unknown as Xrm.FormContext);
    expect(xrm.Utility.closeProgressIndicator).toHaveBeenCalled();
    expect(xrm.Navigation.openErrorDialog).toHaveBeenCalledWith({ message: "ITAHC execute error" });
  });

  test("opens alert dialog when retrieveRecord throws", async () => {
    xrm.WebApi.retrieveRecord.mockRejectedValue(new Error("Network error"));
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromItahc(pc as unknown as Xrm.FormContext);
    expect(xrm.Navigation.openAlertDialog).toHaveBeenCalledWith({ text: "Network error" });
  });

  test("passes non-Error thrown value as string to alert dialog", async () => {
    xrm.WebApi.retrieveRecord.mockRejectedValue(42);
    const pc = buildPrimaryControl();
    await onCreateImportRecordFromItahc(pc as unknown as Xrm.FormContext);
    expect(xrm.Navigation.openAlertDialog).toHaveBeenCalledWith({ text: "42" });
  });
});
