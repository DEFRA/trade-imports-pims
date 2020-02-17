declare namespace WebApi {
  interface defraimp_importrisklevel_Base extends WebEntity {
    createdon?: Date | null;
    defraimp_importrisklevelid?: string | null;
    defraimp_name?: string | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defraimp_importrisklevel_statecode | null;
    statuscode?: defraimp_importrisklevel_statuscode | null;
    timezoneruleversionnumber?: number | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraimp_importrisklevel_Relationships {
    defraimp_defraimp_importrisklevel_defraimp_importapplication_PreviousImportRiskLevelId?: defraimp_importapplication_Result[] | null;
    defraimp_defraimp_importrisklevel_defraimp_importapplication_importrisklevelid?: defraimp_importapplication_Result[] | null;
    defraimp_defraimp_importrisklevel_defraimp_importcountrycommodityrisklevel_risklevelid?: defraimp_importcountrycommodityrisklevel_Result[] | null;
    defraimp_defraimp_importrisklevel_defraimp_inspectioncoveragerule_RiskLevelId?: defraimp_inspectioncoveragerule_Result[] | null;
    defraimp_importrisklevel_ProcessSession?: ProcessSession_Result[] | null;
    defraimp_importrisklevel_SyncErrors?: SyncError_Result[] | null;
  }
  interface defraimp_importrisklevel extends defraimp_importrisklevel_Base, defraimp_importrisklevel_Relationships {
  }
  interface defraimp_importrisklevel_Create extends defraimp_importrisklevel {
  }
  interface defraimp_importrisklevel_Update extends defraimp_importrisklevel {
  }
  interface defraimp_importrisklevel_Select {
    createdby_guid: WebAttribute<defraimp_importrisklevel_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraimp_importrisklevel_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraimp_importrisklevel_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defraimp_importrisklevelid: WebAttribute<defraimp_importrisklevel_Select, { defraimp_importrisklevelid: string | null }, {  }>;
    defraimp_name: WebAttribute<defraimp_importrisklevel_Select, { defraimp_name: string | null }, {  }>;
    importsequencenumber: WebAttribute<defraimp_importrisklevel_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraimp_importrisklevel_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraimp_importrisklevel_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraimp_importrisklevel_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    organizationid_guid: WebAttribute<defraimp_importrisklevel_Select, { organizationid_guid: string | null }, { organizationid_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraimp_importrisklevel_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    statecode: WebAttribute<defraimp_importrisklevel_Select, { statecode: defraimp_importrisklevel_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraimp_importrisklevel_Select, { statuscode: defraimp_importrisklevel_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraimp_importrisklevel_Select, { timezoneruleversionnumber: number | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraimp_importrisklevel_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraimp_importrisklevel_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_importrisklevel_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defraimp_importrisklevelid: XQW.Guid;
    defraimp_name: string;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    organizationid_guid: XQW.Guid;
    overriddencreatedon: Date;
    statecode: defraimp_importrisklevel_statecode;
    statuscode: defraimp_importrisklevel_statuscode;
    timezoneruleversionnumber: number;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraimp_importrisklevel_Expand {
    defraimp_defraimp_importrisklevel_defraimp_importapplication_PreviousImportRiskLevelId: WebExpand<defraimp_importrisklevel_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defraimp_importrisklevel_defraimp_importapplication_PreviousImportRiskLevelId: defraimp_importapplication_Result[] }>;
    defraimp_defraimp_importrisklevel_defraimp_importapplication_importrisklevelid: WebExpand<defraimp_importrisklevel_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defraimp_importrisklevel_defraimp_importapplication_importrisklevelid: defraimp_importapplication_Result[] }>;
    defraimp_defraimp_importrisklevel_defraimp_importcountrycommodityrisklevel_risklevelid: WebExpand<defraimp_importrisklevel_Expand, defraimp_importcountrycommodityrisklevel_Select, defraimp_importcountrycommodityrisklevel_Filter, { defraimp_defraimp_importrisklevel_defraimp_importcountrycommodityrisklevel_risklevelid: defraimp_importcountrycommodityrisklevel_Result[] }>;
    defraimp_defraimp_importrisklevel_defraimp_inspectioncoveragerule_RiskLevelId: WebExpand<defraimp_importrisklevel_Expand, defraimp_inspectioncoveragerule_Select, defraimp_inspectioncoveragerule_Filter, { defraimp_defraimp_importrisklevel_defraimp_inspectioncoveragerule_RiskLevelId: defraimp_inspectioncoveragerule_Result[] }>;
    defraimp_importrisklevel_ProcessSession: WebExpand<defraimp_importrisklevel_Expand, ProcessSession_Select, ProcessSession_Filter, { defraimp_importrisklevel_ProcessSession: ProcessSession_Result[] }>;
    defraimp_importrisklevel_SyncErrors: WebExpand<defraimp_importrisklevel_Expand, SyncError_Select, SyncError_Filter, { defraimp_importrisklevel_SyncErrors: SyncError_Result[] }>;
  }
  interface defraimp_importrisklevel_FormattedResult {
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    modifiedby_formatted?: string;
    modifiedon_formatted?: string;
    modifiedonbehalfby_formatted?: string;
    organizationid_formatted?: string;
    overriddencreatedon_formatted?: string;
    statecode_formatted?: string;
    statuscode_formatted?: string;
  }
  interface defraimp_importrisklevel_Result extends defraimp_importrisklevel_Base, defraimp_importrisklevel_Relationships {
    "@odata.etag": string;
    createdby_guid: string | null;
    createdonbehalfby_guid: string | null;
    modifiedby_guid: string | null;
    modifiedonbehalfby_guid: string | null;
    organizationid_guid: string | null;
  }
  interface defraimp_importrisklevel_RelatedOne {
  }
  interface defraimp_importrisklevel_RelatedMany {
    defraimp_defraimp_importrisklevel_defraimp_importapplication_PreviousImportRiskLevelId: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_defraimp_importrisklevel_defraimp_importapplication_importrisklevelid: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_defraimp_importrisklevel_defraimp_importcountrycommodityrisklevel_risklevelid: WebMappingRetrieve<WebApi.defraimp_importcountrycommodityrisklevel_Select,WebApi.defraimp_importcountrycommodityrisklevel_Expand,WebApi.defraimp_importcountrycommodityrisklevel_Filter,WebApi.defraimp_importcountrycommodityrisklevel_Fixed,WebApi.defraimp_importcountrycommodityrisklevel_Result,WebApi.defraimp_importcountrycommodityrisklevel_FormattedResult>;
    defraimp_defraimp_importrisklevel_defraimp_inspectioncoveragerule_RiskLevelId: WebMappingRetrieve<WebApi.defraimp_inspectioncoveragerule_Select,WebApi.defraimp_inspectioncoveragerule_Expand,WebApi.defraimp_inspectioncoveragerule_Filter,WebApi.defraimp_inspectioncoveragerule_Fixed,WebApi.defraimp_inspectioncoveragerule_Result,WebApi.defraimp_inspectioncoveragerule_FormattedResult>;
    defraimp_importrisklevel_ProcessSession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    defraimp_importrisklevel_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_importrisklevels: WebMappingRetrieve<WebApi.defraimp_importrisklevel_Select,WebApi.defraimp_importrisklevel_Expand,WebApi.defraimp_importrisklevel_Filter,WebApi.defraimp_importrisklevel_Fixed,WebApi.defraimp_importrisklevel_Result,WebApi.defraimp_importrisklevel_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_importrisklevels: WebMappingRelated<WebApi.defraimp_importrisklevel_RelatedOne,WebApi.defraimp_importrisklevel_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_importrisklevels: WebMappingCUDA<WebApi.defraimp_importrisklevel_Create,WebApi.defraimp_importrisklevel_Update,WebApi.defraimp_importrisklevel_Select>;
}
