declare namespace WebApi {
  interface defraimp_itahc_Base extends WebEntity {
    createdon?: Date | null;
    defraimp_animalspeciesproduct?: string | null;
    defraimp_certifiedfor?: string | null;
    defraimp_datecertificatereceived?: Date | null;
    defraimp_datetimeofdeparture?: Date | null;
    defraimp_destinationaddresscity?: string | null;
    defraimp_destinationaddresscounty?: string | null;
    defraimp_destinationaddressline1?: string | null;
    defraimp_destinationaddressline2?: string | null;
    defraimp_destinationaddressline3?: string | null;
    defraimp_destinationaddresspostcode?: string | null;
    defraimp_destinationcountry?: defraimp_ukcountries | null;
    defraimp_estimatedjourneytimedays?: number | null;
    defraimp_estimatedjourneytimehours?: number | null;
    defraimp_healthcertificatenumber?: string | null;
    defraimp_identificationofanimals?: string | null;
    defraimp_itahcid?: string | null;
    defraimp_localveterinaryunit?: string | null;
    defraimp_lvuno?: string | null;
    defraimp_name?: string | null;
    defraimp_numberofpackages?: number | null;
    defraimp_ovname?: string | null;
    defraimp_placeofdestinationapprovalnumber?: string | null;
    defraimp_placeofdestinationtype?: defraimp_placeofdestinationtype | null;
    defraimp_placeoforiginaddresscity?: string | null;
    defraimp_placeoforiginaddressline1?: string | null;
    defraimp_placeoforiginaddressline2?: string | null;
    defraimp_placeoforiginaddressline3?: string | null;
    defraimp_placeoforiginaddresspostcode?: string | null;
    defraimp_placeoforiginname?: string | null;
    defraimp_purpose?: string | null;
    defraimp_quantity?: number | null;
    defraimp_regionoforigin?: string | null;
    defraimp_tracesreceiveddate?: Date | null;
    defraimp_unit?: string | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defraimp_itahc_statecode | null;
    statuscode?: defraimp_itahc_statuscode | null;
    timezoneruleversionnumber?: number | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraimp_itahc_Relationships {
    defraimp_CommodityTypeId?: defraexp_commoditytype_Result | null;
    defraimp_CountryofOrigin?: defra_country_Result | null;
    defraimp_ReplacedById?: defraimp_itahc_Result | null;
    defraimp_ReplacesId?: defraimp_itahc_Result | null;
    defraimp_defraimp_importapplication_defraimp_itahc?: defraimp_importapplication_Result[] | null;
    defraimp_defraimp_itahc_defraimp_importapplication?: defraimp_importapplication_Result[] | null;
    defraimp_defraimp_itahc_defraimp_importinspection_RelatedITAHC?: defraimp_importinspection_Result[] | null;
    defraimp_defraimp_itahc_defraimp_importquery_ITAHC?: defraimp_importquery_Result[] | null;
    defraimp_defraimp_itahc_defraimp_itahc?: defraimp_itahc_Result[] | null;
    defraimp_defraimp_itahc_defraimp_rb?: defraimp_itahc_Result[] | null;
    defraimp_itahc_ProcessSession?: ProcessSession_Result[] | null;
    defraimp_itahc_SyncErrors?: SyncError_Result[] | null;
  }
  interface defraimp_itahc extends defraimp_itahc_Base, defraimp_itahc_Relationships {
    defraimp_CommodityTypeId_bind$defraexp_commoditytypes?: string | null;
    defraimp_CountryofOrigin_bind$defra_countries?: string | null;
    defraimp_ReplacedById_bind$defraimp_itahcs?: string | null;
    defraimp_ReplacesId_bind$defraimp_itahcs?: string | null;
    ownerid_bind$owners?: string | null;
  }
  interface defraimp_itahc_Create extends defraimp_itahc {
  }
  interface defraimp_itahc_Update extends defraimp_itahc {
  }
  interface defraimp_itahc_Select {
    createdby_guid: WebAttribute<defraimp_itahc_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraimp_itahc_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraimp_itahc_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defraimp_animalspeciesproduct: WebAttribute<defraimp_itahc_Select, { defraimp_animalspeciesproduct: string | null }, {  }>;
    defraimp_certifiedfor: WebAttribute<defraimp_itahc_Select, { defraimp_certifiedfor: string | null }, {  }>;
    defraimp_commoditytypeid_guid: WebAttribute<defraimp_itahc_Select, { defraimp_commoditytypeid_guid: string | null }, { defraimp_commoditytypeid_formatted?: string }>;
    defraimp_countryoforigin_guid: WebAttribute<defraimp_itahc_Select, { defraimp_countryoforigin_guid: string | null }, { defraimp_countryoforigin_formatted?: string }>;
    defraimp_datecertificatereceived: WebAttribute<defraimp_itahc_Select, { defraimp_datecertificatereceived: Date | null }, { defraimp_datecertificatereceived_formatted?: string }>;
    defraimp_datetimeofdeparture: WebAttribute<defraimp_itahc_Select, { defraimp_datetimeofdeparture: Date | null }, { defraimp_datetimeofdeparture_formatted?: string }>;
    defraimp_destinationaddresscity: WebAttribute<defraimp_itahc_Select, { defraimp_destinationaddresscity: string | null }, {  }>;
    defraimp_destinationaddresscounty: WebAttribute<defraimp_itahc_Select, { defraimp_destinationaddresscounty: string | null }, {  }>;
    defraimp_destinationaddressline1: WebAttribute<defraimp_itahc_Select, { defraimp_destinationaddressline1: string | null }, {  }>;
    defraimp_destinationaddressline2: WebAttribute<defraimp_itahc_Select, { defraimp_destinationaddressline2: string | null }, {  }>;
    defraimp_destinationaddressline3: WebAttribute<defraimp_itahc_Select, { defraimp_destinationaddressline3: string | null }, {  }>;
    defraimp_destinationaddresspostcode: WebAttribute<defraimp_itahc_Select, { defraimp_destinationaddresspostcode: string | null }, {  }>;
    defraimp_destinationcountry: WebAttribute<defraimp_itahc_Select, { defraimp_destinationcountry: defraimp_ukcountries | null }, { defraimp_destinationcountry_formatted?: string }>;
    defraimp_estimatedjourneytimedays: WebAttribute<defraimp_itahc_Select, { defraimp_estimatedjourneytimedays: number | null }, {  }>;
    defraimp_estimatedjourneytimehours: WebAttribute<defraimp_itahc_Select, { defraimp_estimatedjourneytimehours: number | null }, {  }>;
    defraimp_healthcertificatenumber: WebAttribute<defraimp_itahc_Select, { defraimp_healthcertificatenumber: string | null }, {  }>;
    defraimp_identificationofanimals: WebAttribute<defraimp_itahc_Select, { defraimp_identificationofanimals: string | null }, {  }>;
    defraimp_itahcid: WebAttribute<defraimp_itahc_Select, { defraimp_itahcid: string | null }, {  }>;
    defraimp_localveterinaryunit: WebAttribute<defraimp_itahc_Select, { defraimp_localveterinaryunit: string | null }, {  }>;
    defraimp_lvuno: WebAttribute<defraimp_itahc_Select, { defraimp_lvuno: string | null }, {  }>;
    defraimp_name: WebAttribute<defraimp_itahc_Select, { defraimp_name: string | null }, {  }>;
    defraimp_numberofpackages: WebAttribute<defraimp_itahc_Select, { defraimp_numberofpackages: number | null }, {  }>;
    defraimp_ovname: WebAttribute<defraimp_itahc_Select, { defraimp_ovname: string | null }, {  }>;
    defraimp_placeofdestinationapprovalnumber: WebAttribute<defraimp_itahc_Select, { defraimp_placeofdestinationapprovalnumber: string | null }, {  }>;
    defraimp_placeofdestinationtype: WebAttribute<defraimp_itahc_Select, { defraimp_placeofdestinationtype: defraimp_placeofdestinationtype | null }, { defraimp_placeofdestinationtype_formatted?: string }>;
    defraimp_placeoforiginaddresscity: WebAttribute<defraimp_itahc_Select, { defraimp_placeoforiginaddresscity: string | null }, {  }>;
    defraimp_placeoforiginaddressline1: WebAttribute<defraimp_itahc_Select, { defraimp_placeoforiginaddressline1: string | null }, {  }>;
    defraimp_placeoforiginaddressline2: WebAttribute<defraimp_itahc_Select, { defraimp_placeoforiginaddressline2: string | null }, {  }>;
    defraimp_placeoforiginaddressline3: WebAttribute<defraimp_itahc_Select, { defraimp_placeoforiginaddressline3: string | null }, {  }>;
    defraimp_placeoforiginaddresspostcode: WebAttribute<defraimp_itahc_Select, { defraimp_placeoforiginaddresspostcode: string | null }, {  }>;
    defraimp_placeoforiginname: WebAttribute<defraimp_itahc_Select, { defraimp_placeoforiginname: string | null }, {  }>;
    defraimp_purpose: WebAttribute<defraimp_itahc_Select, { defraimp_purpose: string | null }, {  }>;
    defraimp_quantity: WebAttribute<defraimp_itahc_Select, { defraimp_quantity: number | null }, {  }>;
    defraimp_regionoforigin: WebAttribute<defraimp_itahc_Select, { defraimp_regionoforigin: string | null }, {  }>;
    defraimp_replacedbyid_guid: WebAttribute<defraimp_itahc_Select, { defraimp_replacedbyid_guid: string | null }, { defraimp_replacedbyid_formatted?: string }>;
    defraimp_replacesid_guid: WebAttribute<defraimp_itahc_Select, { defraimp_replacesid_guid: string | null }, { defraimp_replacesid_formatted?: string }>;
    defraimp_tracesreceiveddate: WebAttribute<defraimp_itahc_Select, { defraimp_tracesreceiveddate: Date | null }, { defraimp_tracesreceiveddate_formatted?: string }>;
    defraimp_unit: WebAttribute<defraimp_itahc_Select, { defraimp_unit: string | null }, {  }>;
    importsequencenumber: WebAttribute<defraimp_itahc_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraimp_itahc_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraimp_itahc_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraimp_itahc_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraimp_itahc_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    ownerid_guid: WebAttribute<defraimp_itahc_Select, { ownerid_guid: string | null }, { ownerid_formatted?: string }>;
    owningbusinessunit_guid: WebAttribute<defraimp_itahc_Select, { owningbusinessunit_guid: string | null }, { owningbusinessunit_formatted?: string }>;
    owningteam_guid: WebAttribute<defraimp_itahc_Select, { owningteam_guid: string | null }, { owningteam_formatted?: string }>;
    owninguser_guid: WebAttribute<defraimp_itahc_Select, { owninguser_guid: string | null }, { owninguser_formatted?: string }>;
    statecode: WebAttribute<defraimp_itahc_Select, { statecode: defraimp_itahc_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraimp_itahc_Select, { statuscode: defraimp_itahc_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraimp_itahc_Select, { timezoneruleversionnumber: number | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraimp_itahc_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraimp_itahc_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_itahc_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defraimp_animalspeciesproduct: string;
    defraimp_certifiedfor: string;
    defraimp_commoditytypeid_guid: XQW.Guid;
    defraimp_countryoforigin_guid: XQW.Guid;
    defraimp_datecertificatereceived: Date;
    defraimp_datetimeofdeparture: Date;
    defraimp_destinationaddresscity: string;
    defraimp_destinationaddresscounty: string;
    defraimp_destinationaddressline1: string;
    defraimp_destinationaddressline2: string;
    defraimp_destinationaddressline3: string;
    defraimp_destinationaddresspostcode: string;
    defraimp_destinationcountry: defraimp_ukcountries;
    defraimp_estimatedjourneytimedays: number;
    defraimp_estimatedjourneytimehours: any;
    defraimp_healthcertificatenumber: string;
    defraimp_identificationofanimals: string;
    defraimp_itahcid: XQW.Guid;
    defraimp_localveterinaryunit: string;
    defraimp_lvuno: string;
    defraimp_name: string;
    defraimp_numberofpackages: number;
    defraimp_ovname: string;
    defraimp_placeofdestinationapprovalnumber: string;
    defraimp_placeofdestinationtype: defraimp_placeofdestinationtype;
    defraimp_placeoforiginaddresscity: string;
    defraimp_placeoforiginaddressline1: string;
    defraimp_placeoforiginaddressline2: string;
    defraimp_placeoforiginaddressline3: string;
    defraimp_placeoforiginaddresspostcode: string;
    defraimp_placeoforiginname: string;
    defraimp_purpose: string;
    defraimp_quantity: number;
    defraimp_regionoforigin: string;
    defraimp_replacedbyid_guid: XQW.Guid;
    defraimp_replacesid_guid: XQW.Guid;
    defraimp_tracesreceiveddate: Date;
    defraimp_unit: string;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    overriddencreatedon: Date;
    ownerid_guid: XQW.Guid;
    owningbusinessunit_guid: XQW.Guid;
    owningteam_guid: XQW.Guid;
    owninguser_guid: XQW.Guid;
    statecode: defraimp_itahc_statecode;
    statuscode: defraimp_itahc_statuscode;
    timezoneruleversionnumber: number;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraimp_itahc_Expand {
    createdby: WebExpand<defraimp_itahc_Expand, SystemUser_Select, SystemUser_Filter, { createdby: SystemUser_Result }>;
    createdonbehalfby: WebExpand<defraimp_itahc_Expand, SystemUser_Select, SystemUser_Filter, { createdonbehalfby: SystemUser_Result }>;
    defraimp_CommodityTypeId: WebExpand<defraimp_itahc_Expand, defraexp_commoditytype_Select, defraexp_commoditytype_Filter, { defraimp_CommodityTypeId: defraexp_commoditytype_Result }>;
    defraimp_CountryofOrigin: WebExpand<defraimp_itahc_Expand, defra_country_Select, defra_country_Filter, { defraimp_CountryofOrigin: defra_country_Result }>;
    defraimp_ReplacedById: WebExpand<defraimp_itahc_Expand, defraimp_itahc_Select, defraimp_itahc_Filter, { defraimp_ReplacedById: defraimp_itahc_Result }>;
    defraimp_ReplacesId: WebExpand<defraimp_itahc_Expand, defraimp_itahc_Select, defraimp_itahc_Filter, { defraimp_ReplacesId: defraimp_itahc_Result }>;
    defraimp_defraimp_importapplication_defraimp_itahc: WebExpand<defraimp_itahc_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defraimp_importapplication_defraimp_itahc: defraimp_importapplication_Result[] }>;
    defraimp_defraimp_itahc_defraimp_importapplication: WebExpand<defraimp_itahc_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defraimp_itahc_defraimp_importapplication: defraimp_importapplication_Result[] }>;
    defraimp_defraimp_itahc_defraimp_importinspection_RelatedITAHC: WebExpand<defraimp_itahc_Expand, defraimp_importinspection_Select, defraimp_importinspection_Filter, { defraimp_defraimp_itahc_defraimp_importinspection_RelatedITAHC: defraimp_importinspection_Result[] }>;
    defraimp_defraimp_itahc_defraimp_importquery_ITAHC: WebExpand<defraimp_itahc_Expand, defraimp_importquery_Select, defraimp_importquery_Filter, { defraimp_defraimp_itahc_defraimp_importquery_ITAHC: defraimp_importquery_Result[] }>;
    defraimp_defraimp_itahc_defraimp_itahc: WebExpand<defraimp_itahc_Expand, defraimp_itahc_Select, defraimp_itahc_Filter, { defraimp_defraimp_itahc_defraimp_itahc: defraimp_itahc_Result[] }>;
    defraimp_defraimp_itahc_defraimp_rb: WebExpand<defraimp_itahc_Expand, defraimp_itahc_Select, defraimp_itahc_Filter, { defraimp_defraimp_itahc_defraimp_rb: defraimp_itahc_Result[] }>;
    defraimp_itahc_ProcessSession: WebExpand<defraimp_itahc_Expand, ProcessSession_Select, ProcessSession_Filter, { defraimp_itahc_ProcessSession: ProcessSession_Result[] }>;
    defraimp_itahc_SyncErrors: WebExpand<defraimp_itahc_Expand, SyncError_Select, SyncError_Filter, { defraimp_itahc_SyncErrors: SyncError_Result[] }>;
    modifiedby: WebExpand<defraimp_itahc_Expand, SystemUser_Select, SystemUser_Filter, { modifiedby: SystemUser_Result }>;
    modifiedonbehalfby: WebExpand<defraimp_itahc_Expand, SystemUser_Select, SystemUser_Filter, { modifiedonbehalfby: SystemUser_Result }>;
    owningteam: WebExpand<defraimp_itahc_Expand, Team_Select, Team_Filter, { owningteam: Team_Result }>;
    owninguser: WebExpand<defraimp_itahc_Expand, SystemUser_Select, SystemUser_Filter, { owninguser: SystemUser_Result }>;
  }
  interface defraimp_itahc_FormattedResult {
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    defraimp_commoditytypeid_formatted?: string;
    defraimp_countryoforigin_formatted?: string;
    defraimp_datecertificatereceived_formatted?: string;
    defraimp_datetimeofdeparture_formatted?: string;
    defraimp_destinationcountry_formatted?: string;
    defraimp_placeofdestinationtype_formatted?: string;
    defraimp_replacedbyid_formatted?: string;
    defraimp_replacesid_formatted?: string;
    defraimp_tracesreceiveddate_formatted?: string;
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
  interface defraimp_itahc_Result extends defraimp_itahc_Base, defraimp_itahc_Relationships {
    "@odata.etag": string;
    createdby_guid: string | null;
    createdonbehalfby_guid: string | null;
    defraimp_commoditytypeid_guid: string | null;
    defraimp_countryoforigin_guid: string | null;
    defraimp_replacedbyid_guid: string | null;
    defraimp_replacesid_guid: string | null;
    modifiedby_guid: string | null;
    modifiedonbehalfby_guid: string | null;
    ownerid_guid: string | null;
    owningbusinessunit_guid: string | null;
    owningteam_guid: string | null;
    owninguser_guid: string | null;
  }
  interface defraimp_itahc_RelatedOne {
    createdby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    createdonbehalfby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    defraimp_CommodityTypeId: WebMappingRetrieve<WebApi.defraexp_commoditytype_Select,WebApi.defraexp_commoditytype_Expand,WebApi.defraexp_commoditytype_Filter,WebApi.defraexp_commoditytype_Fixed,WebApi.defraexp_commoditytype_Result,WebApi.defraexp_commoditytype_FormattedResult>;
    defraimp_CountryofOrigin: WebMappingRetrieve<WebApi.defra_country_Select,WebApi.defra_country_Expand,WebApi.defra_country_Filter,WebApi.defra_country_Fixed,WebApi.defra_country_Result,WebApi.defra_country_FormattedResult>;
    defraimp_ReplacedById: WebMappingRetrieve<WebApi.defraimp_itahc_Select,WebApi.defraimp_itahc_Expand,WebApi.defraimp_itahc_Filter,WebApi.defraimp_itahc_Fixed,WebApi.defraimp_itahc_Result,WebApi.defraimp_itahc_FormattedResult>;
    defraimp_ReplacesId: WebMappingRetrieve<WebApi.defraimp_itahc_Select,WebApi.defraimp_itahc_Expand,WebApi.defraimp_itahc_Filter,WebApi.defraimp_itahc_Fixed,WebApi.defraimp_itahc_Result,WebApi.defraimp_itahc_FormattedResult>;
    modifiedby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    modifiedonbehalfby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    owningteam: WebMappingRetrieve<WebApi.Team_Select,WebApi.Team_Expand,WebApi.Team_Filter,WebApi.Team_Fixed,WebApi.Team_Result,WebApi.Team_FormattedResult>;
    owninguser: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
  }
  interface defraimp_itahc_RelatedMany {
    defraimp_defraimp_importapplication_defraimp_itahc: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_defraimp_itahc_defraimp_importapplication: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_defraimp_itahc_defraimp_importinspection_RelatedITAHC: WebMappingRetrieve<WebApi.defraimp_importinspection_Select,WebApi.defraimp_importinspection_Expand,WebApi.defraimp_importinspection_Filter,WebApi.defraimp_importinspection_Fixed,WebApi.defraimp_importinspection_Result,WebApi.defraimp_importinspection_FormattedResult>;
    defraimp_defraimp_itahc_defraimp_importquery_ITAHC: WebMappingRetrieve<WebApi.defraimp_importquery_Select,WebApi.defraimp_importquery_Expand,WebApi.defraimp_importquery_Filter,WebApi.defraimp_importquery_Fixed,WebApi.defraimp_importquery_Result,WebApi.defraimp_importquery_FormattedResult>;
    defraimp_defraimp_itahc_defraimp_itahc: WebMappingRetrieve<WebApi.defraimp_itahc_Select,WebApi.defraimp_itahc_Expand,WebApi.defraimp_itahc_Filter,WebApi.defraimp_itahc_Fixed,WebApi.defraimp_itahc_Result,WebApi.defraimp_itahc_FormattedResult>;
    defraimp_defraimp_itahc_defraimp_rb: WebMappingRetrieve<WebApi.defraimp_itahc_Select,WebApi.defraimp_itahc_Expand,WebApi.defraimp_itahc_Filter,WebApi.defraimp_itahc_Fixed,WebApi.defraimp_itahc_Result,WebApi.defraimp_itahc_FormattedResult>;
    defraimp_itahc_ProcessSession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    defraimp_itahc_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_itahcs: WebMappingRetrieve<WebApi.defraimp_itahc_Select,WebApi.defraimp_itahc_Expand,WebApi.defraimp_itahc_Filter,WebApi.defraimp_itahc_Fixed,WebApi.defraimp_itahc_Result,WebApi.defraimp_itahc_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_itahcs: WebMappingRelated<WebApi.defraimp_itahc_RelatedOne,WebApi.defraimp_itahc_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_itahcs: WebMappingCUDA<WebApi.defraimp_itahc_Create,WebApi.defraimp_itahc_Update,WebApi.defraimp_itahc_Select>;
}
