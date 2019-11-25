declare namespace Form.defraimp_importnotification.QuickCreate {
  namespace QuickCreateImportNotification {
    namespace Tabs {
      interface tab_1 extends Xrm.SectionCollectionBase {
        get(name: "tab_1_column_1_section_1"): Xrm.PageSection;
        get(name: "tab_1_column_2_section_1"): Xrm.PageSection;
        get(name: "tab_1_column_3_section_1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_datenotificationreceived"): Xrm.DateAttribute;
      get(name: "defraimp_ins"): Xrm.Attribute<string>;
      get(name: "defraimp_notificationreceivedintimeframe"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defraimp_referencenumber"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_datenotificationreceived"): Xrm.DateControl;
      get(name: "defraimp_ins"): Xrm.StringControl;
      get(name: "defraimp_notificationreceivedintimeframe"): Xrm.OptionSetControl<boolean>;
      get(name: "defraimp_referencenumber"): Xrm.StringControl;
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
  interface QuickCreateImportNotification extends Xrm.PageBase<QuickCreateImportNotification.Attributes,QuickCreateImportNotification.Tabs,QuickCreateImportNotification.Controls> {
    getAttribute(attributeName: "defraimp_datenotificationreceived"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_ins"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_notificationreceivedintimeframe"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defraimp_referencenumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_datenotificationreceived"): Xrm.DateControl;
    getControl(controlName: "defraimp_ins"): Xrm.StringControl;
    getControl(controlName: "defraimp_notificationreceivedintimeframe"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "defraimp_referencenumber"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
