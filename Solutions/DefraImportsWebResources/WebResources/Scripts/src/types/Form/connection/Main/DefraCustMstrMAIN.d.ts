declare namespace Form.connection.Main {
  namespace DefraCustMstrMAIN {
    namespace Tabs {
      interface details extends Xrm.SectionCollectionBase {
        get(name: "details_section_1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface info extends Xrm.SectionCollectionBase {
        get(name: "description"): Xrm.PageSection;
        get(name: "info_s"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface tab_ConnectFrom extends Xrm.SectionCollectionBase {
        get(name: "connect_from_3"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface tab_CustomerDetails extends Xrm.SectionCollectionBase {
        get(name: "details_2"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "createdby"): Xrm.LookupAttribute<"systemuser">;
      get(name: "createdon"): Xrm.DateAttribute;
      get(name: "defra_connectiondetailsid"): Xrm.LookupAttribute<"defra_connectiondetails">;
      get(name: "defra_iscustomer"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defra_previousconnectiondetail"): Xrm.LookupAttribute<"defra_connectiondetails">;
      get(name: "description"): Xrm.Attribute<string>;
      get(name: "effectiveend"): Xrm.DateAttribute;
      get(name: "effectivestart"): Xrm.DateAttribute;
      get(name: "modifiedby"): Xrm.LookupAttribute<"systemuser">;
      get(name: "modifiedon"): Xrm.DateAttribute;
      get(name: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
      get(name: "record1id"): Xrm.Attribute<any>;
      get(name: "record1roleid"): Xrm.LookupAttribute<"connectionrole">;
      get(name: "record2id"): Xrm.Attribute<any>;
      get(name: "record2roleid"): Xrm.LookupAttribute<"connectionrole">;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defra_connectiondetailsid"): Xrm.LookupControl<"defra_connectiondetails">;
      get(name: "defra_iscustomer"): Xrm.OptionSetControl<boolean>;
      get(name: "defra_previousconnectiondetail"): Xrm.LookupControl<"defra_connectiondetails">;
      get(name: "description"): Xrm.StringControl;
      get(name: "effectiveend"): Xrm.DateControl;
      get(name: "effectivestart"): Xrm.DateControl;
      get(name: "footer_createdby"): Xrm.LookupControl<"systemuser">;
      get(name: "footer_createdon"): Xrm.DateControl;
      get(name: "footer_modifiedby"): Xrm.LookupControl<"systemuser">;
      get(name: "footer_modifiedon"): Xrm.DateControl;
      get(name: "header_record1id"): Xrm.Control<Xrm.Attribute<any>>;
      get(name: "header_record2roleid"): Xrm.LookupControl<"connectionrole">;
      get(name: "ownerid"): Xrm.LookupControl<"systemuser" | "team">;
      get(name: "record1id"): Xrm.Control<Xrm.Attribute<any>>;
      get(name: "record1roleid"): Xrm.LookupControl<"connectionrole">;
      get(name: "record2id"): Xrm.Control<Xrm.Attribute<any>>;
      get(name: "record2roleid"): Xrm.LookupControl<"connectionrole">;
      get(name: string): undefined;
      get(): Xrm.BaseControl[];
      get(index: number): Xrm.BaseControl;
      get(chooser: (item: Xrm.BaseControl, index: number) => boolean): Xrm.BaseControl[];
    }
    interface Tabs extends Xrm.TabCollectionBase {
      get(name: "details"): Xrm.PageTab<Tabs.details>;
      get(name: "info"): Xrm.PageTab<Tabs.info>;
      get(name: "tab_ConnectFrom"): Xrm.PageTab<Tabs.tab_ConnectFrom>;
      get(name: "tab_CustomerDetails"): Xrm.PageTab<Tabs.tab_CustomerDetails>;
      get(name: string): undefined;
      get(): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
      get(index: number): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>;
      get(chooser: (item: Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>, index: number) => boolean): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
    }
  }
  interface DefraCustMstrMAIN extends Xrm.PageBase<DefraCustMstrMAIN.Attributes,DefraCustMstrMAIN.Tabs,DefraCustMstrMAIN.Controls> {
    getAttribute(attributeName: "createdby"): Xrm.LookupAttribute<"systemuser">;
    getAttribute(attributeName: "createdon"): Xrm.DateAttribute;
    getAttribute(attributeName: "defra_connectiondetailsid"): Xrm.LookupAttribute<"defra_connectiondetails">;
    getAttribute(attributeName: "defra_iscustomer"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defra_previousconnectiondetail"): Xrm.LookupAttribute<"defra_connectiondetails">;
    getAttribute(attributeName: "description"): Xrm.Attribute<string>;
    getAttribute(attributeName: "effectiveend"): Xrm.DateAttribute;
    getAttribute(attributeName: "effectivestart"): Xrm.DateAttribute;
    getAttribute(attributeName: "modifiedby"): Xrm.LookupAttribute<"systemuser">;
    getAttribute(attributeName: "modifiedon"): Xrm.DateAttribute;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
    getAttribute(attributeName: "record1id"): Xrm.Attribute<any>;
    getAttribute(attributeName: "record1roleid"): Xrm.LookupAttribute<"connectionrole">;
    getAttribute(attributeName: "record2id"): Xrm.Attribute<any>;
    getAttribute(attributeName: "record2roleid"): Xrm.LookupAttribute<"connectionrole">;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defra_connectiondetailsid"): Xrm.LookupControl<"defra_connectiondetails">;
    getControl(controlName: "defra_iscustomer"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "defra_previousconnectiondetail"): Xrm.LookupControl<"defra_connectiondetails">;
    getControl(controlName: "description"): Xrm.StringControl;
    getControl(controlName: "effectiveend"): Xrm.DateControl;
    getControl(controlName: "effectivestart"): Xrm.DateControl;
    getControl(controlName: "footer_createdby"): Xrm.LookupControl<"systemuser">;
    getControl(controlName: "footer_createdon"): Xrm.DateControl;
    getControl(controlName: "footer_modifiedby"): Xrm.LookupControl<"systemuser">;
    getControl(controlName: "footer_modifiedon"): Xrm.DateControl;
    getControl(controlName: "header_record1id"): Xrm.Control<Xrm.Attribute<any>>;
    getControl(controlName: "header_record2roleid"): Xrm.LookupControl<"connectionrole">;
    getControl(controlName: "ownerid"): Xrm.LookupControl<"systemuser" | "team">;
    getControl(controlName: "record1id"): Xrm.Control<Xrm.Attribute<any>>;
    getControl(controlName: "record1roleid"): Xrm.LookupControl<"connectionrole">;
    getControl(controlName: "record2id"): Xrm.Control<Xrm.Attribute<any>>;
    getControl(controlName: "record2roleid"): Xrm.LookupControl<"connectionrole">;
    getControl(controlName: string): undefined;
  }
}
