import {
  makeAttr,
  makeControl,
  makeFormContext,
  makeExecutionContext,
  setupXrm,
} from './xrm-mock.helper';

setupXrm();

// postimportcheck.form.ts stores formContext at module level, so reset modules
// between tests to avoid state bleed-through.
let subject: typeof import('../src/postimportcheck.form');

beforeEach(() => {
  jest.resetModules();
  // eslint-disable-next-line @typescript-eslint/no-require-imports, @typescript-eslint/no-unsafe-assignment
  subject = require('../src/postimportcheck.form');
});

function buildCtx(samplingRequired: boolean | null) {
  const gridControl = makeControl();
  const attrs = {
    defraimp_samplingrequired: makeAttr(samplingRequired),
  };
  const fc = makeFormContext(attrs, { SamplesTestsRequired: gridControl });
  return { fc, gridControl };
}

// ---------------------------------------------------------------------------
// onLoad
// ---------------------------------------------------------------------------

describe('onLoad', () => {
  test('shows SamplesTestsRequired grid when defraimp_samplingrequired is true', () => {
    const { fc, gridControl } = buildCtx(true);
    subject.onLoad(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(gridControl.setVisible).toHaveBeenCalledWith(true);
  });

  test('hides SamplesTestsRequired grid when defraimp_samplingrequired is false', () => {
    const { fc, gridControl } = buildCtx(false);
    subject.onLoad(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(gridControl.setVisible).toHaveBeenCalledWith(false);
  });

  test('hides SamplesTestsRequired grid when defraimp_samplingrequired is null', () => {
    const { fc, gridControl } = buildCtx(null);
    subject.onLoad(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    expect(gridControl.setVisible).toHaveBeenCalledWith(false);
  });
});

// ---------------------------------------------------------------------------
// Save validation (related importer notification / related import record)
// ---------------------------------------------------------------------------

describe('onSave - related record validation', () => {
  const MESSAGE =
    "Please complete either the 'Related Importer Notification' or 'Related Import Record' field";
  const lookup = [{ id: '1', name: 'x', entityType: 'e' }];

  function buildSaveCtx(notification: unknown, importRecord: unknown) {
    const fc = makeFormContext(
      {
        defraimp_samplingrequired: makeAttr(false),
        defraimp_relatedimporternotification: makeAttr(notification),
        defraimp_relatedimportapplication: makeAttr(importRecord),
      },
      { SamplesTestsRequired: makeControl() }
    );
    return { fc };
  }

  test('prevents save and shows error when both lookups are blank', () => {
    const { fc } = buildSaveCtx(null, null);
    const saveCtx = makeExecutionContext(fc);
    subject.onSave(saveCtx as unknown as Xrm.Events.SaveEventContext);
    expect(saveCtx.getEventArgs().preventDefault).toHaveBeenCalled();
    expect(fc.ui.setFormNotification).toHaveBeenCalledWith(
      MESSAGE,
      'ERROR',
      'missingRelatedRecordError'
    );
  });

  test('prevents save when lookups are empty arrays', () => {
    const { fc } = buildSaveCtx([], []);
    const saveCtx = makeExecutionContext(fc);
    subject.onSave(saveCtx as unknown as Xrm.Events.SaveEventContext);
    expect(saveCtx.getEventArgs().preventDefault).toHaveBeenCalled();
  });

  test.each([
    ['notification only', lookup, null],
    ['import record only', null, lookup],
    ['both', lookup, lookup],
  ])('allows save when %s is populated', (_name, notification, importRecord) => {
    const { fc } = buildSaveCtx(notification, importRecord);
    const saveCtx = makeExecutionContext(fc);
    subject.onSave(saveCtx as unknown as Xrm.Events.SaveEventContext);
    expect(saveCtx.getEventArgs().preventDefault).not.toHaveBeenCalled();
    expect(fc.ui.setFormNotification).not.toHaveBeenCalled();
    expect(fc.ui.clearFormNotification).toHaveBeenCalledWith('missingRelatedRecordError');
  });
});

// ---------------------------------------------------------------------------
// onChangeSamplingRequired (uses module-level formContext stored by onLoad)
// ---------------------------------------------------------------------------

describe('onChangeSamplingRequired', () => {
  test('does nothing when called before onLoad stores a form context', () => {
    expect(() => subject.onChangeSamplingRequired()).not.toThrow();
  });

  test('shows grid when sampling required attribute was true at load time', () => {
    const { fc, gridControl } = buildCtx(true);
    subject.onLoad(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    gridControl.setVisible.mockClear();

    subject.onChangeSamplingRequired();
    expect(gridControl.setVisible).toHaveBeenCalledWith(true);
  });

  test('hides grid when sampling required attribute was false at load time', () => {
    const { fc, gridControl } = buildCtx(false);
    subject.onLoad(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);
    gridControl.setVisible.mockClear();

    subject.onChangeSamplingRequired();
    expect(gridControl.setVisible).toHaveBeenCalledWith(false);
  });

  test('reflects latest attribute value not the value at load time', () => {
    const samplingAttr = makeAttr<boolean | null>(false);
    const gridControl = makeControl();
    const fc = makeFormContext(
      { defraimp_samplingrequired: samplingAttr },
      { SamplesTestsRequired: gridControl }
    );
    subject.onLoad(makeExecutionContext(fc) as unknown as Xrm.Events.EventContext);

    // Simulate attribute change after onLoad
    samplingAttr.getValue.mockReturnValue(true);
    gridControl.setVisible.mockClear();

    subject.onChangeSamplingRequired();
    expect(gridControl.setVisible).toHaveBeenCalledWith(true);
  });

  test('uses the formContext stored by the most recent onLoad call', () => {
    // First load with false
    const { fc: fc1, gridControl: grid1 } = buildCtx(false);
    subject.onLoad(makeExecutionContext(fc1) as unknown as Xrm.Events.EventContext);

    // Second load with a different context
    const { fc: fc2, gridControl: grid2 } = buildCtx(true);
    subject.onLoad(makeExecutionContext(fc2) as unknown as Xrm.Events.EventContext);

    grid1.setVisible.mockClear();
    grid2.setVisible.mockClear();

    subject.onChangeSamplingRequired();
    expect(grid2.setVisible).toHaveBeenCalledWith(true);
    expect(grid1.setVisible).not.toHaveBeenCalled();
  });
});
