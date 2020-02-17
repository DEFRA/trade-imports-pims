declare namespace WebApi {
  interface defraimp_importapplicationbusinessprocessflow_Base extends WebEntity {
    activestagestartedon?: Date | null;
    bpf_duration?: number | null;
    bpf_name?: string | null;
    businessprocessflowinstanceid?: string | null;
    completedon?: Date | null;
    createdon?: Date | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defraimp_importapplicationbusinessprocessflow_statecode | null;
    statuscode?: defraimp_importapplicationbusinessprocessflow_statuscode | null;
    timezoneruleversionnumber?: number | null;
    traversedpath?: string | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraimp_importapplicationbusinessprocessflow_Relationships {
    defraimp_importapplicationbusinessprocessflow_ProcessSession?: ProcessSession_Result[] | null;
    defraimp_importapplicationbusinessprocessflow_SyncErrors?: SyncError_Result[] | null;
  }
  interface defraimp_importapplicationbusinessprocessflow extends defraimp_importapplicationbusinessprocessflow_Base, defraimp_importapplicationbusinessprocessflow_Relationships {
    activestageid_bind$processstages?: string | null;
    bpf_defraimp_importapplicationid_bind$defraimp_importapplications?: string | null;
    processid_bind$workflows?: string | null;
  }
  interface defraimp_importapplicationbusinessprocessflow_Create extends defraimp_importapplicationbusinessprocessflow {
  }
  interface defraimp_importapplicationbusinessprocessflow_Update extends defraimp_importapplicationbusinessprocessflow {
  }
  interface defraimp_importapplicationbusinessprocessflow_Select {
    activestageid_guid: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { activestageid_guid: string | null }, { activestageid_formatted?: string }>;
    activestagestartedon: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { activestagestartedon: Date | null }, { activestagestartedon_formatted?: string }>;
    bpf_defraimp_importapplicationid_guid: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { bpf_defraimp_importapplicationid_guid: string | null }, { bpf_defraimp_importapplicationid_formatted?: string }>;
    bpf_duration: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { bpf_duration: number | null }, {  }>;
    bpf_name: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { bpf_name: string | null }, {  }>;
    businessprocessflowinstanceid: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { businessprocessflowinstanceid: string | null }, {  }>;
    completedon: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { completedon: Date | null }, { completedon_formatted?: string }>;
    createdby_guid: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    importsequencenumber: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    organizationid_guid: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { organizationid_guid: string | null }, { organizationid_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    processid_guid: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { processid_guid: string | null }, { processid_formatted?: string }>;
    statecode: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { statecode: defraimp_importapplicationbusinessprocessflow_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { statuscode: defraimp_importapplicationbusinessprocessflow_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { timezoneruleversionnumber: number | null }, {  }>;
    traversedpath: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { traversedpath: string | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraimp_importapplicationbusinessprocessflow_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_importapplicationbusinessprocessflow_Filter {
    activestageid_guid: XQW.Guid;
    activestagestartedon: Date;
    bpf_defraimp_importapplicationid_guid: XQW.Guid;
    bpf_duration: number;
    bpf_name: string;
    businessprocessflowinstanceid: XQW.Guid;
    completedon: Date;
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    organizationid_guid: XQW.Guid;
    overriddencreatedon: Date;
    processid_guid: XQW.Guid;
    statecode: defraimp_importapplicationbusinessprocessflow_statecode;
    statuscode: defraimp_importapplicationbusinessprocessflow_statuscode;
    timezoneruleversionnumber: number;
    traversedpath: string;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraimp_importapplicationbusinessprocessflow_Expand {
    bpf_defraimp_importapplicationid: WebExpand<defraimp_importapplicationbusinessprocessflow_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { bpf_defraimp_importapplicationid: defraimp_importapplication_Result }>;
    defraimp_importapplicationbusinessprocessflow_ProcessSession: WebExpand<defraimp_importapplicationbusinessprocessflow_Expand, ProcessSession_Select, ProcessSession_Filter, { defraimp_importapplicationbusinessprocessflow_ProcessSession: ProcessSession_Result[] }>;
    defraimp_importapplicationbusinessprocessflow_SyncErrors: WebExpand<defraimp_importapplicationbusinessprocessflow_Expand, SyncError_Select, SyncError_Filter, { defraimp_importapplicationbusinessprocessflow_SyncErrors: SyncError_Result[] }>;
    processid: WebExpand<defraimp_importapplicationbusinessprocessflow_Expand, Workflow_Select, Workflow_Filter, { processid: Workflow_Result }>;
  }
  interface defraimp_importapplicationbusinessprocessflow_FormattedResult {
    activestageid_formatted?: string;
    activestagestartedon_formatted?: string;
    bpf_defraimp_importapplicationid_formatted?: string;
    completedon_formatted?: string;
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    modifiedby_formatted?: string;
    modifiedon_formatted?: string;
    modifiedonbehalfby_formatted?: string;
    organizationid_formatted?: string;
    overriddencreatedon_formatted?: string;
    processid_formatted?: string;
    statecode_formatted?: string;
    statuscode_formatted?: string;
  }
  interface defraimp_importapplicationbusinessprocessflow_Result extends defraimp_importapplicationbusinessprocessflow_Base, defraimp_importapplicationbusinessprocessflow_Relationships {
    "@odata.etag": string;
    activestageid_guid: string | null;
    bpf_defraimp_importapplicationid_guid: string | null;
    createdby_guid: string | null;
    createdonbehalfby_guid: string | null;
    modifiedby_guid: string | null;
    modifiedonbehalfby_guid: string | null;
    organizationid_guid: string | null;
    processid_guid: string | null;
  }
  interface defraimp_importapplicationbusinessprocessflow_RelatedOne {
    bpf_defraimp_importapplicationid: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    processid: WebMappingRetrieve<WebApi.Workflow_Select,WebApi.Workflow_Expand,WebApi.Workflow_Filter,WebApi.Workflow_Fixed,WebApi.Workflow_Result,WebApi.Workflow_FormattedResult>;
  }
  interface defraimp_importapplicationbusinessprocessflow_RelatedMany {
    defraimp_importapplicationbusinessprocessflow_ProcessSession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    defraimp_importapplicationbusinessprocessflow_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_importapplicationbusinessprocessflows: WebMappingRetrieve<WebApi.defraimp_importapplicationbusinessprocessflow_Select,WebApi.defraimp_importapplicationbusinessprocessflow_Expand,WebApi.defraimp_importapplicationbusinessprocessflow_Filter,WebApi.defraimp_importapplicationbusinessprocessflow_Fixed,WebApi.defraimp_importapplicationbusinessprocessflow_Result,WebApi.defraimp_importapplicationbusinessprocessflow_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_importapplicationbusinessprocessflows: WebMappingRelated<WebApi.defraimp_importapplicationbusinessprocessflow_RelatedOne,WebApi.defraimp_importapplicationbusinessprocessflow_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_importapplicationbusinessprocessflows: WebMappingCUDA<WebApi.defraimp_importapplicationbusinessprocessflow_Create,WebApi.defraimp_importapplicationbusinessprocessflow_Update,WebApi.defraimp_importapplicationbusinessprocessflow_Select>;
}
