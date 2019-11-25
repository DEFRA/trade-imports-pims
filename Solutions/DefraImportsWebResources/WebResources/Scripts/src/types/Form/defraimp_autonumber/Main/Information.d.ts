declare namespace Form.defraimp_autonumber.Main {
  namespace Information {
    namespace Tabs {
      interface ca95db8d490b439dab124031517575fb extends Xrm.SectionCollectionBase {
        get(name: "{ca95db8d-490b-439d-ab12-4031517575fb}_section_2"): Xrm.PageSection;
        get(name: "{ca95db8d-490b-439d-ab12-4031517575fb}_section_3"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_currentnumber"): Xrm.NumberAttribute;
      get(name: "defraimp_key"): Xrm.Attribute<string>;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_currentnumber"): Xrm.NumberControl;
      get(name: "defraimp_key"): Xrm.StringControl;
      get(name: "defraimp_name"): Xrm.StringControl;
      get(name: string): undefined;
      get(): Xrm.BaseControl[];
      get(index: number): Xrm.BaseControl;
      get(chooser: (item: Xrm.BaseControl, index: number) => boolean): Xrm.BaseControl[];
    }
    interface Tabs extends Xrm.TabCollectionBase {
      get(name: "{ca95db8d-490b-439d-ab12-4031517575fb}"): Xrm.PageTab<Tabs.ca95db8d490b439dab124031517575fb>;
      get(name: string): undefined;
      get(): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
      get(index: number): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>;
      get(chooser: (item: Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>, index: number) => boolean): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
    }
  }
  interface Information extends Xrm.PageBase<Information.Attributes,Information.Tabs,Information.Controls> {
    getAttribute(attributeName: "defraimp_currentnumber"): Xrm.NumberAttribute;
    getAttribute(attributeName: "defraimp_key"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_currentnumber"): Xrm.NumberControl;
    getControl(controlName: "defraimp_key"): Xrm.StringControl;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
