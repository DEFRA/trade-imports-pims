declare namespace Form.defra_country.Main {
  namespace DefraCustMstr {
    namespace Tabs {
      interface _06f939cfd79e4993941a01ea46d22397 extends Xrm.SectionCollectionBase {
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "createdby"): Xrm.LookupAttribute<"systemuser">;
      get(name: "createdon"): Xrm.DateAttribute;
      get(name: "defra_aka"): Xrm.Attribute<string>;
      get(name: "defra_citizenemonym"): Xrm.Attribute<string>;
      get(name: "defra_codeassignmenttype"): Xrm.Attribute<string>;
      get(name: "defra_enddate"): Xrm.DateAttribute;
      get(name: "defra_independent"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defra_iso31662subdivisioncodes"): Xrm.Attribute<string>;
      get(name: "defra_isocodealpha2"): Xrm.Attribute<string>;
      get(name: "defra_isocodealpha3"): Xrm.Attribute<string>;
      get(name: "defra_isonumericcode"): Xrm.Attribute<string>;
      get(name: "defra_name"): Xrm.Attribute<string>;
      get(name: "defra_notes"): Xrm.Attribute<string>;
      get(name: "defra_startdate"): Xrm.DateAttribute;
      get(name: "modifiedby"): Xrm.LookupAttribute<"systemuser">;
      get(name: "modifiedon"): Xrm.DateAttribute;
      get(name: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defra_aka"): Xrm.StringControl;
      get(name: "defra_citizenemonym"): Xrm.StringControl;
      get(name: "defra_codeassignmenttype"): Xrm.StringControl;
      get(name: "defra_enddate"): Xrm.DateControl;
      get(name: "defra_independent"): Xrm.OptionSetControl<boolean>;
      get(name: "defra_iso31662subdivisioncodes"): Xrm.StringControl;
      get(name: "defra_isocodealpha2"): Xrm.StringControl;
      get(name: "defra_isocodealpha3"): Xrm.StringControl;
      get(name: "defra_isonumericcode"): Xrm.StringControl;
      get(name: "defra_name"): Xrm.StringControl;
      get(name: "defra_notes"): Xrm.StringControl;
      get(name: "defra_startdate"): Xrm.DateControl;
      get(name: "footer_createdby"): Xrm.LookupControl<"systemuser">;
      get(name: "footer_createdon"): Xrm.DateControl;
      get(name: "footer_modifiedby"): Xrm.LookupControl<"systemuser">;
      get(name: "footer_modifiedon"): Xrm.DateControl;
      get(name: "header_defra_enddate"): Xrm.DateControl;
      get(name: "header_defra_startdate"): Xrm.DateControl;
      get(name: "header_ownerid"): Xrm.LookupControl<"systemuser" | "team">;
      get(name: string): undefined;
      get(): Xrm.BaseControl[];
      get(index: number): Xrm.BaseControl;
      get(chooser: (item: Xrm.BaseControl, index: number) => boolean): Xrm.BaseControl[];
    }
    interface Tabs extends Xrm.TabCollectionBase {
      get(name: "{06f939cf-d79e-4993-941a-01ea46d22397}"): Xrm.PageTab<Tabs._06f939cfd79e4993941a01ea46d22397>;
      get(name: string): undefined;
      get(): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
      get(index: number): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>;
      get(chooser: (item: Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>, index: number) => boolean): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
    }
  }
  interface DefraCustMstr extends Xrm.PageBase<DefraCustMstr.Attributes,DefraCustMstr.Tabs,DefraCustMstr.Controls> {
    getAttribute(attributeName: "createdby"): Xrm.LookupAttribute<"systemuser">;
    getAttribute(attributeName: "createdon"): Xrm.DateAttribute;
    getAttribute(attributeName: "defra_aka"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_citizenemonym"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_codeassignmenttype"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_enddate"): Xrm.DateAttribute;
    getAttribute(attributeName: "defra_independent"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defra_iso31662subdivisioncodes"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_isocodealpha2"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_isocodealpha3"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_isonumericcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_notes"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_startdate"): Xrm.DateAttribute;
    getAttribute(attributeName: "modifiedby"): Xrm.LookupAttribute<"systemuser">;
    getAttribute(attributeName: "modifiedon"): Xrm.DateAttribute;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defra_aka"): Xrm.StringControl;
    getControl(controlName: "defra_citizenemonym"): Xrm.StringControl;
    getControl(controlName: "defra_codeassignmenttype"): Xrm.StringControl;
    getControl(controlName: "defra_enddate"): Xrm.DateControl;
    getControl(controlName: "defra_independent"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "defra_iso31662subdivisioncodes"): Xrm.StringControl;
    getControl(controlName: "defra_isocodealpha2"): Xrm.StringControl;
    getControl(controlName: "defra_isocodealpha3"): Xrm.StringControl;
    getControl(controlName: "defra_isonumericcode"): Xrm.StringControl;
    getControl(controlName: "defra_name"): Xrm.StringControl;
    getControl(controlName: "defra_notes"): Xrm.StringControl;
    getControl(controlName: "defra_startdate"): Xrm.DateControl;
    getControl(controlName: "footer_createdby"): Xrm.LookupControl<"systemuser">;
    getControl(controlName: "footer_createdon"): Xrm.DateControl;
    getControl(controlName: "footer_modifiedby"): Xrm.LookupControl<"systemuser">;
    getControl(controlName: "footer_modifiedon"): Xrm.DateControl;
    getControl(controlName: "header_defra_enddate"): Xrm.DateControl;
    getControl(controlName: "header_defra_startdate"): Xrm.DateControl;
    getControl(controlName: "header_ownerid"): Xrm.LookupControl<"systemuser" | "team">;
    getControl(controlName: string): undefined;
  }
}
