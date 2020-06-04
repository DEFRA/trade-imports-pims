declare namespace Form.defraimp_importernotification.Quick {
  namespace PlaceofOriginDetails {
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
      get(name: "defraimp_placeoforiginharvestaddressaddressline1"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginharvestaddressaddressline2"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginharvestaddressaddressline3"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginharvestaddresscity"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginharvestaddresscountryid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_placeoforiginharvestaddresspostalzipcode"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginharvestapprovalnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginharvestcompanyname"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginharvestindividualname"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginharvestotheridentifier"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_placeoforiginharvestaddressaddressline1"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginharvestaddressaddressline2"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginharvestaddressaddressline3"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginharvestaddresscity"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginharvestaddresscountryid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_placeoforiginharvestaddresspostalzipcode"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginharvestapprovalnumber"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginharvestcompanyname"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginharvestindividualname"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginharvestotheridentifier"): Xrm.StringControl;
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
  interface PlaceofOriginDetails extends Xrm.PageBase<PlaceofOriginDetails.Attributes,PlaceofOriginDetails.Tabs,PlaceofOriginDetails.Controls> {
    getAttribute(attributeName: "defraimp_placeoforiginharvestaddressaddressline1"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginharvestaddressaddressline2"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginharvestaddressaddressline3"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginharvestaddresscity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginharvestaddresscountryid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_placeoforiginharvestaddresspostalzipcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginharvestapprovalnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginharvestcompanyname"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginharvestindividualname"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginharvestotheridentifier"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_placeoforiginharvestaddressaddressline1"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginharvestaddressaddressline2"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginharvestaddressaddressline3"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginharvestaddresscity"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginharvestaddresscountryid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_placeoforiginharvestaddresspostalzipcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginharvestapprovalnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginharvestcompanyname"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginharvestindividualname"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginharvestotheridentifier"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
