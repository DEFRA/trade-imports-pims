import {
  makeAttr,
  makeControl,
  makeFormContext,
  makeExecutionContext,
  makeSection,
  makeTab,
  setupXrm,
  flushPromises,
} from "./xrm-mock.helper";

let xrm: ReturnType<typeof setupXrm>;

// Use fresh module state for each test to avoid wasManualPostImportCheckSet bleed-through.
let subject: typeof import("../src/importrecord.form");

beforeEach(() => {
  jest.resetModules();
  xrm = setupXrm();
  // eslint-disable-next-line @typescript-eslint/no-require-imports, @typescript-eslint/no-unsafe-assignment
  subject = require("../src/importrecord.form");
});

function getPreventDefaultMock(
  executionContext: ReturnType<typeof makeExecutionContext>
): jest.Mock {
  return (
    executionContext.getEventArgs() as {
      preventDefault: jest.Mock;
    }
  ).preventDefault;
}

// ---------------------------------------------------------------------------
// OnLoadQuickCreateForm
// ---------------------------------------------------------------------------

describe("OnLoadQuickCreateForm", () => {
  test("sets ownerid to null when form type is Create (1)", () => {
    const ownerid = makeAttr([{ id: "owner-1", name: "Owner", entityType: "systemuser" }]);
    const fc = makeFormContext({ ownerid }, {}, {}, { formType: 1 });
    subject.OnLoadQuickCreateForm(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(ownerid.setValue).toHaveBeenCalledWith(null);
  });

  test("does not touch ownerid when form type is Update (2)", () => {
    const ownerid = makeAttr([{ id: "owner-1", name: "Owner", entityType: "systemuser" }]);
    const fc = makeFormContext({ ownerid }, {}, {}, { formType: 2 });
    subject.OnLoadQuickCreateForm(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(ownerid.setValue).not.toHaveBeenCalled();
  });

  test("does not touch ownerid when form type is ReadOnly (3)", () => {
    const ownerid = makeAttr(null);
    const fc = makeFormContext({ ownerid }, {}, {}, { formType: 3 });
    subject.OnLoadQuickCreateForm(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(ownerid.setValue).not.toHaveBeenCalled();
  });
});

// ---------------------------------------------------------------------------
// showOrHideNonComplianceOther
// ---------------------------------------------------------------------------

describe("showOrHideNonComplianceOther", () => {
  function buildCtx(multiSelectValue: number[] | null) {
    const attr = makeAttr(multiSelectValue);
    const ctrl = makeControl();
    const fc = makeFormContext(
      { defraimp_typesofnoncompliance: attr },
      { defraimp_noncomplianceothercomments: ctrl }
    );
    return { attr, ctrl, fc };
  }

  test("hides control when multi-select value is null", () => {
    const { ctrl, fc } = buildCtx(null);
    subject.showOrHideNonComplianceOther(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(ctrl.setVisible).toHaveBeenCalledWith(false);
  });

  test("shows control when multi-select includes 714100005", () => {
    const { ctrl, fc } = buildCtx([714100001, 714100005]);
    subject.showOrHideNonComplianceOther(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(ctrl.setVisible).toHaveBeenCalledWith(true);
  });

  test("hides control when multi-select does not include 714100005", () => {
    const { ctrl, fc } = buildCtx([714100001, 714100002]);
    subject.showOrHideNonComplianceOther(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(ctrl.setVisible).toHaveBeenCalledWith(false);
  });

  test("shows control when multi-select contains only 714100005", () => {
    const { ctrl, fc } = buildCtx([714100005]);
    subject.showOrHideNonComplianceOther(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(ctrl.setVisible).toHaveBeenCalledWith(true);
  });

  test("hides control when multi-select is an empty array", () => {
    const { ctrl, fc } = buildCtx([]);
    subject.showOrHideNonComplianceOther(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(ctrl.setVisible).toHaveBeenCalledWith(false);
  });
});

// ---------------------------------------------------------------------------
// showRelevantSections
// ---------------------------------------------------------------------------

describe("showRelevantSections", () => {
  function buildCtx(importAppType: number | null, importingFromCharity = false) {
    const summarySections = {
      iv66_section: makeSection(),
      cheda_section: makeSection(),
      chedp_section: makeSection(),
      chedp_controls_section: makeSection(),
    };
    const transportSection = makeSection();
    const tabs = {
      Charity_Tab: makeTab(),
      AdditionalITAHC_Tab: makeTab(),
      Summary: makeTab(summarySections),
      Transporter_Tab: makeTab({ transport_information_section: transportSection }),
    };
    const attrs = {
      defraimp_importingfromcharity: makeAttr(importingFromCharity),
      defraimp_importapplicationtype: makeAttr(importAppType),
    };
    const fc = makeFormContext(attrs, {}, tabs);
    return { tabs, summarySections, transportSection, fc };
  }

  test("ITAHC (714100000): shows AdditionalITAHC and iv66 sections, hides others", () => {
    const { tabs, summarySections, transportSection, fc } = buildCtx(714100000);
    subject.showRelevantSections(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(tabs.AdditionalITAHC_Tab.setVisible).toHaveBeenCalledWith(true);
    expect(summarySections.iv66_section.setVisible).toHaveBeenCalledWith(true);
    expect(summarySections.cheda_section.setVisible).toHaveBeenCalledWith(false);
    expect(summarySections.chedp_section.setVisible).toHaveBeenCalledWith(false);
    expect(summarySections.chedp_controls_section.setVisible).toHaveBeenCalledWith(false);
    expect(transportSection.setVisible).toHaveBeenCalledWith(false);
  });

  test("Notification (714100001): shows iv66 section only, hides AdditionalITAHC", () => {
    const { tabs, summarySections, transportSection, fc } = buildCtx(714100001);
    subject.showRelevantSections(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(tabs.AdditionalITAHC_Tab.setVisible).toHaveBeenCalledWith(false);
    expect(summarySections.iv66_section.setVisible).toHaveBeenCalledWith(true);
    expect(summarySections.cheda_section.setVisible).toHaveBeenCalledWith(false);
    expect(summarySections.chedp_section.setVisible).toHaveBeenCalledWith(false);
    expect(transportSection.setVisible).toHaveBeenCalledWith(false);
  });

  test("CHEDA (714100002): shows cheda_section and transport, hides ITAHC and iv66", () => {
    const { tabs, summarySections, transportSection, fc } = buildCtx(714100002);
    subject.showRelevantSections(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(tabs.AdditionalITAHC_Tab.setVisible).toHaveBeenCalledWith(false);
    expect(summarySections.iv66_section.setVisible).toHaveBeenCalledWith(false);
    expect(summarySections.cheda_section.setVisible).toHaveBeenCalledWith(true);
    expect(summarySections.chedp_section.setVisible).toHaveBeenCalledWith(false);
    expect(transportSection.setVisible).toHaveBeenCalledWith(true);
  });

  test("CHEDP (714100003): shows chedp, chedp_controls and transport sections", () => {
    const { tabs, summarySections, transportSection, fc } = buildCtx(714100003);
    subject.showRelevantSections(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(tabs.AdditionalITAHC_Tab.setVisible).toHaveBeenCalledWith(false);
    expect(summarySections.chedp_section.setVisible).toHaveBeenCalledWith(true);
    expect(summarySections.chedp_controls_section.setVisible).toHaveBeenCalledWith(true);
    expect(transportSection.setVisible).toHaveBeenCalledWith(true);
    expect(summarySections.cheda_section.setVisible).toHaveBeenCalledWith(false);
  });

  test("DOCOM (714100004): hides all special sections", () => {
    const { tabs, summarySections, transportSection, fc } = buildCtx(714100004);
    subject.showRelevantSections(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(tabs.AdditionalITAHC_Tab.setVisible).toHaveBeenCalledWith(false);
    expect(summarySections.iv66_section.setVisible).toHaveBeenCalledWith(false);
    expect(summarySections.cheda_section.setVisible).toHaveBeenCalledWith(false);
    expect(summarySections.chedp_section.setVisible).toHaveBeenCalledWith(false);
    expect(transportSection.setVisible).toHaveBeenCalledWith(false);
  });

  test("ITAHCLandbridge (714100005): treated the same as ITAHC", () => {
    const { tabs, summarySections, transportSection, fc } = buildCtx(714100005);
    subject.showRelevantSections(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(tabs.AdditionalITAHC_Tab.setVisible).toHaveBeenCalledWith(true);
    expect(summarySections.iv66_section.setVisible).toHaveBeenCalledWith(true);
    expect(transportSection.setVisible).toHaveBeenCalledWith(false);
  });

  test("Charity_Tab visibility reflects defraimp_importingfromcharity", () => {
    const { tabs, fc } = buildCtx(714100000, true);
    subject.showRelevantSections(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(tabs.Charity_Tab.setVisible).toHaveBeenCalledWith(true);
  });

  test("Charity_Tab hidden when defraimp_importingfromcharity is false", () => {
    const { tabs, fc } = buildCtx(714100000, false);
    subject.showRelevantSections(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(tabs.Charity_Tab.setVisible).toHaveBeenCalledWith(false);
  });

  test("null type: hides all dynamic sections", () => {
    const { tabs, summarySections, transportSection, fc } = buildCtx(null);
    subject.showRelevantSections(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(tabs.AdditionalITAHC_Tab.setVisible).toHaveBeenCalledWith(false);
    expect(summarySections.iv66_section.setVisible).toHaveBeenCalledWith(false);
    expect(summarySections.cheda_section.setVisible).toHaveBeenCalledWith(false);
    expect(summarySections.chedp_section.setVisible).toHaveBeenCalledWith(false);
    expect(transportSection.setVisible).toHaveBeenCalledWith(false);
  });
});

// ---------------------------------------------------------------------------
// onChangeOfMoveToCompletion
// ---------------------------------------------------------------------------

describe("onChangeOfMoveToCompletion", () => {
  function buildCtx(moveToCompletion: boolean | null) {
    const dateAttr = makeAttr<Date | null>(null);
    const fc = makeFormContext({
      defraimp_movetocompletion: makeAttr(moveToCompletion),
      defraimp_movedtocompletiondate: dateAttr,
    });
    return { dateAttr, fc };
  }

  test("sets defraimp_movedtocompletiondate to a Date when moveToCompletion is true", () => {
    const { dateAttr, fc } = buildCtx(true);
    subject.onChangeOfMoveToCompletion(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(dateAttr.setValue).toHaveBeenCalledWith(expect.any(Date));
  });

  test("sets defraimp_movedtocompletiondate to null when moveToCompletion is false", () => {
    const { dateAttr, fc } = buildCtx(false);
    subject.onChangeOfMoveToCompletion(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(dateAttr.setValue).toHaveBeenCalledWith(null);
  });

  test("sets defraimp_movedtocompletiondate to null when moveToCompletion is null", () => {
    const { dateAttr, fc } = buildCtx(null);
    subject.onChangeOfMoveToCompletion(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(dateAttr.setValue).toHaveBeenCalledWith(null);
  });
});

// ---------------------------------------------------------------------------
// onLoad
// ---------------------------------------------------------------------------

describe("onLoad", () => {
  function buildCtx(opts: {
    manualDecisionValue?: number | null;
    isNonCompliantCalculated?: boolean;
    typesOfNoncompliance?: number[] | null;
  } = {}) {
    const nonComplianceTab = makeTab();
    const nonComplianceOtherCtrl = makeControl();
    const attrs = {
      defraimp_manualpostimportcheckdecision: makeAttr(
        opts.manualDecisionValue ?? null
      ),
      defraimp_isnoncompliantcalculated: makeAttr(
        opts.isNonCompliantCalculated ?? false
      ),
      defraimp_typesofnoncompliance: makeAttr(
        opts.typesOfNoncompliance ?? null
      ),
    };
    const fc = makeFormContext(
      attrs,
      { defraimp_noncomplianceothercomments: nonComplianceOtherCtrl },
      { NonCompliance_Tab: nonComplianceTab }
    );
    return { fc, nonComplianceTab, nonComplianceOtherCtrl };
  }

  test("shows NonCompliance_Tab when isnoncompliantcalculated is true", () => {
    const { fc, nonComplianceTab } = buildCtx({ isNonCompliantCalculated: true });
    subject.onLoad(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(nonComplianceTab.setVisible).toHaveBeenCalledWith(true);
  });

  test("hides NonCompliance_Tab when isnoncompliantcalculated is false", () => {
    const { fc, nonComplianceTab } = buildCtx({ isNonCompliantCalculated: false });
    subject.onLoad(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(nonComplianceTab.setVisible).toHaveBeenCalledWith(false);
  });

  test("shows noncompliance other control when typesofnoncompliance includes 714100005", () => {
    const { fc, nonComplianceOtherCtrl } = buildCtx({
      typesOfNoncompliance: [714100005],
    });
    subject.onLoad(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(nonComplianceOtherCtrl.setVisible).toHaveBeenCalledWith(true);
  });

  test("hides noncompliance other control when typesofnoncompliance is null", () => {
    const { fc, nonComplianceOtherCtrl } = buildCtx({ typesOfNoncompliance: null });
    subject.onLoad(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(nonComplianceOtherCtrl.setVisible).toHaveBeenCalledWith(false);
  });
});

// ---------------------------------------------------------------------------
// onSave - wasManualPostImportCheckSet state interaction
// ---------------------------------------------------------------------------

describe("onSave", () => {
  function buildSaveCtx(manualDecisionValue: number | null, saveMode = 1) {
    const attrs = {
      defraimp_manualpostimportcheckdecision: makeAttr(manualDecisionValue),
    };
    const fc = makeFormContext(attrs);
    const ec = makeExecutionContext(fc, saveMode);
    return { fc, ec };
  }

  test("does not call preventDefault when wasManualPostImportCheckSet is false (initial state)", async () => {
    const { ec } = buildSaveCtx(null);
    subject.onSave(ec as unknown as Xrm.Events.SaveEventContext);
    await flushPromises();
    expect(getPreventDefaultMock(ec)).not.toHaveBeenCalled();
  });

  test("calls preventDefault when decision was previously set then blanked", async () => {
    const manualDecision = makeAttr<number | null>(714100000);
    const loadAttrs = {
      defraimp_manualpostimportcheckdecision: manualDecision,
      defraimp_isnoncompliantcalculated: makeAttr(false),
      defraimp_typesofnoncompliance: makeAttr(null as number[] | null),
    };
    const loadFc = makeFormContext(
      loadAttrs,
      { defraimp_noncomplianceothercomments: makeControl() },
      { NonCompliance_Tab: makeTab() }
    );
    subject.onLoad(makeExecutionContext(loadFc) as unknown as Xrm.Events.EventContext);

    manualDecision.getValue.mockReturnValue(null);
    const ec = makeExecutionContext(loadFc);
    subject.onSave(ec as unknown as Xrm.Events.SaveEventContext);
    await flushPromises();
    expect(getPreventDefaultMock(ec)).toHaveBeenCalled();
  });

  test("does not call preventDefault when decision is still set after being set", async () => {
    const loadAttrs = {
      defraimp_manualpostimportcheckdecision: makeAttr<number | null>(714100000),
      defraimp_isnoncompliantcalculated: makeAttr(false),
      defraimp_typesofnoncompliance: makeAttr(null as number[] | null),
    };
    const loadFc = makeFormContext(
      loadAttrs,
      { defraimp_noncomplianceothercomments: makeControl() },
      { NonCompliance_Tab: makeTab() }
    );
    subject.onLoad(makeExecutionContext(loadFc) as unknown as Xrm.Events.EventContext);

    // Save with a non-null decision should NOT prevent save
    const { ec } = buildSaveCtx(714100000);
    subject.onSave(ec as unknown as Xrm.Events.SaveEventContext);
    await flushPromises();
    expect(getPreventDefaultMock(ec)).not.toHaveBeenCalled();
  });

  test("opens error dialog with the blank-field message when save is prevented", async () => {
    const manualDecision = makeAttr<number | null>(714100000);
    const loadAttrs = {
      defraimp_manualpostimportcheckdecision: manualDecision,
      defraimp_isnoncompliantcalculated: makeAttr(false),
      defraimp_typesofnoncompliance: makeAttr(null as number[] | null),
    };
    const loadFc = makeFormContext(
      loadAttrs,
      { defraimp_noncomplianceothercomments: makeControl() },
      { NonCompliance_Tab: makeTab() }
    );
    subject.onLoad(makeExecutionContext(loadFc) as unknown as Xrm.Events.EventContext);

    manualDecision.getValue.mockReturnValue(null);
    const ec = makeExecutionContext(loadFc);
    subject.onSave(ec as unknown as Xrm.Events.SaveEventContext);
    await flushPromises();
    expect(xrm.Navigation.openErrorDialog).toHaveBeenCalledWith({
      message: "'Manual Post Import Check Decision' must be populated.",
    });
  });
});

// ---------------------------------------------------------------------------
// onChangeOfManualPostImportCheckDecision
// ---------------------------------------------------------------------------

describe("onChangeOfManualPostImportCheckDecision", () => {
  function buildCtx(decision: number | null) {
    const inspectionRequired = makeAttr<number | null>(null);
    const inspectionReason = makeAttr<number | null>(null);
    const declinedReason = makeAttr<string | null>(null);
    const originalRequired = makeAttr<number | null>(714100000);
    const originalReason = makeAttr<number | null>(714100004);
    const attrs = {
      defraimp_manualpostimportcheckdecision: makeAttr<number | null>(decision),
      defraimp_inspectionrequired: inspectionRequired,
      defraimp_inspectionrequiredreason: inspectionReason,
      defraimp_inspectiondeclinedreason: declinedReason,
      defraimp_inspectionrequiredoriginalvalue: originalRequired,
      defraimp_inspectionrequiredreasonoriginalvalue: originalReason,
    };
    const fc = makeFormContext(attrs);
    return { fc, inspectionRequired, inspectionReason, declinedReason };
  }

  test("System (714100004): restores inspection values from original-value fields", () => {
    const { fc, inspectionRequired, inspectionReason } = buildCtx(714100004);
    subject.onChangeOfManualPostImportCheckDecision(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(inspectionRequired.setValue).toHaveBeenCalledWith(714100000);
    expect(inspectionReason.setValue).toHaveBeenCalledWith(714100004);
  });

  test("Other (714100000): sets InspectionRequired=Yes, Reason=ManuallyRequested", () => {
    const { fc, inspectionRequired, inspectionReason } = buildCtx(714100000);
    subject.onChangeOfManualPostImportCheckDecision(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(inspectionRequired.setValue).toHaveBeenCalledWith(714100000); // Yes
    expect(inspectionReason.setValue).toHaveBeenCalledWith(714100013); // ManuallyRequested
  });

  test("Quarantine (714100003): sets InspectionRequired=Yes, Reason=Quarantine", () => {
    const { fc, inspectionRequired, inspectionReason } = buildCtx(714100003);
    subject.onChangeOfManualPostImportCheckDecision(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(inspectionRequired.setValue).toHaveBeenCalledWith(714100000); // Yes
    expect(inspectionReason.setValue).toHaveBeenCalledWith(714100010); // Quarantine
  });

  test("TB (714100002): sets InspectionRequired=No, Reason=NoInspectionRequired", () => {
    const { fc, inspectionRequired, inspectionReason } = buildCtx(714100002);
    subject.onChangeOfManualPostImportCheckDecision(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(inspectionRequired.setValue).toHaveBeenCalledWith(714100001); // No
    expect(inspectionReason.setValue).toHaveBeenCalledWith(714100004); // NoInspectionRequired
  });

  test("DoNotCheck (714100001): sets InspectionRequired=No, Reason=NoInspectionRequired", () => {
    const { fc, inspectionRequired, inspectionReason } = buildCtx(714100001);
    subject.onChangeOfManualPostImportCheckDecision(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(inspectionRequired.setValue).toHaveBeenCalledWith(714100001); // No
    expect(inspectionReason.setValue).toHaveBeenCalledWith(714100004); // NoInspectionRequired
  });

  test("DoNotCheck: sets declined reason when currently null", () => {
    const { fc, declinedReason } = buildCtx(714100001);
    declinedReason.getValue.mockReturnValue(null);
    subject.onChangeOfManualPostImportCheckDecision(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(declinedReason.setValue).toHaveBeenCalledWith(
      "System Required Post Import Check Skipped"
    );
  });

  test("DoNotCheck: sets declined reason when currently empty string", () => {
    const { fc, declinedReason } = buildCtx(714100001);
    declinedReason.getValue.mockReturnValue("");
    subject.onChangeOfManualPostImportCheckDecision(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(declinedReason.setValue).toHaveBeenCalledWith(
      "System Required Post Import Check Skipped"
    );
  });

  test("DoNotCheck: does not overwrite declined reason when already populated", () => {
    const { fc, declinedReason } = buildCtx(714100001);
    declinedReason.getValue.mockReturnValue("Existing reason");
    subject.onChangeOfManualPostImportCheckDecision(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(declinedReason.setValue).not.toHaveBeenCalled();
  });

  test("null decision: does not set any inspection values", () => {
    const { fc, inspectionRequired, inspectionReason } = buildCtx(null);
    subject.onChangeOfManualPostImportCheckDecision(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(inspectionRequired.setValue).not.toHaveBeenCalled();
    expect(inspectionReason.setValue).not.toHaveBeenCalled();
  });
});

