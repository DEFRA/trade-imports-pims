declare namespace Form.defraimp_itahc.Quick {
  namespace KeyDetails {
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
      get(name: "createdon"): Xrm.DateAttribute;
      get(name: "defraimp_animalcertifiedas"): Xrm.OptionSetAttribute<defraimp_animalcertifiedas>;
      get(name: "defraimp_commoditycode"): Xrm.Attribute<string>;
      get(name: "defraimp_commoditytype"): Xrm.LookupAttribute<"defraexp_commoditytype">;
      get(name: "defraimp_countryoforiginid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_departuredatetime"): Xrm.DateAttribute;
      get(name: "defraimp_estimatedjourneytime"): Xrm.NumberAttribute;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: "defraimp_quantity"): Xrm.NumberAttribute;
      get(name: "defraimp_tracesreceiveddate"): Xrm.DateAttribute;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "createdon"): Xrm.DateControl;
      get(name: "defraimp_animalcertifiedas"): Xrm.OptionSetControl<defraimp_animalcertifiedas>;
      get(name: "defraimp_commoditycode"): Xrm.StringControl;
      get(name: "defraimp_commoditytype"): Xrm.LookupControl<"defraexp_commoditytype">;
      get(name: "defraimp_countryoforiginid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_departuredatetime"): Xrm.DateControl;
      get(name: "defraimp_estimatedjourneytime"): Xrm.NumberControl;
      get(name: "defraimp_name"): Xrm.StringControl;
      get(name: "defraimp_quantity"): Xrm.NumberControl;
      get(name: "defraimp_tracesreceiveddate"): Xrm.DateControl;
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
  interface KeyDetails extends Xrm.PageBase<KeyDetails.Attributes,KeyDetails.Tabs,KeyDetails.Controls> {
    getAttribute(attributeName: "createdon"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_animalcertifiedas"): Xrm.OptionSetAttribute<defraimp_animalcertifiedas>;
    getAttribute(attributeName: "defraimp_commoditycode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_commoditytype"): Xrm.LookupAttribute<"defraexp_commoditytype">;
    getAttribute(attributeName: "defraimp_countryoforiginid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_departuredatetime"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_estimatedjourneytime"): Xrm.NumberAttribute;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_quantity"): Xrm.NumberAttribute;
    getAttribute(attributeName: "defraimp_tracesreceiveddate"): Xrm.DateAttribute;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "createdon"): Xrm.DateControl;
    getControl(controlName: "defraimp_animalcertifiedas"): Xrm.OptionSetControl<defraimp_animalcertifiedas>;
    getControl(controlName: "defraimp_commoditycode"): Xrm.StringControl;
    getControl(controlName: "defraimp_commoditytype"): Xrm.LookupControl<"defraexp_commoditytype">;
    getControl(controlName: "defraimp_countryoforiginid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_departuredatetime"): Xrm.DateControl;
    getControl(controlName: "defraimp_estimatedjourneytime"): Xrm.NumberControl;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "defraimp_quantity"): Xrm.NumberControl;
    getControl(controlName: "defraimp_tracesreceiveddate"): Xrm.DateControl;
    getControl(controlName: string): undefined;
  }
}
