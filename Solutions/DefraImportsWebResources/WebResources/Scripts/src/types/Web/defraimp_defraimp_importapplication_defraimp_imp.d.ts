declare namespace WebApi {
  interface defraimp_defraimp_importapplication_defraimp_imp_Base extends WebEntity {
    defraimp_defraimp_importapplication_defraimp_impid?: string | null;
    defraimp_importapplicationid?: string | null;
    defraimp_importnotificationid?: string | null;
    versionnumber?: number | null;
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Relationships {
    defraimp_defraimp_importapplication_defraimp_impor?: defraimp_importnotification_Result[] | null;
  }
  interface defraimp_defraimp_importapplication_defraimp_imp extends defraimp_defraimp_importapplication_defraimp_imp_Base, defraimp_defraimp_importapplication_defraimp_imp_Relationships {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Create extends defraimp_defraimp_importapplication_defraimp_imp {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Update extends defraimp_defraimp_importapplication_defraimp_imp {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Select {
    defraimp_defraimp_importapplication_defraimp_impid: WebAttribute<defraimp_defraimp_importapplication_defraimp_imp_Select, { defraimp_defraimp_importapplication_defraimp_impid: string | null }, {  }>;
    defraimp_importapplicationid: WebAttribute<defraimp_defraimp_importapplication_defraimp_imp_Select, { defraimp_importapplicationid: string | null }, {  }>;
    defraimp_importnotificationid: WebAttribute<defraimp_defraimp_importapplication_defraimp_imp_Select, { defraimp_importnotificationid: string | null }, {  }>;
    versionnumber: WebAttribute<defraimp_defraimp_importapplication_defraimp_imp_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Filter {
    defraimp_defraimp_importapplication_defraimp_impid: XQW.Guid;
    defraimp_importapplicationid: XQW.Guid;
    defraimp_importnotificationid: XQW.Guid;
    versionnumber: number;
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Expand {
    defraimp_defraimp_importapplication_defraimp_impor: WebExpand<defraimp_defraimp_importapplication_defraimp_imp_Expand, defraimp_importnotification_Select, defraimp_importnotification_Filter, { defraimp_defraimp_importapplication_defraimp_impor: defraimp_importnotification_Result[] }>;
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_FormattedResult {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Result extends defraimp_defraimp_importapplication_defraimp_imp_Base, defraimp_defraimp_importapplication_defraimp_imp_Relationships {
    "@odata.etag": string;
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_RelatedOne {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_RelatedMany {
    defraimp_defraimp_importapplication_defraimp_impor: WebMappingRetrieve<WebApi.defraimp_importnotification_Select,WebApi.defraimp_importnotification_Expand,WebApi.defraimp_importnotification_Filter,WebApi.defraimp_importnotification_Fixed,WebApi.defraimp_importnotification_Result,WebApi.defraimp_importnotification_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_defraimp_importapplication_defraimp_imp: WebMappingRetrieve<WebApi.defraimp_defraimp_importapplication_defraimp_imp_Select,WebApi.defraimp_defraimp_importapplication_defraimp_imp_Expand,WebApi.defraimp_defraimp_importapplication_defraimp_imp_Filter,WebApi.defraimp_defraimp_importapplication_defraimp_imp_Fixed,WebApi.defraimp_defraimp_importapplication_defraimp_imp_Result,WebApi.defraimp_defraimp_importapplication_defraimp_imp_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_defraimp_importapplication_defraimp_imp: WebMappingRelated<WebApi.defraimp_defraimp_importapplication_defraimp_imp_RelatedOne,WebApi.defraimp_defraimp_importapplication_defraimp_imp_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_defraimp_importapplication_defraimp_imp: WebMappingCUDA<WebApi.defraimp_defraimp_importapplication_defraimp_imp_Create,WebApi.defraimp_defraimp_importapplication_defraimp_imp_Update,WebApi.defraimp_defraimp_importapplication_defraimp_imp_Select>;
}
