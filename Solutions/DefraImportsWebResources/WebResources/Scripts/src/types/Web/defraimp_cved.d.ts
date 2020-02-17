declare namespace WebApi {
  interface defraimp_cved_Base extends WebEntity {
    createdon?: Date | null;
    defraimp_certificatereferencenumber?: string | null;
    defraimp_channeledconsignment?: defraimp_cved_defraimp_channeledconsignment | null;
    defraimp_cvedid?: string | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defraimp_cved_statecode | null;
    statuscode?: defraimp_cved_statuscode | null;
    timezoneruleversionnumber?: number | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraimp_cved_Relationships {
    defraimp_cved_ProcessSession?: ProcessSession_Result[] | null;
    defraimp_cved_SyncErrors?: SyncError_Result[] | null;
    defraimp_defraimp_cved_defraimp_importapplication_PrimaryCVEDId?: defraimp_importapplication_Result[] | null;
  }
  interface defraimp_cved extends defraimp_cved_Base, defraimp_cved_Relationships {
    ownerid_bind$owners?: string | null;
  }
  interface defraimp_cved_Create extends defraimp_cved {
  }
  interface defraimp_cved_Update extends defraimp_cved {
  }
  interface defraimp_cved_Select {
    createdby_guid: WebAttribute<defraimp_cved_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraimp_cved_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraimp_cved_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defraimp_certificatereferencenumber: WebAttribute<defraimp_cved_Select, { defraimp_certificatereferencenumber: string | null }, {  }>;
    defraimp_channeledconsignment: WebAttribute<defraimp_cved_Select, { defraimp_channeledconsignment: defraimp_cved_defraimp_channeledconsignment | null }, { defraimp_channeledconsignment_formatted?: string }>;
    defraimp_cvedid: WebAttribute<defraimp_cved_Select, { defraimp_cvedid: string | null }, {  }>;
    importsequencenumber: WebAttribute<defraimp_cved_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraimp_cved_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraimp_cved_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraimp_cved_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraimp_cved_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    ownerid_guid: WebAttribute<defraimp_cved_Select, { ownerid_guid: string | null }, { ownerid_formatted?: string }>;
    owningbusinessunit_guid: WebAttribute<defraimp_cved_Select, { owningbusinessunit_guid: string | null }, { owningbusinessunit_formatted?: string }>;
    owningteam_guid: WebAttribute<defraimp_cved_Select, { owningteam_guid: string | null }, { owningteam_formatted?: string }>;
    owninguser_guid: WebAttribute<defraimp_cved_Select, { owninguser_guid: string | null }, { owninguser_formatted?: string }>;
    statecode: WebAttribute<defraimp_cved_Select, { statecode: defraimp_cved_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraimp_cved_Select, { statuscode: defraimp_cved_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraimp_cved_Select, { timezoneruleversionnumber: number | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraimp_cved_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraimp_cved_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_cved_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defraimp_certificatereferencenumber: string;
    defraimp_channeledconsignment: defraimp_cved_defraimp_channeledconsignment;
    defraimp_cvedid: XQW.Guid;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    overriddencreatedon: Date;
    ownerid_guid: XQW.Guid;
    owningbusinessunit_guid: XQW.Guid;
    owningteam_guid: XQW.Guid;
    owninguser_guid: XQW.Guid;
    statecode: defraimp_cved_statecode;
    statuscode: defraimp_cved_statuscode;
    timezoneruleversionnumber: number;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraimp_cved_Expand {
    defraimp_cved_ProcessSession: WebExpand<defraimp_cved_Expand, ProcessSession_Select, ProcessSession_Filter, { defraimp_cved_ProcessSession: ProcessSession_Result[] }>;
    defraimp_cved_SyncErrors: WebExpand<defraimp_cved_Expand, SyncError_Select, SyncError_Filter, { defraimp_cved_SyncErrors: SyncError_Result[] }>;
    defraimp_defraimp_cved_defraimp_importapplication_PrimaryCVEDId: WebExpand<defraimp_cved_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defraimp_cved_defraimp_importapplication_PrimaryCVEDId: defraimp_importapplication_Result[] }>;
  }
  interface defraimp_cved_FormattedResult {
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    defraimp_channeledconsignment_formatted?: string;
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
  interface defraimp_cved_Result extends defraimp_cved_Base, defraimp_cved_Relationships {
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
  interface defraimp_cved_RelatedOne {
  }
  interface defraimp_cved_RelatedMany {
    defraimp_cved_ProcessSession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    defraimp_cved_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
    defraimp_defraimp_cved_defraimp_importapplication_PrimaryCVEDId: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_cveds: WebMappingRetrieve<WebApi.defraimp_cved_Select,WebApi.defraimp_cved_Expand,WebApi.defraimp_cved_Filter,WebApi.defraimp_cved_Fixed,WebApi.defraimp_cved_Result,WebApi.defraimp_cved_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_cveds: WebMappingRelated<WebApi.defraimp_cved_RelatedOne,WebApi.defraimp_cved_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_cveds: WebMappingCUDA<WebApi.defraimp_cved_Create,WebApi.defraimp_cved_Update,WebApi.defraimp_cved_Select>;
}
