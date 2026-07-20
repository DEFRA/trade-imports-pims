declare namespace Form.defraimp_itahc.Quick {
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
      get(name: "defraimp_placeoforiginharvestaddresscity"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginharvestaddresscountryid"): Xrm.LookupAttribute<"defra_country">;
      get(name: "defraimp_placeoforiginharvestaddresspostcode"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginharvestaddressstreet"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginharvestapprovalnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginharvestgeneralnumber"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginharvestname"): Xrm.Attribute<string>;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_placeoforiginharvestaddresscity"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginharvestaddresscountryid"): Xrm.LookupControl<"defra_country">;
      get(name: "defraimp_placeoforiginharvestaddresspostcode"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginharvestaddressstreet"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginharvestapprovalnumber"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginharvestgeneralnumber"): Xrm.StringControl;
      get(name: "defraimp_placeoforiginharvestname"): Xrm.StringControl;
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
    getAttribute(attributeName: "defraimp_placeoforiginharvestaddresscity"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginharvestaddresscountryid"): Xrm.LookupAttribute<"defra_country">;
    getAttribute(attributeName: "defraimp_placeoforiginharvestaddresspostcode"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginharvestaddressstreet"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginharvestapprovalnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginharvestgeneralnumber"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginharvestname"): Xrm.Attribute<string>;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_placeoforiginharvestaddresscity"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginharvestaddresscountryid"): Xrm.LookupControl<"defra_country">;
    getControl(controlName: "defraimp_placeoforiginharvestaddresspostcode"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginharvestaddressstreet"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginharvestapprovalnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginharvestgeneralnumber"): Xrm.StringControl;
    getControl(controlName: "defraimp_placeoforiginharvestname"): Xrm.StringControl;
    getControl(controlName: string): undefined;
  }
}
