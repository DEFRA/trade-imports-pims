declare namespace WebApi {
  interface defraimp_docom_Base extends WebEntity {
    createdon?: Date | null;
    defraimp_aphaabpapprovalregistrationnumber?: string | null;
    defraimp_containernumber?: string | null;
    defraimp_dateofdecision?: Date | null;
    defraimp_docomid?: string | null;
    defraimp_localreferencenumber?: string | null;
    defraimp_name?: string | null;
    defraimp_purpose?: defraimp_docom_defraimp_purpose | null;
    defraimp_receivingcategory?: defraimp_docom_defraimp_receivingcategory | null;
    defraimp_sealnumber?: string | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defraimp_docom_statecode | null;
    statuscode?: defraimp_docom_statuscode | null;
    timezoneruleversionnumber?: number | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraimp_docom_Relationships {
    defraimp_docom_ProcessSession?: ProcessSession_Result[] | null;
    defraimp_docom_SyncErrors?: SyncError_Result[] | null;
    defraimp_importapplication_PrimaryDOCOMId?: defraimp_importapplication_Result[] | null;
  }
  interface defraimp_docom extends defraimp_docom_Base, defraimp_docom_Relationships {
    ownerid_bind$owners?: string | null;
  }
  interface defraimp_docom_Create extends defraimp_docom {
  }
  interface defraimp_docom_Update extends defraimp_docom {
  }
  interface defraimp_docom_Select {
    createdby_guid: WebAttribute<defraimp_docom_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraimp_docom_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraimp_docom_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defraimp_aphaabpapprovalregistrationnumber: WebAttribute<defraimp_docom_Select, { defraimp_aphaabpapprovalregistrationnumber: string | null }, {  }>;
    defraimp_containernumber: WebAttribute<defraimp_docom_Select, { defraimp_containernumber: string | null }, {  }>;
    defraimp_dateofdecision: WebAttribute<defraimp_docom_Select, { defraimp_dateofdecision: Date | null }, { defraimp_dateofdecision_formatted?: string }>;
    defraimp_docomid: WebAttribute<defraimp_docom_Select, { defraimp_docomid: string | null }, {  }>;
    defraimp_localreferencenumber: WebAttribute<defraimp_docom_Select, { defraimp_localreferencenumber: string | null }, {  }>;
    defraimp_name: WebAttribute<defraimp_docom_Select, { defraimp_name: string | null }, {  }>;
    defraimp_purpose: WebAttribute<defraimp_docom_Select, { defraimp_purpose: defraimp_docom_defraimp_purpose | null }, { defraimp_purpose_formatted?: string }>;
    defraimp_receivingcategory: WebAttribute<defraimp_docom_Select, { defraimp_receivingcategory: defraimp_docom_defraimp_receivingcategory | null }, { defraimp_receivingcategory_formatted?: string }>;
    defraimp_sealnumber: WebAttribute<defraimp_docom_Select, { defraimp_sealnumber: string | null }, {  }>;
    importsequencenumber: WebAttribute<defraimp_docom_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraimp_docom_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraimp_docom_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraimp_docom_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraimp_docom_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    ownerid_guid: WebAttribute<defraimp_docom_Select, { ownerid_guid: string | null }, { ownerid_formatted?: string }>;
    owningbusinessunit_guid: WebAttribute<defraimp_docom_Select, { owningbusinessunit_guid: string | null }, { owningbusinessunit_formatted?: string }>;
    owningteam_guid: WebAttribute<defraimp_docom_Select, { owningteam_guid: string | null }, { owningteam_formatted?: string }>;
    owninguser_guid: WebAttribute<defraimp_docom_Select, { owninguser_guid: string | null }, { owninguser_formatted?: string }>;
    statecode: WebAttribute<defraimp_docom_Select, { statecode: defraimp_docom_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraimp_docom_Select, { statuscode: defraimp_docom_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraimp_docom_Select, { timezoneruleversionnumber: number | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraimp_docom_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraimp_docom_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_docom_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defraimp_aphaabpapprovalregistrationnumber: string;
    defraimp_containernumber: string;
    defraimp_dateofdecision: Date;
    defraimp_docomid: XQW.Guid;
    defraimp_localreferencenumber: string;
    defraimp_name: string;
    defraimp_purpose: defraimp_docom_defraimp_purpose;
    defraimp_receivingcategory: defraimp_docom_defraimp_receivingcategory;
    defraimp_sealnumber: string;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    overriddencreatedon: Date;
    ownerid_guid: XQW.Guid;
    owningbusinessunit_guid: XQW.Guid;
    owningteam_guid: XQW.Guid;
    owninguser_guid: XQW.Guid;
    statecode: defraimp_docom_statecode;
    statuscode: defraimp_docom_statuscode;
    timezoneruleversionnumber: number;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraimp_docom_Expand {
    createdby: WebExpand<defraimp_docom_Expand, SystemUser_Select, SystemUser_Filter, { createdby: SystemUser_Result }>;
    createdonbehalfby: WebExpand<defraimp_docom_Expand, SystemUser_Select, SystemUser_Filter, { createdonbehalfby: SystemUser_Result }>;
    defraimp_docom_ProcessSession: WebExpand<defraimp_docom_Expand, ProcessSession_Select, ProcessSession_Filter, { defraimp_docom_ProcessSession: ProcessSession_Result[] }>;
    defraimp_docom_SyncErrors: WebExpand<defraimp_docom_Expand, SyncError_Select, SyncError_Filter, { defraimp_docom_SyncErrors: SyncError_Result[] }>;
    defraimp_importapplication_PrimaryDOCOMId: WebExpand<defraimp_docom_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_importapplication_PrimaryDOCOMId: defraimp_importapplication_Result[] }>;
    modifiedby: WebExpand<defraimp_docom_Expand, SystemUser_Select, SystemUser_Filter, { modifiedby: SystemUser_Result }>;
    modifiedonbehalfby: WebExpand<defraimp_docom_Expand, SystemUser_Select, SystemUser_Filter, { modifiedonbehalfby: SystemUser_Result }>;
    owningteam: WebExpand<defraimp_docom_Expand, Team_Select, Team_Filter, { owningteam: Team_Result }>;
    owninguser: WebExpand<defraimp_docom_Expand, SystemUser_Select, SystemUser_Filter, { owninguser: SystemUser_Result }>;
  }
  interface defraimp_docom_FormattedResult {
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    defraimp_dateofdecision_formatted?: string;
    defraimp_purpose_formatted?: string;
    defraimp_receivingcategory_formatted?: string;
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
  interface defraimp_docom_Result extends defraimp_docom_Base, defraimp_docom_Relationships {
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
  interface defraimp_docom_RelatedOne {
    createdby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    createdonbehalfby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    modifiedby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    modifiedonbehalfby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    owningteam: WebMappingRetrieve<WebApi.Team_Select,WebApi.Team_Expand,WebApi.Team_Filter,WebApi.Team_Fixed,WebApi.Team_Result,WebApi.Team_FormattedResult>;
    owninguser: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
  }
  interface defraimp_docom_RelatedMany {
    defraimp_docom_ProcessSession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    defraimp_docom_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
    defraimp_importapplication_PrimaryDOCOMId: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_docoms: WebMappingRetrieve<WebApi.defraimp_docom_Select,WebApi.defraimp_docom_Expand,WebApi.defraimp_docom_Filter,WebApi.defraimp_docom_Fixed,WebApi.defraimp_docom_Result,WebApi.defraimp_docom_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_docoms: WebMappingRelated<WebApi.defraimp_docom_RelatedOne,WebApi.defraimp_docom_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_docoms: WebMappingCUDA<WebApi.defraimp_docom_Create,WebApi.defraimp_docom_Update,WebApi.defraimp_docom_Select>;
}
