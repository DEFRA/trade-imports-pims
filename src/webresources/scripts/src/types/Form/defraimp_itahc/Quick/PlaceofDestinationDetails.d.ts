declare namespace Form.defraimp_itahc.Quick {
  namespace PlaceofDestinationDetails {
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
      get(name: "defraimp_placeofdestinationaddresscity"): Xrm.Attribute<string>;
      get(name: "defraimp_placeofdestinationaddresscountryid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_placeofdestinationaddresspostcode"): Xrm.Attribute<string>;
      get(name: "defraimp_placeofdestinationaddressstreet"): Xrm.Attribute<string>;
      get(name: "defraimp_placeofdestinationapprovalnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_placeofdestinationgeneralnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_placeofdestinationname"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_placeofdestinationaddresscity"): Xrm.StringControl;
      get(name: "defraimp_placeofdestinationaddresscountryid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_placeofdestinationaddresspostcode"): Xrm.StringControl;
      get(name: "defraimp_placeofdestinationaddressstreet"): Xrm.StringControl;
      get(name: "defraimp_placeofdestinationapprovalnumber"): Xrm.StringControl;
      get(name: "defraimp_placeofdestinationgeneralnumber"): Xrm.StringControl;
      get(name: "defraimp_placeofdestinationname"): Xrm.StringControl;
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
  interface PlaceofDestinationDetails extends Xrm.PageBase<PlaceofDestinationDetails.Attributes,PlaceofDestinationDetails.Tabs,PlaceofDestinationDetails.Controls> {
    getAttribute(attributeName: "defraimp_placeofdestinationaddresscity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeofdestinationaddresscountryid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_placeofdestinationaddresspostcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeofdestinationaddressstreet"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeofdestinationapprovalnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeofdestinationgeneralnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeofdestinationname"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_placeofdestinationaddresscity"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeofdestinationaddresscountryid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_placeofdestinationaddresspostcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeofdestinationaddressstreet"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeofdestinationapprovalnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeofdestinationgeneralnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeofdestinationname"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
