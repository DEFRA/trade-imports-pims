declare namespace Form.defraimp_matchrecord.Main {
  namespace Information {
    namespace Tabs {
      interface _617b6bdb417c4095a2426e77ff7946ca extends Xrm.SectionCollectionBase {
        get(name: "Match_rating "): Xrm.PageSection;
        get(name: "_section_350"): Xrm.PageSection;
        get(name: "_section_800"): Xrm.PageSection;
        get(name: "{617b6bdb-417c-4095-a242-6e77ff7946ca}_section_5"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface tab_2 extends Xrm.SectionCollectionBase {
        get(name: "tab_2_section_1"): Xrm.PageSection;
        get(name: "tab_2_section_2"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface tab_4 extends Xrm.SectionCollectionBase {
        get(name: "tab_4_section_1"): Xrm.PageSection;
        get(name: "tab_4_section_2"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface tab_5 extends Xrm.SectionCollectionBase {
        get(name: "tab_5_section_1"): Xrm.PageSection;
        get(name: "tab_5_section_2"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface tab_6 extends Xrm.SectionCollectionBase {
        get(name: "tab_6_section_1"): Xrm.PageSection;
        get(name: "tab_6_section_2"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface tab_7 extends Xrm.SectionCollectionBase {
        get(name: "tab_7_section_1"): Xrm.PageSection;
        get(name: "tab_7_section_2"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface tab_8 extends Xrm.SectionCollectionBase {
        get(name: "tab_8_section_1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface tab_9 extends Xrm.SectionCollectionBase {
        get(name: "tab_9_section_1"): Xrm.PageSection;
        get(name: "tab_9_section_2"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_commoditycodematchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_countryoforiginmatchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_dateofimportmatchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_destinationpostcode"): Xrm.Attribute<string>;
      get(name: "defraimp_destinationpostcodematchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_importernotification"): Xrm.Attribute<any>;
      get(name: "defraimp_importrecord"): Xrm.LookupAttribute<"defraimp_importapplication">;
      get(name: "defraimp_itahc"): Xrm.Attribute<any>;
      get(name: "defraimp_itahcnumbermatchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: "defraimp_organisationnamematchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_quantitymatchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_speciesmatchrating"): Xrm.Attribute<string>;
      get(name: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
      get(name: "statecode"): Xrm.OptionSetAttribute<defraimp_matchrecord_statecode>;
      get(name: "statuscode"): Xrm.OptionSetAttribute<defraimp_matchrecord_statuscode>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "RelatedImportRecords"): Xrm.BaseControl;
      get(name: "defraimp_commoditycodematchrating"): Xrm.StringControl;
      get(name: "defraimp_countryoforiginmatchrating"): Xrm.StringControl;
      get(name: "defraimp_dateofimportmatchrating"): Xrm.StringControl;
      get(name: "defraimp_destinationpostcodematchrating"): Xrm.StringControl;
      get(name: "defraimp_itahcnumbermatchrating"): Xrm.StringControl;
      get(name: "defraimp_name"): Xrm.StringControl;
      get(name: "defraimp_organisationnamematchrating"): Xrm.StringControl;
      get(name: "defraimp_quantitymatchrating"): Xrm.StringControl;
      get(name: "defraimp_speciesmatchrating"): Xrm.StringControl;
      get(name: "header_process_defraimp_destinationpostcode"): Xrm.StringControl | null;
      get(name: "header_process_defraimp_importrecord"): Xrm.LookupControl<"defraimp_importapplication"> | null;
      get(name: "header_statecode"): Xrm.OptionSetControl<defraimp_matchrecord_statecode>;
      get(name: "header_statuscode"): Xrm.OptionSetControl<defraimp_matchrecord_statuscode>;
      get(name: "ownerid"): Xrm.LookupControl<"systemuser" | "team">;
      get(name: string): undefined;
      get(): Xrm.BaseControl[];
      get(index: number): Xrm.BaseControl;
      get(chooser: (item: Xrm.BaseControl, index: number) => boolean): Xrm.BaseControl[];
    }
    interface Tabs extends Xrm.TabCollectionBase {
      get(name: "{617b6bdb-417c-4095-a242-6e77ff7946ca}"): Xrm.PageTab<Tabs._617b6bdb417c4095a2426e77ff7946ca>;
      get(name: "tab_2"): Xrm.PageTab<Tabs.tab_2>;
      get(name: "tab_4"): Xrm.PageTab<Tabs.tab_4>;
      get(name: "tab_5"): Xrm.PageTab<Tabs.tab_5>;
      get(name: "tab_6"): Xrm.PageTab<Tabs.tab_6>;
      get(name: "tab_7"): Xrm.PageTab<Tabs.tab_7>;
      get(name: "tab_8"): Xrm.PageTab<Tabs.tab_8>;
      get(name: "tab_9"): Xrm.PageTab<Tabs.tab_9>;
      get(name: string): undefined;
      get(): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
      get(index: number): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>;
      get(chooser: (item: Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>, index: number) => boolean): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
    }
  }
  interface Information extends Xrm.PageBase<Information.Attributes,Information.Tabs,Information.Controls> {
    getAttribute(attributeName: "defraimp_commoditycodematchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_countryoforiginmatchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_dateofimportmatchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_destinationpostcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_destinationpostcodematchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_importernotification"): Xrm.Attribute<any>;
    getAttribute(attributeName: "defraimp_importrecord"): Xrm.LookupAttribute<"defraimp_importapplication">;
    getAttribute(attributeName: "defraimp_itahc"): Xrm.Attribute<any>;
    getAttribute(attributeName: "defraimp_itahcnumbermatchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_organisationnamematchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_quantitymatchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_speciesmatchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
    getAttribute(attributeName: "statecode"): Xrm.OptionSetAttribute<defraimp_matchrecord_statecode>;
    getAttribute(attributeName: "statuscode"): Xrm.OptionSetAttribute<defraimp_matchrecord_statuscode>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "RelatedImportRecords"): Xrm.BaseControl;
    getControl(controlName: "defraimp_commoditycodematchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_countryoforiginmatchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_dateofimportmatchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_destinationpostcodematchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_itahcnumbermatchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "defraimp_organisationnamematchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_quantitymatchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_speciesmatchrating"): Xrm.StringControl;
    getControl(controlName: "header_process_defraimp_destinationpostcode"): Xrm.StringControl | null;
    getControl(controlName: "header_process_defraimp_importrecord"): Xrm.LookupControl<"defraimp_importapplication"> | null;
    getControl(controlName: "header_statecode"): Xrm.OptionSetControl<defraimp_matchrecord_statecode>;
    getControl(controlName: "header_statuscode"): Xrm.OptionSetControl<defraimp_matchrecord_statuscode>;
    getControl(controlName: "ownerid"): Xrm.LookupControl<"systemuser" | "team">;
    getControl(controlName: string): undefined;
  }
}
