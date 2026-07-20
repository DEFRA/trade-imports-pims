declare namespace Form.defraimp_placeoforigin.Main {
  namespace Information {
    namespace Tabs {
      interface _70c243d22d164cf5a82fc347d999c6f5 extends Xrm.SectionCollectionBase {
        get(name: "{230c83f6-cca9-4f9c-97ab-928c2dd037cb}"): Xrm.PageSection;
        get(name: "{70c243d2-2d16-4cf5-a82f-c347d999c6f5}_section_2"): Xrm.PageSection;
        get(name: "{70c243d2-2d16-4cf5-a82f-c347d999c6f5}_section_3"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface tab_2 extends Xrm.SectionCollectionBase {
        get(name: "tab_2_section_1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
      interface tab_3 extends Xrm.SectionCollectionBase {
        get(name: "tab_3_section_1"): Xrm.PageSection;
        get(name: string): undefined;
        get(): Xrm.PageSection[];
        get(index: number): Xrm.PageSection;
        get(chooser: (item: Xrm.PageSection, index: number) => boolean): Xrm.PageSection[];
      }
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_addresscity"): Xrm.Attribute<string>;
      get(name: "defraimp_addresscountry"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_addressline1"): Xrm.Attribute<string>;
      get(name: "defraimp_addressline2"): Xrm.Attribute<string>;
      get(name: "defraimp_addressline3"): Xrm.Attribute<string>;
      get(name: "defraimp_addressstateorprovince"): Xrm.Attribute<string>;
      get(name: "defraimp_applicationcounter"): Xrm.NumberAttribute;
      get(name: "defraimp_datelockedtobronze"): Xrm.DateAttribute;
      get(name: "defraimp_datesettogold"): Xrm.DateAttribute;
      get(name: "defraimp_dateunlockedfrombronze"): Xrm.DateAttribute;
      get(name: "defraimp_inspectionquotacounter"): Xrm.NumberAttribute;
      get(name: "defraimp_locktobronze"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: "defraimp_numberofapplications"): Xrm.NumberAttribute;
      get(name: "defraimp_numberofapplicationssincelastinspection"): Xrm.NumberAttribute;
      get(name: "defraimp_numberofsuccessfulapplications"): Xrm.NumberAttribute;
      get(name: "defraimp_postcode"): Xrm.Attribute<string>;
      get(name: "defraimp_previoustrustlevel"): Xrm.OptionSetAttribute<defraimp_trustlevel>;
      get(name: "defraimp_reasonlockedtobronze"): Xrm.Attribute<string>;
      get(name: "defraimp_reasonunlockedfrombronze"): Xrm.Attribute<string>;
      get(name: "defraimp_trustlevel"): Xrm.OptionSetAttribute<defraimp_trustlevel>;
      get(name: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "RelatedImportRecords"): Xrm.SubGridControl<"defraimp_importapplication">;
      get(name: "defraimp_addresscity"): Xrm.StringControl;
      get(name: "defraimp_addresscountry"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_addressline1"): Xrm.StringControl;
      get(name: "defraimp_addressline2"): Xrm.StringControl;
      get(name: "defraimp_addressline3"): Xrm.StringControl;
      get(name: "defraimp_addressstateorprovince"): Xrm.StringControl;
      get(name: "defraimp_applicationcounter"): Xrm.NumberControl;
      get(name: "defraimp_datelockedtobronze"): Xrm.DateControl;
      get(name: "defraimp_datesettogold"): Xrm.DateControl;
      get(name: "defraimp_dateunlockedfrombronze"): Xrm.DateControl;
      get(name: "defraimp_inspectionquotacounter"): Xrm.NumberControl;
      get(name: "defraimp_locktobronze"): Xrm.OptionSetControl<boolean>;
      get(name: "defraimp_name"): Xrm.StringControl;
      get(name: "defraimp_numberofapplications"): Xrm.NumberControl;
      get(name: "defraimp_numberofapplicationssincelastinspection"): Xrm.NumberControl;
      get(name: "defraimp_numberofsuccessfulapplications"): Xrm.NumberControl;
      get(name: "defraimp_postcode"): Xrm.StringControl;
      get(name: "defraimp_previoustrustlevel"): Xrm.OptionSetControl<defraimp_trustlevel>;
      get(name: "defraimp_reasonlockedtobronze"): Xrm.StringControl;
      get(name: "defraimp_reasonunlockedfrombronze"): Xrm.StringControl;
      get(name: "defraimp_trustlevel"): Xrm.OptionSetControl<defraimp_trustlevel>;
      get(name: "header_defraimp_trustlevel"): Xrm.OptionSetControl<defraimp_trustlevel>;
      get(name: "header_ownerid"): Xrm.LookupControl<"systemuser" | "team">;
      get(name: string): undefined;
      get(): Xrm.BaseControl[];
      get(index: number): Xrm.BaseControl;
      get(chooser: (item: Xrm.BaseControl, index: number) => boolean): Xrm.BaseControl[];
    }
    interface Tabs extends Xrm.TabCollectionBase {
      get(name: "{70c243d2-2d16-4cf5-a82f-c347d999c6f5}"): Xrm.PageTab<Tabs._70c243d22d164cf5a82fc347d999c6f5>;
      get(name: "tab_2"): Xrm.PageTab<Tabs.tab_2>;
      get(name: "tab_3"): Xrm.PageTab<Tabs.tab_3>;
      get(name: string): undefined;
      get(): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
      get(index: number): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>;
      get(chooser: (item: Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>, index: number) => boolean): Xrm.PageTab<Xrm.Collection<Xrm.PageSection>>[];
    }
  }
  interface Information extends Xrm.PageBase<Information.Attributes,Information.Tabs,Information.Controls> {
    getAttribute(attributeName: "defraimp_addresscity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_addresscountry"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_addressline1"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_addressline2"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_addressline3"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_addressstateorprovince"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_applicationcounter"): Xrm.NumberAttribute;
    getAttribute(attributeName: "defraimp_datelockedtobronze"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_datesettogold"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_dateunlockedfrombronze"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_inspectionquotacounter"): Xrm.NumberAttribute;
    getAttribute(attributeName: "defraimp_locktobronze"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_numberofapplications"): Xrm.NumberAttribute;
    getAttribute(attributeName: "defraimp_numberofapplicationssincelastinspection"): Xrm.NumberAttribute;
    getAttribute(attributeName: "defraimp_numberofsuccessfulapplications"): Xrm.NumberAttribute;
    getAttribute(attributeName: "defraimp_postcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_previoustrustlevel"): Xrm.OptionSetAttribute<defraimp_trustlevel>;
    getAttribute(attributeName: "defraimp_reasonlockedtobronze"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_reasonunlockedfrombronze"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_trustlevel"): Xrm.OptionSetAttribute<defraimp_trustlevel>;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "RelatedImportRecords"): Xrm.SubGridControl<"defraimp_importapplication">;
    getControl(controlName: "defraimp_addresscity"): Xrm.StringControl;
    getControl(controlName: "defraimp_addresscountry"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_addressline1"): Xrm.StringControl;
    getControl(controlName: "defraimp_addressline2"): Xrm.StringControl;
    getControl(controlName: "defraimp_addressline3"): Xrm.StringControl;
    getControl(controlName: "defraimp_addressstateorprovince"): Xrm.StringControl;
    getControl(controlName: "defraimp_applicationcounter"): Xrm.NumberControl;
    getControl(controlName: "defraimp_datelockedtobronze"): Xrm.DateControl;
    getControl(controlName: "defraimp_datesettogold"): Xrm.DateControl;
    getControl(controlName: "defraimp_dateunlockedfrombronze"): Xrm.DateControl;
    getControl(controlName: "defraimp_inspectionquotacounter"): Xrm.NumberControl;
    getControl(controlName: "defraimp_locktobronze"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "defraimp_numberofapplications"): Xrm.NumberControl;
    getControl(controlName: "defraimp_numberofapplicationssincelastinspection"): Xrm.NumberControl;
    getControl(controlName: "defraimp_numberofsuccessfulapplications"): Xrm.NumberControl;
    getControl(controlName: "defraimp_postcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_previoustrustlevel"): Xrm.OptionSetControl<defraimp_trustlevel>;
    getControl(controlName: "defraimp_reasonlockedtobronze"): Xrm.StringControl;
    getControl(controlName: "defraimp_reasonunlockedfrombronze"): Xrm.StringControl;
    getControl(controlName: "defraimp_trustlevel"): Xrm.OptionSetControl<defraimp_trustlevel>;
    getControl(controlName: "header_defraimp_trustlevel"): Xrm.OptionSetControl<defraimp_trustlevel>;
    getControl(controlName: "header_ownerid"): Xrm.LookupControl<"systemuser" | "team">;
    getControl(controlName: string): undefined;
  }
}
