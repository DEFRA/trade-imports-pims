declare namespace Form.defraimp_importernotification.Quick {
  namespace ConsigneeDetails {
    namespace Tabs {
      interface tab_1 extends Xrm.SectionCollectionBase {
        get(name: "tab_1_column_1_section_1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_consigneeaddressaddressline1"): Xrm.Attribute<string>;
      get(name: "defraimp_consigneeaddressaddressline2"): Xrm.Attribute<string>;
      get(name: "defraimp_consigneeaddressaddressline3"): Xrm.Attribute<string>;
      get(name: "defraimp_consigneeaddresscity"): Xrm.Attribute<string>;
      get(name: "defraimp_consigneeaddresscountryid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_consigneeaddresspostalzipcode"): Xrm.Attribute<string>;
      get(name: "defraimp_consigneeapprovalnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_consigneecompanyname"): Xrm.Attribute<string>;
      get(name: "defraimp_consigneeotheridentifier"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_consigneeaddressaddressline1"): Xrm.StringControl;
      get(name: "defraimp_consigneeaddressaddressline2"): Xrm.StringControl;
      get(name: "defraimp_consigneeaddressaddressline3"): Xrm.StringControl;
      get(name: "defraimp_consigneeaddresscity"): Xrm.StringControl;
      get(name: "defraimp_consigneeaddresscountryid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_consigneeaddresspostalzipcode"): Xrm.StringControl;
      get(name: "defraimp_consigneeapprovalnumber"): Xrm.StringControl;
      get(name: "defraimp_consigneecompanyname"): Xrm.StringControl;
      get(name: "defraimp_consigneeotheridentifier"): Xrm.StringControl;
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
  interface ConsigneeDetails extends Xrm.PageBase<ConsigneeDetails.Attributes,ConsigneeDetails.Tabs,ConsigneeDetails.Controls> {
    getAttribute(attributeName: "defraimp_consigneeaddressaddressline1"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consigneeaddressaddressline2"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consigneeaddressaddressline3"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consigneeaddresscity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consigneeaddresscountryid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_consigneeaddresspostalzipcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consigneeapprovalnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consigneecompanyname"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consigneeotheridentifier"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_consigneeaddressaddressline1"): Xrm.StringControl;
    getControl(controlName: "defraimp_consigneeaddressaddressline2"): Xrm.StringControl;
    getControl(controlName: "defraimp_consigneeaddressaddressline3"): Xrm.StringControl;
    getControl(controlName: "defraimp_consigneeaddresscity"): Xrm.StringControl;
    getControl(controlName: "defraimp_consigneeaddresscountryid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_consigneeaddresspostalzipcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_consigneeapprovalnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_consigneecompanyname"): Xrm.StringControl;
    getControl(controlName: "defraimp_consigneeotheridentifier"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
