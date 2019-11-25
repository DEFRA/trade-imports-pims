declare namespace WebApi {
  interface defraimp_defraimp_importapplication_defraimp_ita_Base extends WebEntity {
    defraimp_defraimp_importapplication_defraimp_itaid?: string | null;
    defraimp_importapplicationid?: string | null;
    defraimp_itahcid?: string | null;
    versionnumber?: number | null;
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Relationships {
    defraimp_defraimp_importapplication_defraimp_itahc?: defraimp_itahc_Result[] | null;
  }
  interface defraimp_defraimp_importapplication_defraimp_ita extends defraimp_defraimp_importapplication_defraimp_ita_Base, defraimp_defraimp_importapplication_defraimp_ita_Relationships {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Create extends defraimp_defraimp_importapplication_defraimp_ita {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Update extends defraimp_defraimp_importapplication_defraimp_ita {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Select {
    defraimp_defraimp_importapplication_defraimp_itaid: WebAttribute<defraimp_defraimp_importapplication_defraimp_ita_Select, { defraimp_defraimp_importapplication_defraimp_itaid: string | null }, {  }>;
    defraimp_importapplicationid: WebAttribute<defraimp_defraimp_importapplication_defraimp_ita_Select, { defraimp_importapplicationid: string | null }, {  }>;
    defraimp_itahcid: WebAttribute<defraimp_defraimp_importapplication_defraimp_ita_Select, { defraimp_itahcid: string | null }, {  }>;
    versionnumber: WebAttribute<defraimp_defraimp_importapplication_defraimp_ita_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Filter {
    defraimp_defraimp_importapplication_defraimp_itaid: XQW.Guid;
    defraimp_importapplicationid: XQW.Guid;
    defraimp_itahcid: XQW.Guid;
    versionnumber: number;
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Expand {
    defraimp_defraimp_importapplication_defraimp_itahc: WebExpand<defraimp_defraimp_importapplication_defraimp_ita_Expand, defraimp_itahc_Select, defraimp_itahc_Filter, { defraimp_defraimp_importapplication_defraimp_itahc: defraimp_itahc_Result[] }>;
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_FormattedResult {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Result extends defraimp_defraimp_importapplication_defraimp_ita_Base, defraimp_defraimp_importapplication_defraimp_ita_Relationships {
    "@odata.etag": string;
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_RelatedOne {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_RelatedMany {
    defraimp_defraimp_importapplication_defraimp_itahc: WebMappingRetrieve<WebApi.defraimp_itahc_Select,WebApi.defraimp_itahc_Expand,WebApi.defraimp_itahc_Filter,WebApi.defraimp_itahc_Fixed,WebApi.defraimp_itahc_Result,WebApi.defraimp_itahc_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_defraimp_importapplication_defraimp_ita: WebMappingRetrieve<WebApi.defraimp_defraimp_importapplication_defraimp_ita_Select,WebApi.defraimp_defraimp_importapplication_defraimp_ita_Expand,WebApi.defraimp_defraimp_importapplication_defraimp_ita_Filter,WebApi.defraimp_defraimp_importapplication_defraimp_ita_Fixed,WebApi.defraimp_defraimp_importapplication_defraimp_ita_Result,WebApi.defraimp_defraimp_importapplication_defraimp_ita_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_defraimp_importapplication_defraimp_ita: WebMappingRelated<WebApi.defraimp_defraimp_importapplication_defraimp_ita_RelatedOne,WebApi.defraimp_defraimp_importapplication_defraimp_ita_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_defraimp_importapplication_defraimp_ita: WebMappingCUDA<WebApi.defraimp_defraimp_importapplication_defraimp_ita_Create,WebApi.defraimp_defraimp_importapplication_defraimp_ita_Update,WebApi.defraimp_defraimp_importapplication_defraimp_ita_Select>;
}
