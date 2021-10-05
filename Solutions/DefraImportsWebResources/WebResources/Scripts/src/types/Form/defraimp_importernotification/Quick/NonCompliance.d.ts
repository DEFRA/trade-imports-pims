declare namespace Form.defraimp_importernotification.Quick {
  namespace NonCompliance {
    namespace Tabs {
      interface tab_1 extends Xrm.SectionCollectionBase {
        get(name: "tab_1_column_1_section_1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_contactedduetononcompliance"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defraimp_datecompleted"): Xrm.DateAttribute;
      get(name: "defraimp_dateemailsent"): Xrm.DateAttribute;
      get(name: "defraimp_datetelephonecallmade"): Xrm.DateAttribute;
      get(name: "defraimp_irmspersonresponsible"): Xrm.LookupAttribute<"systemuser">;
      get(name: "defraimp_noncomplianceothercomments"): Xrm.Attribute<string>;
      get(name: "defraimp_noncompliancestatus"): Xrm.OptionSetAttribute<defraimp_noncompliancestatus>;
      get(name: "defraimp_pimsstatus"): Xrm.OptionSetAttribute<defraimp_pimsstatus>;
      get(name: "defraimp_typeofnoncompliance"): Xrm.OptionSetAttribute<defraimp_noncompliancetypenotification>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_contactedduetononcompliance"): Xrm.OptionSetControl<boolean>;
      get(name: "defraimp_datecompleted"): Xrm.DateControl;
      get(name: "defraimp_dateemailsent"): Xrm.DateControl;
      get(name: "defraimp_datetelephonecallmade"): Xrm.DateControl;
      get(name: "defraimp_irmspersonresponsible"): Xrm.LookupControl<"systemuser">;
      get(name: "defraimp_noncomplianceothercomments"): Xrm.StringControl;
      get(name: "defraimp_noncompliancestatus"): Xrm.OptionSetControl<defraimp_noncompliancestatus>;
      get(name: "defraimp_pimsstatus"): Xrm.OptionSetControl<defraimp_pimsstatus>;
      get(name: "defraimp_typeofnoncompliance"): Xrm.OptionSetControl<defraimp_noncompliancetypenotification>;
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
  interface NonCompliance extends Xrm.PageBase<NonCompliance.Attributes,NonCompliance.Tabs,NonCompliance.Controls> {
    getAttribute(attributeName: "defraimp_contactedduetononcompliance"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defraimp_datecompleted"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_dateemailsent"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_datetelephonecallmade"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_irmspersonresponsible"): Xrm.LookupAttribute<"systemuser">;
    getAttribute(attributeName: "defraimp_noncomplianceothercomments"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_noncompliancestatus"): Xrm.OptionSetAttribute<defraimp_noncompliancestatus>;
    getAttribute(attributeName: "defraimp_pimsstatus"): Xrm.OptionSetAttribute<defraimp_pimsstatus>;
    getAttribute(attributeName: "defraimp_typeofnoncompliance"): Xrm.OptionSetAttribute<defraimp_noncompliancetypenotification>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_contactedduetononcompliance"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "defraimp_datecompleted"): Xrm.DateControl;
    getControl(controlName: "defraimp_dateemailsent"): Xrm.DateControl;
    getControl(controlName: "defraimp_datetelephonecallmade"): Xrm.DateControl;
    getControl(controlName: "defraimp_irmspersonresponsible"): Xrm.LookupControl<"systemuser">;
    getControl(controlName: "defraimp_noncomplianceothercomments"): Xrm.StringControl;
    getControl(controlName: "defraimp_noncompliancestatus"): Xrm.OptionSetControl<defraimp_noncompliancestatus>;
    getControl(controlName: "defraimp_pimsstatus"): Xrm.OptionSetControl<defraimp_pimsstatus>;
    getControl(controlName: "defraimp_typeofnoncompliance"): Xrm.OptionSetControl<defraimp_noncompliancetypenotification>;
    getControl(controlName: string): undefined;
  }
}
