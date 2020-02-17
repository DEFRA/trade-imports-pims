declare namespace WebApi {
  interface defraimp_goldbronzecommodity_Base extends WebEntity {
    createdon?: Date | null;
    defraimp_goldbronzecommodityid?: string | null;
    defraimp_name?: string | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defraimp_goldbronzecommodity_statecode | null;
    statuscode?: defraimp_goldbronzecommodity_statuscode | null;
    timezoneruleversionnumber?: number | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraimp_goldbronzecommodity_Relationships {
    defraimp_CommodityTypeid?: defraexp_commoditytype_Result | null;
    defraimp_goldbronzecommodity_ProcessSession?: ProcessSession_Result[] | null;
    defraimp_goldbronzecommodity_SyncErrors?: SyncError_Result[] | null;
    defraimp_goldbronzecountriesnn?: defra_country_Result[] | null;
  }
  interface defraimp_goldbronzecommodity extends defraimp_goldbronzecommodity_Base, defraimp_goldbronzecommodity_Relationships {
    defraimp_CommodityTypeid_bind$defraexp_commoditytypes?: string | null;
    ownerid_bind$owners?: string | null;
  }
  interface defraimp_goldbronzecommodity_Create extends defraimp_goldbronzecommodity {
  }
  interface defraimp_goldbronzecommodity_Update extends defraimp_goldbronzecommodity {
  }
  interface defraimp_goldbronzecommodity_Select {
    createdby_guid: WebAttribute<defraimp_goldbronzecommodity_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraimp_goldbronzecommodity_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraimp_goldbronzecommodity_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defraimp_commoditytypeid_guid: WebAttribute<defraimp_goldbronzecommodity_Select, { defraimp_commoditytypeid_guid: string | null }, { defraimp_commoditytypeid_formatted?: string }>;
    defraimp_goldbronzecommodityid: WebAttribute<defraimp_goldbronzecommodity_Select, { defraimp_goldbronzecommodityid: string | null }, {  }>;
    defraimp_name: WebAttribute<defraimp_goldbronzecommodity_Select, { defraimp_name: string | null }, {  }>;
    importsequencenumber: WebAttribute<defraimp_goldbronzecommodity_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraimp_goldbronzecommodity_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraimp_goldbronzecommodity_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraimp_goldbronzecommodity_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraimp_goldbronzecommodity_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    ownerid_guid: WebAttribute<defraimp_goldbronzecommodity_Select, { ownerid_guid: string | null }, { ownerid_formatted?: string }>;
    owningbusinessunit_guid: WebAttribute<defraimp_goldbronzecommodity_Select, { owningbusinessunit_guid: string | null }, { owningbusinessunit_formatted?: string }>;
    owningteam_guid: WebAttribute<defraimp_goldbronzecommodity_Select, { owningteam_guid: string | null }, { owningteam_formatted?: string }>;
    owninguser_guid: WebAttribute<defraimp_goldbronzecommodity_Select, { owninguser_guid: string | null }, { owninguser_formatted?: string }>;
    statecode: WebAttribute<defraimp_goldbronzecommodity_Select, { statecode: defraimp_goldbronzecommodity_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraimp_goldbronzecommodity_Select, { statuscode: defraimp_goldbronzecommodity_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraimp_goldbronzecommodity_Select, { timezoneruleversionnumber: number | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraimp_goldbronzecommodity_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraimp_goldbronzecommodity_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_goldbronzecommodity_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defraimp_commoditytypeid_guid: XQW.Guid;
    defraimp_goldbronzecommodityid: XQW.Guid;
    defraimp_name: string;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    overriddencreatedon: Date;
    ownerid_guid: XQW.Guid;
    owningbusinessunit_guid: XQW.Guid;
    owningteam_guid: XQW.Guid;
    owninguser_guid: XQW.Guid;
    statecode: defraimp_goldbronzecommodity_statecode;
    statuscode: defraimp_goldbronzecommodity_statuscode;
    timezoneruleversionnumber: number;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraimp_goldbronzecommodity_Expand {
    defraimp_CommodityTypeid: WebExpand<defraimp_goldbronzecommodity_Expand, defraexp_commoditytype_Select, defraexp_commoditytype_Filter, { defraimp_CommodityTypeid: defraexp_commoditytype_Result }>;
    defraimp_goldbronzecommodity_ProcessSession: WebExpand<defraimp_goldbronzecommodity_Expand, ProcessSession_Select, ProcessSession_Filter, { defraimp_goldbronzecommodity_ProcessSession: ProcessSession_Result[] }>;
    defraimp_goldbronzecommodity_SyncErrors: WebExpand<defraimp_goldbronzecommodity_Expand, SyncError_Select, SyncError_Filter, { defraimp_goldbronzecommodity_SyncErrors: SyncError_Result[] }>;
    defraimp_goldbronzecountriesnn: WebExpand<defraimp_goldbronzecommodity_Expand, defra_country_Select, defra_country_Filter, { defraimp_goldbronzecountriesnn: defra_country_Result[] }>;
  }
  interface defraimp_goldbronzecommodity_FormattedResult {
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    defraimp_commoditytypeid_formatted?: string;
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
  interface defraimp_goldbronzecommodity_Result extends defraimp_goldbronzecommodity_Base, defraimp_goldbronzecommodity_Relationships {
    "@odata.etag": string;
    createdby_guid: string | null;
    createdonbehalfby_guid: string | null;
    defraimp_commoditytypeid_guid: string | null;
    modifiedby_guid: string | null;
    modifiedonbehalfby_guid: string | null;
    ownerid_guid: string | null;
    owningbusinessunit_guid: string | null;
    owningteam_guid: string | null;
    owninguser_guid: string | null;
  }
  interface defraimp_goldbronzecommodity_RelatedOne {
    defraimp_CommodityTypeid: WebMappingRetrieve<WebApi.defraexp_commoditytype_Select,WebApi.defraexp_commoditytype_Expand,WebApi.defraexp_commoditytype_Filter,WebApi.defraexp_commoditytype_Fixed,WebApi.defraexp_commoditytype_Result,WebApi.defraexp_commoditytype_FormattedResult>;
  }
  interface defraimp_goldbronzecommodity_RelatedMany {
    defraimp_goldbronzecommodity_ProcessSession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    defraimp_goldbronzecommodity_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
    defraimp_goldbronzecountriesnn: WebMappingRetrieve<WebApi.defra_country_Select,WebApi.defra_country_Expand,WebApi.defra_country_Filter,WebApi.defra_country_Fixed,WebApi.defra_country_Result,WebApi.defra_country_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_goldbronzecommodities: WebMappingRetrieve<WebApi.defraimp_goldbronzecommodity_Select,WebApi.defraimp_goldbronzecommodity_Expand,WebApi.defraimp_goldbronzecommodity_Filter,WebApi.defraimp_goldbronzecommodity_Fixed,WebApi.defraimp_goldbronzecommodity_Result,WebApi.defraimp_goldbronzecommodity_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_goldbronzecommodities: WebMappingRelated<WebApi.defraimp_goldbronzecommodity_RelatedOne,WebApi.defraimp_goldbronzecommodity_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_goldbronzecommodities: WebMappingCUDA<WebApi.defraimp_goldbronzecommodity_Create,WebApi.defraimp_goldbronzecommodity_Update,WebApi.defraimp_goldbronzecommodity_Select>;
}
