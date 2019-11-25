declare namespace Form.defraimp_importcountrycommodityrisklevel.Quick {
  namespace Information {
    namespace Tabs {
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_commoditytypeid"): Xrm.LookupAttribute<"defraexp_commoditytype">;
      get(name: "defraimp_countryid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_importrisklevelid"): Xrm.LookupAttribute<"defraimp_importrisklevel">;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: "defraimp_risklevelnotes"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_commoditytypeid"): Xrm.LookupControl<"defraexp_commoditytype">;
      get(name: "defraimp_countryid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_importrisklevelid"): Xrm.LookupControl<"defraimp_importrisklevel">;
      get(name: "defraimp_name"): Xrm.StringControl;
      get(name: "defraimp_risklevelnotes"): Xrm.StringControl;
      get(name: string): undefined;
      get(): Xrm.BaseControl[];
      get(index: number): Xrm.BaseControl;
      get(chooser: (item: Xrm.BaseControl, index: number) => boolean): Xrm.BaseControl[];
    }
    interface Tabs extends Xrm.TabCollectionBase {
      get(name: string): undefined;
      get(): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
      get(index: number): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>;
      get(chooser: (item: Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>, index: number) => boolean): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
    }
  }
  interface Information extends Xrm.PageBase<Information.Attributes,Information.Tabs,Information.Controls> {
    getAttribute(attributeName: "defraimp_commoditytypeid"): Xrm.LookupAttribute<"defraexp_commoditytype">;
    getAttribute(attributeName: "defraimp_countryid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_importrisklevelid"): Xrm.LookupAttribute<"defraimp_importrisklevel">;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_risklevelnotes"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_commoditytypeid"): Xrm.LookupControl<"defraexp_commoditytype">;
    getControl(controlName: "defraimp_countryid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_importrisklevelid"): Xrm.LookupControl<"defraimp_importrisklevel">;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "defraimp_risklevelnotes"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
