declare namespace WebApi {
  interface defraimp_autonumber_Base extends WebEntity {
    createdon?: Date | null;
    defraimp_autonumberid?: string | null;
    defraimp_currentnumber?: number | null;
    defraimp_key?: string | null;
    defraimp_name?: string | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defraimp_autonumber_statecode | null;
    statuscode?: defraimp_autonumber_statuscode | null;
    timezoneruleversionnumber?: number | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraimp_autonumber_Relationships {
    defraimp_autonumber_ProcessSession?: ProcessSession_Result[] | null;
    defraimp_autonumber_SyncErrors?: SyncError_Result[] | null;
  }
  interface defraimp_autonumber extends defraimp_autonumber_Base, defraimp_autonumber_Relationships {
  }
  interface defraimp_autonumber_Create extends defraimp_autonumber {
  }
  interface defraimp_autonumber_Update extends defraimp_autonumber {
  }
  interface defraimp_autonumber_Select {
    createdby_guid: WebAttribute<defraimp_autonumber_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraimp_autonumber_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraimp_autonumber_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defraimp_autonumberid: WebAttribute<defraimp_autonumber_Select, { defraimp_autonumberid: string | null }, {  }>;
    defraimp_currentnumber: WebAttribute<defraimp_autonumber_Select, { defraimp_currentnumber: number | null }, {  }>;
    defraimp_key: WebAttribute<defraimp_autonumber_Select, { defraimp_key: string | null }, {  }>;
    defraimp_name: WebAttribute<defraimp_autonumber_Select, { defraimp_name: string | null }, {  }>;
    importsequencenumber: WebAttribute<defraimp_autonumber_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraimp_autonumber_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraimp_autonumber_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraimp_autonumber_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    organizationid_guid: WebAttribute<defraimp_autonumber_Select, { organizationid_guid: string | null }, { organizationid_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraimp_autonumber_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    statecode: WebAttribute<defraimp_autonumber_Select, { statecode: defraimp_autonumber_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraimp_autonumber_Select, { statuscode: defraimp_autonumber_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraimp_autonumber_Select, { timezoneruleversionnumber: number | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraimp_autonumber_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraimp_autonumber_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_autonumber_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defraimp_autonumberid: XQW.Guid;
    defraimp_currentnumber: number;
    defraimp_key: string;
    defraimp_name: string;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    organizationid_guid: XQW.Guid;
    overriddencreatedon: Date;
    statecode: defraimp_autonumber_statecode;
    statuscode: defraimp_autonumber_statuscode;
    timezoneruleversionnumber: number;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraimp_autonumber_Expand {
    defraimp_autonumber_ProcessSession: WebExpand<defraimp_autonumber_Expand, ProcessSession_Select, ProcessSession_Filter, { defraimp_autonumber_ProcessSession: ProcessSession_Result[] }>;
    defraimp_autonumber_SyncErrors: WebExpand<defraimp_autonumber_Expand, SyncError_Select, SyncError_Filter, { defraimp_autonumber_SyncErrors: SyncError_Result[] }>;
  }
  interface defraimp_autonumber_FormattedResult {
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
  interface defraimp_autonumber_Result extends defraimp_autonumber_Base, defraimp_autonumber_Relationships {
    "@odata.etag": string;
    createdby_guid: string | null;
    createdonbehalfby_guid: string | null;
    modifiedby_guid: string | null;
    modifiedonbehalfby_guid: string | null;
    organizationid_guid: string | null;
  }
  interface defraimp_autonumber_RelatedOne {
  }
  interface defraimp_autonumber_RelatedMany {
    defraimp_autonumber_ProcessSession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    defraimp_autonumber_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_autonumbers: WebMappingRetrieve<WebApi.defraimp_autonumber_Select,WebApi.defraimp_autonumber_Expand,WebApi.defraimp_autonumber_Filter,WebApi.defraimp_autonumber_Fixed,WebApi.defraimp_autonumber_Result,WebApi.defraimp_autonumber_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_autonumbers: WebMappingRelated<WebApi.defraimp_autonumber_RelatedOne,WebApi.defraimp_autonumber_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_autonumbers: WebMappingCUDA<WebApi.defraimp_autonumber_Create,WebApi.defraimp_autonumber_Update,WebApi.defraimp_autonumber_Select>;
}
