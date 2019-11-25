declare namespace WebApi {
  interface defraimp_apharegion_Base extends WebEntity {
    createdon?: Date | null;
    defraimp_apharegionid?: string | null;
    defraimp_name?: string | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defraimp_apharegion_statecode | null;
    statuscode?: defraimp_apharegion_statuscode | null;
    timezoneruleversionnumber?: number | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraimp_apharegion_Relationships {
    defraimp_apharegion_ProcessSession?: ProcessSession_Result[] | null;
    defraimp_apharegion_SyncErrors?: SyncError_Result[] | null;
    defraimp_defraimp_apharegion_defraimp_importapplic?: defraimp_importapplication_Result[] | null;
    defraimp_defraimp_apharegion_defraimp_importinspection_RegionAreaAllocatedtoID?: defraimp_importinspection_Result[] | null;
  }
  interface defraimp_apharegion extends defraimp_apharegion_Base, defraimp_apharegion_Relationships {
    ownerid_bind$owners?: string | null;
  }
  interface defraimp_apharegion_Create extends defraimp_apharegion {
  }
  interface defraimp_apharegion_Update extends defraimp_apharegion {
  }
  interface defraimp_apharegion_Select {
    createdby_guid: WebAttribute<defraimp_apharegion_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraimp_apharegion_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraimp_apharegion_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defraimp_apharegionid: WebAttribute<defraimp_apharegion_Select, { defraimp_apharegionid: string | null }, {  }>;
    defraimp_name: WebAttribute<defraimp_apharegion_Select, { defraimp_name: string | null }, {  }>;
    importsequencenumber: WebAttribute<defraimp_apharegion_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraimp_apharegion_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraimp_apharegion_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraimp_apharegion_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraimp_apharegion_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    ownerid_guid: WebAttribute<defraimp_apharegion_Select, { ownerid_guid: string | null }, { ownerid_formatted?: string }>;
    owningbusinessunit_guid: WebAttribute<defraimp_apharegion_Select, { owningbusinessunit_guid: string | null }, { owningbusinessunit_formatted?: string }>;
    owningteam_guid: WebAttribute<defraimp_apharegion_Select, { owningteam_guid: string | null }, { owningteam_formatted?: string }>;
    owninguser_guid: WebAttribute<defraimp_apharegion_Select, { owninguser_guid: string | null }, { owninguser_formatted?: string }>;
    statecode: WebAttribute<defraimp_apharegion_Select, { statecode: defraimp_apharegion_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraimp_apharegion_Select, { statuscode: defraimp_apharegion_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraimp_apharegion_Select, { timezoneruleversionnumber: number | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraimp_apharegion_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraimp_apharegion_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_apharegion_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defraimp_apharegionid: XQW.Guid;
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
    statecode: defraimp_apharegion_statecode;
    statuscode: defraimp_apharegion_statuscode;
    timezoneruleversionnumber: number;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraimp_apharegion_Expand {
    createdby: WebExpand<defraimp_apharegion_Expand, SystemUser_Select, SystemUser_Filter, { createdby: SystemUser_Result }>;
    createdonbehalfby: WebExpand<defraimp_apharegion_Expand, SystemUser_Select, SystemUser_Filter, { createdonbehalfby: SystemUser_Result }>;
    defraimp_apharegion_ProcessSession: WebExpand<defraimp_apharegion_Expand, ProcessSession_Select, ProcessSession_Filter, { defraimp_apharegion_ProcessSession: ProcessSession_Result[] }>;
    defraimp_apharegion_SyncErrors: WebExpand<defraimp_apharegion_Expand, SyncError_Select, SyncError_Filter, { defraimp_apharegion_SyncErrors: SyncError_Result[] }>;
    defraimp_defraimp_apharegion_defraimp_importapplic: WebExpand<defraimp_apharegion_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defraimp_apharegion_defraimp_importapplic: defraimp_importapplication_Result[] }>;
    defraimp_defraimp_apharegion_defraimp_importinspection_RegionAreaAllocatedtoID: WebExpand<defraimp_apharegion_Expand, defraimp_importinspection_Select, defraimp_importinspection_Filter, { defraimp_defraimp_apharegion_defraimp_importinspection_RegionAreaAllocatedtoID: defraimp_importinspection_Result[] }>;
    modifiedby: WebExpand<defraimp_apharegion_Expand, SystemUser_Select, SystemUser_Filter, { modifiedby: SystemUser_Result }>;
    modifiedonbehalfby: WebExpand<defraimp_apharegion_Expand, SystemUser_Select, SystemUser_Filter, { modifiedonbehalfby: SystemUser_Result }>;
    owningteam: WebExpand<defraimp_apharegion_Expand, Team_Select, Team_Filter, { owningteam: Team_Result }>;
    owninguser: WebExpand<defraimp_apharegion_Expand, SystemUser_Select, SystemUser_Filter, { owninguser: SystemUser_Result }>;
  }
  interface defraimp_apharegion_FormattedResult {
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
  interface defraimp_apharegion_Result extends defraimp_apharegion_Base, defraimp_apharegion_Relationships {
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
  interface defraimp_apharegion_RelatedOne {
    createdby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    createdonbehalfby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    modifiedby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    modifiedonbehalfby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    owningteam: WebMappingRetrieve<WebApi.Team_Select,WebApi.Team_Expand,WebApi.Team_Filter,WebApi.Team_Fixed,WebApi.Team_Result,WebApi.Team_FormattedResult>;
    owninguser: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
  }
  interface defraimp_apharegion_RelatedMany {
    defraimp_apharegion_ProcessSession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    defraimp_apharegion_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
    defraimp_defraimp_apharegion_defraimp_importapplic: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_defraimp_apharegion_defraimp_importinspection_RegionAreaAllocatedtoID: WebMappingRetrieve<WebApi.defraimp_importinspection_Select,WebApi.defraimp_importinspection_Expand,WebApi.defraimp_importinspection_Filter,WebApi.defraimp_importinspection_Fixed,WebApi.defraimp_importinspection_Result,WebApi.defraimp_importinspection_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_apharegions: WebMappingRetrieve<WebApi.defraimp_apharegion_Select,WebApi.defraimp_apharegion_Expand,WebApi.defraimp_apharegion_Filter,WebApi.defraimp_apharegion_Fixed,WebApi.defraimp_apharegion_Result,WebApi.defraimp_apharegion_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_apharegions: WebMappingRelated<WebApi.defraimp_apharegion_RelatedOne,WebApi.defraimp_apharegion_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_apharegions: WebMappingCUDA<WebApi.defraimp_apharegion_Create,WebApi.defraimp_apharegion_Update,WebApi.defraimp_apharegion_Select>;
}
