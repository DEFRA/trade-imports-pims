declare namespace WebApi {
  interface defraimp_placeoforigin_Base extends WebEntity {
    createdon?: Date | null;
    defraimp_addresscity?: string | null;
    defraimp_addressline1?: string | null;
    defraimp_addressline2?: string | null;
    defraimp_addressline3?: string | null;
    defraimp_addressstateorprovince?: string | null;
    defraimp_applicationcounter?: number | null;
    defraimp_datelockedtobronze?: Date | null;
    defraimp_datesettogold?: Date | null;
    defraimp_dateunlockedfrombronze?: Date | null;
    defraimp_inspectionquotacounter?: number | null;
    defraimp_locktobronze?: boolean | null;
    defraimp_name?: string | null;
    defraimp_numberofapplications?: number | null;
    defraimp_numberofapplicationssincelastinspection?: number | null;
    defraimp_numberofsuccessfulapplications?: number | null;
    defraimp_placeoforiginid?: string | null;
    defraimp_postcode?: string | null;
    defraimp_previousnumberofapplicationssincelastins?: number | null;
    defraimp_previoustrustlevel?: defraimp_trustlevel | null;
    defraimp_reasonlockedtobronze?: string | null;
    defraimp_reasonunlockedfrombronze?: string | null;
    defraimp_trustlevel?: defraimp_trustlevel | null;
    importsequencenumber?: number | null;
    modifiedon?: Date | null;
    overriddencreatedon?: Date | null;
    statecode?: defraimp_placeoforigin_statecode | null;
    statuscode?: defraimp_placeoforigin_statuscode | null;
    timezoneruleversionnumber?: number | null;
    utcconversiontimezonecode?: number | null;
    versionnumber?: number | null;
  }
  interface defraimp_placeoforigin_Relationships {
    defraimp_defraimp_placeoforigin_defraimp_importapplication_PlaceofOriginid?: defraimp_importapplication_Result[] | null;
    defraimp_defraimp_placeoforigin_defraimp_importapplication_previousplaceoforiginid?: defraimp_importapplication_Result[] | null;
  }
  interface defraimp_placeoforigin extends defraimp_placeoforigin_Base, defraimp_placeoforigin_Relationships {
    defraimp_AddressCountry_bind$defra_countries?: string | null;
    ownerid_bind$owners?: string | null;
  }
  interface defraimp_placeoforigin_Create extends defraimp_placeoforigin {
  }
  interface defraimp_placeoforigin_Update extends defraimp_placeoforigin {
  }
  interface defraimp_placeoforigin_Select {
    createdby_guid: WebAttribute<defraimp_placeoforigin_Select, { createdby_guid: string | null }, { createdby_formatted?: string }>;
    createdon: WebAttribute<defraimp_placeoforigin_Select, { createdon: Date | null }, { createdon_formatted?: string }>;
    createdonbehalfby_guid: WebAttribute<defraimp_placeoforigin_Select, { createdonbehalfby_guid: string | null }, { createdonbehalfby_formatted?: string }>;
    defraimp_addresscity: WebAttribute<defraimp_placeoforigin_Select, { defraimp_addresscity: string | null }, {  }>;
    defraimp_addresscountry_guid: WebAttribute<defraimp_placeoforigin_Select, { defraimp_addresscountry_guid: string | null }, { defraimp_addresscountry_formatted?: string }>;
    defraimp_addressline1: WebAttribute<defraimp_placeoforigin_Select, { defraimp_addressline1: string | null }, {  }>;
    defraimp_addressline2: WebAttribute<defraimp_placeoforigin_Select, { defraimp_addressline2: string | null }, {  }>;
    defraimp_addressline3: WebAttribute<defraimp_placeoforigin_Select, { defraimp_addressline3: string | null }, {  }>;
    defraimp_addressstateorprovince: WebAttribute<defraimp_placeoforigin_Select, { defraimp_addressstateorprovince: string | null }, {  }>;
    defraimp_applicationcounter: WebAttribute<defraimp_placeoforigin_Select, { defraimp_applicationcounter: number | null }, {  }>;
    defraimp_datelockedtobronze: WebAttribute<defraimp_placeoforigin_Select, { defraimp_datelockedtobronze: Date | null }, { defraimp_datelockedtobronze_formatted?: string }>;
    defraimp_datesettogold: WebAttribute<defraimp_placeoforigin_Select, { defraimp_datesettogold: Date | null }, { defraimp_datesettogold_formatted?: string }>;
    defraimp_dateunlockedfrombronze: WebAttribute<defraimp_placeoforigin_Select, { defraimp_dateunlockedfrombronze: Date | null }, { defraimp_dateunlockedfrombronze_formatted?: string }>;
    defraimp_inspectionquotacounter: WebAttribute<defraimp_placeoforigin_Select, { defraimp_inspectionquotacounter: number | null }, {  }>;
    defraimp_locktobronze: WebAttribute<defraimp_placeoforigin_Select, { defraimp_locktobronze: boolean | null }, {  }>;
    defraimp_name: WebAttribute<defraimp_placeoforigin_Select, { defraimp_name: string | null }, {  }>;
    defraimp_numberofapplications: WebAttribute<defraimp_placeoforigin_Select, { defraimp_numberofapplications: number | null }, {  }>;
    defraimp_numberofapplicationssincelastinspection: WebAttribute<defraimp_placeoforigin_Select, { defraimp_numberofapplicationssincelastinspection: number | null }, {  }>;
    defraimp_numberofsuccessfulapplications: WebAttribute<defraimp_placeoforigin_Select, { defraimp_numberofsuccessfulapplications: number | null }, {  }>;
    defraimp_placeoforiginid: WebAttribute<defraimp_placeoforigin_Select, { defraimp_placeoforiginid: string | null }, {  }>;
    defraimp_postcode: WebAttribute<defraimp_placeoforigin_Select, { defraimp_postcode: string | null }, {  }>;
    defraimp_previousnumberofapplicationssincelastins: WebAttribute<defraimp_placeoforigin_Select, { defraimp_previousnumberofapplicationssincelastins: number | null }, {  }>;
    defraimp_previoustrustlevel: WebAttribute<defraimp_placeoforigin_Select, { defraimp_previoustrustlevel: defraimp_trustlevel | null }, { defraimp_previoustrustlevel_formatted?: string }>;
    defraimp_reasonlockedtobronze: WebAttribute<defraimp_placeoforigin_Select, { defraimp_reasonlockedtobronze: string | null }, {  }>;
    defraimp_reasonunlockedfrombronze: WebAttribute<defraimp_placeoforigin_Select, { defraimp_reasonunlockedfrombronze: string | null }, {  }>;
    defraimp_trustlevel: WebAttribute<defraimp_placeoforigin_Select, { defraimp_trustlevel: defraimp_trustlevel | null }, { defraimp_trustlevel_formatted?: string }>;
    importsequencenumber: WebAttribute<defraimp_placeoforigin_Select, { importsequencenumber: number | null }, {  }>;
    modifiedby_guid: WebAttribute<defraimp_placeoforigin_Select, { modifiedby_guid: string | null }, { modifiedby_formatted?: string }>;
    modifiedon: WebAttribute<defraimp_placeoforigin_Select, { modifiedon: Date | null }, { modifiedon_formatted?: string }>;
    modifiedonbehalfby_guid: WebAttribute<defraimp_placeoforigin_Select, { modifiedonbehalfby_guid: string | null }, { modifiedonbehalfby_formatted?: string }>;
    overriddencreatedon: WebAttribute<defraimp_placeoforigin_Select, { overriddencreatedon: Date | null }, { overriddencreatedon_formatted?: string }>;
    ownerid_guid: WebAttribute<defraimp_placeoforigin_Select, { ownerid_guid: string | null }, { ownerid_formatted?: string }>;
    owningbusinessunit_guid: WebAttribute<defraimp_placeoforigin_Select, { owningbusinessunit_guid: string | null }, { owningbusinessunit_formatted?: string }>;
    owningteam_guid: WebAttribute<defraimp_placeoforigin_Select, { owningteam_guid: string | null }, { owningteam_formatted?: string }>;
    owninguser_guid: WebAttribute<defraimp_placeoforigin_Select, { owninguser_guid: string | null }, { owninguser_formatted?: string }>;
    statecode: WebAttribute<defraimp_placeoforigin_Select, { statecode: defraimp_placeoforigin_statecode | null }, { statecode_formatted?: string }>;
    statuscode: WebAttribute<defraimp_placeoforigin_Select, { statuscode: defraimp_placeoforigin_statuscode | null }, { statuscode_formatted?: string }>;
    timezoneruleversionnumber: WebAttribute<defraimp_placeoforigin_Select, { timezoneruleversionnumber: number | null }, {  }>;
    utcconversiontimezonecode: WebAttribute<defraimp_placeoforigin_Select, { utcconversiontimezonecode: number | null }, {  }>;
    versionnumber: WebAttribute<defraimp_placeoforigin_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_placeoforigin_Filter {
    createdby_guid: XQW.Guid;
    createdon: Date;
    createdonbehalfby_guid: XQW.Guid;
    defraimp_addresscity: string;
    defraimp_addresscountry_guid: XQW.Guid;
    defraimp_addressline1: string;
    defraimp_addressline2: string;
    defraimp_addressline3: string;
    defraimp_addressstateorprovince: string;
    defraimp_applicationcounter: number;
    defraimp_datelockedtobronze: Date;
    defraimp_datesettogold: Date;
    defraimp_dateunlockedfrombronze: Date;
    defraimp_inspectionquotacounter: number;
    defraimp_locktobronze: boolean;
    defraimp_name: string;
    defraimp_numberofapplications: number;
    defraimp_numberofapplicationssincelastinspection: number;
    defraimp_numberofsuccessfulapplications: number;
    defraimp_placeoforiginid: XQW.Guid;
    defraimp_postcode: string;
    defraimp_previousnumberofapplicationssincelastins: number;
    defraimp_previoustrustlevel: defraimp_trustlevel;
    defraimp_reasonlockedtobronze: string;
    defraimp_reasonunlockedfrombronze: string;
    defraimp_trustlevel: defraimp_trustlevel;
    importsequencenumber: number;
    modifiedby_guid: XQW.Guid;
    modifiedon: Date;
    modifiedonbehalfby_guid: XQW.Guid;
    overriddencreatedon: Date;
    ownerid_guid: XQW.Guid;
    owningbusinessunit_guid: XQW.Guid;
    owningteam_guid: XQW.Guid;
    owninguser_guid: XQW.Guid;
    statecode: defraimp_placeoforigin_statecode;
    statuscode: defraimp_placeoforigin_statuscode;
    timezoneruleversionnumber: number;
    utcconversiontimezonecode: number;
    versionnumber: number;
  }
  interface defraimp_placeoforigin_Expand {
    defraimp_defraimp_placeoforigin_defraimp_importapplication_PlaceofOriginid: WebExpand<defraimp_placeoforigin_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defraimp_placeoforigin_defraimp_importapplication_PlaceofOriginid: defraimp_importapplication_Result[] }>;
    defraimp_defraimp_placeoforigin_defraimp_importapplication_previousplaceoforiginid: WebExpand<defraimp_placeoforigin_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_defraimp_placeoforigin_defraimp_importapplication_previousplaceoforiginid: defraimp_importapplication_Result[] }>;
  }
  interface defraimp_placeoforigin_FormattedResult {
    createdby_formatted?: string;
    createdon_formatted?: string;
    createdonbehalfby_formatted?: string;
    defraimp_addresscountry_formatted?: string;
    defraimp_datelockedtobronze_formatted?: string;
    defraimp_datesettogold_formatted?: string;
    defraimp_dateunlockedfrombronze_formatted?: string;
    defraimp_previoustrustlevel_formatted?: string;
    defraimp_trustlevel_formatted?: string;
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
  interface defraimp_placeoforigin_Result extends defraimp_placeoforigin_Base, defraimp_placeoforigin_Relationships {
    "@odata.etag": string;
    createdby_guid: string | null;
    createdonbehalfby_guid: string | null;
    defraimp_addresscountry_guid: string | null;
    modifiedby_guid: string | null;
    modifiedonbehalfby_guid: string | null;
    ownerid_guid: string | null;
    owningbusinessunit_guid: string | null;
    owningteam_guid: string | null;
    owninguser_guid: string | null;
  }
  interface defraimp_placeoforigin_RelatedOne {
  }
  interface defraimp_placeoforigin_RelatedMany {
    defraimp_defraimp_placeoforigin_defraimp_importapplication_PlaceofOriginid: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
    defraimp_defraimp_placeoforigin_defraimp_importapplication_previousplaceoforiginid: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_placeoforigins: WebMappingRetrieve<WebApi.defraimp_placeoforigin_Select,WebApi.defraimp_placeoforigin_Expand,WebApi.defraimp_placeoforigin_Filter,WebApi.defraimp_placeoforigin_Fixed,WebApi.defraimp_placeoforigin_Result,WebApi.defraimp_placeoforigin_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_placeoforigins: WebMappingRelated<WebApi.defraimp_placeoforigin_RelatedOne,WebApi.defraimp_placeoforigin_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_placeoforigins: WebMappingCUDA<WebApi.defraimp_placeoforigin_Create,WebApi.defraimp_placeoforigin_Update,WebApi.defraimp_placeoforigin_Select>;
}
