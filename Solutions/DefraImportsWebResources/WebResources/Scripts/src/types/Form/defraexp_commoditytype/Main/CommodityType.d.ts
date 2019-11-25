declare namespace Form.defraexp_commoditytype.Main {
  namespace CommodityType {
    namespace Tabs {
      interface hidden_fields extends Xrm.SectionCollectionBase {
        get(name: "tab_2_section_1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraexp_commoditygroupid"): Xrm.LookupAttribute<"defraexp_commoditygroup">;
      get(name: "defraexp_name"): Xrm.Attribute<string>;
      get(name: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraexp_commoditygroupid"): Xrm.LookupControl<"defraexp_commoditygroup">;
      get(name: "defraexp_name"): Xrm.StringControl;
      get(name: "ownerid"): Xrm.LookupControl<"systemuser" | "team">;
      get(name: "subgrid_commodityName"): Xrm.SubGridControl<"defraexp_commodityname">;
      get(name: string): undefined;
      get(): Xrm.BaseControl[];
      get(index: number): Xrm.BaseControl;
      get(chooser: (item: Xrm.BaseControl, index: number) => boolean): Xrm.BaseControl[];
    }
    interface Tabs extends Xrm.TabCollectionBase {
      get(name: "hidden_fields"): Xrm.PageTab<Tabs.hidden_fields>;
      get(name: string): undefined;
      get(): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
      get(index: number): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>;
      get(chooser: (item: Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>, index: number) => boolean): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
    }
  }
  interface CommodityType extends Xrm.PageBase<CommodityType.Attributes,CommodityType.Tabs,CommodityType.Controls> {
    getAttribute(attributeName: "defraexp_commoditygroupid"): Xrm.LookupAttribute<"defraexp_commoditygroup">;
    getAttribute(attributeName: "defraexp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraexp_commoditygroupid"): Xrm.LookupControl<"defraexp_commoditygroup">;
    getControl(controlName: "defraexp_name"): Xrm.StringControl;
    getControl(controlName: "ownerid"): Xrm.LookupControl<"systemuser" | "team">;
    getControl(controlName: "subgrid_commodityName"): Xrm.SubGridControl<"defraexp_commodityname">;
    getControl(controlName: string): undefined;
  }
}
