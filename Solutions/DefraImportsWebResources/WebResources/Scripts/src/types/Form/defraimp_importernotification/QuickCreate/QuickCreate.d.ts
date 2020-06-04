declare namespace Form.defraimp_importernotification.QuickCreate {
  namespace QuickCreate {
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
      get(name: "defraimp_commoditiesregionoforigin"): Xrm.Attribute<string>;
      get(name: "defraimp_countryoforiginid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_departuredate"): Xrm.DateAttribute;
      get(name: "defraimp_estimatedjourneytimeinminutes"): Xrm.NumberAttribute;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: "defraimp_personresponsibleaddress"): Xrm.Attribute<string>;
      get(name: "defraimp_personresponsiblecity"): Xrm.Attribute<string>;
      get(name: "defraimp_personresponsiblecompanyname"): Xrm.Attribute<string>;
      get(name: "defraimp_personresponsiblecountry"): Xrm.Attribute<string>;
      get(name: "defraimp_personresponsiblecounty"): Xrm.Attribute<string>;
      get(name: "defraimp_personresponsibleemail"): Xrm.Attribute<string>;
      get(name: "defraimp_personresponsiblefax"): Xrm.Attribute<string>;
      get(name: "defraimp_personresponsiblename"): Xrm.Attribute<string>;
      get(name: "defraimp_personresponsiblephone"): Xrm.Attribute<string>;
      get(name: "defraimp_personresponsiblepostcode"): Xrm.Attribute<string>;
      get(name: "defraimp_portofentry"): Xrm.Attribute<string>;
      get(name: "defraimp_submissiondate"): Xrm.DateAttribute;
      get(name: "defraimp_veterinaryinformationveterinarydocument"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_commoditiesregionoforigin"): Xrm.StringControl;
      get(name: "defraimp_countryoforiginid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_departuredate"): Xrm.DateControl;
      get(name: "defraimp_estimatedjourneytimeinminutes"): Xrm.NumberControl;
      get(name: "defraimp_name"): Xrm.StringControl;
      get(name: "defraimp_personresponsibleaddress"): Xrm.StringControl;
      get(name: "defraimp_personresponsiblecity"): Xrm.StringControl;
      get(name: "defraimp_personresponsiblecompanyname"): Xrm.StringControl;
      get(name: "defraimp_personresponsiblecountry"): Xrm.StringControl;
      get(name: "defraimp_personresponsiblecounty"): Xrm.StringControl;
      get(name: "defraimp_personresponsibleemail"): Xrm.StringControl;
      get(name: "defraimp_personresponsiblefax"): Xrm.StringControl;
      get(name: "defraimp_personresponsiblename"): Xrm.StringControl;
      get(name: "defraimp_personresponsiblephone"): Xrm.StringControl;
      get(name: "defraimp_personresponsiblepostcode"): Xrm.StringControl;
      get(name: "defraimp_portofentry"): Xrm.StringControl;
      get(name: "defraimp_submissiondate"): Xrm.DateControl;
      get(name: "defraimp_veterinaryinformationveterinarydocument"): Xrm.StringControl;
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
  interface QuickCreate extends Xrm.PageBase<QuickCreate.Attributes,QuickCreate.Tabs,QuickCreate.Controls> {
    getAttribute(attributeName: "defraimp_commoditiesregionoforigin"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_countryoforiginid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_departuredate"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_estimatedjourneytimeinminutes"): Xrm.NumberAttribute;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_personresponsibleaddress"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_personresponsiblecity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_personresponsiblecompanyname"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_personresponsiblecountry"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_personresponsiblecounty"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_personresponsibleemail"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_personresponsiblefax"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_personresponsiblename"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_personresponsiblephone"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_personresponsiblepostcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_portofentry"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_submissiondate"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_veterinaryinformationveterinarydocument"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_commoditiesregionoforigin"): Xrm.StringControl;
    getControl(controlName: "defraimp_countryoforiginid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_departuredate"): Xrm.DateControl;
    getControl(controlName: "defraimp_estimatedjourneytimeinminutes"): Xrm.NumberControl;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "defraimp_personresponsibleaddress"): Xrm.StringControl;
    getControl(controlName: "defraimp_personresponsiblecity"): Xrm.StringControl;
    getControl(controlName: "defraimp_personresponsiblecompanyname"): Xrm.StringControl;
    getControl(controlName: "defraimp_personresponsiblecountry"): Xrm.StringControl;
    getControl(controlName: "defraimp_personresponsiblecounty"): Xrm.StringControl;
    getControl(controlName: "defraimp_personresponsibleemail"): Xrm.StringControl;
    getControl(controlName: "defraimp_personresponsiblefax"): Xrm.StringControl;
    getControl(controlName: "defraimp_personresponsiblename"): Xrm.StringControl;
    getControl(controlName: "defraimp_personresponsiblephone"): Xrm.StringControl;
    getControl(controlName: "defraimp_personresponsiblepostcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_portofentry"): Xrm.StringControl;
    getControl(controlName: "defraimp_submissiondate"): Xrm.DateControl;
    getControl(controlName: "defraimp_veterinaryinformationveterinarydocument"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
