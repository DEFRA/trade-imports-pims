declare namespace WebApi {
  interface defraimp_inspectioncoveragerule_Base extends WebEntity {
    createdon?: Date | null;
    defraimp_inspectioncoverageruleid?: string | null;
    defraimp_name?: string | null;
    defraimp_numberofrecordsuntilinspection?: number | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defraimp_inspectioncoveragerule_statecode | null;
    statuscode?: defraimp_inspectioncoveragerule_statuscode | null;
    timezoneruleversionnumber?: number | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraimp_inspectioncoveragerule_Relationships {
    defraimp_RiskLevelId?: defraimp_importrisklevel_Result | null;
    defraimp_inspectioncoveragerule_ProcessSession?: ProcessSession_Result[] | null;
    defraimp_inspectioncoveragerule_SyncErrors?: SyncError_Result[] | null;
  }
  interface defraimp_inspectioncoveragerule extends defraimp_inspectioncoveragerule_Base, defraimp_inspectioncoveragerule_Relationships {
    defraimp_RiskLevelId_bind$defraimp_importrisklevels?: string | null;
    ownerid_bind$owners?: string | null;
  }
  interface defraimp_inspectioncoveragerule_Create extends defraimp_inspectioncoveragerule {
  }
  interface defraimp_inspectioncoveragerule_Update extends defraimp_inspectioncoveragerule {
  }
  interface defraimp_inspectioncoveragerule_Select {
    createdby_guid: WebAttribute<defraimp_inspectioncoveragerule_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraimp_inspectioncoveragerule_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraimp_inspectioncoveragerule_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defraimp_inspectioncoverageruleid: WebAttribute<defraimp_inspectioncoveragerule_Select, { defraimp_inspectioncoverageruleid: string | null }, {  }>;
    defraimp_name: WebAttribute<defraimp_inspectioncoveragerule_Select, { defraimp_name: string | null }, {  }>;
    defraimp_numberofrecordsuntilinspection: WebAttribute<defraimp_inspectioncoveragerule_Select, { defraimp_numberofrecordsuntilinspection: number | null }, {  }>;
    defraimp_risklevelid_guid: WebAttribute<defraimp_inspectioncoveragerule_Select, { defraimp_risklevelid_guid: string | null }, { defraimp_risklevelid_formatted?: string }>;
    importsequencenumber: WebAttribute<defraimp_inspectioncoveragerule_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraimp_inspectioncoveragerule_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraimp_inspectioncoveragerule_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraimp_inspectioncoveragerule_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraimp_inspectioncoveragerule_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    ownerid_guid: WebAttribute<defraimp_inspectioncoveragerule_Select, { ownerid_guid: string | null }, { ownerid_formatted?: string }>;
    owningbusinessunit_guid: WebAttribute<defraimp_inspectioncoveragerule_Select, { owningbusinessunit_guid: string | null }, { owningbusinessunit_formatted?: string }>;
    owningteam_guid: WebAttribute<defraimp_inspectioncoveragerule_Select, { owningteam_guid: string | null }, { owningteam_formatted?: string }>;
    owninguser_guid: WebAttribute<defraimp_inspectioncoveragerule_Select, { owninguser_guid: string | null }, { owninguser_formatted?: string }>;
    statecode: WebAttribute<defraimp_inspectioncoveragerule_Select, { statecode: defraimp_inspectioncoveragerule_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraimp_inspectioncoveragerule_Select, { statuscode: defraimp_inspectioncoveragerule_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraimp_inspectioncoveragerule_Select, { timezoneruleversionnumber: number | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraimp_inspectioncoveragerule_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraimp_inspectioncoveragerule_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_inspectioncoveragerule_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defraimp_inspectioncoverageruleid: XQW.Guid;
    defraimp_name: string;
    defraimp_numberofrecordsuntilinspection: number;
    defraimp_risklevelid_guid: XQW.Guid;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    overriddencreatedon: Date;
    ownerid_guid: XQW.Guid;
    owningbusinessunit_guid: XQW.Guid;
    owningteam_guid: XQW.Guid;
    owninguser_guid: XQW.Guid;
    statecode: defraimp_inspectioncoveragerule_statecode;
    statuscode: defraimp_inspectioncoveragerule_statuscode;
    timezoneruleversionnumber: number;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraimp_inspectioncoveragerule_Expand {
    defraimp_RiskLevelId: WebExpand<defraimp_inspectioncoveragerule_Expand, defraimp_importrisklevel_Select, defraimp_importrisklevel_Filter, { defraimp_RiskLevelId: defraimp_importrisklevel_Result }>;
    defraimp_inspectioncoveragerule_ProcessSession: WebExpand<defraimp_inspectioncoveragerule_Expand, ProcessSession_Select, ProcessSession_Filter, { defraimp_inspectioncoveragerule_ProcessSession: ProcessSession_Result[] }>;
    defraimp_inspectioncoveragerule_SyncErrors: WebExpand<defraimp_inspectioncoveragerule_Expand, SyncError_Select, SyncError_Filter, { defraimp_inspectioncoveragerule_SyncErrors: SyncError_Result[] }>;
  }
  interface defraimp_inspectioncoveragerule_FormattedResult {
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    defraimp_risklevelid_formatted?: string;
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
  interface defraimp_inspectioncoveragerule_Result extends defraimp_inspectioncoveragerule_Base, defraimp_inspectioncoveragerule_Relationships {
    "@odata.etag": string;
    createdby_guid: string | null;
    createdonbehalfby_guid: string | null;
    defraimp_risklevelid_guid: string | null;
    modifiedby_guid: string | null;
    modifiedonbehalfby_guid: string | null;
    ownerid_guid: string | null;
    owningbusinessunit_guid: string | null;
    owningteam_guid: string | null;
    owninguser_guid: string | null;
  }
  interface defraimp_inspectioncoveragerule_RelatedOne {
    defraimp_RiskLevelId: WebMappingRetrieve<WebApi.defraimp_importrisklevel_Select,WebApi.defraimp_importrisklevel_Expand,WebApi.defraimp_importrisklevel_Filter,WebApi.defraimp_importrisklevel_Fixed,WebApi.defraimp_importrisklevel_Result,WebApi.defraimp_importrisklevel_FormattedResult>;
  }
  interface defraimp_inspectioncoveragerule_RelatedMany {
    defraimp_inspectioncoveragerule_ProcessSession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    defraimp_inspectioncoveragerule_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_inspectioncoveragerules: WebMappingRetrieve<WebApi.defraimp_inspectioncoveragerule_Select,WebApi.defraimp_inspectioncoveragerule_Expand,WebApi.defraimp_inspectioncoveragerule_Filter,WebApi.defraimp_inspectioncoveragerule_Fixed,WebApi.defraimp_inspectioncoveragerule_Result,WebApi.defraimp_inspectioncoveragerule_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_inspectioncoveragerules: WebMappingRelated<WebApi.defraimp_inspectioncoveragerule_RelatedOne,WebApi.defraimp_inspectioncoveragerule_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_inspectioncoveragerules: WebMappingCUDA<WebApi.defraimp_inspectioncoveragerule_Create,WebApi.defraimp_inspectioncoveragerule_Update,WebApi.defraimp_inspectioncoveragerule_Select>;
}
