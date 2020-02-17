declare namespace Form.defraimp_importapplicationbusinessprocessflow.Main {
  namespace Information {
    namespace Tabs {
      interface StageStep17 extends Xrm.SectionCollectionBase {
        get(name: "StageStep17_section1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface StageStep3 extends Xrm.SectionCollectionBase {
        get(name: "StageStep3_section1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface StageStep38 extends Xrm.SectionCollectionBase {
        get(name: "StageStep38_section1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface StageStep47 extends Xrm.SectionCollectionBase {
        get(name: "StageStep47_section1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface StageStep52 extends Xrm.SectionCollectionBase {
        get(name: "StageStep52_section1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_certificatecompliantfirsttime"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defraimp_certificatenoncompliancereason"): Xrm.OptionSetAttribute<number>;
      get(name: "defraimp_certificatenoncompliancereasonother"): Xrm.Attribute<string>;
      get(name: "defraimp_certificateverified"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defraimp_commoditytypeid"): Xrm.LookupAttribute<"defraimp_importapplications">;
      get(name: "defraimp_completionsummary"): Xrm.Attribute<string>;
      get(name: "defraimp_countryoforiginid"): Xrm.LookupAttribute<"defraimp_importapplications">;
      get(name: "defraimp_importrisklevelid"): Xrm.LookupAttribute<"defraimp_importapplications">;
      get(name: "defraimp_inspectiondeclinedreason"): Xrm.Attribute<string>;
      get(name: "defraimp_inspectionrequested"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defraimp_inspectionrequired"): Xrm.OptionSetAttribute<number>;
      get(name: "defraimp_manualpostimportcheckdecision"): Xrm.OptionSetAttribute<number>;
      get(name: "defraimp_movedtocompletiondate"): Xrm.DateAttribute;
      get(name: "defraimp_movetocompletion"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defraimp_primaryitahcid"): Xrm.LookupAttribute<"defraimp_importapplications">;
      get(name: "defraimp_regionareaallocatedtoid"): Xrm.LookupAttribute<"defraimp_importapplications">;
      get(name: "defraimp_resettrustleveltobronze"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defraimp_risklevelverified"): Xrm.OptionSetAttribute<boolean>;
      get(name: "ownerid"): Xrm.LookupAttribute<"defraimp_importapplications">;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_certificatecompliantfirsttime"): Xrm.OptionSetControl<boolean>;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_certificatenoncompliancereason"): Xrm.OptionSetControl<number>;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_certificatenoncompliancereasonother"): Xrm.StringControl;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_certificateverified"): Xrm.OptionSetControl<boolean>;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_commoditytypeid"): Xrm.LookupControl<"defraimp_importapplications">;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_completionsummary"): Xrm.StringControl;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_completionsummary1"): Xrm.StringControl;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_countryoforiginid"): Xrm.LookupControl<"defraimp_importapplications">;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_importrisklevelid"): Xrm.LookupControl<"defraimp_importapplications">;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_inspectiondeclinedreason"): Xrm.StringControl;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_inspectionrequested"): Xrm.OptionSetControl<boolean>;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_inspectionrequired"): Xrm.OptionSetControl<number>;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_manualpostimportcheckdecision"): Xrm.OptionSetControl<number>;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_movedtocompletiondate"): Xrm.DateControl;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_movedtocompletiondate1"): Xrm.DateControl;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_movetocompletion"): Xrm.OptionSetControl<boolean>;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_movetocompletion1"): Xrm.OptionSetControl<boolean>;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_primaryitahcid"): Xrm.LookupControl<"defraimp_importapplications">;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_regionareaallocatedtoid"): Xrm.LookupControl<"defraimp_importapplications">;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_resettrustleveltobronze"): Xrm.OptionSetControl<boolean>;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_risklevelverified"): Xrm.OptionSetControl<boolean>;
      get(name: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:ownerid"): Xrm.LookupControl<"defraimp_importapplications">;
      get(name: string): undefined;
      get(): Xrm.BaseControl[];
      get(index: number): Xrm.BaseControl;
      get(chooser: (item: Xrm.BaseControl, index: number) => boolean): Xrm.BaseControl[];
    }
    interface Tabs extends Xrm.TabCollectionBase {
      get(name: "StageStep17"): Xrm.PageTab<Tabs.StageStep17>;
      get(name: "StageStep3"): Xrm.PageTab<Tabs.StageStep3>;
      get(name: "StageStep38"): Xrm.PageTab<Tabs.StageStep38>;
      get(name: "StageStep47"): Xrm.PageTab<Tabs.StageStep47>;
      get(name: "StageStep52"): Xrm.PageTab<Tabs.StageStep52>;
      get(name: string): undefined;
      get(): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
      get(index: number): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>;
      get(chooser: (item: Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>, index: number) => boolean): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
    }
  }
  interface Information extends Xrm.PageBase<Information.Attributes,Information.Tabs,Information.Controls> {
    getAttribute(attributeName: "defraimp_certificatecompliantfirsttime"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defraimp_certificatenoncompliancereason"): Xrm.OptionSetAttribute<number>;
    getAttribute(attributeName: "defraimp_certificatenoncompliancereasonother"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_certificateverified"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defraimp_commoditytypeid"): Xrm.LookupAttribute<"defraimp_importapplications">;
    getAttribute(attributeName: "defraimp_completionsummary"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_countryoforiginid"): Xrm.LookupAttribute<"defraimp_importapplications">;
    getAttribute(attributeName: "defraimp_importrisklevelid"): Xrm.LookupAttribute<"defraimp_importapplications">;
    getAttribute(attributeName: "defraimp_inspectiondeclinedreason"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_inspectionrequested"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defraimp_inspectionrequired"): Xrm.OptionSetAttribute<number>;
    getAttribute(attributeName: "defraimp_manualpostimportcheckdecision"): Xrm.OptionSetAttribute<number>;
    getAttribute(attributeName: "defraimp_movedtocompletiondate"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_movetocompletion"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defraimp_primaryitahcid"): Xrm.LookupAttribute<"defraimp_importapplications">;
    getAttribute(attributeName: "defraimp_regionareaallocatedtoid"): Xrm.LookupAttribute<"defraimp_importapplications">;
    getAttribute(attributeName: "defraimp_resettrustleveltobronze"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defraimp_risklevelverified"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"defraimp_importapplications">;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_certificatecompliantfirsttime"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_certificatenoncompliancereason"): Xrm.OptionSetControl<number>;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_certificatenoncompliancereasonother"): Xrm.StringControl;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_certificateverified"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_commoditytypeid"): Xrm.LookupControl<"defraimp_importapplications">;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_completionsummary"): Xrm.StringControl;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_completionsummary1"): Xrm.StringControl;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_countryoforiginid"): Xrm.LookupControl<"defraimp_importapplications">;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_importrisklevelid"): Xrm.LookupControl<"defraimp_importapplications">;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_inspectiondeclinedreason"): Xrm.StringControl;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_inspectionrequested"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_inspectionrequired"): Xrm.OptionSetControl<number>;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_manualpostimportcheckdecision"): Xrm.OptionSetControl<number>;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_movedtocompletiondate"): Xrm.DateControl;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_movedtocompletiondate1"): Xrm.DateControl;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_movetocompletion"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_movetocompletion1"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_primaryitahcid"): Xrm.LookupControl<"defraimp_importapplications">;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_regionareaallocatedtoid"): Xrm.LookupControl<"defraimp_importapplications">;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_resettrustleveltobronze"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:defraimp_risklevelverified"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "bpf_defraimp_importapplication_defraimp_importapplicationbusinessprocessflow:ownerid"): Xrm.LookupControl<"defraimp_importapplications">;
    getControl(controlName: string): undefined;
  }
}
