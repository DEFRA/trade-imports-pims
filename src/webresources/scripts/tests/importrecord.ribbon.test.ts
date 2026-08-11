import {
  makeAttr,
  makeFormContext,
  setupXrm,
  flushPromises,
} from "./xrm-mock.helper";
import * as subject from "../src/importrecord.ribbon";

let xrm: ReturnType<typeof setupXrm>;

function firstMockCallArg<T>(mockFn: jest.Mock): T {
  return (mockFn.mock.calls as Array<[T]>)[0][0];
}

beforeEach(() => {
  xrm = setupXrm();
});

function buildPrimaryControl(entityRef = { id: "record-1", name: "Import 1", entityType: "defraimp_importapplication" }) {
  const fc = makeFormContext();
  fc.data.entity.getEntityReference.mockReturnValue(entityRef);
  return fc;
}

// ---------------------------------------------------------------------------
// onFillEmptyDataWithItahc
// ---------------------------------------------------------------------------

describe("onFillEmptyDataWithItahc", () => {
  test("shows progress indicator before executing", async () => {
    const pc = buildPrimaryControl();
    subject.onFillEmptyDataWithItahc(pc as unknown as Xrm.FormContext);
    expect(xrm.Utility.showProgressIndicator).toHaveBeenCalledWith("Loading");
    await flushPromises();
  });

  test("calls WebApi.online.execute with overwriteExistingData = false", async () => {
    const entityRef = { id: "r1", name: "R1", entityType: "defraimp_importapplication" };
    const pc = buildPrimaryControl(entityRef);
    subject.onFillEmptyDataWithItahc(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.WebApi.online.execute).toHaveBeenCalledTimes(1);
    const request = firstMockCallArg<{
      entity: typeof entityRef;
      overwriteExistingData: boolean;
    }>(xrm.WebApi.online.execute);
    expect(request.entity).toEqual(entityRef);
    expect(request.overwriteExistingData).toBe(false);
  });

  test("closes progress indicator and refreshes form on success", async () => {
    const pc = buildPrimaryControl();
    subject.onFillEmptyDataWithItahc(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.Utility.closeProgressIndicator).toHaveBeenCalled();
    expect(pc.data.refresh).toHaveBeenCalledWith(false);
  });

  test("closes progress indicator and opens error dialog on failure", async () => {
    xrm.WebApi.online.execute.mockRejectedValue(new Error("Execute failed"));
    const pc = buildPrimaryControl();
    subject.onFillEmptyDataWithItahc(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.Utility.closeProgressIndicator).toHaveBeenCalled();
    expect(xrm.Navigation.openErrorDialog).toHaveBeenCalledWith({
      message: "Execute failed",
    });
  });

  test("passes non-Error thrown value as string to openErrorDialog", async () => {
    xrm.WebApi.online.execute.mockRejectedValue("plain string error");
    const pc = buildPrimaryControl();
    subject.onFillEmptyDataWithItahc(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.Navigation.openErrorDialog).toHaveBeenCalledWith({
      message: "plain string error",
    });
  });
});

// ---------------------------------------------------------------------------
// onOverwriteDataWithNotification
// ---------------------------------------------------------------------------

