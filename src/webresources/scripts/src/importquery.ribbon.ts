export function CloneImportQueryButton(primaryControl: Xrm.FormContext): void {
  const emailToSend = requiredAttribute<Xrm.Attributes.StringAttribute>(
    primaryControl,
    "defraimp_querysentto"
  ).getValue();
  const subject = requiredAttribute<Xrm.Attributes.StringAttribute>(
    primaryControl,
    "subject"
  ).getValue();
  const dueDate = requiredAttribute<Xrm.Attributes.DateAttribute>(
    primaryControl,
    "defraimp_duedate"
  ).getValue();
  const queryId = normalizeEntityId(primaryControl.data.entity.getId());

  const parameters: Xrm.Utility.OpenParameters = {
    defraimp_querysentto: emailToSend ?? undefined,
    subject: subject ?? undefined,
    defraimp_duedate: dueDate?.toISOString(),
    defraimp_originalquery: JSON.stringify({
      id: queryId,
      name: subject ?? "",
      entityType: "defraimp_importquery",
    }),
  };

  Xrm.Navigation.openForm({ entityName: "defraimp_importquery" }, parameters);
}

function requiredAttribute<T extends Xrm.Attributes.Attribute>(
  formContext: Xrm.FormContext,
  name: string
): T {
  const attribute = formContext.getAttribute<T>(name);
  if (!attribute) {
    throw new Error(`Required attribute '${name}' was not found.`);
  }

  return attribute;
}

function normalizeEntityId(entityId: string): string {
  return entityId.replace(/[{}]/g, "");
}
