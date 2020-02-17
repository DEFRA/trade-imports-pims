declare namespace WebApi {
  interface defraexp_commoditytype_Base extends WebEntity {
    createdon?: Date | null;
    defraexp_commoditytypeid?: string | null;
    defraexp_name?: string | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defraexp_commoditytype_statecode | null;
    statuscode?: defraexp_commoditytype_statuscode | null;
    timezoneruleversionnumber?: number | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraexp_commoditytype_Relationships {
    defraexp_commoditytype_ProcessSession?: ProcessSession_Result[] | null;
    defraexp_commoditytype_SyncErrors?: SyncError_Result[] | null;
    defraimp_defraexp_commoditytype_defraimp_goldbronzecommodity_CommodityTypeid?: defraimp_goldbronzecommodity_Result[] | null;
    defraimp_defraexp_commoditytype_defraimp_importapp?: defraimp_importapplication_Result[] | null;
    defraimp_defraexp_commoditytype_defraimp_importcountrycommodityrisklevel_commoditytypeid?: defraimp_importcountrycommodityrisklevel_Result[] | null;
  }
  interface defraexp_commoditytype extends defraexp_commoditytype_Base, defraexp_commoditytype_Relationships {
    defraexp_CommodityGroupid_bind$defraexp_commoditygroups?: string | null;
    ownerid_bind$owners?: string | null;
  }
  interface defraexp_commoditytype_Create extends defraexp_commoditytype {
  }
  interface defraexp_commoditytype_Update extends defraexp_commoditytype {
  }
  interface defraexp_commoditytype_Select {
    createdby_guid: WebAttribute<defraexp_commoditytype_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraexp_commoditytype_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraexp_commoditytype_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defraexp_commoditygroupid_guid: WebAttribute<defraexp_commoditytype_Select, { defraexp_commoditygroupid_guid: string | null }, { defraexp_commoditygroupid_formatted?: string }>;
    defraexp_commoditytypeid: WebAttribute<defraexp_commoditytype_Select, { defraexp_commoditytypeid: string | null }, {  }>;
    defraexp_name: WebAttribute<defraexp_commoditytype_Select, { defraexp_name: string | null }, {  }>;
    importsequencenumber: WebAttribute<defraexp_commoditytype_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraexp_commoditytype_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraexp_commoditytype_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraexp_commoditytype_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraexp_commoditytype_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    ownerid_guid: WebAttribute<defraexp_commoditytype_Select, { ownerid_guid: string | null }, { ownerid_formatted?: string }>;
    owningbusinessunit_guid: WebAttribute<defraexp_commoditytype_Select, { owningbusinessunit_guid: string | null }, { owningbusinessunit_formatted?: string }>;
    owningteam_guid: WebAttribute<defraexp_commoditytype_Select, { owningteam_guid: string | null }, { owningteam_formatted?: string }>;
    owninguser_guid: WebAttribute<defraexp_commoditytype_Select, { owninguser_guid: string | null }, { owninguser_formatted?: string }>;
    statecode: WebAttribute<defraexp_commoditytype_Select, { statecode: defraexp_commoditytype_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraexp_commoditytype_Select, { statuscode: defraexp_commoditytype_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraexp_commoditytype_Select, { timezoneruleversionnumber: number | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraexp_commoditytype_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraexp_commoditytype_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraexp_commoditytype_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defraexp_commoditygroupid_guid: XQW.Guid;
    defraexp_commoditytypeid: XQW.Guid;
    defraexp_name: string;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    overriddencreatedon: Date;
    ownerid_guid: XQW.Guid;
    owningbusinessunit_guid: XQW.Guid;
    owningteam_guid: XQW.Guid;
    owninguser_guid: XQW.Guid;
    statecode: defraexp_commoditytype_statecode;
    statuscode: defraexp_commoditytype_statuscode;
    timezoneruleversionnumber: number;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraexp_commoditytype_Expand {
    defraexp_commoditytype_ProcessSession: WebExpand<defraexp_commoditytype_Expand, ProcessSession_Select, ProcessSession_Filter, { defraexp_commoditytype_ProcessSession: ProcessSession_Result[] }>;
    defraexp_commoditytype_SyncErrors: WebExpand<defraexp_commoditytype_Expand, SyncError_Select, SyncError_Filter, { defraexp_commoditytype_SyncErrors: SyncError_Result[] }>;
    defraimp_defraexp_commoditytype_defraimp_goldbronzecommodity_CommodityTypeid: WebExpand<defraexp_commoditytype_Expand, defraimp_goldbronzecommodity_Select, defraimp_goldbronzecommodity_Filter, { defraimp_defraexp_commoditytype_defraimp_goldbronzecommodity_CommodityTypeid: defraimp_goldbronzecommodity_Result[] }>;
    defraimp_defraexp_commoditytype_defraimp_importapp: WebExpand<defraexp_commoditytype_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defraexp_commoditytype_defraimp_importapp: defraimp_importapplication_Result[] }>;
    defraimp_defraexp_commoditytype_defraimp_importcountrycommodityrisklevel_commoditytypeid: WebExpand<defraexp_commoditytype_Expand, defraimp_importcountrycommodityrisklevel_Select, defraimp_importcountrycommodityrisklevel_Filter, { defraimp_defraexp_commoditytype_defraimp_importcountrycommodityrisklevel_commoditytypeid: defraimp_importcountrycommodityrisklevel_Result[] }>;
  }
  interface defraexp_commoditytype_FormattedResult {
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    defraexp_commoditygroupid_formatted?: string;
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
  interface defraexp_commoditytype_Result extends defraexp_commoditytype_Base, defraexp_commoditytype_Relationships {
    "@odata.etag": string;
    createdby_guid: string | null;
    createdonbehalfby_guid: string | null;
    defraexp_commoditygroupid_guid: string | null;
    modifiedby_guid: string | null;
    modifiedonbehalfby_guid: string | null;
    ownerid_guid: string | null;
    owningbusinessunit_guid: string | null;
    owningteam_guid: string | null;
    owninguser_guid: string | null;
  }
  interface defraexp_commoditytype_RelatedOne {
  }
  interface defraexp_commoditytype_RelatedMany {
    defraexp_commoditytype_ProcessSession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    defraexp_commoditytype_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
    defraimp_defraexp_commoditytype_defraimp_goldbronzecommodity_CommodityTypeid: WebMappingRetrieve<WebApi.defraimp_goldbronzecommodity_Select,WebApi.defraimp_goldbronzecommodity_Expand,WebApi.defraimp_goldbronzecommodity_Filter,WebApi.defraimp_goldbronzecommodity_Fixed,WebApi.defraimp_goldbronzecommodity_Result,WebApi.defraimp_goldbronzecommodity_FormattedResult>;
    defraimp_defraexp_commoditytype_defraimp_importapp: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_defraexp_commoditytype_defraimp_importcountrycommodityrisklevel_commoditytypeid: WebMappingRetrieve<WebApi.defraimp_importcountrycommodityrisklevel_Select,WebApi.defraimp_importcountrycommodityrisklevel_Expand,WebApi.defraimp_importcountrycommodityrisklevel_Filter,WebApi.defraimp_importcountrycommodityrisklevel_Fixed,WebApi.defraimp_importcountrycommodityrisklevel_Result,WebApi.defraimp_importcountrycommodityrisklevel_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraexp_commoditytypes: WebMappingRetrieve<WebApi.defraexp_commoditytype_Select,WebApi.defraexp_commoditytype_Expand,WebApi.defraexp_commoditytype_Filter,WebApi.defraexp_commoditytype_Fixed,WebApi.defraexp_commoditytype_Result,WebApi.defraexp_commoditytype_FormattedResult>;
}
interface WebEntitiesRelated {
  defraexp_commoditytypes: WebMappingRelated<WebApi.defraexp_commoditytype_RelatedOne,WebApi.defraexp_commoditytype_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraexp_commoditytypes: WebMappingCUDA<WebApi.defraexp_commoditytype_Create,WebApi.defraexp_commoditytype_Update,WebApi.defraexp_commoditytype_Select>;
}
