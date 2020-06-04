declare namespace Form.defraimp_importernotification.Quick {
  namespace KeyDetails {
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
      get(name: "createdon"): Xrm.DateAttribute;
      get(name: "defraimp_arrivaldate"): Xrm.DateAttribute;
      get(name: "defraimp_arrivaltime"): Xrm.Attribute<string>;
      get(name: "defraimp_commoditiescommodityintendedfor"): Xrm.Attribute<string>;
      get(name: "defraimp_commoditiesnumberofanimals"): Xrm.NumberAttribute;
      get(name: "defraimp_commodityid"): Xrm.Attribute<string>;
      get(name: "defraimp_commodityspeciescommonname"): Xrm.Attribute<string>;
      get(name: "defraimp_countryoforiginid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_departuredate"): Xrm.DateAttribute;
      get(name: "defraimp_departuretime"): Xrm.Attribute<string>;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: "defraimp_submissiondate"): Xrm.DateAttribute;
      get(name: "defraimp_veterinaryinformationveterinarydocument"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "createdon"): Xrm.DateControl;
      get(name: "defraimp_arrivaldate"): Xrm.DateControl;
      get(name: "defraimp_arrivaltime"): Xrm.StringControl;
      get(name: "defraimp_commoditiescommodityintendedfor"): Xrm.StringControl;
      get(name: "defraimp_commoditiesnumberofanimals"): Xrm.NumberControl;
      get(name: "defraimp_commodityid"): Xrm.StringControl;
      get(name: "defraimp_commodityspeciescommonname"): Xrm.StringControl;
      get(name: "defraimp_countryoforiginid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_departuredate"): Xrm.DateControl;
      get(name: "defraimp_departuretime"): Xrm.StringControl;
      get(name: "defraimp_name"): Xrm.StringControl;
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
  interface KeyDetails extends Xrm.PageBase<KeyDetails.Attributes,KeyDetails.Tabs,KeyDetails.Controls> {
    getAttribute(attributeName: "createdon"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_arrivaldate"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_arrivaltime"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_commoditiescommodityintendedfor"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_commoditiesnumberofanimals"): Xrm.NumberAttribute;
    getAttribute(attributeName: "defraimp_commodityid"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_commodityspeciescommonname"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_countryoforiginid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_departuredate"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_departuretime"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_submissiondate"): Xrm.DateAttribute;
    getAttribute(attributeName: "defraimp_veterinaryinformationveterinarydocument"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "createdon"): Xrm.DateControl;
    getControl(controlName: "defraimp_arrivaldate"): Xrm.DateControl;
    getControl(controlName: "defraimp_arrivaltime"): Xrm.StringControl;
    getControl(controlName: "defraimp_commoditiescommodityintendedfor"): Xrm.StringControl;
    getControl(controlName: "defraimp_commoditiesnumberofanimals"): Xrm.NumberControl;
    getControl(controlName: "defraimp_commodityid"): Xrm.StringControl;
    getControl(controlName: "defraimp_commodityspeciescommonname"): Xrm.StringControl;
    getControl(controlName: "defraimp_countryoforiginid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_departuredate"): Xrm.DateControl;
    getControl(controlName: "defraimp_departuretime"): Xrm.StringControl;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "defraimp_submissiondate"): Xrm.DateControl;
    getControl(controlName: "defraimp_veterinaryinformationveterinarydocument"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
