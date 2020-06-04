declare namespace WebApi {
  interface defraimp_matchrecord_Base extends WebEntity {
    createdon?: Date | null;
    defraimp_commoditycodematchrating?: string | null;
    defraimp_copyconsigneefrom?: boolean | null;
    defraimp_copyconsignorfrom?: boolean | null;
    defraimp_copykeydetailsfrom?: boolean | null;
    defraimp_copyplaceofdestinationfrom?: boolean | null;
    defraimp_copyplaceoforiginfrom?: boolean | null;
    defraimp_copytransporterfrom?: boolean | null;
    defraimp_countryoforiginmatchrating?: string | null;
    defraimp_dateofimportmatchrating?: string | null;
    defraimp_destinationpostcode?: string | null;
    defraimp_destinationpostcodematchrating?: string | null;
    defraimp_itahcnumbermatchrating?: string | null;
    defraimp_matchrecordid?: string | null;
    defraimp_name?: string | null;
    defraimp_organisationnamematchrating?: string | null;
    defraimp_overallmatchrating?: string | null;
    defraimp_overwriteexistingfieldsonimportrecord?: boolean | null;
    defraimp_quantitymatchrating?: string | null;
    defraimp_speciesmatchrating?: string | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    processid?: string | null;
    stageid?: string | null;
    statecode?: defraimp_matchrecord_statecode | null;
    statuscode?: defraimp_matchrecord_statuscode | null;
    timezoneruleversionnumber?: number | null;
    traversedpath?: string | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraimp_matchrecord_Relationships {
    defraimp_ITAHC?: defraimp_itahc_Result | null;
    defraimp_ImportRecord?: defraimp_importapplication_Result | null;
    defraimp_ImporterNotification?: defraimp_ImporterNotification_Result | null;
    defraimp_PotentiallyRelatedImportRecords?: defraimp_importapplication_Result[] | null;
  }
  interface defraimp_matchrecord extends defraimp_matchrecord_Base, defraimp_matchrecord_Relationships {
    defraimp_ITAHC_bind$defraimp_itahcs?: string | null;
    defraimp_ImportRecord_bind$defraimp_importapplications?: string | null;
    defraimp_ImporterNotification_bind$defraimp_importernotifications?: string | null;
    ownerid_bind$owners?: string | null;
    stageid_bind$processstages?: string | null;
  }
  interface defraimp_matchrecord_Create extends defraimp_matchrecord {
  }
  interface defraimp_matchrecord_Update extends defraimp_matchrecord {
  }
  interface defraimp_matchrecord_Select {
    createdby_guid: WebAttribute<defraimp_matchrecord_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraimp_matchrecord_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraimp_matchrecord_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defraimp_commoditycodematchrating: WebAttribute<defraimp_matchrecord_Select, { defraimp_commoditycodematchrating: string | null }, {  }>;
    defraimp_copyconsigneefrom: WebAttribute<defraimp_matchrecord_Select, { defraimp_copyconsigneefrom: boolean | null }, {  }>;
    defraimp_copyconsignorfrom: WebAttribute<defraimp_matchrecord_Select, { defraimp_copyconsignorfrom: boolean | null }, {  }>;
    defraimp_copykeydetailsfrom: WebAttribute<defraimp_matchrecord_Select, { defraimp_copykeydetailsfrom: boolean | null }, {  }>;
    defraimp_copyplaceofdestinationfrom: WebAttribute<defraimp_matchrecord_Select, { defraimp_copyplaceofdestinationfrom: boolean | null }, {  }>;
    defraimp_copyplaceoforiginfrom: WebAttribute<defraimp_matchrecord_Select, { defraimp_copyplaceoforiginfrom: boolean | null }, {  }>;
    defraimp_copytransporterfrom: WebAttribute<defraimp_matchrecord_Select, { defraimp_copytransporterfrom: boolean | null }, {  }>;
    defraimp_countryoforiginmatchrating: WebAttribute<defraimp_matchrecord_Select, { defraimp_countryoforiginmatchrating: string | null }, {  }>;
    defraimp_dateofimportmatchrating: WebAttribute<defraimp_matchrecord_Select, { defraimp_dateofimportmatchrating: string | null }, {  }>;
    defraimp_destinationpostcode: WebAttribute<defraimp_matchrecord_Select, { defraimp_destinationpostcode: string | null }, {  }>;
    defraimp_destinationpostcodematchrating: WebAttribute<defraimp_matchrecord_Select, { defraimp_destinationpostcodematchrating: string | null }, {  }>;
    defraimp_importernotification_guid: WebAttribute<defraimp_matchrecord_Select, { defraimp_importernotification_guid: string | null }, { defraimp_importernotification_formatted?: string }>;
    defraimp_importrecord_guid: WebAttribute<defraimp_matchrecord_Select, { defraimp_importrecord_guid: string | null }, { defraimp_importrecord_formatted?: string }>;
    defraimp_itahc_guid: WebAttribute<defraimp_matchrecord_Select, { defraimp_itahc_guid: string | null }, { defraimp_itahc_formatted?: string }>;
    defraimp_itahcnumbermatchrating: WebAttribute<defraimp_matchrecord_Select, { defraimp_itahcnumbermatchrating: string | null }, {  }>;
    defraimp_matchrecordid: WebAttribute<defraimp_matchrecord_Select, { defraimp_matchrecordid: string | null }, {  }>;
    defraimp_name: WebAttribute<defraimp_matchrecord_Select, { defraimp_name: string | null }, {  }>;
    defraimp_organisationnamematchrating: WebAttribute<defraimp_matchrecord_Select, { defraimp_organisationnamematchrating: string | null }, {  }>;
    defraimp_overallmatchrating: WebAttribute<defraimp_matchrecord_Select, { defraimp_overallmatchrating: string | null }, {  }>;
    defraimp_overwriteexistingfieldsonimportrecord: WebAttribute<defraimp_matchrecord_Select, { defraimp_overwriteexistingfieldsonimportrecord: boolean | null }, {  }>;
    defraimp_quantitymatchrating: WebAttribute<defraimp_matchrecord_Select, { defraimp_quantitymatchrating: string | null }, {  }>;
    defraimp_speciesmatchrating: WebAttribute<defraimp_matchrecord_Select, { defraimp_speciesmatchrating: string | null }, {  }>;
    importsequencenumber: WebAttribute<defraimp_matchrecord_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraimp_matchrecord_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraimp_matchrecord_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraimp_matchrecord_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraimp_matchrecord_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    ownerid_guid: WebAttribute<defraimp_matchrecord_Select, { ownerid_guid: string | null }, { ownerid_formatted?: string }>;
    owningbusinessunit_guid: WebAttribute<defraimp_matchrecord_Select, { owningbusinessunit_guid: string | null }, { owningbusinessunit_formatted?: string }>;
    owningteam_guid: WebAttribute<defraimp_matchrecord_Select, { owningteam_guid: string | null }, { owningteam_formatted?: string }>;
    owninguser_guid: WebAttribute<defraimp_matchrecord_Select, { owninguser_guid: string | null }, { owninguser_formatted?: string }>;
    processid: WebAttribute<defraimp_matchrecord_Select, { processid: string | null }, {  }>;
    stageid: WebAttribute<defraimp_matchrecord_Select, { stageid: string | null }, {  }>;
    statecode: WebAttribute<defraimp_matchrecord_Select, { statecode: defraimp_matchrecord_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraimp_matchrecord_Select, { statuscode: defraimp_matchrecord_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraimp_matchrecord_Select, { timezoneruleversionnumber: number | null }, {  }>;
    traversedpath: WebAttribute<defraimp_matchrecord_Select, { traversedpath: string | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraimp_matchrecord_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraimp_matchrecord_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_matchrecord_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defraimp_commoditycodematchrating: string;
    defraimp_copyconsigneefrom: boolean;
    defraimp_copyconsignorfrom: boolean;
    defraimp_copykeydetailsfrom: boolean;
    defraimp_copyplaceofdestinationfrom: boolean;
    defraimp_copyplaceoforiginfrom: boolean;
    defraimp_copytransporterfrom: boolean;
    defraimp_countryoforiginmatchrating: string;
    defraimp_dateofimportmatchrating: string;
    defraimp_destinationpostcode: string;
    defraimp_destinationpostcodematchrating: string;
    defraimp_importernotification_guid: XQW.Guid;
    defraimp_importrecord_guid: XQW.Guid;
    defraimp_itahc_guid: XQW.Guid;
    defraimp_itahcnumbermatchrating: string;
    defraimp_matchrecordid: XQW.Guid;
    defraimp_name: string;
    defraimp_organisationnamematchrating: string;
    defraimp_overallmatchrating: string;
    defraimp_overwriteexistingfieldsonimportrecord: boolean;
    defraimp_quantitymatchrating: string;
    defraimp_speciesmatchrating: string;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    overriddencreatedon: Date;
    ownerid_guid: XQW.Guid;
    owningbusinessunit_guid: XQW.Guid;
    owningteam_guid: XQW.Guid;
    owninguser_guid: XQW.Guid;
    processid: XQW.Guid;
    stageid: XQW.Guid;
    statecode: defraimp_matchrecord_statecode;
    statuscode: defraimp_matchrecord_statuscode;
    timezoneruleversionnumber: number;
    traversedpath: string;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraimp_matchrecord_Expand {
    defraimp_ITAHC: WebExpand<defraimp_matchrecord_Expand, defraimp_itahc_Select, defraimp_itahc_Filter, { defraimp_ITAHC: defraimp_itahc_Result }>;
    defraimp_ImportRecord: WebExpand<defraimp_matchrecord_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_ImportRecord: defraimp_importapplication_Result }>;
    defraimp_ImporterNotification: WebExpand<defraimp_matchrecord_Expand, defraimp_ImporterNotification_Select, defraimp_ImporterNotification_Filter, { defraimp_ImporterNotification: defraimp_ImporterNotification_Result }>;
    defraimp_PotentiallyRelatedImportRecords: WebExpand<defraimp_matchrecord_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_PotentiallyRelatedImportRecords: defraimp_importapplication_Result[] }>;
  }
  interface defraimp_matchrecord_FormattedResult {
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    defraimp_importernotification_formatted?: string;
    defraimp_importrecord_formatted?: string;
    defraimp_itahc_formatted?: string;
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
  interface defraimp_matchrecord_Result extends defraimp_matchrecord_Base, defraimp_matchrecord_Relationships {
    "@odata.etag": string;
    createdby_guid: string | null;
    createdonbehalfby_guid: string | null;
    defraimp_importernotification_guid: string | null;
    defraimp_importrecord_guid: string | null;
    defraimp_itahc_guid: string | null;
    modifiedby_guid: string | null;
    modifiedonbehalfby_guid: string | null;
    ownerid_guid: string | null;
    owningbusinessunit_guid: string | null;
    owningteam_guid: string | null;
    owninguser_guid: string | null;
  }
  interface defraimp_matchrecord_RelatedOne {
    defraimp_ITAHC: WebMappingRetrieve<WebApi.defraimp_itahc_Select,WebApi.defraimp_itahc_Expand,WebApi.defraimp_itahc_Filter,WebApi.defraimp_itahc_Fixed,WebApi.defraimp_itahc_Result,WebApi.defraimp_itahc_FormattedResult>;
    defraimp_ImportRecord: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_ImporterNotification: WebMappingRetrieve<WebApi.defraimp_ImporterNotification_Select,WebApi.defraimp_ImporterNotification_Expand,WebApi.defraimp_ImporterNotification_Filter,WebApi.defraimp_ImporterNotification_Fixed,WebApi.defraimp_ImporterNotification_Result,WebApi.defraimp_ImporterNotification_FormattedResult>;
  }
  interface defraimp_matchrecord_RelatedMany {
    defraimp_PotentiallyRelatedImportRecords: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_matchrecords: WebMappingRetrieve<WebApi.defraimp_matchrecord_Select,WebApi.defraimp_matchrecord_Expand,WebApi.defraimp_matchrecord_Filter,WebApi.defraimp_matchrecord_Fixed,WebApi.defraimp_matchrecord_Result,WebApi.defraimp_matchrecord_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_matchrecords: WebMappingRelated<WebApi.defraimp_matchrecord_RelatedOne,WebApi.defraimp_matchrecord_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_matchrecords: WebMappingCUDA<WebApi.defraimp_matchrecord_Create,WebApi.defraimp_matchrecord_Update,WebApi.defraimp_matchrecord_Select>;
}