describe("onOverwriteDataWithNotification", () => {
  test("calls WebApi.online.execute with overwriteExistingData = true", async () => {
    jest.resetModules();
    setupXrm();
    xrm = setupXrm();
    const entityRef = { id: "r2", name: "R2", entityType: "defraimp_importapplication" };
    const pc = buildPrimaryControl(entityRef);
    subject.onOverwriteDataWithNotification(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const request = firstMockCallArg<{
      entity: typeof entityRef;
      overwriteExistingData: boolean;
    }>(xrm.WebApi.online.execute);
    expect(request.entity).toEqual(entityRef);
    expect(request.overwriteExistingData).toBe(true);
  });

  test("uses UpdateImportRecordWithNotificationRequest operation name", async () => {
    jest.resetModules();
    xrm = setupXrm();
    const pc = buildPrimaryControl();
    subject.onOverwriteDataWithNotification(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const request = firstMockCallArg<{ getMetadata(): { operationName: string } }>(
      xrm.WebApi.online.execute
    );
    expect(request.getMetadata().operationName).toBe("defraimp_UpdateImportRecordWithNotification");
  });
});

// ---------------------------------------------------------------------------
// onOverwriteDataWithItahc / onFillEmptyDataWithNotification
// ---------------------------------------------------------------------------

describe("additional importrecord ribbon wrappers", () => {
  test("onOverwriteDataWithItahc calls execute with overwriteExistingData = true", async () => {
    jest.resetModules();
    xrm = setupXrm();
    const entityRef = {
      id: "r3",
      name: "R3",
      entityType: "defraimp_importapplication",
    };
    const pc = buildPrimaryControl(entityRef);
    subject.onOverwriteDataWithItahc(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const request = firstMockCallArg<{
      entity: typeof entityRef;
      overwriteExistingData: boolean;
    }>(xrm.WebApi.online.execute);
    expect(request.entity).toEqual(entityRef);
    expect(request.overwriteExistingData).toBe(true);
  });

  test("onOverwriteDataWithItahc uses UpdateImportRecordWithItahc operation", async () => {
    jest.resetModules();
    xrm = setupXrm();
    const pc = buildPrimaryControl();
    subject.onOverwriteDataWithItahc(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const request = firstMockCallArg<{
      getMetadata(): { operationName: string };
    }>(xrm.WebApi.online.execute);
    expect(request.getMetadata().operationName).toBe(
      "defraimp_UpdateImportRecordWithItahc"
    );
  });

  test("onFillEmptyDataWithNotification calls execute with overwriteExistingData = false", async () => {
    jest.resetModules();
    xrm = setupXrm();
    const entityRef = {
      id: "r4",
      name: "R4",
      entityType: "defraimp_importapplication",
    };
    const pc = buildPrimaryControl(entityRef);
    subject.onFillEmptyDataWithNotification(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const request = firstMockCallArg<{
      entity: typeof entityRef;
      overwriteExistingData: boolean;
    }>(xrm.WebApi.online.execute);
    expect(request.entity).toEqual(entityRef);
    expect(request.overwriteExistingData).toBe(false);
  });

  test("onFillEmptyDataWithNotification uses UpdateImportRecordWithNotification operation", async () => {
    jest.resetModules();
    xrm = setupXrm();
    const pc = buildPrimaryControl();
    subject.onFillEmptyDataWithNotification(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const request = firstMockCallArg<{
      getMetadata(): { operationName: string };
    }>(xrm.WebApi.online.execute);
    expect(request.getMetadata().operationName).toBe(
      "defraimp_UpdateImportRecordWithNotification"
    );
  });
});

// ---------------------------------------------------------------------------
// onFillEmptyDataWithItahc uses UpdateImportRecordWithItahcRequest
// ---------------------------------------------------------------------------

describe("onFillEmptyDataWithItahc request metadata", () => {
  test("uses UpdateImportRecordWithItahc operation name", async () => {
    jest.resetModules();
    xrm = setupXrm();
    const pc = buildPrimaryControl();
    subject.onFillEmptyDataWithItahc(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const request = firstMockCallArg<{ getMetadata(): { operationName: string } }>(
      xrm.WebApi.online.execute
    );
    expect(request.getMetadata().operationName).toBe("defraimp_UpdateImportRecordWithItahc");
  });
});

// ---------------------------------------------------------------------------
// openUrlFromRibbon / ImportRibbon.openUrlField
// ---------------------------------------------------------------------------

describe("openUrlFromRibbon", () => {
  test("clears existing notifications before attempting to open URL", () => {
    const fc = makeFormContext({});
    // getAttribute returns undefined - field not found path
    subject.openUrlFromRibbon(fc as unknown as Xrm.FormContext, "missing_field");
    expect(fc.ui.clearFormNotification).toHaveBeenCalledWith("ERROR_OPENURL");
    expect(fc.ui.clearFormNotification).toHaveBeenCalledWith("ERROR_MISSINGURLSCHEMA");
  });

  test("shows missing schema notification when attribute does not exist", () => {
    const fc = makeFormContext({});
    subject.openUrlFromRibbon(fc as unknown as Xrm.FormContext, "nonexistent_field");
    expect(fc.ui.setFormNotification).toHaveBeenCalledWith(
      "Notification id field not found on Import Record.",
      "ERROR",
      "ERROR_MISSINGURLSCHEMA"
    );
  });

  test("shows URL error notification when attribute value is null", () => {
    const urlField = makeAttr<null>(null);
    const fc = makeFormContext({ defraimp_url: urlField });
    subject.openUrlFromRibbon(fc as unknown as Xrm.FormContext, "defraimp_url");
    expect(fc.ui.setFormNotification).toHaveBeenCalledWith(
      "URL is missing for this Import Record.",
      "ERROR",
      "ERROR_OPENURL"
    );
  });

  test("shows URL error notification when lookup name is empty string", () => {
    const urlField = makeAttr([{ id: "x", name: "", entityType: "y" }]);
    const fc = makeFormContext({ defraimp_url: urlField });
    subject.openUrlFromRibbon(fc as unknown as Xrm.FormContext, "defraimp_url");
    expect(fc.ui.setFormNotification).toHaveBeenCalledWith(
      "URL is missing for this Import Record.",
      "ERROR",
      "ERROR_OPENURL"
    );
  });

  test("retrieves config record and opens URL when notification id is present", async () => {
    const notificationId = "NOTIF-ABC-123";
    xrm.WebApi.retrieveRecord.mockResolvedValue({
      defraexp_value: "https://example.com/item?id=#notificationid",
    });
    const urlField = makeAttr([{ id: "x", name: notificationId, entityType: "y" }]);
    const fc = makeFormContext({ defraimp_url: urlField });
    subject.openUrlFromRibbon(fc as unknown as Xrm.FormContext, "defraimp_url");
    await flushPromises();
    expect(xrm.Navigation.openUrl).toHaveBeenCalledWith(
      `https://example.com/item?id=${notificationId}`
    );
  });

  test("does not open URL when config record retrieval fails", async () => {
    xrm.WebApi.retrieveRecord.mockRejectedValue(new Error("Not found"));
    const urlField = makeAttr([{ id: "x", name: "NOTIF-1", entityType: "y" }]);
    const fc = makeFormContext({ defraimp_url: urlField });
    subject.openUrlFromRibbon(fc as unknown as Xrm.FormContext, "defraimp_url");
    await flushPromises();
    expect(xrm.Navigation.openUrl).not.toHaveBeenCalled();
  });
});
