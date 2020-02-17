declare namespace WebApi {
  interface defraimp_importcountrycommodityrisklevel_Base extends WebEntity {
    createdon?: Date | null;
    defraimp_importcountrycommodityrisklevelid?: string | null;
    defraimp_name?: string | null;
    defraimp_risklevelnotes?: string | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defraimp_importcountrycommodityrisklevel_statecode | null;
    statuscode?: defraimp_importcountrycommodityrisklevel_statuscode | null;
    timezoneruleversionnumber?: number | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraimp_importcountrycommodityrisklevel_Relationships {
    defraimp_importcountrycommodityrisklevel_ProcessSession?: ProcessSession_Result[] | null;
    defraimp_importcountrycommodityrisklevel_SyncErrors?: SyncError_Result[] | null;
  }
  interface defraimp_importcountrycommodityrisklevel extends defraimp_importcountrycommodityrisklevel_Base, defraimp_importcountrycommodityrisklevel_Relationships {
    defraimp_commoditytypeid_bind$defraexp_commoditytypes?: string | null;
    defraimp_countryid_bind$defra_countries?: string | null;
    defraimp_importrisklevelid_bind$defraimp_importrisklevels?: string | null;
  }
  interface defraimp_importcountrycommodityrisklevel_Create extends defraimp_importcountrycommodityrisklevel {
  }
  interface defraimp_importcountrycommodityrisklevel_Update extends defraimp_importcountrycommodityrisklevel {
  }
  interface defraimp_importcountrycommodityrisklevel_Select {
    createdby_guid: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defraimp_commoditytypeid_guid: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { defraimp_commoditytypeid_guid: string | null }, { defraimp_commoditytypeid_formatted?: string }>;
    defraimp_countryid_guid: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { defraimp_countryid_guid: string | null }, { defraimp_countryid_formatted?: string }>;
    defraimp_importcountrycommodityrisklevelid: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { defraimp_importcountrycommodityrisklevelid: string | null }, {  }>;
    defraimp_importrisklevelid_guid: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { defraimp_importrisklevelid_guid: string | null }, { defraimp_importrisklevelid_formatted?: string }>;
    defraimp_name: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { defraimp_name: string | null }, {  }>;
    defraimp_risklevelnotes: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { defraimp_risklevelnotes: string | null }, {  }>;
    importsequencenumber: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    organizationid_guid: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { organizationid_guid: string | null }, { organizationid_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    statecode: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { statecode: defraimp_importcountrycommodityrisklevel_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { statuscode: defraimp_importcountrycommodityrisklevel_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { timezoneruleversionnumber: number | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraimp_importcountrycommodityrisklevel_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_importcountrycommodityrisklevel_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defraimp_commoditytypeid_guid: XQW.Guid;
    defraimp_countryid_guid: XQW.Guid;
    defraimp_importcountrycommodityrisklevelid: XQW.Guid;
    defraimp_importrisklevelid_guid: XQW.Guid;
    defraimp_name: string;
    defraimp_risklevelnotes: string;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    organizationid_guid: XQW.Guid;
    overriddencreatedon: Date;
    statecode: defraimp_importcountrycommodityrisklevel_statecode;
    statuscode: defraimp_importcountrycommodityrisklevel_statuscode;
    timezoneruleversionnumber: number;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraimp_importcountrycommodityrisklevel_Expand {
    defraimp_commoditytypeid: WebExpand<defraimp_importcountrycommodityrisklevel_Expand, defraexp_commoditytype_Select, defraexp_commoditytype_Filter, { defraimp_commoditytypeid: defraexp_commoditytype_Result }>;
    defraimp_countryid: WebExpand<defraimp_importcountrycommodityrisklevel_Expand, defra_country_Select, defra_country_Filter, { defraimp_countryid: defra_country_Result }>;
    defraimp_importcountrycommodityrisklevel_ProcessSession: WebExpand<defraimp_importcountrycommodityrisklevel_Expand, ProcessSession_Select, ProcessSession_Filter, { defraimp_importcountrycommodityrisklevel_ProcessSession: ProcessSession_Result[] }>;
    defraimp_importcountrycommodityrisklevel_SyncErrors: WebExpand<defraimp_importcountrycommodityrisklevel_Expand, SyncError_Select, SyncError_Filter, { defraimp_importcountrycommodityrisklevel_SyncErrors: SyncError_Result[] }>;
    defraimp_importrisklevelid: WebExpand<defraimp_importcountrycommodityrisklevel_Expand, defraimp_importrisklevel_Select, defraimp_importrisklevel_Filter, { defraimp_importrisklevelid: defraimp_importrisklevel_Result }>;
  }
  interface defraimp_importcountrycommodityrisklevel_FormattedResult {
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    defraimp_commoditytypeid_formatted?: string;
    defraimp_countryid_formatted?: string;
    defraimp_importrisklevelid_formatted?: string;
    modifiedby_formatted?: string;
    modifiedon_formatted?: string;
    modifiedonbehalfby_formatted?: string;
    organizationid_formatted?: string;
    overriddencreatedon_formatted?: string;
    statecode_formatted?: string;
    statuscode_formatted?: string;
  }
  interface defraimp_importcountrycommodityrisklevel_Result extends defraimp_importcountrycommodityrisklevel_Base, defraimp_importcountrycommodityrisklevel_Relationships {
    "@odata.etag": string;
    createdby_guid: string | null;
    createdonbehalfby_guid: string | null;
    defraimp_commoditytypeid_guid: string | null;
    defraimp_countryid_guid: string | null;
    defraimp_importrisklevelid_guid: string | null;
    modifiedby_guid: string | null;
    modifiedonbehalfby_guid: string | null;
    organizationid_guid: string | null;
  }
  interface defraimp_importcountrycommodityrisklevel_RelatedOne {
    defraimp_commoditytypeid: WebMappingRetrieve<WebApi.defraexp_commoditytype_Select,WebApi.defraexp_commoditytype_Expand,WebApi.defraexp_commoditytype_Filter,WebApi.defraexp_commoditytype_Fixed,WebApi.defraexp_commoditytype_Result,WebApi.defraexp_commoditytype_FormattedResult>;
    defraimp_countryid: WebMappingRetrieve<WebApi.defra_country_Select,WebApi.defra_country_Expand,WebApi.defra_country_Filter,WebApi.defra_country_Fixed,WebApi.defra_country_Result,WebApi.defra_country_FormattedResult>;
    defraimp_importrisklevelid: WebMappingRetrieve<WebApi.defraimp_importrisklevel_Select,WebApi.defraimp_importrisklevel_Expand,WebApi.defraimp_importrisklevel_Filter,WebApi.defraimp_importrisklevel_Fixed,WebApi.defraimp_importrisklevel_Result,WebApi.defraimp_importrisklevel_FormattedResult>;
  }
  interface defraimp_importcountrycommodityrisklevel_RelatedMany {
    defraimp_importcountrycommodityrisklevel_ProcessSession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    defraimp_importcountrycommodityrisklevel_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_importcountrycommodityrisklevels: WebMappingRetrieve<WebApi.defraimp_importcountrycommodityrisklevel_Select,WebApi.defraimp_importcountrycommodityrisklevel_Expand,WebApi.defraimp_importcountrycommodityrisklevel_Filter,WebApi.defraimp_importcountrycommodityrisklevel_Fixed,WebApi.defraimp_importcountrycommodityrisklevel_Result,WebApi.defraimp_importcountrycommodityrisklevel_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_importcountrycommodityrisklevels: WebMappingRelated<WebApi.defraimp_importcountrycommodityrisklevel_RelatedOne,WebApi.defraimp_importcountrycommodityrisklevel_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_importcountrycommodityrisklevels: WebMappingCUDA<WebApi.defraimp_importcountrycommodityrisklevel_Create,WebApi.defraimp_importcountrycommodityrisklevel_Update,WebApi.defraimp_importcountrycommodityrisklevel_Select>;
}
