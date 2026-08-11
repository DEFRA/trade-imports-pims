import {
  makeAttr,
  makeFormContext,
  makeExecutionContext,
  makeSection,
  makeTab,
  setupXrm,
} from "./xrm-mock.helper";
import { showHideCharity, checkForMultipleCommodities } from "../src/importernotification.form";

setupXrm();

// ---------------------------------------------------------------------------
// showHideCharity
// ---------------------------------------------------------------------------

describe("showHideCharity", () => {
  const TYPE_CVEDA = 714100000;

  function buildCtx(type: number | null | undefined, importingFromCharity: boolean | null) {
    const charityTab = makeTab();
    const attrs: Record<string, ReturnType<typeof makeAttr>> = {
      defraimp_importingfromcharity: makeAttr(importingFromCharity),
    };
    if (type !== undefined) {
      attrs["defraimp_type"] = makeAttr(type);
    }
    const fc = makeFormContext(attrs, {}, { Charity_Tab: charityTab });
    return { charityTab, fc };
  }

  test("shows Charity_Tab when importingFromCharity is true", () => {
    const { charityTab, fc } = buildCtx(null, true);
    showHideCharity(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(charityTab.setVisible).toHaveBeenCalledWith(true);
  });

  test("hides Charity_Tab when importingFromCharity is false", () => {
    const { charityTab, fc } = buildCtx(null, false);
    showHideCharity(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(charityTab.setVisible).toHaveBeenCalledWith(false);
  });

  test("hides Charity_Tab when importingFromCharity is null (coerces to false)", () => {
    const { charityTab, fc } = buildCtx(null, null);
    showHideCharity(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(charityTab.setVisible).toHaveBeenCalledWith(false);
  });

  test("adds OnSave and OnPostSave handlers when type is CVEDA (714100000)", () => {
    const { fc } = buildCtx(TYPE_CVEDA, false);
    showHideCharity(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(fc.data.entity.addOnSave).toHaveBeenCalledTimes(1);
    expect(fc.data.entity.addOnPostSave).toHaveBeenCalledTimes(1);
  });

  test("does not add OnSave and OnPostSave handlers when type is not CVEDA", () => {
    const { fc } = buildCtx(714100001, false);
    showHideCharity(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(fc.data.entity.addOnSave).not.toHaveBeenCalled();
    expect(fc.data.entity.addOnPostSave).not.toHaveBeenCalled();
  });

  test("does not add handlers when type is null", () => {
    const { fc } = buildCtx(null, false);
    showHideCharity(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(fc.data.entity.addOnSave).not.toHaveBeenCalled();
    expect(fc.data.entity.addOnPostSave).not.toHaveBeenCalled();
  });
});

// ---------------------------------------------------------------------------
// checkForMultipleCommodities
// ---------------------------------------------------------------------------

describe("checkForMultipleCommodities", () => {
  const caseworkerSection = "caseworker_intervention_section";

  function buildCtx(hasMultiple: boolean | null, caseworkerIntervened: boolean | null) {
    const section = makeSection();
    const attrs = {
      defraimp_hasmultiplecommoditycodes: makeAttr(hasMultiple),
      defraimp_caseworkerintervention: makeAttr(caseworkerIntervened),
    };
    const fc = makeFormContext(
      attrs,
      {},
      { details_tab: makeTab({ [caseworkerSection]: section }) }
    );
    return { section, fc };
  }

  test("shows caseworker section when hasMultipleCommodities is true", () => {
    const { section, fc } = buildCtx(true, false);
    checkForMultipleCommodities(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(section.setVisible).toHaveBeenCalledWith(true);
  });

  test("shows ERROR notification when multiple commodities and no caseworker intervention", () => {
    const { fc } = buildCtx(true, false);
    checkForMultipleCommodities(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(fc.ui.clearFormNotification).toHaveBeenCalledWith("multipleCommodityNotification");
    expect(fc.ui.setFormNotification).toHaveBeenCalledWith(
      "More than 1 Commodity Code - No caseworker intervention",
      "ERROR",
      "multipleCommodityError"
    );
  });

  test("shows INFO notification when multiple commodities and caseworker has intervened", () => {
    const { fc } = buildCtx(true, true);
    checkForMultipleCommodities(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(fc.ui.clearFormNotification).toHaveBeenCalledWith("multipleCommodityError");
    expect(fc.ui.setFormNotification).toHaveBeenCalledWith(
      "More than 1 Commodity Code - caseworker has intervened",
      "INFO",
      "multipleCommodityNotification"
    );
  });

  test("hides caseworker section when hasMultipleCommodities is false", () => {
    const { section, fc } = buildCtx(false, null);
    checkForMultipleCommodities(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(section.setVisible).toHaveBeenCalledWith(false);
  });

  test("clears both notifications when hasMultipleCommodities is false", () => {
    const { fc } = buildCtx(false, null);
    checkForMultipleCommodities(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(fc.ui.clearFormNotification).toHaveBeenCalledWith("multipleCommodityError");
    expect(fc.ui.clearFormNotification).toHaveBeenCalledWith("multipleCommodityNotification");
    expect(fc.ui.setFormNotification).not.toHaveBeenCalled();
  });

  test("hides caseworker section when hasMultipleCommodities is null", () => {
    const { section, fc } = buildCtx(null, null);
    checkForMultipleCommodities(
      makeExecutionContext(fc) as unknown as Xrm.Events.EventContext
    );
    expect(section.setVisible).toHaveBeenCalledWith(false);
  });
});
