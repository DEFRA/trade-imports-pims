import {
  makeAttr,
  makeControl,
  makeFormContext,
  makeSelectedRows,
  setupXrm,
  flushPromises,
} from "./xrm-mock.helper";
import {
  onAppendItahc,
  onAppendImporterNotification,
  onCreateImportRecordFromITAHC,
} from "../src/matchrecord.subgrid.ribbon";

let xrm: ReturnType<typeof setupXrm>;

function firstMockCallArg<T>(mockFn: jest.Mock): T {
  return (mockFn.mock.calls as Array<[T]>)[0][0];
}

beforeEach(() => {
  xrm = setupXrm();
});

function buildPrimaryControl(opts: {
  selectedRefs?: Array<{ id: string; name: string; entityType: string }>;
  itahcRef?: { id: string; name: string; entityType: string };
  importerNotificationRef?: { id: string; name: string; entityType: string };
  matchRef?: { id: string; name: string; entityType: string };
} = {}) {
  const selectedRefs = opts.selectedRefs ?? [
    { id: "import-1", name: "Import 1", entityType: "defraimp_importapplication" },
  ];
  const itahcRef = opts.itahcRef ?? { id: "itahc-1", name: "ITAHC 1", entityType: "defraimp_itahc" };
  const importerNotificationRef = opts.importerNotificationRef ?? {
    id: "notif-1",
    name: "Notification 1",
    entityType: "defraimp_importernotification",
  };
  const matchRef = opts.matchRef ?? { id: "match-1", name: "Match 1", entityType: "defraimp_matchrecord" };

  const gridControl = makeControl();
  gridControl.getGrid.mockReturnValue({
    getSelectedRows: jest.fn().mockReturnValue(makeSelectedRows(selectedRefs)),
  });

  const attrs = {
    defraimp_itahc: makeAttr([itahcRef]),
    defraimp_importernotification: makeAttr([importerNotificationRef]),
  };

  const fc = makeFormContext(attrs, { RelatedImportRecords: gridControl });
  fc.data.entity.getEntityReference.mockReturnValue(matchRef);
  return fc;
}

// ---------------------------------------------------------------------------
// onAppendItahc
// ---------------------------------------------------------------------------

