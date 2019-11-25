declare namespace Form.defraimp_importquery.Main {
  namespace Information {
    namespace Tabs {
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "createdby"): Xrm.LookupAttribute<"systemuser">;
      get(name: "createdon"): Xrm.DateAttribute;
      get(name: "defraimp_dateresolved"): Xrm.DateAttribute;
      get(name: "defraimp_duedate"): Xrm.DateAttribute;
      get(name: "defraimp_itahc"): Xrm.LookupAttribute<"defraimp_itahc">;
      get(name: "defraimp_querynumber"): Xrm.Attribute<string>;
      get(name: "defraimp_querysentto"): Xrm.Attribute<string>;
      get(name: "description"): Xrm.Attribute<string>;
      get(name: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
      get(name: "regardingobjectid"): Xrm.LookupAttribute<"account" | "bookableresourcebooking" | "bookableresourcebookingheader" | "bulkoperation" | "campaign" | "campaignactivity" | "contact" | "contract" | "defraexp_exportapplication" | "defraimp_importapplication" | "defra_addressdetails" | "defra_invitation" | "entitlement" | "entitlementtemplate" | "incident" | "interactionforemail" | "invoice" | "knowledgearticle" | "knowledgebaserecord" | "lead" | "msdyn_playbookinstance" | "msdyn_postalbum" | "opportunity" | "quote" | "salesorder" | "site">;
      get(name: "statecode"): Xrm.OptionSetAttribute<defraimp_importquery_statecode>;
      get(name: "subject"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "createdby"): Xrm.LookupControl<"systemuser">;
      get(name: "createdon"): Xrm.DateControl;
      get(name: "defraimp_duedate"): Xrm.DateControl;
      get(name: "defraimp_itahc"): Xrm.LookupControl<"defraimp_itahc">;
      get(name: "defraimp_querynumber"): Xrm.StringControl;
      get(name: "defraimp_querysentto"): Xrm.StringControl;
      get(name: "description"): Xrm.StringControl;
      get(name: "header_defraimp_dateresolved"): Xrm.DateControl;
      get(name: "header_defraimp_duedate"): Xrm.DateControl;
      get(name: "header_ownerid"): Xrm.LookupControl<"systemuser" | "team">;
      get(name: "header_statecode"): Xrm.OptionSetControl<defraimp_importquery_statecode>;
      get(name: "notescontrol"): Xrm.BaseControl;
      get(name: "regardingobjectid"): Xrm.LookupControl<"account" | "bookableresourcebooking" | "bookableresourcebookingheader" | "bulkoperation" | "campaign" | "campaignactivity" | "contact" | "contract" | "defraexp_exportapplication" | "defraimp_importapplication" | "defra_addressdetails" | "defra_invitation" | "entitlement" | "entitlementtemplate" | "incident" | "interactionforemail" | "invoice" | "knowledgearticle" | "knowledgebaserecord" | "lead" | "msdyn_playbookinstance" | "msdyn_postalbum" | "opportunity" | "quote" | "salesorder" | "site">;
      get(name: "subject"): Xrm.StringControl;
      get(name: string): undefined;
      get(): Xrm.BaseControl[];
      get(index: number): Xrm.BaseControl;
      get(chooser: (item: Xrm.BaseControl, index: number) => boolean): Xrm.BaseControl[];
    }
    interface Tabs extends Xrm.TabCollectionBase {
      get(name: string): undefined;
      get(): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
      get(index: number): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>;
      get(chooser: (item: Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>, index: number) => boolean): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
    }
  }
  interface Information extends Xrm.PageBase<Information.Attributes,Information.Tabs,Information.Controls> {
    getAttribute(attributeName: "createdby"): Xrm.LookupAttribute<"systemuser">;
    getAttribute(attributeName: "createdon"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_dateresolved"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_duedate"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_itahc"): Xrm.LookupAttribute<"defraimp_itahc">;
    getAttribute(attributeName: "defraimp_querynumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_querysentto"): Xrm.Attribute<string>;
    getAttribute(attributeName: "description"): Xrm.Attribute<string>;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
    getAttribute(attributeName: "regardingobjectid"): Xrm.LookupAttribute<"account" | "bookableresourcebooking" | "bookableresourcebookingheader" | "bulkoperation" | "campaign" | "campaignactivity" | "contact" | "contract" | "defraexp_exportapplication" | "defraimp_importapplication" | "defra_addressdetails" | "defra_invitation" | "entitlement" | "entitlementtemplate" | "incident" | "interactionforemail" | "invoice" | "knowledgearticle" | "knowledgebaserecord" | "lead" | "msdyn_playbookinstance" | "msdyn_postalbum" | "opportunity" | "quote" | "salesorder" | "site">;
    getAttribute(attributeName: "statecode"): Xrm.OptionSetAttribute<defraimp_importquery_statecode>;
    getAttribute(attributeName: "subject"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "createdby"): Xrm.LookupControl<"systemuser">;
    getControl(controlName: "createdon"): Xrm.DateControl;
    getControl(controlName: "defraimp_duedate"): Xrm.DateControl;
    getControl(controlName: "defraimp_itahc"): Xrm.LookupControl<"defraimp_itahc">;
    getControl(controlName: "defraimp_querynumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_querysentto"): Xrm.StringControl;
    getControl(controlName: "description"): Xrm.StringControl;
    getControl(controlName: "header_defraimp_dateresolved"): Xrm.DateControl;
    getControl(controlName: "header_defraimp_duedate"): Xrm.DateControl;
    getControl(controlName: "header_ownerid"): Xrm.LookupControl<"systemuser" | "team">;
    getControl(controlName: "header_statecode"): Xrm.OptionSetControl<defraimp_importquery_statecode>;
    getControl(controlName: "notescontrol"): Xrm.BaseControl;
    getControl(controlName: "regardingobjectid"): Xrm.LookupControl<"account" | "bookableresourcebooking" | "bookableresourcebookingheader" | "bulkoperation" | "campaign" | "campaignactivity" | "contact" | "contract" | "defraexp_exportapplication" | "defraimp_importapplication" | "defra_addressdetails" | "defra_invitation" | "entitlement" | "entitlementtemplate" | "incident" | "interactionforemail" | "invoice" | "knowledgearticle" | "knowledgebaserecord" | "lead" | "msdyn_playbookinstance" | "msdyn_postalbum" | "opportunity" | "quote" | "salesorder" | "site">;
    getControl(controlName: "subject"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
