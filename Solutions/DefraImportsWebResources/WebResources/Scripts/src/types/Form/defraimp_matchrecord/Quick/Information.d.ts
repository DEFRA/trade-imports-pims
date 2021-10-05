declare namespace Form.defraimp_matchrecord.Quick {
  namespace Information {
    namespace Tabs {
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_appendrecordstoimportrecords"): Xrm.OptionSetAttribute<boolean> | null;
      get(name: "defraimp_closerecordascompleted"): Xrm.OptionSetAttribute<boolean> | null;
      get(name: "defraimp_closerecordasrejected"): Xrm.OptionSetAttribute<boolean> | null;
      get(name: "defraimp_isrecordvalidmatch"): Xrm.OptionSetAttribute<boolean> | null;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: "defraimp_rejectedreason"): Xrm.Attribute<string> | null;
      get(name: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_name"): Xrm.StringControl;
      get(name: "header_process_defraimp_appendrecordstoimportrecords"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_appendrecordstoimportrecords_1"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_appendrecordstoimportrecords_2"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_appendrecordstoimportrecords_3"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_appendrecordstoimportrecords_4"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordascompleted"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordascompleted_1"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordascompleted_2"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordascompleted_3"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordascompleted_4"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordasrejected"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordasrejected_1"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordasrejected_2"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordasrejected_3"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordasrejected_4"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_isrecordvalidmatch"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_isrecordvalidmatch_1"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_isrecordvalidmatch_2"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_isrecordvalidmatch_3"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_isrecordvalidmatch_4"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_rejectedreason"): Xrm.StringControl | null;
      get(name: "header_process_defraimp_rejectedreason_1"): Xrm.StringControl | null;
      get(name: "header_process_defraimp_rejectedreason_2"): Xrm.StringControl | null;
      get(name: "header_process_defraimp_rejectedreason_3"): Xrm.StringControl | null;
      get(name: "header_process_defraimp_rejectedreason_4"): Xrm.StringControl | null;
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
    getAttribute(attributeName: "defraimp_appendrecordstoimportrecords"): Xrm.OptionSetAttribute<boolean> | null;
    getAttribute(attributeName: "defraimp_closerecordascompleted"): Xrm.OptionSetAttribute<boolean> | null;
    getAttribute(attributeName: "defraimp_closerecordasrejected"): Xrm.OptionSetAttribute<boolean> | null;
    getAttribute(attributeName: "defraimp_isrecordvalidmatch"): Xrm.OptionSetAttribute<boolean> | null;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_rejectedreason"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "header_process_defraimp_appendrecordstoimportrecords"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_appendrecordstoimportrecords_1"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_appendrecordstoimportrecords_2"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_appendrecordstoimportrecords_3"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_appendrecordstoimportrecords_4"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordascompleted"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordascompleted_1"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordascompleted_2"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordascompleted_3"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordascompleted_4"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordasrejected"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordasrejected_1"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordasrejected_2"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordasrejected_3"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordasrejected_4"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_isrecordvalidmatch"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_isrecordvalidmatch_1"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_isrecordvalidmatch_2"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_isrecordvalidmatch_3"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_isrecordvalidmatch_4"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_rejectedreason"): Xrm.StringControl | null;
    getControl(controlName: "header_process_defraimp_rejectedreason_1"): Xrm.StringControl | null;
    getControl(controlName: "header_process_defraimp_rejectedreason_2"): Xrm.StringControl | null;
    getControl(controlName: "header_process_defraimp_rejectedreason_3"): Xrm.StringControl | null;
    getControl(controlName: "header_process_defraimp_rejectedreason_4"): Xrm.StringControl | null;
    getControl(controlName: "ownerid"): Xrm.LookupControl<"systemuser" | "team">;
    getControl(controlName: string): undefined;
  }
}
