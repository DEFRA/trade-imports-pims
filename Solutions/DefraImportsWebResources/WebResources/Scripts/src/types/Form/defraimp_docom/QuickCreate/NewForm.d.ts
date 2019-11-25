declare namespace Form.defraimp_docom.QuickCreate {
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
      get(name: "defraimp_aphaabpapprovalregistrationnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_containernumber"): Xrm.Attribute<string>;
      get(name: "defraimp_dateofdecision"): Xrm.DateAttribute;
      get(name: "defraimp_localreferencenumber"): Xrm.Attribute<string>;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: "defraimp_purpose"): Xrm.OptionSetAttribute<defraimp_docom_defraimp_purpose>;
      get(name: "defraimp_receivingcategory"): Xrm.OptionSetAttribute<defraimp_docom_defraimp_receivingcategory>;
      get(name: "defraimp_sealnumber"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_aphaabpapprovalregistrationnumber"): Xrm.StringControl;
      get(name: "defraimp_containernumber"): Xrm.StringControl;
      get(name: "defraimp_dateofdecision"): Xrm.DateControl;
      get(name: "defraimp_localreferencenumber"): Xrm.StringControl;
      get(name: "defraimp_name"): Xrm.StringControl;
      get(name: "defraimp_purpose"): Xrm.OptionSetControl<defraimp_docom_defraimp_purpose>;
      get(name: "defraimp_receivingcategory"): Xrm.OptionSetControl<defraimp_docom_defraimp_receivingcategory>;
      get(name: "defraimp_sealnumber"): Xrm.StringControl;
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
    getAttribute(attributeName: "defraimp_aphaabpapprovalregistrationnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_containernumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_dateofdecision"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_localreferencenumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_purpose"): Xrm.OptionSetAttribute<defraimp_docom_defraimp_purpose>;
    getAttribute(attributeName: "defraimp_receivingcategory"): Xrm.OptionSetAttribute<defraimp_docom_defraimp_receivingcategory>;
    getAttribute(attributeName: "defraimp_sealnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_aphaabpapprovalregistrationnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_containernumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_dateofdecision"): Xrm.DateControl;
    getControl(controlName: "defraimp_localreferencenumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "defraimp_purpose"): Xrm.OptionSetControl<defraimp_docom_defraimp_purpose>;
    getControl(controlName: "defraimp_receivingcategory"): Xrm.OptionSetControl<defraimp_docom_defraimp_receivingcategory>;
    getControl(controlName: "defraimp_sealnumber"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
