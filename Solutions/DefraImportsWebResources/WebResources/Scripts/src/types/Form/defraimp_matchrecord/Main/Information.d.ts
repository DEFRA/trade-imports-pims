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
      interface tab_10 extends Xrm.SectionCollectionBase {
        get(name: "tab_10_section_1"): Xrm.PageSection;
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
        get(name: "tab_8_section_2"): Xrm.PageSection;
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
      get(name: "defraimp_appendrecordstoimportrecords"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defraimp_closerecordascompleted"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defraimp_closerecordasrejected"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defraimp_commoditycodematchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_copyconsigneefrom"): Xrm.Attribute<any>;
      get(name: "defraimp_copyconsignorfrom"): Xrm.Attribute<any>;
      get(name: "defraimp_copykeydetailsfrom"): Xrm.Attribute<any>;
      get(name: "defraimp_copyplaceofdestinationfrom"): Xrm.Attribute<any>;
      get(name: "defraimp_copyplaceoforiginfrom"): Xrm.Attribute<any>;
      get(name: "defraimp_copytransporterfrom"): Xrm.Attribute<any>;
      get(name: "defraimp_countryoforiginmatchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_dateofimportmatchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_destinationpostcodematchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_importernotification"): Xrm.LookupAttribute<"defraimp_importernotification">;
      get(name: "defraimp_isrecordvalidmatch"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defraimp_itahc"): Xrm.LookupAttribute<"defraimp_itahc">;
      get(name: "defraimp_itahcnumbermatchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: "defraimp_organisationnamematchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_overallmatchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_overwriteexistingfieldsonimportrecord"): Xrm.Attribute<any>;
      get(name: "defraimp_placeoforiginnamematchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginpostcodematchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_quantitymatchrating"): Xrm.Attribute<string>;
      get(name: "defraimp_rejectedreason"): Xrm.Attribute<string>;
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
      get(name: "RelatedImportRecords"): Xrm.SubGridControl<"defraimp_importapplication">;
      get(name: "defraimp_commoditycodematchrating"): Xrm.StringControl;
      get(name: "defraimp_copyconsigneefrom"): Xrm.Control<Xrm.Attribute<any>>;
      get(name: "defraimp_copyconsignorfrom"): Xrm.Control<Xrm.Attribute<any>>;
      get(name: "defraimp_copykeydetailsfrom"): Xrm.Control<Xrm.Attribute<any>>;
      get(name: "defraimp_copyplaceofdestinationfrom"): Xrm.Control<Xrm.Attribute<any>>;
      get(name: "defraimp_copyplaceoforiginfrom"): Xrm.Control<Xrm.Attribute<any>>;
      get(name: "defraimp_copytransporterfrom"): Xrm.Control<Xrm.Attribute<any>>;
      get(name: "defraimp_countryoforiginmatchrating"): Xrm.StringControl;
      get(name: "defraimp_dateofimportmatchrating"): Xrm.StringControl;
      get(name: "defraimp_destinationpostcodematchrating"): Xrm.StringControl;
      get(name: "defraimp_importernotification"): Xrm.LookupControl<"defraimp_importernotification">;
      get(name: "defraimp_itahc"): Xrm.LookupControl<"defraimp_itahc">;
      get(name: "defraimp_itahcnumbermatchrating"): Xrm.StringControl;
      get(name: "defraimp_name"): Xrm.StringControl;
      get(name: "defraimp_organisationnamematchrating"): Xrm.StringControl;
      get(name: "defraimp_overwriteexistingfieldsonimportrecord"): Xrm.Control<Xrm.Attribute<any>>;
      get(name: "defraimp_placeoforiginnamematchrating"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginpostcodematchrating"): Xrm.StringControl;
      get(name: "defraimp_quantitymatchrating"): Xrm.StringControl;
      get(name: "defraimp_speciesmatchrating"): Xrm.StringControl;
      get(name: "header_defraimp_overallmatchrating"): Xrm.StringControl;
      get(name: "header_process_defraimp_appendrecordstoimportrecords"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_appendrecordstoimportrecords1"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_appendrecordstoimportrecords2"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_appendrecordstoimportrecords3"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_appendrecordstoimportrecords4"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordascompleted"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordascompleted1"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordascompleted2"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordascompleted3"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordascompleted4"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordasrejected"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordasrejected1"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordasrejected2"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordasrejected3"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_closerecordasrejected4"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_isrecordvalidmatch"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_isrecordvalidmatch1"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_isrecordvalidmatch2"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_isrecordvalidmatch3"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_isrecordvalidmatch4"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_rejectedreason"): Xrm.StringControl | null;
      get(name: "header_process_defraimp_rejectedreason1"): Xrm.StringControl | null;
      get(name: "header_process_defraimp_rejectedreason2"): Xrm.StringControl | null;
      get(name: "header_process_defraimp_rejectedreason3"): Xrm.StringControl | null;
      get(name: "header_process_defraimp_rejectedreason4"): Xrm.StringControl | null;
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
      get(name: "tab_10"): Xrm.PageTab<Tabs.tab_10>;
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
    getAttribute(attributeName: "defraimp_appendrecordstoimportrecords"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defraimp_closerecordascompleted"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defraimp_closerecordasrejected"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defraimp_commoditycodematchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_copyconsigneefrom"): Xrm.Attribute<any>;
    getAttribute(attributeName: "defraimp_copyconsignorfrom"): Xrm.Attribute<any>;
    getAttribute(attributeName: "defraimp_copykeydetailsfrom"): Xrm.Attribute<any>;
    getAttribute(attributeName: "defraimp_copyplaceofdestinationfrom"): Xrm.Attribute<any>;
    getAttribute(attributeName: "defraimp_copyplaceoforiginfrom"): Xrm.Attribute<any>;
    getAttribute(attributeName: "defraimp_copytransporterfrom"): Xrm.Attribute<any>;
    getAttribute(attributeName: "defraimp_countryoforiginmatchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_dateofimportmatchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_destinationpostcodematchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_importernotification"): Xrm.LookupAttribute<"defraimp_importernotification">;
    getAttribute(attributeName: "defraimp_isrecordvalidmatch"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defraimp_itahc"): Xrm.LookupAttribute<"defraimp_itahc">;
    getAttribute(attributeName: "defraimp_itahcnumbermatchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_organisationnamematchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_overallmatchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_overwriteexistingfieldsonimportrecord"): Xrm.Attribute<any>;
    getAttribute(attributeName: "defraimp_placeoforiginnamematchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginpostcodematchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_quantitymatchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_rejectedreason"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_speciesmatchrating"): Xrm.Attribute<string>;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
    getAttribute(attributeName: "statecode"): Xrm.OptionSetAttribute<defraimp_matchrecord_statecode>;
    getAttribute(attributeName: "statuscode"): Xrm.OptionSetAttribute<defraimp_matchrecord_statuscode>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "RelatedImportRecords"): Xrm.SubGridControl<"defraimp_importapplication">;
    getControl(controlName: "defraimp_commoditycodematchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_copyconsigneefrom"): Xrm.Control<Xrm.Attribute<any>>;
    getControl(controlName: "defraimp_copyconsignorfrom"): Xrm.Control<Xrm.Attribute<any>>;
    getControl(controlName: "defraimp_copykeydetailsfrom"): Xrm.Control<Xrm.Attribute<any>>;
    getControl(controlName: "defraimp_copyplaceofdestinationfrom"): Xrm.Control<Xrm.Attribute<any>>;
    getControl(controlName: "defraimp_copyplaceoforiginfrom"): Xrm.Control<Xrm.Attribute<any>>;
    getControl(controlName: "defraimp_copytransporterfrom"): Xrm.Control<Xrm.Attribute<any>>;
    getControl(controlName: "defraimp_countryoforiginmatchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_dateofimportmatchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_destinationpostcodematchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_importernotification"): Xrm.LookupControl<"defraimp_importernotification">;
    getControl(controlName: "defraimp_itahc"): Xrm.LookupControl<"defraimp_itahc">;
    getControl(controlName: "defraimp_itahcnumbermatchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "defraimp_organisationnamematchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_overwriteexistingfieldsonimportrecord"): Xrm.Control<Xrm.Attribute<any>>;
    getControl(controlName: "defraimp_placeoforiginnamematchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginpostcodematchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_quantitymatchrating"): Xrm.StringControl;
    getControl(controlName: "defraimp_speciesmatchrating"): Xrm.StringControl;
    getControl(controlName: "header_defraimp_overallmatchrating"): Xrm.StringControl;
    getControl(controlName: "header_process_defraimp_appendrecordstoimportrecords"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_appendrecordstoimportrecords1"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_appendrecordstoimportrecords2"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_appendrecordstoimportrecords3"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_appendrecordstoimportrecords4"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordascompleted"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordascompleted1"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordascompleted2"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordascompleted3"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordascompleted4"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordasrejected"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordasrejected1"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordasrejected2"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordasrejected3"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_closerecordasrejected4"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_isrecordvalidmatch"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_isrecordvalidmatch1"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_isrecordvalidmatch2"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_isrecordvalidmatch3"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_isrecordvalidmatch4"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_rejectedreason"): Xrm.StringControl | null;
    getControl(controlName: "header_process_defraimp_rejectedreason1"): Xrm.StringControl | null;
    getControl(controlName: "header_process_defraimp_rejectedreason2"): Xrm.StringControl | null;
    getControl(controlName: "header_process_defraimp_rejectedreason3"): Xrm.StringControl | null;
    getControl(controlName: "header_process_defraimp_rejectedreason4"): Xrm.StringControl | null;
    getControl(controlName: "header_statecode"): Xrm.OptionSetControl<defraimp_matchrecord_statecode>;
    getControl(controlName: "header_statuscode"): Xrm.OptionSetControl<defraimp_matchrecord_statuscode>;
    getControl(controlName: "ownerid"): Xrm.LookupControl<"systemuser" | "team">;
    getControl(controlName: string): undefined;
  }
}
