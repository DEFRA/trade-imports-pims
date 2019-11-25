declare namespace WebApi {
  interface TeamMembership_Base extends WebEntity {
    systemuserid?: string | null;
    teamid?: string | null;
    teammembershipid?: string | null;
    versionnumber?: number | null;
  }
  interface TeamMembership_Relationships {
    teammembership_association?: SystemUser_Result[] | null;
  }
  interface TeamMembership extends TeamMembership_Base, TeamMembership_Relationships {
  }
  interface TeamMembership_Create extends TeamMembership {
  }
  interface TeamMembership_Update extends TeamMembership {
  }
  interface TeamMembership_Select {
    systemuserid: WebAttribute<TeamMembership_Select, { systemuserid: string | null }, {  }>;
    teamid: WebAttribute<TeamMembership_Select, { teamid: string | null }, {  }>;
    teammembershipid: WebAttribute<TeamMembership_Select, { teammembershipid: string | null }, {  }>;
    versionnumber: WebAttribute<TeamMembership_Select, { versionnumber: number | null }, {  }>;
  }
  interface TeamMembership_Filter {
    systemuserid: XQW.Guid;
    teamid: XQW.Guid;
    teammembershipid: XQW.Guid;
    versionnumber: number;
  }
  interface TeamMembership_Expand {
    teammembership_association: WebExpand<TeamMembership_Expand, SystemUser_Select, SystemUser_Filter, { teammembership_association: SystemUser_Result[] }>;
  }
  interface TeamMembership_FormattedResult {
  }
  interface TeamMembership_Result extends TeamMembership_Base, TeamMembership_Relationships {
    "@odata.etag": string;
  }
  interface TeamMembership_RelatedOne {
  }
  interface TeamMembership_RelatedMany {
    teammembership_association: WebMappingRetrieve<WebApi.SystemUser_Select,WebApi.SystemUser_Expand,WebApi.SystemUser_Filter,WebApi.SystemUser_Fixed,WebApi.SystemUser_Result,WebApi.SystemUser_FormattedResult>;
  }
}
interface WebEntitiesRetrieve {
  teammemberships: WebMappingRetrieve<WebApi.TeamMembership_Select,WebApi.TeamMembership_Expand,WebApi.TeamMembership_Filter,WebApi.TeamMembership_Fixed,WebApi.TeamMembership_Result,WebApi.TeamMembership_FormattedResult>;
}
interface WebEntitiesRelated {
  teammemberships: WebMappingRelated<WebApi.TeamMembership_RelatedOne,WebApi.TeamMembership_RelatedMany>;
}
interface WebEntitiesCUDA {
  teammemberships: WebMappingCUDA<WebApi.TeamMembership_Create,WebApi.TeamMembership_Update,WebApi.TeamMembership_Select>;
}
