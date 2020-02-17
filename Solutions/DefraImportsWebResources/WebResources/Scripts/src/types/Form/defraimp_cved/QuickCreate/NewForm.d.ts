declare namespace Form.defraimp_cved.QuickCreate {
  namespace NewForm {
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
      get(name: "defraimp_certificatereferencenumber"): Xrm.Attribute<string>;
      get(name: "defraimp_channeledconsignment"): Xrm.OptionSetAttribute<defraimp_cved_defraimp_channeledconsignment>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_certificatereferencenumber"): Xrm.StringControl;
      get(name: "defraimp_channeledconsignment"): Xrm.OptionSetControl<defraimp_cved_defraimp_channeledconsignment>;
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
  interface NewForm extends Xrm.PageBase<NewForm.Attributes,NewForm.Tabs,NewForm.Controls> {
    getAttribute(attributeName: "defraimp_certificatereferencenumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_channeledconsignment"): Xrm.OptionSetAttribute<defraimp_cved_defraimp_channeledconsignment>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_certificatereferencenumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_channeledconsignment"): Xrm.OptionSetControl<defraimp_cved_defraimp_channeledconsignment>;
    getControl(controlName: string): undefined;
  }
}
