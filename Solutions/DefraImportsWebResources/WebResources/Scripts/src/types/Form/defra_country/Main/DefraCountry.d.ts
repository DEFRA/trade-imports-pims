declare namespace Form.defra_country.Main {
  namespace DefraCountry {
    namespace Tabs {
      interface _06f939cfd79e4993941a01ea46d22397 extends Xrm.SectionCollectionBase {
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defra_aka"): Xrm.Attribute<string>;
      get(name: "defra_citizenemonym"): Xrm.Attribute<string>;
      get(name: "defra_enddate"): Xrm.DateAttribute;
      get(name: "defra_isocodealpha2"): Xrm.Attribute<string>;
      get(name: "defra_isocodealpha3"): Xrm.Attribute<string>;
      get(name: "defra_isonumericcode"): Xrm.Attribute<string>;
      get(name: "defra_name"): Xrm.Attribute<string>;
      get(name: "defra_startdate"): Xrm.DateAttribute;
      get(name: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defra_aka"): Xrm.StringControl;
      get(name: "defra_citizenemonym"): Xrm.StringControl;
      get(name: "defra_enddate"): Xrm.DateControl;
      get(name: "defra_isocodealpha2"): Xrm.StringControl;
      get(name: "defra_isocodealpha3"): Xrm.StringControl;
      get(name: "defra_isonumericcode"): Xrm.StringControl;
      get(name: "defra_name"): Xrm.StringControl;
      get(name: "defra_startdate"): Xrm.DateControl;
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
  interface DefraCountry extends Xrm.PageBase<DefraCountry.Attributes,DefraCountry.Tabs,DefraCountry.Controls> {
    getAttribute(attributeName: "defra_aka"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_citizenemonym"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_enddate"): Xrm.DateAttribute;
    getAttribute(attributeName: "defra_isocodealpha2"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_isocodealpha3"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_isonumericcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defra_startdate"): Xrm.DateAttribute;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defra_aka"): Xrm.StringControl;
    getControl(controlName: "defra_citizenemonym"): Xrm.StringControl;
    getControl(controlName: "defra_enddate"): Xrm.DateControl;
    getControl(controlName: "defra_isocodealpha2"): Xrm.StringControl;
    getControl(controlName: "defra_isocodealpha3"): Xrm.StringControl;
    getControl(controlName: "defra_isonumericcode"): Xrm.StringControl;
    getControl(controlName: "defra_name"): Xrm.StringControl;
    getControl(controlName: "defra_startdate"): Xrm.DateControl;
    getControl(controlName: "header_ownerid"): Xrm.LookupControl<"systemuser" | "team">;
    getControl(controlName: string): undefined;
  }
}
