declare namespace Form.defraimp_importcountrycommodityrisklevel.Main {
  namespace Information {
    namespace Tabs {
      interface _1ebba9cd5afb414984c2418d693390b0 extends Xrm.SectionCollectionBase {
        get(name: "{1ebba9cd-5afb-4149-84c2-418d693390b0}_section_2"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "createdby"): Xrm.LookupAttribute<"systemuser">;
      get(name: "createdon"): Xrm.DateAttribute;
      get(name: "defraimp_commoditytypeid"): Xrm.LookupAttribute<"defraexp_commoditytype">;
      get(name: "defraimp_countryid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_importrisklevelid"): Xrm.LookupAttribute<"defraimp_importrisklevel">;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: "defraimp_risklevelnotes"): Xrm.Attribute<string>;
      get(name: "modifiedby"): Xrm.LookupAttribute<"systemuser">;
      get(name: "modifiedon"): Xrm.DateAttribute;
      get(name: "statecode"): Xrm.OptionSetAttribute<defraimp_importcountrycommodityrisklevel_statecode>;
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
      get(name: "footer_createdby"): Xrm.LookupControl<"systemuser">;
      get(name: "footer_createdon"): Xrm.DateControl;
      get(name: "footer_modifiedby"): Xrm.LookupControl<"systemuser">;
      get(name: "footer_modifiedon"): Xrm.DateControl;
      get(name: "footer_statecode"): Xrm.OptionSetControl<defraimp_importcountrycommodityrisklevel_statecode>;
      get(name: "header_statecode"): Xrm.OptionSetControl<defraimp_importcountrycommodityrisklevel_statecode>;
      get(name: string): undefined;
      get(): Xrm.BaseControl[];
      get(index: number): Xrm.BaseControl;
      get(chooser: (item: Xrm.BaseControl, index: number) => boolean): Xrm.BaseControl[];
    }
    interface Tabs extends Xrm.TabCollectionBase {
      get(name: "{1ebba9cd-5afb-4149-84c2-418d693390b0}"): Xrm.PageTab<Tabs._1ebba9cd5afb414984c2418d693390b0>;
      get(name: string): undefined;
      get(): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
      get(index: number): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>;
      get(chooser: (item: Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>, index: number) => boolean): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
    }
  }
  interface Information extends Xrm.PageBase<Information.Attributes,Information.Tabs,Information.Controls> {
    getAttribute(attributeName: "createdby"): Xrm.LookupAttribute<"systemuser">;
    getAttribute(attributeName: "createdon"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_commoditytypeid"): Xrm.LookupAttribute<"defraexp_commoditytype">;
    getAttribute(attributeName: "defraimp_countryid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_importrisklevelid"): Xrm.LookupAttribute<"defraimp_importrisklevel">;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_risklevelnotes"): Xrm.Attribute<string>;
    getAttribute(attributeName: "modifiedby"): Xrm.LookupAttribute<"systemuser">;
    getAttribute(attributeName: "modifiedon"): Xrm.DateAttribute;
    getAttribute(attributeName: "statecode"): Xrm.OptionSetAttribute<defraimp_importcountrycommodityrisklevel_statecode>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_commoditytypeid"): Xrm.LookupControl<"defraexp_commoditytype">;
    getControl(controlName: "defraimp_countryid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_importrisklevelid"): Xrm.LookupControl<"defraimp_importrisklevel">;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "defraimp_risklevelnotes"): Xrm.StringControl;
    getControl(controlName: "footer_createdby"): Xrm.LookupControl<"systemuser">;
    getControl(controlName: "footer_createdon"): Xrm.DateControl;
    getControl(controlName: "footer_modifiedby"): Xrm.LookupControl<"systemuser">;
    getControl(controlName: "footer_modifiedon"): Xrm.DateControl;
    getControl(controlName: "footer_statecode"): Xrm.OptionSetControl<defraimp_importcountrycommodityrisklevel_statecode>;
    getControl(controlName: "header_statecode"): Xrm.OptionSetControl<defraimp_importcountrycommodityrisklevel_statecode>;
    getControl(controlName: string): undefined;
  }
}
