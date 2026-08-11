import { makeAttr, makeFormContext, setupXrm } from "./xrm-mock.helper";
import { CloneImportQueryButton } from "../src/importquery.ribbon";

let xrm: ReturnType<typeof setupXrm>;

beforeEach(() => {
  xrm = setupXrm();
});

function buildPrimaryControl(opts: {
  querysentto?: string | null;
  subject?: string | null;
  duedate?: Date | null;
  queryId?: string;
} = {}) {
  const attrs = {
    defraimp_querysentto: makeAttr(opts.querysentto ?? "test@example.com"),
    subject: makeAttr(opts.subject ?? "Test Query"),
    defraimp_duedate: makeAttr(opts.duedate !== undefined ? opts.duedate : new Date("2025-01-01")),
  };
  const fc = makeFormContext(attrs, {}, {}, { entityId: opts.queryId ?? "{abc-123-def}" });
  return fc;
}

// ---------------------------------------------------------------------------
// CloneImportQueryButton
// ---------------------------------------------------------------------------

describe("CloneImportQueryButton", () => {
  test("opens defraimp_importquery form", () => {
    const pc = buildPrimaryControl();
    CloneImportQueryButton(pc as unknown as Xrm.FormContext);
    expect(xrm.Navigation.openForm).toHaveBeenCalledTimes(1);
    const [options] = xrm.Navigation.openForm.mock.calls[0] as [{ entityName: string }, unknown];
    expect(options.entityName).toBe("defraimp_importquery");
  });

  test("passes defraimp_querysentto value to openForm parameters", () => {
    const pc = buildPrimaryControl({ querysentto: "user@defra.gov.uk" });
    CloneImportQueryButton(pc as unknown as Xrm.FormContext);
    const [, params] = xrm.Navigation.openForm.mock.calls[0] as [unknown, Record<string, unknown>];
    expect(params["defraimp_querysentto"]).toBe("user@defra.gov.uk");
  });

  test("passes subject value to openForm parameters", () => {
    const pc = buildPrimaryControl({ subject: "My Subject" });
    CloneImportQueryButton(pc as unknown as Xrm.FormContext);
    const [, params] = xrm.Navigation.openForm.mock.calls[0] as [unknown, Record<string, unknown>];
    expect(params["subject"]).toBe("My Subject");
  });

  test("passes duedate value to openForm parameters", () => {
    const dueDate = new Date("2025-06-15");
    const pc = buildPrimaryControl({ duedate: dueDate });
    CloneImportQueryButton(pc as unknown as Xrm.FormContext);
    const [, params] = xrm.Navigation.openForm.mock.calls[0] as [unknown, Record<string, unknown>];
    expect(params["defraimp_duedate"]).toBe(dueDate.toISOString());
  });

  test("strips curly braces from entity ID in defraimp_originalquery lookup", () => {
    const pc = buildPrimaryControl({ queryId: "{abc-123-def}" });
    CloneImportQueryButton(pc as unknown as Xrm.FormContext);
    const [, params] = xrm.Navigation.openForm.mock.calls[0] as [unknown, Record<string, unknown>];
    const lookup = JSON.parse(
      params["defraimp_originalquery"] as string
    ) as { id: string; name: string; entityType: string };
    expect(lookup.id).toBe("abc-123-def");
  });

  test("ID without braces is passed through unchanged", () => {
    const pc = buildPrimaryControl({ queryId: "no-braces-id" });
    CloneImportQueryButton(pc as unknown as Xrm.FormContext);
    const [, params] = xrm.Navigation.openForm.mock.calls[0] as [unknown, Record<string, unknown>];
    const lookup = JSON.parse(params["defraimp_originalquery"] as string) as {
      id: string;
    };
    expect(lookup.id).toBe("no-braces-id");
  });

  test("removes all curly braces from entity ID", () => {
    const pc = buildPrimaryControl({ queryId: "{{double-brace}}" });
    CloneImportQueryButton(pc as unknown as Xrm.FormContext);
    const [, params] = xrm.Navigation.openForm.mock.calls[0] as [unknown, Record<string, unknown>];
    const lookup = JSON.parse(params["defraimp_originalquery"] as string) as {
      id: string;
    };
    expect(lookup.id).toBe("double-brace");
  });

  test("sets defraimp_originalquery entity type to defraimp_importquery", () => {
    const pc = buildPrimaryControl();
    CloneImportQueryButton(pc as unknown as Xrm.FormContext);
    const [, params] = xrm.Navigation.openForm.mock.calls[0] as [unknown, Record<string, unknown>];
    const lookup = JSON.parse(
      params["defraimp_originalquery"] as string
    ) as { entityType: string };
    expect(lookup.entityType).toBe("defraimp_importquery");
  });

  test("sets defraimp_originalquery name to the subject value", () => {
    const pc = buildPrimaryControl({ subject: "Clone Source" });
    CloneImportQueryButton(pc as unknown as Xrm.FormContext);
    const [, params] = xrm.Navigation.openForm.mock.calls[0] as [unknown, Record<string, unknown>];
    const lookup = JSON.parse(params["defraimp_originalquery"] as string) as {
      name: string;
    };
    expect(lookup.name).toBe("Clone Source");
  });

  test("handles null duedate value", () => {
    const pc = buildPrimaryControl({ duedate: null });
    CloneImportQueryButton(pc as unknown as Xrm.FormContext);
    const [, params] = xrm.Navigation.openForm.mock.calls[0] as [unknown, Record<string, unknown>];
    expect(params["defraimp_duedate"]).toBeUndefined();
  });
});
