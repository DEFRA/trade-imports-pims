declare namespace Form.defraimp_placeoforigin.QuickCreate {
  namespace PlaceofOriginQuickCreate {
    namespace Tabs {
      interface tab_1 extends Xrm.SectionCollectionBase {
        get(name: "tab_1_column_1_section_1"): Xrm.PageSection;
        get(name: "tab_1_column_2_section_1"): Xrm.PageSection;
        get(name: "tab_1_column_3_section_1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_addresscity"): Xrm.Attribute<string>;
      get(name: "defraimp_addresscountry"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_addressline1"): Xrm.Attribute<string>;
      get(name: "defraimp_addressline2"): Xrm.Attribute<string>;
      get(name: "defraimp_addressline3"): Xrm.Attribute<string>;
      get(name: "defraimp_addressstateorprovince"): Xrm.Attribute<string>;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: "defraimp_postcode"): Xrm.Attribute<string>;
      get(name: "defraimp_trustlevel"): Xrm.OptionSetAttribute<defraimp_trustlevel>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_addresscity"): Xrm.StringControl;
      get(name: "defraimp_addresscountry"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_addressline1"): Xrm.StringControl;
      get(name: "defraimp_addressline2"): Xrm.StringControl;
      get(name: "defraimp_addressline3"): Xrm.StringControl;
      get(name: "defraimp_addressstateorprovince"): Xrm.StringControl;
      get(name: "defraimp_name"): Xrm.StringControl;
      get(name: "defraimp_postcode"): Xrm.StringControl;
      get(name: "defraimp_trustlevel"): Xrm.OptionSetControl<defraimp_trustlevel>;
      get(name: string): undefined;
      get(): Xrm.BaseControl[];
      get(index: number): Xrm.BaseControl;
      get(chooser: (item: Xrm.BaseControl, index: number) => boolean): Xrm.BaseControl[];
    }
    interface Tabs extends Xrm.TabCollectionBase {
      get(name: "tab_1"): Xrm.PageTab<Tabs.tab_1>;
      get(name: string): undefined;
      get(): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
      get(index: number): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>;
      get(chooser: (item: Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>, index: number) => boolean): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
    }
  }
  interface PlaceofOriginQuickCreate extends Xrm.PageBase<PlaceofOriginQuickCreate.Attributes,PlaceofOriginQuickCreate.Tabs,PlaceofOriginQuickCreate.Controls> {
    getAttribute(attributeName: "defraimp_addresscity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_addresscountry"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_addressline1"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_addressline2"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_addressline3"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_addressstateorprovince"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_postcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_trustlevel"): Xrm.OptionSetAttribute<defraimp_trustlevel>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_addresscity"): Xrm.StringControl;
    getControl(controlName: "defraimp_addresscountry"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_addressline1"): Xrm.StringControl;
    getControl(controlName: "defraimp_addressline2"): Xrm.StringControl;
    getControl(controlName: "defraimp_addressline3"): Xrm.StringControl;
    getControl(controlName: "defraimp_addressstateorprovince"): Xrm.StringControl;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "defraimp_postcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_trustlevel"): Xrm.OptionSetControl<defraimp_trustlevel>;
    getControl(controlName: string): undefined;
  }
}
