declare namespace Form.defraimp_itahc.Quick {
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
      get(name: "defraimp_consigneeaddresscity"): Xrm.Attribute<string>;
      get(name: "defraimp_consigneeaddresscountryid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_consigneeaddresspostcode"): Xrm.Attribute<string>;
      get(name: "defraimp_consigneeaddressstreet"): Xrm.Attribute<string>;
      get(name: "defraimp_consigneeapprovalnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_consigneegeneralnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_consigneename"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_consigneeaddresscity"): Xrm.StringControl;
      get(name: "defraimp_consigneeaddresscountryid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_consigneeaddresspostcode"): Xrm.StringControl;
      get(name: "defraimp_consigneeaddressstreet"): Xrm.StringControl;
      get(name: "defraimp_consigneeapprovalnumber"): Xrm.StringControl;
      get(name: "defraimp_consigneegeneralnumber"): Xrm.StringControl;
      get(name: "defraimp_consigneename"): Xrm.StringControl;
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
    getAttribute(attributeName: "defraimp_consigneeaddresscity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consigneeaddresscountryid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_consigneeaddresspostcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consigneeaddressstreet"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consigneeapprovalnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consigneegeneralnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_consigneename"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_consigneeaddresscity"): Xrm.StringControl;
    getControl(controlName: "defraimp_consigneeaddresscountryid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_consigneeaddresspostcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_consigneeaddressstreet"): Xrm.StringControl;
    getControl(controlName: "defraimp_consigneeapprovalnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_consigneegeneralnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_consigneename"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
