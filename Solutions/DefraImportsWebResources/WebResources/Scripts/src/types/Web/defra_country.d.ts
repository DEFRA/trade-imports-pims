declare namespace WebApi {
  interface defra_country_Base extends WebEntity {
    createdon?: Date | null;
    defra_aka?: string | null;
    defra_citizenemonym?: string | null;
    defra_codeassignmenttype?: string | null;
    defra_countryid?: string | null;
    defra_enddate?: Date | null;
    defra_independent?: boolean | null;
    defra_iso31662subdivisioncodes?: string | null;
    defra_isocodealpha2?: string | null;
    defra_isocodealpha3?: string | null;
    defra_isonumericcode?: string | null;
    defra_name?: string | null;
    defra_notes?: string | null;
    defra_startdate?: Date | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defra_country_statecode | null;
    statuscode?: defra_country_statuscode | null;
    timezoneruleversionnumber?: number | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defra_country_Relationships {
    defra_country_ProcessSession?: ProcessSession_Result[] | null;
    defra_country_SyncErrors?: SyncError_Result[] | null;
    defraimp_defra_country_defraimp_importapplication?: defraimp_importapplication_Result[] | null;
    defraimp_defra_country_defraimp_importapplication_ImporterAddressCountryID?: defraimp_importapplication_Result[] | null;
    defraimp_defra_country_defraimp_importapplication_PlaceofOriginCountryID?: defraimp_importapplication_Result[] | null;
    defraimp_defra_country_defraimp_importapppermdest?: defraimp_importapplication_Result[] | null;
    defraimp_defra_country_defraimp_importappplacedest?: defraimp_importapplication_Result[] | null;
    defraimp_defra_country_defraimp_importcountrycommodityrisklevel_countryid?: defraimp_importcountrycommodityrisklevel_Result[] | null;
    defraimp_defra_country_defraimp_importnotification_CharityAddressCountry?: defraimp_importnotification_Result[] | null;
    defraimp_defra_country_defraimp_importnotification_CountryofOriginId?: defraimp_importnotification_Result[] | null;
    defraimp_defra_country_defraimp_importnotification_ImporterAddressCountryid?: defraimp_importnotification_Result[] | null;
    defraimp_defra_country_defraimp_importnotification_PermanentDestinationAddressCountryid?: defraimp_importnotification_Result[] | null;
    defraimp_defra_country_defraimp_importnotification_PlaceofDestinationAddressCountryid?: defraimp_importnotification_Result[] | null;
    defraimp_defra_country_defraimp_itahc_CountryofOrigin?: defraimp_itahc_Result[] | null;
    defraimp_defra_country_defraimp_placeoforigin_AddressCountry?: defraimp_placeoforigin_Result[] | null;
    defraimp_defraimp_importapplication_defra_country?: defraimp_importapplication_Result[] | null;
    defraimp_goldbronzecountriesnn?: defraimp_goldbronzecommodity_Result[] | null;
  }
  interface defra_country extends defra_country_Base, defra_country_Relationships {
    ownerid_bind$owners?: string | null;
  }
  interface defra_country_Create extends defra_country {
  }
  interface defra_country_Update extends defra_country {
  }
  interface defra_country_Select {
    createdby_guid: WebAttribute<defra_country_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defra_country_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defra_country_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defra_aka: WebAttribute<defra_country_Select, { defra_aka: string | null }, {  }>;
    defra_citizenemonym: WebAttribute<defra_country_Select, { defra_citizenemonym: string | null }, {  }>;
    defra_codeassignmenttype: WebAttribute<defra_country_Select, { defra_codeassignmenttype: string | null }, {  }>;
    defra_countryid: WebAttribute<defra_country_Select, { defra_countryid: string | null }, {  }>;
    defra_enddate: WebAttribute<defra_country_Select, { defra_enddate: Date | null }, { defra_enddate_formatted?: string }>;
    defra_independent: WebAttribute<defra_country_Select, { defra_independent: boolean | null }, {  }>;
    defra_iso31662subdivisioncodes: WebAttribute<defra_country_Select, { defra_iso31662subdivisioncodes: string | null }, {  }>;
    defra_isocodealpha2: WebAttribute<defra_country_Select, { defra_isocodealpha2: string | null }, {  }>;
    defra_isocodealpha3: WebAttribute<defra_country_Select, { defra_isocodealpha3: string | null }, {  }>;
    defra_isonumericcode: WebAttribute<defra_country_Select, { defra_isonumericcode: string | null }, {  }>;
    defra_name: WebAttribute<defra_country_Select, { defra_name: string | null }, {  }>;
    defra_notes: WebAttribute<defra_country_Select, { defra_notes: string | null }, {  }>;
    defra_startdate: WebAttribute<defra_country_Select, { defra_startdate: Date | null }, { defra_startdate_formatted?: string }>;
    importsequencenumber: WebAttribute<defra_country_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defra_country_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defra_country_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defra_country_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    overriddencreatedon: WebAttribute<defra_country_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    ownerid_guid: WebAttribute<defra_country_Select, { ownerid_guid: string | null }, { ownerid_formatted?: string }>;
    owningbusinessunit_guid: WebAttribute<defra_country_Select, { owningbusinessunit_guid: string | null }, { owningbusinessunit_formatted?: string }>;
    owningteam_guid: WebAttribute<defra_country_Select, { owningteam_guid: string | null }, { owningteam_formatted?: string }>;
    owninguser_guid: WebAttribute<defra_country_Select, { owninguser_guid: string | null }, { owninguser_formatted?: string }>;
    statecode: WebAttribute<defra_country_Select, { statecode: defra_country_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defra_country_Select, { statuscode: defra_country_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defra_country_Select, { timezoneruleversionnumber: number | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defra_country_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defra_country_Select, { versionnumber: number | null }, {  }>;
  }
  interface defra_country_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defra_aka: string;
    defra_citizenemonym: string;
    defra_codeassignmenttype: string;
    defra_countryid: XQW.Guid;
    defra_enddate: Date;
    defra_independent: boolean;
    defra_iso31662subdivisioncodes: string;
    defra_isocodealpha2: string;
    defra_isocodealpha3: string;
    defra_isonumericcode: string;
    defra_name: string;
    defra_notes: string;
    defra_startdate: Date;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    overriddencreatedon: Date;
    ownerid_guid: XQW.Guid;
    owningbusinessunit_guid: XQW.Guid;
    owningteam_guid: XQW.Guid;
    owninguser_guid: XQW.Guid;
    statecode: defra_country_statecode;
    statuscode: defra_country_statuscode;
    timezoneruleversionnumber: number;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defra_country_Expand {
    createdby: WebExpand<defra_country_Expand, SystemUser_Select, SystemUser_Filter, { createdby: SystemUser_Result }>;
    createdonbehalfby: WebExpand<defra_country_Expand, SystemUser_Select, SystemUser_Filter, { createdonbehalfby: SystemUser_Result }>;
    defra_country_ProcessSession: WebExpand<defra_country_Expand, ProcessSession_Select, ProcessSession_Filter, { defra_country_ProcessSession: ProcessSession_Result[] }>;
    defra_country_SyncErrors: WebExpand<defra_country_Expand, SyncError_Select, SyncError_Filter, { defra_country_SyncErrors: SyncError_Result[] }>;
    defraimp_defra_country_defraimp_importapplication: WebExpand<defra_country_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defra_country_defraimp_importapplication: defraimp_importapplication_Result[] }>;
    defraimp_defra_country_defraimp_importapplication_ImporterAddressCountryID: WebExpand<defra_country_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defra_country_defraimp_importapplication_ImporterAddressCountryID: defraimp_importapplication_Result[] }>;
    defraimp_defra_country_defraimp_importapplication_PlaceofOriginCountryID: WebExpand<defra_country_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defra_country_defraimp_importapplication_PlaceofOriginCountryID: defraimp_importapplication_Result[] }>;
    defraimp_defra_country_defraimp_importapppermdest: WebExpand<defra_country_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defra_country_defraimp_importapppermdest: defraimp_importapplication_Result[] }>;
    defraimp_defra_country_defraimp_importappplacedest: WebExpand<defra_country_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defra_country_defraimp_importappplacedest: defraimp_importapplication_Result[] }>;
    defraimp_defra_country_defraimp_importcountrycommodityrisklevel_countryid: WebExpand<defra_country_Expand, defraimp_importcountrycommodityrisklevel_Select, defraimp_importcountrycommodityrisklevel_Filter, { defraimp_defra_country_defraimp_importcountrycommodityrisklevel_countryid: defraimp_importcountrycommodityrisklevel_Result[] }>;
    defraimp_defra_country_defraimp_importnotification_CharityAddressCountry: WebExpand<defra_country_Expand, defraimp_importnotification_Select, defraimp_importnotification_Filter, { defraimp_defra_country_defraimp_importnotification_CharityAddressCountry: defraimp_importnotification_Result[] }>;
    defraimp_defra_country_defraimp_importnotification_CountryofOriginId: WebExpand<defra_country_Expand, defraimp_importnotification_Select, defraimp_importnotification_Filter, { defraimp_defra_country_defraimp_importnotification_CountryofOriginId: defraimp_importnotification_Result[] }>;
    defraimp_defra_country_defraimp_importnotification_ImporterAddressCountryid: WebExpand<defra_country_Expand, defraimp_importnotification_Select, defraimp_importnotification_Filter, { defraimp_defra_country_defraimp_importnotification_ImporterAddressCountryid: defraimp_importnotification_Result[] }>;
    defraimp_defra_country_defraimp_importnotification_PermanentDestinationAddressCountryid: WebExpand<defra_country_Expand, defraimp_importnotification_Select, defraimp_importnotification_Filter, { defraimp_defra_country_defraimp_importnotification_PermanentDestinationAddressCountryid: defraimp_importnotification_Result[] }>;
    defraimp_defra_country_defraimp_importnotification_PlaceofDestinationAddressCountryid: WebExpand<defra_country_Expand, defraimp_importnotification_Select, defraimp_importnotification_Filter, { defraimp_defra_country_defraimp_importnotification_PlaceofDestinationAddressCountryid: defraimp_importnotification_Result[] }>;
    defraimp_defra_country_defraimp_itahc_CountryofOrigin: WebExpand<defra_country_Expand, defraimp_itahc_Select, defraimp_itahc_Filter, { defraimp_defra_country_defraimp_itahc_CountryofOrigin: defraimp_itahc_Result[] }>;
    defraimp_defra_country_defraimp_placeoforigin_AddressCountry: WebExpand<defra_country_Expand, defraimp_placeoforigin_Select, defraimp_placeoforigin_Filter, { defraimp_defra_country_defraimp_placeoforigin_AddressCountry: defraimp_placeoforigin_Result[] }>;
    defraimp_defraimp_importapplication_defra_country: WebExpand<defra_country_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defraimp_importapplication_defra_country: defraimp_importapplication_Result[] }>;
    defraimp_goldbronzecountriesnn: WebExpand<defra_country_Expand, defraimp_goldbronzecommodity_Select, defraimp_goldbronzecommodity_Filter, { defraimp_goldbronzecountriesnn: defraimp_goldbronzecommodity_Result[] }>;
    modifiedby: WebExpand<defra_country_Expand, SystemUser_Select, SystemUser_Filter, { modifiedby: SystemUser_Result }>;
    modifiedonbehalfby: WebExpand<defra_country_Expand, SystemUser_Select, SystemUser_Filter, { modifiedonbehalfby: SystemUser_Result }>;
    owningteam: WebExpand<defra_country_Expand, Team_Select, Team_Filter, { owningteam: Team_Result }>;
    owninguser: WebExpand<defra_country_Expand, SystemUser_Select, SystemUser_Filter, { owninguser: SystemUser_Result }>;
  }
  interface defra_country_FormattedResult {
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    defra_enddate_formatted?: string;
    defra_startdate_formatted?: string;
    modifiedby_formatted?: string;
    modifiedon_formatted?: string;
    modifiedonbehalfby_formatted?: string;
    overriddencreatedon_formatted?: string;
    ownerid_formatted?: string;
    owningbusinessunit_formatted?: string;
    owningteam_formatted?: string;
    owninguser_formatted?: string;
    statecode_formatted?: string;
    statuscode_formatted?: string;
  }
  interface defra_country_Result extends defra_country_Base, defra_country_Relationships {
    "@odata.etag": string;
    createdby_guid: string | null;
    createdonbehalfby_guid: string | null;
    modifiedby_guid: string | null;
    modifiedonbehalfby_guid: string | null;
    ownerid_guid: string | null;
    owningbusinessunit_guid: string | null;
    owningteam_guid: string | null;
    owninguser_guid: string | null;
  }
  interface defra_country_RelatedOne {
    createdby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    createdonbehalfby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    modifiedby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    modifiedonbehalfby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    owningteam: WebMappingRetrieve<WebApi.Team_Select,WebApi.Team_Expand,WebApi.Team_Filter,WebApi.Team_Fixed,WebApi.Team_Result,WebApi.Team_FormattedResult>;
    owninguser: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
  }
  interface defra_country_RelatedMany {
    defra_country_ProcessSession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    defra_country_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
    defraimp_defra_country_defraimp_importapplication: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_defra_country_defraimp_importapplication_ImporterAddressCountryID: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_defra_country_defraimp_importapplication_PlaceofOriginCountryID: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_defra_country_defraimp_importapppermdest: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_defra_country_defraimp_importappplacedest: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_defra_country_defraimp_importcountrycommodityrisklevel_countryid: WebMappingRetrieve<WebApi.defraimp_importcountrycommodityrisklevel_Select,WebApi.defraimp_importcountrycommodityrisklevel_Expand,WebApi.defraimp_importcountrycommodityrisklevel_Filter,WebApi.defraimp_importcountrycommodityrisklevel_Fixed,WebApi.defraimp_importcountrycommodityrisklevel_Result,WebApi.defraimp_importcountrycommodityrisklevel_FormattedResult>;
    defraimp_defra_country_defraimp_importnotification_CharityAddressCountry: WebMappingRetrieve<WebApi.defraimp_importnotification_Select,WebApi.defraimp_importnotification_Expand,WebApi.defraimp_importnotification_Filter,WebApi.defraimp_importnotification_Fixed,WebApi.defraimp_importnotification_Result,WebApi.defraimp_importnotification_FormattedResult>;
    defraimp_defra_country_defraimp_importnotification_CountryofOriginId: WebMappingRetrieve<WebApi.defraimp_importnotification_Select,WebApi.defraimp_importnotification_Expand,WebApi.defraimp_importnotification_Filter,WebApi.defraimp_importnotification_Fixed,WebApi.defraimp_importnotification_Result,WebApi.defraimp_importnotification_FormattedResult>;
    defraimp_defra_country_defraimp_importnotification_ImporterAddressCountryid: WebMappingRetrieve<WebApi.defraimp_importnotification_Select,WebApi.defraimp_importnotification_Expand,WebApi.defraimp_importnotification_Filter,WebApi.defraimp_importnotification_Fixed,WebApi.defraimp_importnotification_Result,WebApi.defraimp_importnotification_FormattedResult>;
    defraimp_defra_country_defraimp_importnotification_PermanentDestinationAddressCountryid: WebMappingRetrieve<WebApi.defraimp_importnotification_Select,WebApi.defraimp_importnotification_Expand,WebApi.defraimp_importnotification_Filter,WebApi.defraimp_importnotification_Fixed,WebApi.defraimp_importnotification_Result,WebApi.defraimp_importnotification_FormattedResult>;
    defraimp_defra_country_defraimp_importnotification_PlaceofDestinationAddressCountryid: WebMappingRetrieve<WebApi.defraimp_importnotification_Select,WebApi.defraimp_importnotification_Expand,WebApi.defraimp_importnotification_Filter,WebApi.defraimp_importnotification_Fixed,WebApi.defraimp_importnotification_Result,WebApi.defraimp_importnotification_FormattedResult>;
    defraimp_defra_country_defraimp_itahc_CountryofOrigin: WebMappingRetrieve<WebApi.defraimp_itahc_Select,WebApi.defraimp_itahc_Expand,WebApi.defraimp_itahc_Filter,WebApi.defraimp_itahc_Fixed,WebApi.defraimp_itahc_Result,WebApi.defraimp_itahc_FormattedResult>;
    defraimp_defra_country_defraimp_placeoforigin_AddressCountry: WebMappingRetrieve<WebApi.defraimp_placeoforigin_Select,WebApi.defraimp_placeoforigin_Expand,WebApi.defraimp_placeoforigin_Filter,WebApi.defraimp_placeoforigin_Fixed,WebApi.defraimp_placeoforigin_Result,WebApi.defraimp_placeoforigin_FormattedResult>;
    defraimp_defraimp_importapplication_defra_country: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_goldbronzecountriesnn: WebMappingRetrieve<WebApi.defraimp_goldbronzecommodity_Select,WebApi.defraimp_goldbronzecommodity_Expand,WebApi.defraimp_goldbronzecommodity_Filter,WebApi.defraimp_goldbronzecommodity_Fixed,WebApi.defraimp_goldbronzecommodity_Result,WebApi.defraimp_goldbronzecommodity_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defra_countries: WebMappingRetrieve<WebApi.defra_country_Select,WebApi.defra_country_Expand,WebApi.defra_country_Filter,WebApi.defra_country_Fixed,WebApi.defra_country_Result,WebApi.defra_country_FormattedResult>;
}
interface WebEntitiesRelated {
  defra_countries: WebMappingRelated<WebApi.defra_country_RelatedOne,WebApi.defra_country_RelatedMany>;
}
interface WebEntitiesCUDA {
  defra_countries: WebMappingCUDA<WebApi.defra_country_Create,WebApi.defra_country_Update,WebApi.defra_country_Select>;
}
