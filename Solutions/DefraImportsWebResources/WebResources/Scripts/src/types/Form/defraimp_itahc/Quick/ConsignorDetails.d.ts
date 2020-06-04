declare namespace Form.defraimp_itahc.Quick {
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
      get(name: "defraimp_consignoraddresscity"): Xrm.Attribute<string>;
      get(name: "defraimp_consignoraddresscountryid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_consignoraddresspostcode"): Xrm.Attribute<string>;
      get(name: "defraimp_consignoraddressstreet"): Xrm.Attribute<string>;
      get(name: "defraimp_consignorapprovalnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_consignorgeneralnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_consignorname"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_consignoraddresscity"): Xrm.StringControl;
      get(name: "defraimp_consignoraddresscountryid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_consignoraddresspostcode"): Xrm.StringControl;
      get(name: "defraimp_consignoraddressstreet"): Xrm.StringControl;
      get(name: "defraimp_consignorapprovalnumber"): Xrm.StringControl;
      get(name: "defraimp_consignorgeneralnumber"): Xrm.StringControl;
      get(name: "defraimp_consignorname"): Xrm.StringControl;
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
    getAttribute(attributeName: "defraimp_consignoraddresscity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consignoraddresscountryid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_consignoraddresspostcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consignoraddressstreet"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consignorapprovalnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consignorgeneralnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consignorname"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_consignoraddresscity"): Xrm.StringControl;
    getControl(controlName: "defraimp_consignoraddresscountryid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_consignoraddresspostcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_consignoraddressstreet"): Xrm.StringControl;
    getControl(controlName: "defraimp_consignorapprovalnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_consignorgeneralnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_consignorname"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
