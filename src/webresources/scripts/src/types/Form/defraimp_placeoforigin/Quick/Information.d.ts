declare namespace Form.defraimp_placeoforigin.Quick {
  namespace Information {
    namespace Tabs {
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_addresscity"): Xrm.Attribute<string>;
      get(name: "defraimp_addresscountry"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_addressline1"): Xrm.Attribute<string>;
      get(name: "defraimp_addressline2"): Xrm.Attribute<string>;
      get(name: "defraimp_addressline3"): Xrm.Attribute<string>;
      get(name: "defraimp_locktobronze"): Xrm.OptionSetAttribute<boolean>;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: "defraimp_numberofapplications"): Xrm.NumberAttribute;
      get(name: "defraimp_numberofsuccessfulapplications"): Xrm.NumberAttribute;
      get(name: "defraimp_postcode"): Xrm.Attribute<string>;
      get(name: "defraimp_trustlevel"): Xrm.OptionSetAttribute<defraimp_trustlevel>;
      get(name: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_addresscity"): Xrm.StringControl;
      get(name: "defraimp_addresscountry"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_addressline1"): Xrm.StringControl;
      get(name: "defraimp_addressline2"): Xrm.StringControl;
      get(name: "defraimp_addressline3"): Xrm.StringControl;
      get(name: "defraimp_locktobronze"): Xrm.OptionSetControl<boolean>;
      get(name: "defraimp_name"): Xrm.StringControl;
      get(name: "defraimp_numberofapplications"): Xrm.NumberControl;
      get(name: "defraimp_numberofsuccessfulapplications"): Xrm.NumberControl;
      get(name: "defraimp_postcode"): Xrm.StringControl;
      get(name: "defraimp_trustlevel"): Xrm.OptionSetControl<defraimp_trustlevel>;
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
    getAttribute(attributeName: "defraimp_addresscity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_addresscountry"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_addressline1"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_addressline2"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_addressline3"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_locktobronze"): Xrm.OptionSetAttribute<boolean>;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_numberofapplications"): Xrm.NumberAttribute;
    getAttribute(attributeName: "defraimp_numberofsuccessfulapplications"): Xrm.NumberAttribute;
    getAttribute(attributeName: "defraimp_postcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_trustlevel"): Xrm.OptionSetAttribute<defraimp_trustlevel>;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_addresscity"): Xrm.StringControl;
    getControl(controlName: "defraimp_addresscountry"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_addressline1"): Xrm.StringControl;
    getControl(controlName: "defraimp_addressline2"): Xrm.StringControl;
    getControl(controlName: "defraimp_addressline3"): Xrm.StringControl;
    getControl(controlName: "defraimp_locktobronze"): Xrm.OptionSetControl<boolean>;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "defraimp_numberofapplications"): Xrm.NumberControl;
    getControl(controlName: "defraimp_numberofsuccessfulapplications"): Xrm.NumberControl;
    getControl(controlName: "defraimp_postcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_trustlevel"): Xrm.OptionSetControl<defraimp_trustlevel>;
    getControl(controlName: "ownerid"): Xrm.LookupControl<"systemuser" | "team">;
    getControl(controlName: string): undefined;
  }
}