describe("onAppendItahc", () => {
  test("saves the form before executing", async () => {
    const pc = buildPrimaryControl();
    onAppendItahc(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(pc.data.save).toHaveBeenCalled();
  });

  test("shows 'Appending ITAHCs' progress indicator", async () => {
    const pc = buildPrimaryControl();
    onAppendItahc(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.Utility.showProgressIndicator).toHaveBeenCalledWith("Appending ITAHCs");
  });

  test("calls executeMultiple with one request per selected record", async () => {
    const selectedRefs = [
      { id: "import-1", name: "Import 1", entityType: "defraimp_importapplication" },
      { id: "import-2", name: "Import 2", entityType: "defraimp_importapplication" },
    ];
    const pc = buildPrimaryControl({ selectedRefs });
    onAppendItahc(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const [requests] = xrm.WebApi.online.executeMultiple.mock.calls[0] as [unknown[]];
    expect(requests).toHaveLength(2);
  });

  test("each request uses defraimp_AppendITAHCtoImportRecord operation", async () => {
    const pc = buildPrimaryControl();
    onAppendItahc(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const [requests] = xrm.WebApi.online.executeMultiple.mock.calls[0] as [Array<{ getMetadata(): { operationName: string } }>];
    expect(requests[0].getMetadata().operationName).toBe("defraimp_AppendITAHCtoImportRecord");
  });

  test("closes progress and refreshes form on success", async () => {
    const pc = buildPrimaryControl();
    onAppendItahc(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.Utility.closeProgressIndicator).toHaveBeenCalled();
    expect(pc.data.refresh).toHaveBeenCalledWith(false);
  });

  test("closes progress and opens error dialog on executeMultiple failure", async () => {
    xrm.WebApi.online.executeMultiple.mockRejectedValue(new Error("Append failed"));
    const pc = buildPrimaryControl();
    onAppendItahc(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.Utility.closeProgressIndicator).toHaveBeenCalled();
    expect(xrm.Navigation.openErrorDialog).toHaveBeenCalledWith({ message: "Append failed" });
  });
});

// ---------------------------------------------------------------------------
// onAppendImporterNotification
// ---------------------------------------------------------------------------

describe("onAppendImporterNotification", () => {
  test("saves the form before executing", async () => {
    const pc = buildPrimaryControl();
    onAppendImporterNotification(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(pc.data.save).toHaveBeenCalled();
  });

  test("shows 'Appending Importer Notifications' progress indicator", async () => {
    const pc = buildPrimaryControl();
    onAppendImporterNotification(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.Utility.showProgressIndicator).toHaveBeenCalledWith("Appending Importer Notifications");
  });

  test("calls executeMultiple with one request per selected record", async () => {
    const selectedRefs = [
      { id: "import-1", name: "Import 1", entityType: "defraimp_importapplication" },
      { id: "import-2", name: "Import 2", entityType: "defraimp_importapplication" },
      { id: "import-3", name: "Import 3", entityType: "defraimp_importapplication" },
    ];
    const pc = buildPrimaryControl({ selectedRefs });
    onAppendImporterNotification(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const [requests] = xrm.WebApi.online.executeMultiple.mock.calls[0] as [unknown[]];
    expect(requests).toHaveLength(3);
  });

  test("each request uses defraimp_AppendImporterNotificationtoImportRecord operation", async () => {
    const pc = buildPrimaryControl();
    onAppendImporterNotification(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const [requests] = xrm.WebApi.online.executeMultiple.mock.calls[0] as [Array<{ getMetadata(): { operationName: string } }>];
    expect(requests[0].getMetadata().operationName).toBe(
      "defraimp_AppendImporterNotificationtoImportRecord"
    );
  });

  test("closes progress and refreshes form on success", async () => {
    const pc = buildPrimaryControl();
    onAppendImporterNotification(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.Utility.closeProgressIndicator).toHaveBeenCalled();
    expect(pc.data.refresh).toHaveBeenCalledWith(false);
  });

  test("passes non-Error thrown value as string to openErrorDialog", async () => {
    xrm.WebApi.online.executeMultiple.mockRejectedValue({ code: 404 });
    const pc = buildPrimaryControl();
    onAppendImporterNotification(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.Navigation.openErrorDialog).toHaveBeenCalledWith({ message: "[object Object]" });
  });
});

// ---------------------------------------------------------------------------
// onCreateImportRecordFromITAHC
// ---------------------------------------------------------------------------

describe("onCreateImportRecordFromITAHC", () => {
  test("saves the form before executing", async () => {
    const pc = buildPrimaryControl();
    onCreateImportRecordFromITAHC(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(pc.data.save).toHaveBeenCalled();
  });

  test("shows 'Creating Import Record' progress indicator", async () => {
    const pc = buildPrimaryControl();
    onCreateImportRecordFromITAHC(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.Utility.showProgressIndicator).toHaveBeenCalledWith("Creating Import Record");
  });

  test("calls WebApi.online.execute (not executeMultiple) with a single request", async () => {
    const pc = buildPrimaryControl();
    onCreateImportRecordFromITAHC(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.WebApi.online.execute).toHaveBeenCalledTimes(1);
    expect(xrm.WebApi.online.executeMultiple).not.toHaveBeenCalled();
  });

  test("uses defraimp_MatchRecordCreateImportRecordfromITAHC operation name", async () => {
    const pc = buildPrimaryControl();
    onCreateImportRecordFromITAHC(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const request = firstMockCallArg<{ getMetadata(): { operationName: string } }>(
      xrm.WebApi.online.execute
    );
    expect(request.getMetadata().operationName).toBe(
      "defraimp_MatchRecordCreateImportRecordfromITAHC"
    );
  });

  test("request entity is the match record reference", async () => {
    const matchRef = { id: "match-99", name: "Match 99", entityType: "defraimp_matchrecord" };
    const pc = buildPrimaryControl({ matchRef });
    onCreateImportRecordFromITAHC(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const request = firstMockCallArg<{
      entity: typeof matchRef;
      itahc: typeof matchRef;
    }>(xrm.WebApi.online.execute);
    expect(request.entity).toEqual(matchRef);
  });

  test("request includes itahc reference from the form attribute", async () => {
    const itahcRef = { id: "itahc-77", name: "ITAHC 77", entityType: "defraimp_itahc" };
    const pc = buildPrimaryControl({ itahcRef });
    onCreateImportRecordFromITAHC(pc as unknown as Xrm.FormContext);
    await flushPromises();
    const request = firstMockCallArg<{ itahc: typeof itahcRef }>(
      xrm.WebApi.online.execute
    );
    expect(request.itahc).toEqual(itahcRef);
  });

  test("closes progress and refreshes form on success", async () => {
    const pc = buildPrimaryControl();
    onCreateImportRecordFromITAHC(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.Utility.closeProgressIndicator).toHaveBeenCalled();
    expect(pc.data.refresh).toHaveBeenCalledWith(false);
  });

  test("closes progress and opens error dialog on failure", async () => {
    xrm.WebApi.online.execute.mockRejectedValue(new Error("Create failed"));
    const pc = buildPrimaryControl();
    onCreateImportRecordFromITAHC(pc as unknown as Xrm.FormContext);
    await flushPromises();
    expect(xrm.Utility.closeProgressIndicator).toHaveBeenCalled();
    expect(xrm.Navigation.openErrorDialog).toHaveBeenCalledWith({ message: "Create failed" });
  });
});
