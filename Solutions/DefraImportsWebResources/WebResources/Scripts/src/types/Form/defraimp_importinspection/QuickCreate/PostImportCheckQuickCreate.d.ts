declare namespace Form.defraimp_importinspection.QuickCreate {
  namespace PostImportCheckQuickCreate {
    namespace Tabs {
      interface tab_1 extends Xrm.SectionCollectionBase {
        get(name: "Section2"): Xrm.PageSection;
        get(name: "Section3"): Xrm.PageSection;
        get(name: "tab_1_column_1_section_1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_cphnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_customernumber"): Xrm.Attribute<string>;
      get(name: "defraimp_datevisitallocated"): Xrm.DateAttribute;
      get(name: "defraimp_regionareaallocatedtoid"): Xrm.LookupAttribute<"defraimp_apharegion">;
      get(name: "defraimp_relatedimportapplication"): Xrm.LookupAttribute<"defraimp_importapplication">;
      get(name: "defraimp_relateditahc"): Xrm.LookupAttribute<"defraimp_itahc">;
      get(name: "defraimp_samworkschedulenumber"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_cphnumber"): Xrm.StringControl;
      get(name: "defraimp_customernumber"): Xrm.StringControl;
      get(name: "defraimp_datevisitallocated"): Xrm.DateControl;
      get(name: "defraimp_regionareaallocatedtoid"): Xrm.LookupControl<"defraimp_apharegion">;
      get(name: "defraimp_relatedimportapplication"): Xrm.LookupControl<"defraimp_importapplication">;
      get(name: "defraimp_relateditahc"): Xrm.LookupControl<"defraimp_itahc">;
      get(name: "defraimp_samworkschedulenumber"): Xrm.StringControl;
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
  interface PostImportCheckQuickCreate extends Xrm.PageBase<PostImportCheckQuickCreate.Attributes,PostImportCheckQuickCreate.Tabs,PostImportCheckQuickCreate.Controls> {
    getAttribute(attributeName: "defraimp_cphnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_customernumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_datevisitallocated"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_regionareaallocatedtoid"): Xrm.LookupAttribute<"defraimp_apharegion">;
    getAttribute(attributeName: "defraimp_relatedimportapplication"): Xrm.LookupAttribute<"defraimp_importapplication">;
    getAttribute(attributeName: "defraimp_relateditahc"): Xrm.LookupAttribute<"defraimp_itahc">;
    getAttribute(attributeName: "defraimp_samworkschedulenumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_cphnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_customernumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_datevisitallocated"): Xrm.DateControl;
    getControl(controlName: "defraimp_regionareaallocatedtoid"): Xrm.LookupControl<"defraimp_apharegion">;
    getControl(controlName: "defraimp_relatedimportapplication"): Xrm.LookupControl<"defraimp_importapplication">;
    getControl(controlName: "defraimp_relateditahc"): Xrm.LookupControl<"defraimp_itahc">;
    getControl(controlName: "defraimp_samworkschedulenumber"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
