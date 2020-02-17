declare namespace WebApi {
  interface Workflow_Base extends WebEntity {
    asyncautodelete?: boolean | null;
    businessprocesstype?: workflow_businessprocesstype | null;
    category?: workflow_category | null;
    clientdata?: string | null;
    componentstate?: componentstate | null;
    createdon?: Date | null;
    createstage?: workflow_stage | null;
    deletestage?: workflow_stage | null;
    description?: string | null;
    entityimageid?: string | null;
    formid?: string | null;
    inputparameters?: string | null;
    introducedversion?: string | null;
    iscrmuiworkflow?: boolean | null;
    iscustomizable?: any | null;
    ismanaged?: boolean | null;
    istransacted?: boolean | null;
    languagecode?: number | null;
    mode?: workflow_mode | null;
    modifiedon?: Date | null;
    name?: string | null;
    ondemand?: boolean | null;
    overwritetime?: Date | null;
    primaryentity?: string | null;
    processorder?: number | null;
    processroleassignment?: string | null;
    processtriggerformid?: string | null;
    processtriggerscope?: processtrigger_scope | null;
    rank?: number | null;
    rendererobjecttypecode?: string | null;
    runas?: workflow_runas | null;
    scope?: workflow_scope | null;
    solutionid?: string | null;
    statecode?: workflow_statecode | null;
    statuscode?: workflow_statuscode | null;
    subprocess?: boolean | null;
    supportingsolutionid?: string | null;
    syncworkflowlogonfailure?: boolean | null;
    triggeroncreate?: boolean | null;
    triggerondelete?: boolean | null;
    triggeronupdateattributelist?: string | null;
    type?: workflow_type | null;
    uidata?: string | null;
    uiflowtype?: workflow_uiflowtype | null;
    uniquename?: string | null;
    updatestage?: workflow_stage | null;
    versionnumber?: number | null;
    workflowid?: string | null;
    workflowidunique?: string | null;
    xaml?: string | null;
  }
  interface Workflow_Relationships {
    Workflow_SyncErrors?: SyncError_Result[] | null;
    lk_defraimp_importapplicationbusinessprocessflow_processid?: defraimp_importapplicationbusinessprocessflow_Result[] | null;
    lk_processsession_processid?: ProcessSession_Result[] | null;
    workflow_active_workflow?: Workflow_Result[] | null;
    workflow_parent_workflow?: Workflow_Result[] | null;
  }
  interface Workflow extends Workflow_Base, Workflow_Relationships {
    ownerid_bind$owners?: string | null;
  }
  interface Workflow_Create extends Workflow {
  }
  interface Workflow_Update extends Workflow {
  }
  interface Workflow_Select {
    activeworkflowid_guid: WebAttribute<Workflow_Select, { activeworkflowid_guid: string | null }, { activeworkflowid_formatted?: string }>;
    asyncautodelete: WebAttribute<Workflow_Select, { asyncautodelete: boolean | null }, {  }>;
    businessprocesstype: WebAttribute<Workflow_Select, { businessprocesstype: workflow_businessprocesstype | null }, { businessprocesstype_formatted?: string }>;
    category: WebAttribute<Workflow_Select, { category: workflow_category | null }, { category_formatted?: string }>;
    clientdata: WebAttribute<Workflow_Select, { clientdata: string | null }, {  }>;
    componentstate: WebAttribute<Workflow_Select, { componentstate: componentstate | null }, { componentstate_formatted?: string }>;
    createdby_guid: WebAttribute<Workflow_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<Workflow_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<Workflow_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    createstage: WebAttribute<Workflow_Select, { createstage: workflow_stage | null }, { createstage_formatted?: string }>;
    deletestage: WebAttribute<Workflow_Select, { deletestage: workflow_stage | null }, { deletestage_formatted?: string }>;
    description: WebAttribute<Workflow_Select, { description: string | null }, {  }>;
    entityimageid: WebAttribute<Workflow_Select, { entityimageid: string | null }, {  }>;
    formid: WebAttribute<Workflow_Select, { formid: string | null }, {  }>;
    inputparameters: WebAttribute<Workflow_Select, { inputparameters: string | null }, {  }>;
    introducedversion: WebAttribute<Workflow_Select, { introducedversion: string | null }, {  }>;
    iscrmuiworkflow: WebAttribute<Workflow_Select, { iscrmuiworkflow: boolean | null }, {  }>;
    iscustomizable: WebAttribute<Workflow_Select, { iscustomizable: any | null }, {  }>;
    ismanaged: WebAttribute<Workflow_Select, { ismanaged: boolean | null }, {  }>;
    istransacted: WebAttribute<Workflow_Select, { istransacted: boolean | null }, {  }>;
    languagecode: WebAttribute<Workflow_Select, { languagecode: number | null }, {  }>;
    mode: WebAttribute<Workflow_Select, { mode: workflow_mode | null }, { mode_formatted?: string }>;
    modifiedby_guid: WebAttribute<Workflow_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<Workflow_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<Workflow_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    name: WebAttribute<Workflow_Select, { name: string | null }, {  }>;
    ondemand: WebAttribute<Workflow_Select, { ondemand: boolean | null }, {  }>;
    overwritetime: WebAttribute<Workflow_Select, { overwritetime: Date | null }, { overwritetime_formatted?: string }>;
    ownerid_guid: WebAttribute<Workflow_Select, { ownerid_guid: string | null }, { ownerid_formatted?: string }>;
    owningbusinessunit_guid: WebAttribute<Workflow_Select, { owningbusinessunit_guid: string | null }, { owningbusinessunit_formatted?: string }>;
    owningteam_guid: WebAttribute<Workflow_Select, { owningteam_guid: string | null }, { owningteam_formatted?: string }>;
    owninguser_guid: WebAttribute<Workflow_Select, { owninguser_guid: string | null }, { owninguser_formatted?: string }>;
    parentworkflowid_guid: WebAttribute<Workflow_Select, { parentworkflowid_guid: string | null }, { parentworkflowid_formatted?: string }>;
    plugintypeid_guid: WebAttribute<Workflow_Select, { plugintypeid_guid: string | null }, { plugintypeid_formatted?: string }>;
    primaryentity: WebAttribute<Workflow_Select, { primaryentity: string | null }, {  }>;
    processorder: WebAttribute<Workflow_Select, { processorder: number | null }, {  }>;
    processroleassignment: WebAttribute<Workflow_Select, { processroleassignment: string | null }, {  }>;
    processtriggerformid: WebAttribute<Workflow_Select, { processtriggerformid: string | null }, {  }>;
    processtriggerscope: WebAttribute<Workflow_Select, { processtriggerscope: processtrigger_scope | null }, { processtriggerscope_formatted?: string }>;
    rank: WebAttribute<Workflow_Select, { rank: number | null }, {  }>;
    rendererobjecttypecode: WebAttribute<Workflow_Select, { rendererobjecttypecode: string | null }, {  }>;
    runas: WebAttribute<Workflow_Select, { runas: workflow_runas | null }, { runas_formatted?: string }>;
    scope: WebAttribute<Workflow_Select, { scope: workflow_scope | null }, { scope_formatted?: string }>;
    sdkmessageid_guid: WebAttribute<Workflow_Select, { sdkmessageid_guid: string | null }, { sdkmessageid_formatted?: string }>;
    solutionid: WebAttribute<Workflow_Select, { solutionid: string | null }, {  }>;
    statecode: WebAttribute<Workflow_Select, { statecode: workflow_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<Workflow_Select, { statuscode: workflow_statuscode | null }, { statuscode_formatted?: string }>;
    subprocess: WebAttribute<Workflow_Select, { subprocess: boolean | null }, {  }>;
    supportingsolutionid: WebAttribute<Workflow_Select, { supportingsolutionid: string | null }, {  }>;
    syncworkflowlogonfailure: WebAttribute<Workflow_Select, { syncworkflowlogonfailure: boolean | null }, {  }>;
    triggeroncreate: WebAttribute<Workflow_Select, { triggeroncreate: boolean | null }, {  }>;
    triggerondelete: WebAttribute<Workflow_Select, { triggerondelete: boolean | null }, {  }>;
    triggeronupdateattributelist: WebAttribute<Workflow_Select, { triggeronupdateattributelist: string | null }, {  }>;
    type: WebAttribute<Workflow_Select, { type: workflow_type | null }, { type_formatted?: string }>;
    uidata: WebAttribute<Workflow_Select, { uidata: string | null }, {  }>;
    uiflowtype: WebAttribute<Workflow_Select, { uiflowtype: workflow_uiflowtype | null }, { uiflowtype_formatted?: string }>;
    uniquename: WebAttribute<Workflow_Select, { uniquename: string | null }, {  }>;
    updatestage: WebAttribute<Workflow_Select, { updatestage: workflow_stage | null }, { updatestage_formatted?: string }>;
    versionnumber: WebAttribute<Workflow_Select, { versionnumber: number | null }, {  }>;
    workflowid: WebAttribute<Workflow_Select, { workflowid: string | null }, {  }>;
    workflowidunique: WebAttribute<Workflow_Select, { workflowidunique: string | null }, {  }>;
    xaml: WebAttribute<Workflow_Select, { xaml: string | null }, {  }>;
  }
  interface Workflow_Filter {
    activeworkflowid_guid: XQW.Guid;
    asyncautodelete: boolean;
    businessprocesstype: workflow_businessprocesstype;
    category: workflow_category;
    clientdata: string;
    componentstate: componentstate;
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    createstage: workflow_stage;
    deletestage: workflow_stage;
    description: string;
    entityimageid: XQW.Guid;
    formid: XQW.Guid;
    inputparameters: string;
    introducedversion: string;
    iscrmuiworkflow: boolean;
    iscustomizable: any;
    ismanaged: boolean;
    istransacted: boolean;
    languagecode: number;
    mode: workflow_mode;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    name: string;
    ondemand: boolean;
    overwritetime: Date;
    ownerid_guid: XQW.Guid;
    owningbusinessunit_guid: XQW.Guid;
    owningteam_guid: XQW.Guid;
    owninguser_guid: XQW.Guid;
    parentworkflowid_guid: XQW.Guid;
    plugintypeid_guid: XQW.Guid;
    primaryentity: string;
    processorder: number;
    processroleassignment: string;
    processtriggerformid: XQW.Guid;
    processtriggerscope: processtrigger_scope;
    rank: number;
    rendererobjecttypecode: string;
    runas: workflow_runas;
    scope: workflow_scope;
    sdkmessageid_guid: XQW.Guid;
    solutionid: XQW.Guid;
    statecode: workflow_statecode;
    statuscode: workflow_statuscode;
    subprocess: boolean;
    supportingsolutionid: XQW.Guid;
    syncworkflowlogonfailure: boolean;
    triggeroncreate: boolean;
    triggerondelete: boolean;
    triggeronupdateattributelist: string;
    type: workflow_type;
    uidata: string;
    uiflowtype: workflow_uiflowtype;
    uniquename: string;
    updatestage: workflow_stage;
    versionnumber: number;
    workflowid: XQW.Guid;
    workflowidunique: XQW.Guid;
    xaml: string;
  }
  interface Workflow_Expand {
    Workflow_SyncErrors: WebExpand<Workflow_Expand, SyncError_Select, SyncError_Filter, { Workflow_SyncErrors: SyncError_Result[] }>;
    activeworkflowid: WebExpand<Workflow_Expand, Workflow_Select, Workflow_Filter, { activeworkflowid: Workflow_Result }>;
    lk_defraimp_importapplicationbusinessprocessflow_processid: WebExpand<Workflow_Expand, defraimp_importapplicationbusinessprocessflow_Select, defraimp_importapplicationbusinessprocessflow_Filter, { lk_defraimp_importapplicationbusinessprocessflow_processid: defraimp_importapplicationbusinessprocessflow_Result[] }>;
    lk_processsession_processid: WebExpand<Workflow_Expand, ProcessSession_Select, ProcessSession_Filter, { lk_processsession_processid: ProcessSession_Result[] }>;
    parentworkflowid: WebExpand<Workflow_Expand, Workflow_Select, Workflow_Filter, { parentworkflowid: Workflow_Result }>;
    workflow_active_workflow: WebExpand<Workflow_Expand, Workflow_Select, Workflow_Filter, { workflow_active_workflow: Workflow_Result[] }>;
    workflow_parent_workflow: WebExpand<Workflow_Expand, Workflow_Select, Workflow_Filter, { workflow_parent_workflow: Workflow_Result[] }>;
  }
  interface Workflow_FormattedResult {
    activeworkflowid_formatted?: string;
    businessprocesstype_formatted?: string;
    category_formatted?: string;
    componentstate_formatted?: string;
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    createstage_formatted?: string;
    deletestage_formatted?: string;
    mode_formatted?: string;
    modifiedby_formatted?: string;
    modifiedon_formatted?: string;
    modifiedonbehalfby_formatted?: string;
    overwritetime_formatted?: string;
    ownerid_formatted?: string;
    owningbusinessunit_formatted?: string;
    owningteam_formatted?: string;
    owninguser_formatted?: string;
    parentworkflowid_formatted?: string;
    plugintypeid_formatted?: string;
    processtriggerscope_formatted?: string;
    runas_formatted?: string;
    scope_formatted?: string;
    sdkmessageid_formatted?: string;
    statecode_formatted?: string;
    statuscode_formatted?: string;
    type_formatted?: string;
    uiflowtype_formatted?: string;
    updatestage_formatted?: string;
  }
  interface Workflow_Result extends Workflow_Base, Workflow_Relationships {
    "@odata.etag": string;
    activeworkflowid_guid: string | null;
    createdby_guid: string | null;
    createdonbehalfby_guid: string | null;
    modifiedby_guid: string | null;
    modifiedonbehalfby_guid: string | null;
    ownerid_guid: string | null;
    owningbusinessunit_guid: string | null;
    owningteam_guid: string | null;
    owninguser_guid: string | null;
    parentworkflowid_guid: string | null;
    plugintypeid_guid: string | null;
    sdkmessageid_guid: string | null;
  }
  interface Workflow_RelatedOne {
    activeworkflowid: WebMappingRetrieve<WebApi.Workflow_Select,WebApi.Workflow_Expand,WebApi.Workflow_Filter,WebApi.Workflow_Fixed,WebApi.Workflow_Result,WebApi.Workflow_FormattedResult>;
    parentworkflowid: WebMappingRetrieve<WebApi.Workflow_Select,WebApi.Workflow_Expand,WebApi.Workflow_Filter,WebApi.Workflow_Fixed,WebApi.Workflow_Result,WebApi.Workflow_FormattedResult>;
  }
  interface Workflow_RelatedMany {
    Workflow_SyncErrors: WebMappingRetrieve<WebApi.SyncError_Select,WebApi.SyncError_Expand,WebApi.SyncError_Filter,WebApi.SyncError_Fixed,WebApi.SyncError_Result,WebApi.SyncError_FormattedResult>;
    lk_defraimp_importapplicationbusinessprocessflow_processid: WebMappingRetrieve<WebApi.defraimp_importapplicationbusinessprocessflow_Select,WebApi.defraimp_importapplicationbusinessprocessflow_Expand,WebApi.defraimp_importapplicationbusinessprocessflow_Filter,WebApi.defraimp_importapplicationbusinessprocessflow_Fixed,WebApi.defraimp_importapplicationbusinessprocessflow_Result,WebApi.defraimp_importapplicationbusinessprocessflow_FormattedResult>;
    lk_processsession_processid: WebMappingRetrieve<WebApi.ProcessSession_Select,WebApi.ProcessSession_Expand,WebApi.ProcessSession_Filter,WebApi.ProcessSession_Fixed,WebApi.ProcessSession_Result,WebApi.ProcessSession_FormattedResult>;
    workflow_active_workflow: WebMappingRetrieve<WebApi.Workflow_Select,WebApi.Workflow_Expand,WebApi.Workflow_Filter,WebApi.Workflow_Fixed,WebApi.Workflow_Result,WebApi.Workflow_FormattedResult>;
    workflow_parent_workflow: WebMappingRetrieve<WebApi.Workflow_Select,WebApi.Workflow_Expand,WebApi.Workflow_Filter,WebApi.Workflow_Fixed,WebApi.Workflow_Result,WebApi.Workflow_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  workflows: WebMappingRetrieve<WebApi.Workflow_Select,WebApi.Workflow_Expand,WebApi.Workflow_Filter,WebApi.Workflow_Fixed,WebApi.Workflow_Result,WebApi.Workflow_FormattedResult>;
}
interface WebEntitiesRelated {
  workflows: WebMappingRelated<WebApi.Workflow_RelatedOne,WebApi.Workflow_RelatedMany>;
}
interface WebEntitiesCUDA {
  workflows: WebMappingCUDA<WebApi.Workflow_Create,WebApi.Workflow_Update,WebApi.Workflow_Select>;
}
