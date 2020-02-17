declare namespace Form.defraimp_cved.Main {
  namespace Information {
    namespace Tabs {
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_certificatereferencenumber"): Xrm.Attribute<string>;
      get(name: "defraimp_channeledconsignment"): Xrm.OptionSetAttribute<defraimp_cved_defraimp_channeledconsignment>;
      get(name: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_certificatereferencenumber"): Xrm.StringControl;
      get(name: "defraimp_channeledconsignment"): Xrm.OptionSetControl<defraimp_cved_defraimp_channeledconsignment>;
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
    getAttribute(attributeName: "defraimp_certificatereferencenumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_channeledconsignment"): Xrm.OptionSetAttribute<defraimp_cved_defraimp_channeledconsignment>;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_certificatereferencenumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_channeledconsignment"): Xrm.OptionSetControl<defraimp_cved_defraimp_channeledconsignment>;
    getControl(controlName: "ownerid"): Xrm.LookupControl<"systemuser" | "team">;
    getControl(controlName: string): undefined;
  }
}
