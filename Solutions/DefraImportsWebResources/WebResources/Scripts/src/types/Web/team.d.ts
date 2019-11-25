declare namespace WebApi {
  interface Team_Base extends WebEntity {
    azureactivedirectoryobjectid?: string | null;
    createdon?: Date | null;
    description?: string | null;
    emailaddress?: string | null;
    exchangerate?: number | null;
    importsequencenumber?: number | null;
    isdefault?: boolean | null;
    modifiedon?: Date | null;
    name?: string | null;
    organizationid?: string | null;
    overriddencreatedon?: Date | null;
    processid?: string | null;
    stageid?: string | null;
    systemmanaged?: boolean | null;
    teamid?: string | null;
    teamtype?: team_type | null;
    traversedpath?: string | null;
    versionnumber?: number | null;
  }
  interface Team_Relationships {
    OwningTeam_postfollows?: PostFollow_Result[] | null;
    Team_ProcessSessions?: ProcessSession_Result[] | null;
    Team_SyncErrors?: SyncError_Result[] | null;
    defraimp_importquery_team_owningteam?: defraimp_importquery_Result[] | null;
    defraimp_team_defraimp_importapplication?: defraimp_importapplication_Result[] | null;
    team_SyncError?: SyncError_Result[] | null;
    team_connections1?: Connection_Result[] | null;
    team_connections2?: Connection_Result[] | null;
    team_defra_country?: defra_country_Result[] | null;
    team_defraexp_commoditytype?: defraexp_commoditytype_Result[] | null;
    team_defraimp_apharegion?: defraimp_apharegion_Result[] | null;
    team_defraimp_docom?: defraimp_docom_Result[] | null;
    team_defraimp_goldbronzecommodity?: defraimp_goldbronzecommodity_Result[] | null;
    team_defraimp_importapplication?: defraimp_importapplication_Result[] | null;
    team_defraimp_importinspection?: defraimp_importinspection_Result[] | null;
    team_defraimp_importnotification?: defraimp_importnotification_Result[] | null;
    team_defraimp_inspectioncoveragerule?: defraimp_inspectioncoveragerule_Result[] | null;
    team_defraimp_itahc?: defraimp_itahc_Result[] | null;
    team_defraimp_placeoforigin?: defraimp_placeoforigin_Result[] | null;
    team_defraimp_sampletest?: defraimp_sampletest_Result[] | null;
    team_processsession?: ProcessSession_Result[] | null;
    team_workflow?: Workflow_Result[] | null;
    teammembership_association?: SystemUser_Result[] | null;
  }
  interface Team extends Team_Base, Team_Relationships {
    administratorid_bind$systemusers?: string | null;
    businessunitid_bind$businessunits?: string | null;
    queueid_bind$queues?: string | null;
    stageid_processstage_bind$processstages?: string | null;
    transactioncurrencyid_bind$transactioncurrencies?: string | null;
  }
  interface Team_Create extends Team {
    associatedteamtemplateid_bind$teamtemplates?: string | null;
    regardingobjectid_knowledgearticle_bind$knowledgearticles?: string | null;
    regardingobjectid_opportunity_bind$opportunities?: string | null;
  }
  interface Team_Update extends Team {
  }
  interface Team_Select {
    administratorid_guid: WebAttribute<Team_Select, { administratorid_guid: string | null }, { administratorid_formatted?: string }>;
    azureactivedirectoryobjectid: WebAttribute<Team_Select, { azureactivedirectoryobjectid: string | null }, {  }>;
    businessunitid_guid: WebAttribute<Team_Select, { businessunitid_guid: string | null }, { businessunitid_formatted?: string }>;
    createdby_guid: WebAttribute<Team_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<Team_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<Team_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    description: WebAttribute<Team_Select, { description: string | null }, {  }>;
    emailaddress: WebAttribute<Team_Select, { emailaddress: string | null }, {  }>;
    exchangerate: WebAttribute<Team_Select, { exchangerate: number | null }, {  }>;
    importsequencenumber: WebAttribute<Team_Select, { importsequencenumber: number | null }, {  }>;
    isdefault: WebAttribute<Team_Select, { isdefault: boolean | null }, {  }>;
    modifiedby_guid: WebAttribute<Team_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<Team_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<Team_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    name: WebAttribute<Team_Select, { name: string | null }, {  }>;
    organizationid: WebAttribute<Team_Select, { organizationid: string | null }, {  }>;
    overriddencreatedon: WebAttribute<Team_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    processid: WebAttribute<Team_Select, { processid: string | null }, {  }>;
    queueid_guid: WebAttribute<Team_Select, { queueid_guid: string | null }, { queueid_formatted?: string }>;
    regardingobjectid_guid: WebAttribute<Team_Select, { regardingobjectid_guid: string | null }, { regardingobjectid_formatted?: string }>;
    stageid: WebAttribute<Team_Select, { stageid: string | null }, {  }>;
    systemmanaged: WebAttribute<Team_Select, { systemmanaged: boolean | null }, {  }>;
    teamid: WebAttribute<Team_Select, { teamid: string | null }, {  }>;
    teamtemplateid_guid: WebAttribute<Team_Select, { teamtemplateid_guid: string | null }, { teamtemplateid_formatted?: string }>;
    teamtype: WebAttribute<Team_Select, { teamtype: team_type | null }, { teamtype_formatted?: string }>;
    transactioncurrencyid_guid: WebAttribute<Team_Select, { transactioncurrencyid_guid: string | null }, { transactioncurrencyid_formatted?: string }>;
    traversedpath: WebAttribute<Team_Select, { traversedpath: string | null }, {  }>;
    versionnumber: WebAttribute<Team_Select, { versionnumber: number | null }, {  }>;
  }
  interface Team_Filter {
    administratorid_guid: XQW.Guid;
    azureactivedirectoryobjectid: XQW.Guid;
    businessunitid_guid: XQW.Guid;
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    description: string;
    emailaddress: string;
    exchangerate: any;
    importsequencenumber: number;
    isdefault: boolean;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    name: string;
    organizationid: XQW.Guid;
    overriddencreatedon: Date;
    processid: XQW.Guid;
    queueid_guid: XQW.Guid;
    regardingobjectid_guid: XQW.Guid;
    stageid: XQW.Guid;
    systemmanaged: boolean;
    teamid: XQW.Guid;
    teamtemplateid_guid: XQW.Guid;
    teamtype: team_type;
    transactioncurrencyid_guid: XQW.Guid;
    traversedpath: string;
    versionnumber: number;
  }
  interface Team_Expand {
    OwningTeam_postfollows: WebExpand<Team_Expand, PostFollow_Select, PostFollow_Filter, { OwningTeam_postfollows: PostFollow_Result[] }>;
    Team_ProcessSessions: WebExpand<Team_Expand, ProcessSession_Select, ProcessSession_Filter, { Team_ProcessSessions: ProcessSession_Result[] }>;
    Team_SyncErrors: WebExpand<Team_Expand, SyncError_Select, SyncError_Filter, { Team_SyncErrors: SyncError_Result[] }>;
    administratorid: WebExpand<Team_Expand, SystemUser_Select, SystemUser_Filter, { administratorid: SystemUser_Result }>;
    createdby: WebExpand<Team_Expand, SystemUser_Select, SystemUser_Filter, { createdby: SystemUser_Result }>;
    createdonbehalfby: WebExpand<Team_Expand, SystemUser_Select, SystemUser_Filter, { createdonbehalfby: SystemUser_Result }>;
    defraimp_importquery_team_owningteam: WebExpand<Team_Expand, defraimp_importquery_Select, defraimp_importquery_Filter, { defraimp_importquery_team_owningteam: defraimp_importquery_Result[] }>;
    defraimp_team_defraimp_importapplication: WebExpand<Team_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_team_defraimp_importapplication: defraimp_importapplication_Result[] }>;
    modifiedby: WebExpand<Team_Expand, SystemUser_Select, SystemUser_Filter, { modifiedby: SystemUser_Result }>;
    modifiedonbehalfby: WebExpand<Team_Expand, SystemUser_Select, SystemUser_Filter, { modifiedonbehalfby: SystemUser_Result }>;
    team_SyncError: WebExpand<Team_Expand, SyncError_Select, SyncError_Filter, { team_SyncError: SyncError_Result[] }>;
    team_connections1: WebExpand<Team_Expand, Connection_Select, Connection_Filter, { team_connections1: Connection_Result[] }>;
    team_connections2: WebExpand<Team_Expand, Connection_Select, Connection_Filter, { team_connections2: Connection_Result[] }>;
    team_defra_country: WebExpand<Team_Expand, defra_country_Select, defra_country_Filter, { team_defra_country: defra_country_Result[] }>;
    team_defraexp_commoditytype: WebExpand<Team_Expand, defraexp_commoditytype_Select, defraexp_commoditytype_Filter, { team_defraexp_commoditytype: defraexp_commoditytype_Result[] }>;
    team_defraimp_apharegion: WebExpand<Team_Expand, defraimp_apharegion_Select, defraimp_apharegion_Filter, { team_defraimp_apharegion: defraimp_apharegion_Result[] }>;
    team_defraimp_docom: WebExpand<Team_Expand, defraimp_docom_Select, defraimp_docom_Filter, { team_defraimp_docom: defraimp_docom_Result[] }>;
    team_defraimp_goldbronzecommodity: WebExpand<Team_Expand, defraimp_goldbronzecommodity_Select, defraimp_goldbronzecommodity_Filter, { team_defraimp_goldbronzecommodity: defraimp_goldbronzecommodity_Result[] }>;
    team_defraimp_importapplication: WebExpand<Team_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { team_defraimp_importapplication: defraimp_importapplication_Result[] }>;
    team_defraimp_importinspection: WebExpand<Team_Expand, defraimp_importinspection_Select, defraimp_importinspection_Filter, { team_defraimp_importinspection: defraimp_importinspection_Result[] }>;
    team_defraimp_importnotification: WebExpand<Team_Expand, defraimp_importnotification_Select, defraimp_importnotification_Filter, { team_defraimp_importnotification: defraimp_importnotification_Result[] }>;
    team_defraimp_inspectioncoveragerule: WebExpand<Team_Expand, defraimp_inspectioncoveragerule_Select, defraimp_inspectioncoveragerule_Filter, { team_defraimp_inspectioncoveragerule: defraimp_inspectioncoveragerule_Result[] }>;
    team_defraimp_itahc: WebExpand<Team_Expand, defraimp_itahc_Select, defraimp_itahc_Filter, { team_defraimp_itahc: defraimp_itahc_Result[] }>;
    team_defraimp_placeoforigin: WebExpand<Team_Expand, defraimp_placeoforigin_Select, defraimp_placeoforigin_Filter, { team_defraimp_placeoforigin: defraimp_placeoforigin_Result[] }>;
    team_defraimp_sampletest: WebExpand<Team_Expand, defraimp_sampletest_Select, defraimp_sampletest_Filter, { team_defraimp_sampletest: defraimp_sampletest_Result[] }>;
    team_processsession: WebExpand<Team_Expand, ProcessSession_Select, ProcessSession_Filter, { team_processsession: ProcessSession_Result[] }>;
    team_workflow: WebExpand<Team_Expand, Workflow_Select, Workflow_Filter, { team_workflow: Workflow_Result[] }>;
    teammembership_association: WebExpand<Team_Expand, SystemUser_Select, SystemUser_Filter, { teammembership_association: SystemUser_Result[] }>;
  }
  interface Team_FormattedResult {
    administratorid_formatted?: string;
    businessunitid_formatted?: string;
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    modifiedby_formatted?: string;
    modifiedon_formatted?: string;
    modifiedonbehalfby_formatted?: string;
    overriddencreatedon_formatted?: string;
    queueid_formatted?: string;
    regardingobjectid_formatted?: string;
    teamtemplateid_formatted?: string;
    teamtype_formatted?: string;
    transactioncurrencyid_formatted?: string;
  }
  interface Team_Result extends Team_Base, Team_Relationships {
    "@odata.etag": string;
    administratorid_guid: string | null;
    businessunitid_guid: string | null;
    createdby_guid: string | null;
    createdonbehalfby_guid: string | null;
    modifiedby_guid: string | null;
    modifiedonbehalfby_guid: string | null;
    queueid_guid: string | null;
    regardingobjectid_guid: string | null;
    teamtemplateid_guid: string | null;
    transactioncurrencyid_guid: string | null;
  }
  interface Team_RelatedOne {
    administratorid: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    createdby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    createdonbehalfby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    modifiedby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
    modifiedonbehalfby: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
  }
  interface Team_RelatedMany {
    OwningTeam_postfollows: WebMappingRetrieve<WebApi.PostFollow_Select,WebApi.PostFollow_Expand,WebApi.PostFollow_Filter,WebApi.PostFollow_Fixed,WebApi.PostFollow_Result,WebApi.PostFollow_FormattedResult>;
    Team_ProcessSessions: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    Team_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
    defraimp_importquery_team_owningteam: WebMappingRetrieve<WebApi.defraimp_importquery_Select,WebApi.defraimp_importquery_Expand,WebApi.defraimp_importquery_Filter,WebApi.defraimp_importquery_Fixed,WebApi.defraimp_importquery_Result,WebApi.defraimp_importquery_FormattedResult>;
    defraimp_team_defraimp_importapplication: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    team_SyncError: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
    team_connections1: WebMappingRetrieve<WebApi.Connection_Select,WebApi.Connection_Expand,WebApi.Connection_Filter,WebApi.Connection_Fixed,WebApi.Connection_Result,WebApi.Connection_FormattedResult>;
    team_connections2: WebMappingRetrieve<WebApi.Connection_Select,WebApi.Connection_Expand,WebApi.Connection_Filter,WebApi.Connection_Fixed,WebApi.Connection_Result,WebApi.Connection_FormattedResult>;
    team_defra_country: WebMappingRetrieve<WebApi.defra_country_Select,WebApi.defra_country_Expand,WebApi.defra_country_Filter,WebApi.defra_country_Fixed,WebApi.defra_country_Result,WebApi.defra_country_FormattedResult>;
    team_defraexp_commoditytype: WebMappingRetrieve<WebApi.defraexp_commoditytype_Select,WebApi.defraexp_commoditytype_Expand,WebApi.defraexp_commoditytype_Filter,WebApi.defraexp_commoditytype_Fixed,WebApi.defraexp_commoditytype_Result,WebApi.defraexp_commoditytype_FormattedResult>;
    team_defraimp_apharegion: WebMappingRetrieve<WebApi.defraimp_apharegion_Select,WebApi.defraimp_apharegion_Expand,WebApi.defraimp_apharegion_Filter,WebApi.defraimp_apharegion_Fixed,WebApi.defraimp_apharegion_Result,WebApi.defraimp_apharegion_FormattedResult>;
    team_defraimp_docom: WebMappingRetrieve<WebApi.defraimp_docom_Select,WebApi.defraimp_docom_Expand,WebApi.defraimp_docom_Filter,WebApi.defraimp_docom_Fixed,WebApi.defraimp_docom_Result,WebApi.defraimp_docom_FormattedResult>;
    team_defraimp_goldbronzecommodity: WebMappingRetrieve<WebApi.defraimp_goldbronzecommodity_Select,WebApi.defraimp_goldbronzecommodity_Expand,WebApi.defraimp_goldbronzecommodity_Filter,WebApi.defraimp_goldbronzecommodity_Fixed,WebApi.defraimp_goldbronzecommodity_Result,WebApi.defraimp_goldbronzecommodity_FormattedResult>;
    team_defraimp_importapplication: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    team_defraimp_importinspection: WebMappingRetrieve<WebApi.defraimp_importinspection_Select,WebApi.defraimp_importinspection_Expand,WebApi.defraimp_importinspection_Filter,WebApi.defraimp_importinspection_Fixed,WebApi.defraimp_importinspection_Result,WebApi.defraimp_importinspection_FormattedResult>;
    team_defraimp_importnotification: WebMappingRetrieve<WebApi.defraimp_importnotification_Select,WebApi.defraimp_importnotification_Expand,WebApi.defraimp_importnotification_Filter,WebApi.defraimp_importnotification_Fixed,WebApi.defraimp_importnotification_Result,WebApi.defraimp_importnotification_FormattedResult>;
    team_defraimp_inspectioncoveragerule: WebMappingRetrieve<WebApi.defraimp_inspectioncoveragerule_Select,WebApi.defraimp_inspectioncoveragerule_Expand,WebApi.defraimp_inspectioncoveragerule_Filter,WebApi.defraimp_inspectioncoveragerule_Fixed,WebApi.defraimp_inspectioncoveragerule_Result,WebApi.defraimp_inspectioncoveragerule_FormattedResult>;
    team_defraimp_itahc: WebMappingRetrieve<WebApi.defraimp_itahc_Select,WebApi.defraimp_itahc_Expand,WebApi.defraimp_itahc_Filter,WebApi.defraimp_itahc_Fixed,WebApi.defraimp_itahc_Result,WebApi.defraimp_itahc_FormattedResult>;
    team_defraimp_placeoforigin: WebMappingRetrieve<WebApi.defraimp_placeoforigin_Select,WebApi.defraimp_placeoforigin_Expand,WebApi.defraimp_placeoforigin_Filter,WebApi.defraimp_placeoforigin_Fixed,WebApi.defraimp_placeoforigin_Result,WebApi.defraimp_placeoforigin_FormattedResult>;
    team_defraimp_sampletest: WebMappingRetrieve<WebApi.defraimp_sampletest_Select,WebApi.defraimp_sampletest_Expand,WebApi.defraimp_sampletest_Filter,WebApi.defraimp_sampletest_Fixed,WebApi.defraimp_sampletest_Result,WebApi.defraimp_sampletest_FormattedResult>;
    team_processsession: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    team_workflow: WebMappingRetrieve<WebApi.Workflow_Select,WebApi.Workflow_Expand,WebApi.Workflow_Filter,WebApi.Workflow_Fixed,WebApi.Workflow_Result,WebApi.Workflow_FormattedResult>;
    teammembership_association: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  teams: WebMappingRetrieve<WebApi.Team_Select,WebApi.Team_Expand,WebApi.Team_Filter,WebApi.Team_Fixed,WebApi.Team_Result,WebApi.Team_FormattedResult>;
}
interface WebEntitiesRelated {
  teams: WebMappingRelated<WebApi.Team_RelatedOne,WebApi.Team_RelatedMany>;
}
interface WebEntitiesCUDA {
  teams: WebMappingCUDA<WebApi.Team_Create,WebApi.Team_Update,WebApi.Team_Select>;
}
