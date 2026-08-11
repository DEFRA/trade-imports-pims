/** Shared Xrm mock utilities for unit tests. */

export const flushPromises = (): Promise<void> =>
  new Promise((resolve) => process.nextTick(resolve));

// ---------------------------------------------------------------------------
// Low-level builders
// ---------------------------------------------------------------------------

export function makeSection() {
  return { setVisible: jest.fn() };
}

export function makeTab(
  sectionsMap: Record<string, ReturnType<typeof makeSection>> = {}
) {
  return {
    setVisible: jest.fn(),
    sections: { get: jest.fn((n: string) => sectionsMap[n]) },
  };
}

export function makeAttr<T>(initial: T) {
  return {
    getValue: jest.fn().mockReturnValue(initial),
    setValue: jest.fn(),
    addOnChange: jest.fn(),
    removeOnChange: jest.fn(),
  };
}

export function makeControl() {
  return { setVisible: jest.fn(), getGrid: jest.fn() };
}

export function makeGridRow(ref: { id: string; name: string; entityType: string }) {
  return {
    getData: jest.fn().mockReturnValue({
      getEntity: jest.fn().mockReturnValue({
        getEntityReference: jest.fn().mockReturnValue(ref),
      }),
    }),
  };
}

export function makeSelectedRows(refs: Array<{ id: string; name: string; entityType: string }>) {
  const rows = refs.map(makeGridRow);
  return {
    forEach: jest.fn((cb: (r: ReturnType<typeof makeGridRow>) => void) =>
      rows.forEach(cb)
    ),
  };
}

// ---------------------------------------------------------------------------
// Form context builder
// ---------------------------------------------------------------------------

export interface FormContextOptions {
  formType?: number;
  entityId?: string;
  entityRef?: { id: string; name: string; entityType: string };
}

export function makeFormContext(
  attributes: Record<string, ReturnType<typeof makeAttr>> = {},
  controls: Record<string, ReturnType<typeof makeControl>> = {},
  tabs: Record<string, ReturnType<typeof makeTab>> = {},
  opts: FormContextOptions = {}
) {
  return {
    ui: {
      getFormType: jest.fn().mockReturnValue(opts.formType ?? 2),
      tabs: { get: jest.fn((n: string) => tabs[n]) },
      clearFormNotification: jest.fn(),
      setFormNotification: jest.fn(),
    },
    getAttribute: jest.fn((n: string) => attributes[n]),
    getControl: jest.fn((n: string) => controls[n]),
    data: {
      entity: {
        getId: jest.fn().mockReturnValue(opts.entityId ?? "{test-guid}"),
        addOnSave: jest.fn(),
        addOnPostSave: jest.fn(),
        removeOnPostSave: jest.fn(),
        getEntityReference: jest.fn().mockReturnValue(
          opts.entityRef ?? { id: "test-id", name: "Test", entityType: "entity" }
        ),
      },
      save: jest.fn().mockResolvedValue(undefined),
      refresh: jest.fn().mockResolvedValue(undefined),
    },
  };
}

export function makeExecutionContext(
  formContext: ReturnType<typeof makeFormContext>,
  saveMode = 1
) {
  return {
    getFormContext: jest.fn().mockReturnValue(formContext),
    getEventArgs: jest.fn().mockReturnValue({
      preventDefault: jest.fn(),
      getSaveMode: jest.fn().mockReturnValue(saveMode),
    }),
  };
}

// ---------------------------------------------------------------------------
// Global Xrm setup
// ---------------------------------------------------------------------------

export function setupXrm() {
  const xrm = {
    WebApi: {
      online: {
        execute: jest.fn().mockResolvedValue(undefined),
        executeMultiple: jest.fn().mockResolvedValue(undefined),
      },
      retrieveRecord: jest
        .fn()
        .mockResolvedValue({ defraexp_value: "true" }),
    },
    Navigation: {
      openAlertDialog: jest.fn().mockResolvedValue(undefined),
      openErrorDialog: jest.fn().mockResolvedValue(undefined),
      openForm: jest.fn().mockResolvedValue(undefined),
      openUrl: jest.fn(),
    },
    Utility: {
      showProgressIndicator: jest.fn(),
      closeProgressIndicator: jest.fn(),
    },
  };
  (global as unknown as Record<string, unknown>).Xrm = xrm;
  (global as unknown as Record<string, unknown>).XrmEnum = {
    FormType: {
      Create: 1,
      Update: 2,
      ReadOnly: 3,
      Disabled: 4,
      BulkEdit: 6,
    },
  };
  return xrm;
}
