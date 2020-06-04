declare namespace Form.defraimp_itahc.Quick {
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
      get(name: "defraimp_transporteraddresscity"): Xrm.Attribute<string>;
      get(name: "defraimp_transporteraddresscountryid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_transporteraddresspostcode"): Xrm.Attribute<string>;
      get(name: "defraimp_transporteraddressstreet"): Xrm.Attribute<string>;
      get(name: "defraimp_transporterapprovalnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_transportergeneralnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_transportername"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_transporteraddresscity"): Xrm.StringControl;
      get(name: "defraimp_transporteraddresscountryid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_transporteraddresspostcode"): Xrm.StringControl;
      get(name: "defraimp_transporteraddressstreet"): Xrm.StringControl;
      get(name: "defraimp_transporterapprovalnumber"): Xrm.StringControl;
      get(name: "defraimp_transportergeneralnumber"): Xrm.StringControl;
      get(name: "defraimp_transportername"): Xrm.StringControl;
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
    getAttribute(attributeName: "defraimp_transporteraddresscity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_transporteraddresscountryid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_transporteraddresspostcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_transporteraddressstreet"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_transporterapprovalnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_transportergeneralnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_transportername"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_transporteraddresscity"): Xrm.StringControl;
    getControl(controlName: "defraimp_transporteraddresscountryid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_transporteraddresspostcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_transporteraddressstreet"): Xrm.StringControl;
    getControl(controlName: "defraimp_transporterapprovalnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_transportergeneralnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_transportername"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
