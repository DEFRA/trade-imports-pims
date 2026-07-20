declare namespace Form.defraimp_itahc.QuickCreate {
  namespace ITAHCQuickCreate {
    namespace Tabs {
      interface tab_1 extends Xrm.SectionCollectionBase {
        get(name: "General_Section"): Xrm.PageSection;
        get(name: "HiddenSection1"): Xrm.PageSection;
        get(name: "HiddenSection2"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_healthcertificatenumber"): Xrm.Attribute<string>;
      get(name: "defraimp_localveterinaryunit"): Xrm.Attribute<string>;
      get(name: "defraimp_lvuno"): Xrm.Attribute<string>;
      get(name: "defraimp_name"): Xrm.Attribute<any>;
      get(name: "defraimp_ovname"): Xrm.Attribute<string>;
      get(name: "defraimp_replacedbyid"): Xrm.LookupAttribute<"defraimp_itahc">;
      get(name: "defraimp_replacesid"): Xrm.LookupAttribute<"defraimp_itahc">;
      get(name: "defraimp_tracesreceiveddate"): Xrm.DateAttribute;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_healthcertificatenumber"): Xrm.StringControl;
      get(name: "defraimp_localveterinaryunit"): Xrm.StringControl;
      get(name: "defraimp_lvuno"): Xrm.StringControl;
      get(name: "defraimp_name"): Xrm.Control<Xrm.Attribute<any>>;
      get(name: "defraimp_ovname"): Xrm.StringControl;
      get(name: "defraimp_replacedbyid"): Xrm.LookupControl<"defraimp_itahc">;
      get(name: "defraimp_replacesid"): Xrm.LookupControl<"defraimp_itahc">;
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
  interface ITAHCQuickCreate extends Xrm.PageBase<ITAHCQuickCreate.Attributes,ITAHCQuickCreate.Tabs,ITAHCQuickCreate.Controls> {
    getAttribute(attributeName: "defraimp_healthcertificatenumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_localveterinaryunit"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_lvuno"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<any>;
    getAttribute(attributeName: "defraimp_ovname"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_replacedbyid"): Xrm.LookupAttribute<"defraimp_itahc">;
    getAttribute(attributeName: "defraimp_replacesid"): Xrm.LookupAttribute<"defraimp_itahc">;
    getAttribute(attributeName: "defraimp_tracesreceiveddate"): Xrm.DateAttribute;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_healthcertificatenumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_localveterinaryunit"): Xrm.StringControl;
    getControl(controlName: "defraimp_lvuno"): Xrm.StringControl;
    getControl(controlName: "defraimp_name"): Xrm.Control<Xrm.Attribute<any>>;
    getControl(controlName: "defraimp_ovname"): Xrm.StringControl;
    getControl(controlName: "defraimp_replacedbyid"): Xrm.LookupControl<"defraimp_itahc">;
    getControl(controlName: "defraimp_replacesid"): Xrm.LookupControl<"defraimp_itahc">;
    getControl(controlName: "defraimp_tracesreceiveddate"): Xrm.DateControl;
    getControl(controlName: string): undefined;
  }
}
