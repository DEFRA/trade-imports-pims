declare namespace Form.defraimp_importernotification.Quick {
  namespace ConsignorDetails {
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
      get(name: "defraimp_consignoraddressaddressline1"): Xrm.Attribute<string>;
      get(name: "defraimp_consignoraddressaddressline2"): Xrm.Attribute<string>;
      get(name: "defraimp_consignoraddressaddressline3"): Xrm.Attribute<string>;
      get(name: "defraimp_consignoraddresscity"): Xrm.Attribute<string>;
      get(name: "defraimp_consignoraddresscountryid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_consignoraddresspostalzipcode"): Xrm.Attribute<string>;
      get(name: "defraimp_consignorapprovalnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_consignorcompanyname"): Xrm.Attribute<string>;
      get(name: "defraimp_consignorotheridentifier"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_consignoraddressaddressline1"): Xrm.StringControl;
      get(name: "defraimp_consignoraddressaddressline2"): Xrm.StringControl;
      get(name: "defraimp_consignoraddressaddressline3"): Xrm.StringControl;
      get(name: "defraimp_consignoraddresscity"): Xrm.StringControl;
      get(name: "defraimp_consignoraddresscountryid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_consignoraddresspostalzipcode"): Xrm.StringControl;
      get(name: "defraimp_consignorapprovalnumber"): Xrm.StringControl;
      get(name: "defraimp_consignorcompanyname"): Xrm.StringControl;
      get(name: "defraimp_consignorotheridentifier"): Xrm.StringControl;
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
  interface ConsignorDetails extends Xrm.PageBase<ConsignorDetails.Attributes,ConsignorDetails.Tabs,ConsignorDetails.Controls> {
    getAttribute(attributeName: "defraimp_consignoraddressaddressline1"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consignoraddressaddressline2"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consignoraddressaddressline3"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consignoraddresscity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consignoraddresscountryid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_consignoraddresspostalzipcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consignorapprovalnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consignorcompanyname"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consignorotheridentifier"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_consignoraddressaddressline1"): Xrm.StringControl;
    getControl(controlName: "defraimp_consignoraddressaddressline2"): Xrm.StringControl;
    getControl(controlName: "defraimp_consignoraddressaddressline3"): Xrm.StringControl;
    getControl(controlName: "defraimp_consignoraddresscity"): Xrm.StringControl;
    getControl(controlName: "defraimp_consignoraddresscountryid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_consignoraddresspostalzipcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_consignorapprovalnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_consignorcompanyname"): Xrm.StringControl;
    getControl(controlName: "defraimp_consignorotheridentifier"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
