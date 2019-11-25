declare namespace WebApi {
  interface defraimp_defraimp_importinspection_defraimp_samp_Base extends WebEntity {
    defraimp_defraimp_importinspection_defraimp_sampid?: string | null;
    defraimp_importinspectionid?: string | null;
    defraimp_sampletestid?: string | null;
    versionnumber?: number | null;
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Relationships {
    defraimp_defraimp_importinspection_defraimp_sample?: defraimp_sampletest_Result[] | null;
  }
  interface defraimp_defraimp_importinspection_defraimp_samp extends defraimp_defraimp_importinspection_defraimp_samp_Base, defraimp_defraimp_importinspection_defraimp_samp_Relationships {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Create extends defraimp_defraimp_importinspection_defraimp_samp {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Update extends defraimp_defraimp_importinspection_defraimp_samp {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Select {
    defraimp_defraimp_importinspection_defraimp_sampid: WebAttribute<defraimp_defraimp_importinspection_defraimp_samp_Select, { defraimp_defraimp_importinspection_defraimp_sampid: string | null }, {  }>;
    defraimp_importinspectionid: WebAttribute<defraimp_defraimp_importinspection_defraimp_samp_Select, { defraimp_importinspectionid: string | null }, {  }>;
    defraimp_sampletestid: WebAttribute<defraimp_defraimp_importinspection_defraimp_samp_Select, { defraimp_sampletestid: string | null }, {  }>;
    versionnumber: WebAttribute<defraimp_defraimp_importinspection_defraimp_samp_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Filter {
    defraimp_defraimp_importinspection_defraimp_sampid: XQW.Guid;
    defraimp_importinspectionid: XQW.Guid;
    defraimp_sampletestid: XQW.Guid;
    versionnumber: number;
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Expand {
    defraimp_defraimp_importinspection_defraimp_sample: WebExpand<defraimp_defraimp_importinspection_defraimp_samp_Expand, defraimp_sampletest_Select, defraimp_sampletest_Filter, { defraimp_defraimp_importinspection_defraimp_sample: defraimp_sampletest_Result[] }>;
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_FormattedResult {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Result extends defraimp_defraimp_importinspection_defraimp_samp_Base, defraimp_defraimp_importinspection_defraimp_samp_Relationships {
    "@odata.etag": string;
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_RelatedOne {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_RelatedMany {
    defraimp_defraimp_importinspection_defraimp_sample: WebMappingRetrieve<WebApi.defraimp_sampletest_Select,WebApi.defraimp_sampletest_Expand,WebApi.defraimp_sampletest_Filter,WebApi.defraimp_sampletest_Fixed,WebApi.defraimp_sampletest_Result,WebApi.defraimp_sampletest_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_defraimp_importinspection_defraimp_sampset: WebMappingRetrieve<WebApi.defraimp_defraimp_importinspection_defraimp_samp_Select,WebApi.defraimp_defraimp_importinspection_defraimp_samp_Expand,WebApi.defraimp_defraimp_importinspection_defraimp_samp_Filter,WebApi.defraimp_defraimp_importinspection_defraimp_samp_Fixed,WebApi.defraimp_defraimp_importinspection_defraimp_samp_Result,WebApi.defraimp_defraimp_importinspection_defraimp_samp_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_defraimp_importinspection_defraimp_sampset: WebMappingRelated<WebApi.defraimp_defraimp_importinspection_defraimp_samp_RelatedOne,WebApi.defraimp_defraimp_importinspection_defraimp_samp_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_defraimp_importinspection_defraimp_sampset: WebMappingCUDA<WebApi.defraimp_defraimp_importinspection_defraimp_samp_Create,WebApi.defraimp_defraimp_importinspection_defraimp_samp_Update,WebApi.defraimp_defraimp_importinspection_defraimp_samp_Select>;
}
