declare namespace WebApi {
  interface defraimp_PotentiallyRelatedImportRecords_Base extends WebEntity {
    defraimp_importapplicationid?: string | null;
    defraimp_matchrecordid?: string | null;
    defraimp_potentiallyrelatedimportrecordsid?: string | null;
    versionnumber?: number | null;
  }
  interface defraimp_PotentiallyRelatedImportRecords_Relationships {
    defraimp_PotentiallyRelatedImportRecords?: defraimp_importapplication_Result[] | null;
  }
  interface defraimp_PotentiallyRelatedImportRecords extends defraimp_PotentiallyRelatedImportRecords_Base, defraimp_PotentiallyRelatedImportRecords_Relationships {
  }
  interface defraimp_PotentiallyRelatedImportRecords_Create extends defraimp_PotentiallyRelatedImportRecords {
  }
  interface defraimp_PotentiallyRelatedImportRecords_Update extends defraimp_PotentiallyRelatedImportRecords {
  }
  interface defraimp_PotentiallyRelatedImportRecords_Select {
    defraimp_importapplicationid: WebAttribute<defraimp_PotentiallyRelatedImportRecords_Select, { defraimp_importapplicationid: string | null }, {  }>;
    defraimp_matchrecordid: WebAttribute<defraimp_PotentiallyRelatedImportRecords_Select, { defraimp_matchrecordid: string | null }, {  }>;
    defraimp_potentiallyrelatedimportrecordsid: WebAttribute<defraimp_PotentiallyRelatedImportRecords_Select, { defraimp_potentiallyrelatedimportrecordsid: string | null }, {  }>;
    versionnumber: WebAttribute<defraimp_PotentiallyRelatedImportRecords_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_PotentiallyRelatedImportRecords_Filter {
    defraimp_importapplicationid: XQW.Guid;
    defraimp_matchrecordid: XQW.Guid;
    defraimp_potentiallyrelatedimportrecordsid: XQW.Guid;
    versionnumber: number;
  }
  interface defraimp_PotentiallyRelatedImportRecords_Expand {
    defraimp_PotentiallyRelatedImportRecords: WebExpand<defraimp_PotentiallyRelatedImportRecords_Expand, defraimp_importapplication_Select, defraimp_importapplication_Filter, { defraimp_PotentiallyRelatedImportRecords: defraimp_importapplication_Result[] }>;
  }
  interface defraimp_PotentiallyRelatedImportRecords_FormattedResult {
  }
  interface defraimp_PotentiallyRelatedImportRecords_Result extends defraimp_PotentiallyRelatedImportRecords_Base, defraimp_PotentiallyRelatedImportRecords_Relationships {
    "@odata.etag": string;
  }
  interface defraimp_PotentiallyRelatedImportRecords_RelatedOne {
  }
  interface defraimp_PotentiallyRelatedImportRecords_RelatedMany {
    defraimp_PotentiallyRelatedImportRecords: WebMappingRetrieve<WebApi.defraimp_importapplication_Select,WebApi.defraimp_importapplication_Expand,WebApi.defraimp_importapplication_Filter,WebApi.defraimp_importapplication_Fixed,WebApi.defraimp_importapplication_Result,WebApi.defraimp_importapplication_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_potentiallyrelatedimportrecordsset: WebMappingRetrieve<WebApi.defraimp_PotentiallyRelatedImportRecords_Select,WebApi.defraimp_PotentiallyRelatedImportRecords_Expand,WebApi.defraimp_PotentiallyRelatedImportRecords_Filter,WebApi.defraimp_PotentiallyRelatedImportRecords_Fixed,WebApi.defraimp_PotentiallyRelatedImportRecords_Result,WebApi.defraimp_PotentiallyRelatedImportRecords_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_potentiallyrelatedimportrecordsset: WebMappingRelated<WebApi.defraimp_PotentiallyRelatedImportRecords_RelatedOne,WebApi.defraimp_PotentiallyRelatedImportRecords_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_potentiallyrelatedimportrecordsset: WebMappingCUDA<WebApi.defraimp_PotentiallyRelatedImportRecords_Create,WebApi.defraimp_PotentiallyRelatedImportRecords_Update,WebApi.defraimp_PotentiallyRelatedImportRecords_Select>;
}
