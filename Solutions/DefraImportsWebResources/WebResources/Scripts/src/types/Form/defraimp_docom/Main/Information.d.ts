declare namespace Form.defraimp_docom.Main {
  namespace Information {
    namespace Tabs {
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
      get(name: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
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
      get(name: "ownerid"): Xrm.LookupControl<"systemuser" | "team">;
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
    getAttribute(attributeName: "defraimp_aphaabpapprovalregistrationnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_containernumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_dateofdecision"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_localreferencenumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_purpose"): Xrm.OptionSetAttribute<defraimp_docom_defraimp_purpose>;
    getAttribute(attributeName: "defraimp_receivingcategory"): Xrm.OptionSetAttribute<defraimp_docom_defraimp_receivingcategory>;
    getAttribute(attributeName: "defraimp_sealnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_aphaabpapprovalregistrationnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_containernumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_dateofdecision"): Xrm.DateControl;
    getControl(controlName: "defraimp_localreferencenumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "defraimp_purpose"): Xrm.OptionSetControl<defraimp_docom_defraimp_purpose>;
    getControl(controlName: "defraimp_receivingcategory"): Xrm.OptionSetControl<defraimp_docom_defraimp_receivingcategory>;
    getControl(controlName: "defraimp_sealnumber"): Xrm.StringControl;
    getControl(controlName: "ownerid"): Xrm.LookupControl<"systemuser" | "team">;
    getControl(controlName: string): undefined;
  }
}
