declare namespace WebApi {
  interface defraimp_sampletest_Base extends WebEntity {
    createdon?: Date | null;
    defraimp_name?: string | null;
    defraimp_sampletestid?: string | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defraimp_sampletest_statecode | null;
    statuscode?: defraimp_sampletest_statuscode | null;
    timezoneruleversionnumber?: number | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraimp_sampletest_Relationships {
    defraimp_defraimp_importinspection_defraimp_sample?: defraimp_importinspection_Result[] | null;
    defraimp_sampletest_ProcessSession?: ProcessSession_Result[] | null;
    defraimp_sampletest_SyncErrors?: SyncError_Result[] | null;
  }
  interface defraimp_sampletest extends defraimp_sampletest_Base, defraimp_sampletest_Relationships {
    ownerid_bind$owners?: string | null;
  }
  interface defraimp_sampletest_Create extends defraimp_sampletest {
  }
  interface defraimp_sampletest_Update extends defraimp_sampletest {
  }
  interface defraimp_sampletest_Select {
    createdby_guid: WebAttribute<defraimp_sampletest_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraimp_sampletest_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraimp_sampletest_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defraimp_name: WebAttribute<defraimp_sampletest_Select, { defraimp_name: string | null }, {  }>;
    defraimp_sampletestid: WebAttribute<defraimp_sampletest_Select, { defraimp_sampletestid: string | null }, {  }>;
    importsequencenumber: WebAttribute<defraimp_sampletest_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraimp_sampletest_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraimp_sampletest_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraimp_sampletest_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraimp_sampletest_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    ownerid_guid: WebAttribute<defraimp_sampletest_Select, { ownerid_guid: string | null }, { ownerid_formatted?: string }>;
    owningbusinessunit_guid: WebAttribute<defraimp_sampletest_Select, { owningbusinessunit_guid: string | null }, { owningbusinessunit_formatted?: string }>;
    owningteam_guid: WebAttribute<defraimp_sampletest_Select, { owningteam_guid: string | null }, { owningteam_formatted?: string }>;
    owninguser_guid: WebAttribute<defraimp_sampletest_Select, { owninguser_guid: string | null }, { owninguser_formatted?: string }>;
    statecode: WebAttribute<defraimp_sampletest_Select, { statecode: defraimp_sampletest_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraimp_sampletest_Select, { statuscode: defraimp_sampletest_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraimp_sampletest_Select, { timezoneruleversionnumber: number | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraimp_sampletest_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraimp_sampletest_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_sampletest_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defraimp_name: string;
    defraimp_sampletestid: XQW.Guid;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    overriddencreatedon: Date;
    ownerid_guid: XQW.Guid;
    owningbusinessunit_guid: XQW.Guid;
    owningteam_guid: XQW.Guid;
    owninguser_guid: XQW.Guid;
    statecode: defraimp_sampletest_statecode;
    statuscode: defraimp_sampletest_statuscode;
    timezoneruleversionnumber: number;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraimp_sampletest_Expand {
    createdby: WebExpand<defraimp_sampletest_Expand, SystemUser_Select, SystemUser_Filter, { createdby: SystemUser_Result }>;
    createdonbehalfby: WebExpand<defraimp_sampletest_Expand, SystemUser_Select, SystemUser_Filter, { createdonbehalfby: SystemUser_Result }>;
    defraimp_defraimp_importinspection_defraimp_sample: WebExpand<defraimp_sampletest_Expand, defraimp_importinspection_Select, defraimp_importinspection_Filter, { defraimp_defraimp_importinspection_defraimp_sample: defraimp_importinspection_Result[] }>;
    defraimp_sampletest_ProcessSession: WebExpand<defraimp_sampletest_Expand, ProcessSession_Select, ProcessSession_Filter, { defraimp_sampletest_ProcessSession: ProcessSession_Result[] }>;
    defraimp_sampletest_SyncErrors: WebExpand<defraimp_sampletest_Expand, SyncError_Select, SyncError_Filter, { defraimp_sampletest_SyncErrors: SyncError_Result[] }>;
    modifiedby: WebExpand<defraimp_sampletest_Expand, SystemUser_Select, SystemUser_Filter, { modifiedby: SystemUser_Result }>;
    modifiedonbehalfby: WebExpand<defraimp_sampletest_Expand, SystemUser_Select, SystemUser_Filter, { modifiedonbehalfby: SystemUser_Result }>;
    owningteam: WebExpand<defraimp_sampletest_Expand, Team_Select, Team_Filter, { owningteam: Team_Result }>;
    owninguser: WebExpand<defraimp_sampletest_Expand, SystemUser_Select, SystemUser_Filter, { owninguser: SystemUser_Result }>;
  }
  interface defraimp_sampletest_FormattedResult {
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
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
  interface defraimp_sampletest_Result extends defraimp_sampletest_Base, defraimp_sampletest_Relationships {
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
  interface defraimp_sampletest_RelatedOne {
    createdby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    createdonbehalfby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    modifiedby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    modifiedonbehalfby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    owningteam: WebMappingRetrieve<WebApi.Team_Select,WebApi.Team_Expand,WebApi.Team_Filter,WebApi.Team_Fixed,WebApi.Team_Result,WebApi.Team_FormattedResult>;
    owninguser: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
  }
  interface defraimp_sampletest_RelatedMany {
    defraimp_defraimp_importinspection_defraimp_sample: WebMappingRetrieve<WebApi.defraimp_importinspection_Select,WebApi.defraimp_importinspection_Expand,WebApi.defraimp_importinspection_Filter,WebApi.defraimp_importinspection_Fixed,WebApi.defraimp_importinspection_Result,WebApi.defraimp_importinspection_FormattedResult>;
    defraimp_sampletest_ProcessSession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    defraimp_sampletest_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_sampletests: WebMappingRetrieve<WebApi.defraimp_sampletest_Select,WebApi.defraimp_sampletest_Expand,WebApi.defraimp_sampletest_Filter,WebApi.defraimp_sampletest_Fixed,WebApi.defraimp_sampletest_Result,WebApi.defraimp_sampletest_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_sampletests: WebMappingRelated<WebApi.defraimp_sampletest_RelatedOne,WebApi.defraimp_sampletest_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_sampletests: WebMappingCUDA<WebApi.defraimp_sampletest_Create,WebApi.defraimp_sampletest_Update,WebApi.defraimp_sampletest_Select>;
}
