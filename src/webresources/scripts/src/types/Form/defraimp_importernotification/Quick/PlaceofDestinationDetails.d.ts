declare namespace Form.defraimp_importernotification.Quick {
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
      get(name: "defraimp_placeofdestinationaddressaddressline1"): Xrm.Attribute<string>;
      get(name: "defraimp_placeofdestinationaddressaddressline2"): Xrm.Attribute<string>;
      get(name: "defraimp_placeofdestinationaddressaddressline3"): Xrm.Attribute<string>;
      get(name: "defraimp_placeofdestinationaddresscity"): Xrm.Attribute<string>;
      get(name: "defraimp_placeofdestinationaddresspostalzipcode"): Xrm.Attribute<string>;
      get(name: "defraimp_placeofdestinationapprovalnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_placeofdestinationcompanyname"): Xrm.Attribute<string>;
      get(name: "defraimp_placeofdestinationcountryid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_placeofdestinationotheridentifier"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_placeofdestinationaddressaddressline1"): Xrm.StringControl;
      get(name: "defraimp_placeofdestinationaddressaddressline2"): Xrm.StringControl;
      get(name: "defraimp_placeofdestinationaddressaddressline3"): Xrm.StringControl;
      get(name: "defraimp_placeofdestinationaddresscity"): Xrm.StringControl;
      get(name: "defraimp_placeofdestinationaddresspostalzipcode"): Xrm.StringControl;
      get(name: "defraimp_placeofdestinationapprovalnumber"): Xrm.StringControl;
      get(name: "defraimp_placeofdestinationcompanyname"): Xrm.StringControl;
      get(name: "defraimp_placeofdestinationcountryid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_placeofdestinationotheridentifier"): Xrm.StringControl;
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
    getAttribute(attributeName: "defraimp_placeofdestinationaddressaddressline1"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeofdestinationaddressaddressline2"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeofdestinationaddressaddressline3"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeofdestinationaddresscity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeofdestinationaddresspostalzipcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeofdestinationapprovalnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeofdestinationcompanyname"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeofdestinationcountryid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_placeofdestinationotheridentifier"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_placeofdestinationaddressaddressline1"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeofdestinationaddressaddressline2"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeofdestinationaddressaddressline3"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeofdestinationaddresscity"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeofdestinationaddresspostalzipcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeofdestinationapprovalnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeofdestinationcompanyname"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeofdestinationcountryid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_placeofdestinationotheridentifier"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
