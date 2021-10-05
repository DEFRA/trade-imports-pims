declare namespace Form.defraimp_importapplication.Quick {
  namespace Information {
    namespace Tabs {
    }
    interface Attributes extends Xrm.AttributeCollectionBase {
      get(name: "defraimp_certificatecompliantfirsttime"): Xrm.OptionSetAttribute<boolean> | null;
      get(name: "defraimp_certificatenoncompliancereason"): Xrm.OptionSetAttribute<defraimp_importapplication_defraimp_certificatenoncompliancereason> | null;
      get(name: "defraimp_certificatenoncompliancereasonother"): Xrm.Attribute<string> | null;
      get(name: "defraimp_certificateverified"): Xrm.OptionSetAttribute<boolean> | null;
      get(name: "defraimp_commoditytypeid"): Xrm.LookupAttribute<"defraexp_commoditytype"> | null;
      get(name: "defraimp_completionsummary"): Xrm.Attribute<string> | null;
      get(name: "defraimp_countryoforiginid"): Xrm.LookupAttribute<"defra_country"> | null;
      get(name: "defraimp_importrisklevelid"): Xrm.LookupAttribute<"defraimp_importrisklevel"> | null;
      get(name: "defraimp_inspectiondeclinedreason"): Xrm.Attribute<string> | null;
      get(name: "defraimp_inspectionrequested"): Xrm.OptionSetAttribute<boolean> | null;
      get(name: "defraimp_inspectionrequired"): Xrm.OptionSetAttribute<defraimp_importapplication_defraimp_inspectionrequired> | null;
      get(name: "defraimp_manualpostimportcheckdecision"): Xrm.OptionSetAttribute<defraimp_importapplication_defraimp_manualpostimportcheckdecision> | null;
      get(name: "defraimp_movedtocompletiondate"): Xrm.DateAttribute | null;
      get(name: "defraimp_movetocompletion"): Xrm.OptionSetAttribute<boolean> | null;
      get(name: "defraimp_name"): Xrm.Attribute<string>;
      get(name: "defraimp_placeoforiginid"): Xrm.LookupAttribute<"defraimp_placeoforigin"> | null;
      get(name: "defraimp_primaryitahcid"): Xrm.LookupAttribute<"defraimp_itahc"> | null;
      get(name: "defraimp_regionareaallocatedtoid"): Xrm.LookupAttribute<"defraimp_apharegion"> | null;
      get(name: "defraimp_resettrustleveltobronze"): Xrm.OptionSetAttribute<boolean> | null;
      get(name: "defraimp_risklevelverified"): Xrm.OptionSetAttribute<boolean> | null;
      get(name: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
      get(name: string): undefined;
      get(): Xrm.Attribute<any>[];
      get(index: number): Xrm.Attribute<any>;
      get(chooser: (item: Xrm.Attribute<any>, index: number) => boolean): Xrm.Attribute<any>[];
    }
    interface Controls extends Xrm.ControlCollectionBase {
      get(name: "defraimp_name"): Xrm.StringControl;
      get(name: "header_process_defraimp_certificatecompliantfirsttime"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_certificatenoncompliancereason"): Xrm.OptionSetControl<defraimp_importapplication_defraimp_certificatenoncompliancereason> | null;
      get(name: "header_process_defraimp_certificatenoncompliancereasonother"): Xrm.StringControl | null;
      get(name: "header_process_defraimp_certificateverified"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_commoditytypeid"): Xrm.LookupControl<"defraexp_commoditytype"> | null;
      get(name: "header_process_defraimp_completionsummary"): Xrm.StringControl | null;
      get(name: "header_process_defraimp_completionsummary_1"): Xrm.StringControl | null;
      get(name: "header_process_defraimp_countryoforiginid"): Xrm.LookupControl<"defra_country"> | null;
      get(name: "header_process_defraimp_importrisklevelid"): Xrm.LookupControl<"defraimp_importrisklevel"> | null;
      get(name: "header_process_defraimp_inspectiondeclinedreason"): Xrm.StringControl | null;
      get(name: "header_process_defraimp_inspectionrequested"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_inspectionrequired"): Xrm.OptionSetControl<defraimp_importapplication_defraimp_inspectionrequired> | null;
      get(name: "header_process_defraimp_manualpostimportcheckdecision"): Xrm.OptionSetControl<defraimp_importapplication_defraimp_manualpostimportcheckdecision> | null;
      get(name: "header_process_defraimp_movedtocompletiondate"): Xrm.DateControl | null;
      get(name: "header_process_defraimp_movedtocompletiondate_1"): Xrm.DateControl | null;
      get(name: "header_process_defraimp_movetocompletion"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_movetocompletion_1"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_placeoforiginid"): Xrm.LookupControl<"defraimp_placeoforigin"> | null;
      get(name: "header_process_defraimp_primaryitahcid"): Xrm.LookupControl<"defraimp_itahc"> | null;
      get(name: "header_process_defraimp_regionareaallocatedtoid"): Xrm.LookupControl<"defraimp_apharegion"> | null;
      get(name: "header_process_defraimp_resettrustleveltobronze"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_defraimp_risklevelverified"): Xrm.OptionSetControl<boolean> | null;
      get(name: "header_process_ownerid"): Xrm.LookupControl<"systemuser" | "team"> | null;
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
    getAttribute(attributeName: "defraimp_certificatecompliantfirsttime"): Xrm.OptionSetAttribute<boolean> | null;
    getAttribute(attributeName: "defraimp_certificatenoncompliancereason"): Xrm.OptionSetAttribute<defraimp_importapplication_defraimp_certificatenoncompliancereason> | null;
    getAttribute(attributeName: "defraimp_certificatenoncompliancereasonother"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "defraimp_certificateverified"): Xrm.OptionSetAttribute<boolean> | null;
    getAttribute(attributeName: "defraimp_commoditytypeid"): Xrm.LookupAttribute<"defraexp_commoditytype"> | null;
    getAttribute(attributeName: "defraimp_completionsummary"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "defraimp_countryoforiginid"): Xrm.LookupAttribute<"defra_country"> | null;
    getAttribute(attributeName: "defraimp_importrisklevelid"): Xrm.LookupAttribute<"defraimp_importrisklevel"> | null;
    getAttribute(attributeName: "defraimp_inspectiondeclinedreason"): Xrm.Attribute<string> | null;
    getAttribute(attributeName: "defraimp_inspectionrequested"): Xrm.OptionSetAttribute<boolean> | null;
    getAttribute(attributeName: "defraimp_inspectionrequired"): Xrm.OptionSetAttribute<defraimp_importapplication_defraimp_inspectionrequired> | null;
    getAttribute(attributeName: "defraimp_manualpostimportcheckdecision"): Xrm.OptionSetAttribute<defraimp_importapplication_defraimp_manualpostimportcheckdecision> | null;
    getAttribute(attributeName: "defraimp_movedtocompletiondate"): Xrm.DateAttribute | null;
    getAttribute(attributeName: "defraimp_movetocompletion"): Xrm.OptionSetAttribute<boolean> | null;
    getAttribute(attributeName: "defraimp_name"): Xrm.Attribute<string>;
    getAttribute(attributeName: "defraimp_placeoforiginid"): Xrm.LookupAttribute<"defraimp_placeoforigin"> | null;
    getAttribute(attributeName: "defraimp_primaryitahcid"): Xrm.LookupAttribute<"defraimp_itahc"> | null;
    getAttribute(attributeName: "defraimp_regionareaallocatedtoid"): Xrm.LookupAttribute<"defraimp_apharegion"> | null;
    getAttribute(attributeName: "defraimp_resettrustleveltobronze"): Xrm.OptionSetAttribute<boolean> | null;
    getAttribute(attributeName: "defraimp_risklevelverified"): Xrm.OptionSetAttribute<boolean> | null;
    getAttribute(attributeName: "ownerid"): Xrm.LookupAttribute<"systemuser" | "team">;
    getAttribute(attributeName: string): undefined;
    getControl(controlName: "defraimp_name"): Xrm.StringControl;
    getControl(controlName: "header_process_defraimp_certificatecompliantfirsttime"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_certificatenoncompliancereason"): Xrm.OptionSetControl<defraimp_importapplication_defraimp_certificatenoncompliancereason> | null;
    getControl(controlName: "header_process_defraimp_certificatenoncompliancereasonother"): Xrm.StringControl | null;
    getControl(controlName: "header_process_defraimp_certificateverified"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_commoditytypeid"): Xrm.LookupControl<"defraexp_commoditytype"> | null;
    getControl(controlName: "header_process_defraimp_completionsummary"): Xrm.StringControl | null;
    getControl(controlName: "header_process_defraimp_completionsummary_1"): Xrm.StringControl | null;
    getControl(controlName: "header_process_defraimp_countryoforiginid"): Xrm.LookupControl<"defra_country"> | null;
    getControl(controlName: "header_process_defraimp_importrisklevelid"): Xrm.LookupControl<"defraimp_importrisklevel"> | null;
    getControl(controlName: "header_process_defraimp_inspectiondeclinedreason"): Xrm.StringControl | null;
    getControl(controlName: "header_process_defraimp_inspectionrequested"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_inspectionrequired"): Xrm.OptionSetControl<defraimp_importapplication_defraimp_inspectionrequired> | null;
    getControl(controlName: "header_process_defraimp_manualpostimportcheckdecision"): Xrm.OptionSetControl<defraimp_importapplication_defraimp_manualpostimportcheckdecision> | null;
    getControl(controlName: "header_process_defraimp_movedtocompletiondate"): Xrm.DateControl | null;
    getControl(controlName: "header_process_defraimp_movedtocompletiondate_1"): Xrm.DateControl | null;
    getControl(controlName: "header_process_defraimp_movetocompletion"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_movetocompletion_1"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_placeoforiginid"): Xrm.LookupControl<"defraimp_placeoforigin"> | null;
    getControl(controlName: "header_process_defraimp_primaryitahcid"): Xrm.LookupControl<"defraimp_itahc"> | null;
    getControl(controlName: "header_process_defraimp_regionareaallocatedtoid"): Xrm.LookupControl<"defraimp_apharegion"> | null;
    getControl(controlName: "header_process_defraimp_resettrustleveltobronze"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_defraimp_risklevelverified"): Xrm.OptionSetControl<boolean> | null;
    getControl(controlName: "header_process_ownerid"): Xrm.LookupControl<"systemuser" | "team"> | null;
    getControl(controlName: "ownerid"): Xrm.LookupControl<"systemuser" | "team">;
    getControl(controlName: string): undefined;
  }
}
