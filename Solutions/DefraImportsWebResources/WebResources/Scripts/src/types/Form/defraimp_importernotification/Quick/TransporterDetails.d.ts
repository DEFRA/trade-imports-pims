declare namespace Form.defraimp_importernotification.Quick {
  namespace TransporterDetails {
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
      get(name: "defraimp_transporteraddressaddressline1"): Xrm.Attribute<string>;
      get(name: "defraimp_transporteraddressaddressline2"): Xrm.Attribute<string>;
      get(name: "defraimp_transporteraddressaddressline3"): Xrm.Attribute<string>;
      get(name: "defraimp_transporteraddresscity"): Xrm.Attribute<string>;
      get(name: "defraimp_transporteraddresscountryid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_transporteraddresspostalzipcode"): Xrm.Attribute<string>;
      get(name: "defraimp_transporterapprovalnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_transportercompanyname"): Xrm.Attribute<string>;
      get(name: "defraimp_transporterindividualname"): Xrm.Attribute<string>;
      get(name: "defraimp_transporterotheridentifier"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_transporteraddressaddressline1"): Xrm.StringControl;
      get(name: "defraimp_transporteraddressaddressline2"): Xrm.StringControl;
      get(name: "defraimp_transporteraddressaddressline3"): Xrm.StringControl;
      get(name: "defraimp_transporteraddresscity"): Xrm.StringControl;
      get(name: "defraimp_transporteraddresscountryid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_transporteraddresspostalzipcode"): Xrm.StringControl;
      get(name: "defraimp_transporterapprovalnumber"): Xrm.StringControl;
      get(name: "defraimp_transportercompanyname"): Xrm.StringControl;
      get(name: "defraimp_transporterindividualname"): Xrm.StringControl;
      get(name: "defraimp_transporterotheridentifier"): Xrm.StringControl;
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
  interface TransporterDetails extends Xrm.PageBase<TransporterDetails.Attributes,TransporterDetails.Tabs,TransporterDetails.Controls> {
    getAttribute(attributeName: "defraimp_transporteraddressaddressline1"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_transporteraddressaddressline2"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_transporteraddressaddressline3"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_transporteraddresscity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_transporteraddresscountryid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_transporteraddresspostalzipcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_transporterapprovalnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_transportercompanyname"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_transporterindividualname"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_transporterotheridentifier"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_transporteraddressaddressline1"): Xrm.StringControl;
    getControl(controlName: "defraimp_transporteraddressaddressline2"): Xrm.StringControl;
    getControl(controlName: "defraimp_transporteraddressaddressline3"): Xrm.StringControl;
    getControl(controlName: "defraimp_transporteraddresscity"): Xrm.StringControl;
    getControl(controlName: "defraimp_transporteraddresscountryid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_transporteraddresspostalzipcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_transporterapprovalnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_transportercompanyname"): Xrm.StringControl;
    getControl(controlName: "defraimp_transporterindividualname"): Xrm.StringControl;
    getControl(controlName: "defraimp_transporterotheridentifier"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
