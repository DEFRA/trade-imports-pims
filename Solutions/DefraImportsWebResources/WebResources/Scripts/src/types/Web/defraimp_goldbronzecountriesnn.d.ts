declare namespace WebApi {
  interface defraimp_goldbronzecountriesnn_Base extends WebEntity {
    defra_countryid?: string | null;
    defraimp_goldbronzecommodityid?: string | null;
    defraimp_goldbronzecountriesnnid?: string | null;
    versionnumber?: number | null;
  }
  interface defraimp_goldbronzecountriesnn_Relationships {
    defraimp_goldbronzecountriesnn?: defra_country_Result[] | null;
  }
  interface defraimp_goldbronzecountriesnn extends defraimp_goldbronzecountriesnn_Base, defraimp_goldbronzecountriesnn_Relationships {
  }
  interface defraimp_goldbronzecountriesnn_Create extends defraimp_goldbronzecountriesnn {
  }
  interface defraimp_goldbronzecountriesnn_Update extends defraimp_goldbronzecountriesnn {
  }
  interface defraimp_goldbronzecountriesnn_Select {
    defra_countryid: WebAttribute<defraimp_goldbronzecountriesnn_Select, { defra_countryid: string | null }, {  }>;
    defraimp_goldbronzecommodityid: WebAttribute<defraimp_goldbronzecountriesnn_Select, { defraimp_goldbronzecommodityid: string | null }, {  }>;
    defraimp_goldbronzecountriesnnid: WebAttribute<defraimp_goldbronzecountriesnn_Select, { defraimp_goldbronzecountriesnnid: string | null }, {  }>;
    versionnumber: WebAttribute<defraimp_goldbronzecountriesnn_Select, { versionnumber: number | null }, {  }>;
  }
  interface defraimp_goldbronzecountriesnn_Filter {
    defra_countryid: XQW.Guid;
    defraimp_goldbronzecommodityid: XQW.Guid;
    defraimp_goldbronzecountriesnnid: XQW.Guid;
    versionnumber: number;
  }
  interface defraimp_goldbronzecountriesnn_Expand {
    defraimp_goldbronzecountriesnn: WebExpand<defraimp_goldbronzecountriesnn_Expand, defra_country_Select, defra_country_Filter, { defraimp_goldbronzecountriesnn: defra_country_Result[] }>;
  }
  interface defraimp_goldbronzecountriesnn_FormattedResult {
  }
  interface defraimp_goldbronzecountriesnn_Result extends defraimp_goldbronzecountriesnn_Base, defraimp_goldbronzecountriesnn_Relationships {
    "@odata.etag": string;
  }
  interface defraimp_goldbronzecountriesnn_RelatedOne {
  }
  interface defraimp_goldbronzecountriesnn_RelatedMany {
    defraimp_goldbronzecountriesnn: WebMappingRetrieve<WebApi.defra_country_Select,WebApi.defra_country_Expand,WebApi.defra_country_Filter,WebApi.defra_country_Fixed,WebApi.defra_country_Result,WebApi.defra_country_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  defraimp_goldbronzecountriesnnset: WebMappingRetrieve<WebApi.defraimp_goldbronzecountriesnn_Select,WebApi.defraimp_goldbronzecountriesnn_Expand,WebApi.defraimp_goldbronzecountriesnn_Filter,WebApi.defraimp_goldbronzecountriesnn_Fixed,WebApi.defraimp_goldbronzecountriesnn_Result,WebApi.defraimp_goldbronzecountriesnn_FormattedResult>;
}
interface WebEntitiesRelated {
  defraimp_goldbronzecountriesnnset: WebMappingRelated<WebApi.defraimp_goldbronzecountriesnn_RelatedOne,WebApi.defraimp_goldbronzecountriesnn_RelatedMany>;
}
interface WebEntitiesCUDA {
  defraimp_goldbronzecountriesnnset: WebMappingCUDA<WebApi.defraimp_goldbronzecountriesnn_Create,WebApi.defraimp_goldbronzecountriesnn_Update,WebApi.defraimp_goldbronzecountriesnn_Select>;
}
