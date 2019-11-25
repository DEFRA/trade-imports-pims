declare namespace WebApi {
  interface defraimp_importapplication_defra_country_Base extends WebEntity {
    defra_countryid?: string | null;
    defraimp_importapplication_defra_countryid?: string | null;
    defraimp_importapplicationid?: string | null;
    versionnumber?: number | null;
  }
  interface defraimp_importapplication_defra_country_Relationships {
    defraimp_defraimp_importapplication_defra_country?: defra_country_Result[] | null;
  }
  interface defraimp_importapplication_defra_country extends defraimp_importapplication_defra_country_Base, defraimp_importapplication_defra_country_Relationships {
  }
  interface defraimp_importapplication_defra_country_Create extends defraimp_importapplication_defra_country {
  }
  interface defraimp_importapplication_defra_country_Update extends defraimp_importapplication_defra_country {
  }
  interface defraimp_importapplication_defra_country_Select {
    defra_countryid: WebAttribute<defraimp_importapplication_defra_country_Select, { defra_countryid: string | null }, {  }>;
    defraimp_importapplication_defra_countryid: WebAttribute<defraimp_importapplication_defra_country_Select, { defraimp_importapplication_defra_countryid: string | null }, {  }>;
    defraimp_importapplicationid: WebAttribute<defraimp_importapplication_defra_country_Select, { defraimp_importapplicationid: string | null }, {  }>;
    versionnumber: WebAttribute<defraimp_importapplication_defra_country_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_importapplication_defra_country_Filter {
    defra_countryid: XQW.Guid;
    defraimp_importapplication_defra_countryid: XQW.Guid;
    defraimp_importapplicationid: XQW.Guid;
    versionnumber: number;
  }
  interface defraimp_importapplication_defra_country_Expand {
    defraimp_defraimp_importapplication_defra_country: WebExpand<defraimp_importapplication_defra_country_Expand, defra_country_Select, defra_country_Filter, { defraimp_defraimp_importapplication_defra_country: defra_country_Result[] }>;
  }
  interface defraimp_importapplication_defra_country_FormattedResult {
  }
  interface defraimp_importapplication_defra_country_Result extends defraimp_importapplication_defra_country_Base, defraimp_importapplication_defra_country_Relationships {
    "@odata.etag": string;
  }
  interface defraimp_importapplication_defra_country_RelatedOne {
  }
  interface defraimp_importapplication_defra_country_RelatedMany {
    defraimp_defraimp_importapplication_defra_country: WebMappingRetrieve<WebApi.defra_country_Select,WebApi.defra_country_Expand,WebApi.defra_country_Filter,WebApi.defra_country_Fixed,WebApi.defra_country_Result,WebApi.defra_country_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_importapplication_defra_countryset: WebMappingRetrieve<WebApi.defraimp_importapplication_defra_country_Select,WebApi.defraimp_importapplication_defra_country_Expand,WebApi.defraimp_importapplication_defra_country_Filter,WebApi.defraimp_importapplication_defra_country_Fixed,WebApi.defraimp_importapplication_defra_country_Result,WebApi.defraimp_importapplication_defra_country_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_importapplication_defra_countryset: WebMappingRelated<WebApi.defraimp_importapplication_defra_country_RelatedOne,WebApi.defraimp_importapplication_defra_country_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_importapplication_defra_countryset: WebMappingCUDA<WebApi.defraimp_importapplication_defra_country_Create,WebApi.defraimp_importapplication_defra_country_Update,WebApi.defraimp_importapplication_defra_country_Select>;
}
