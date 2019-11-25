interface WebMappingRetrieve<ISelect, IExpand, IFilter, IFixed, Result, FormattedResult> {
}
interface WebMappingCUDA<ICreate, IUpdate, ISelect> {
}
interface WebMappingRelated<ISingle, IMultiple> {
}
declare namespace WebApi {
  interface WebEntity {
  }
  interface WebEntity_Fixed {
    "@odata.etag": string;
  }
  interface ActivityParty_Base extends WebEntity {
  }
  interface ActivityParty_Fixed extends WebEntity_Fixed {
    activitypartyid: string;
  }
  interface ActivityParty extends ActivityParty_Base, ActivityParty_Relationships {
  }
  interface ActivityParty_Relationships {
  }
  interface ActivityParty_Result extends ActivityParty_Base, ActivityParty_Relationships {
  }
  interface ActivityParty_FormattedResult {
  }
  interface ActivityParty_Select {
  }
  interface ActivityParty_Expand {
  }
  interface ActivityParty_Filter {
  }
  interface ActivityParty_Create extends ActivityParty {
  }
  interface ActivityParty_Update extends ActivityParty {
  }
  interface defra_country_Base extends WebEntity {
  }
  interface defra_country_Fixed extends WebEntity_Fixed {
    defra_countryid: string;
  }
  interface defra_country extends defra_country_Base, defra_country_Relationships {
  }
  interface defra_country_Relationships {
  }
  interface defra_country_Result extends defra_country_Base, defra_country_Relationships {
  }
  interface defra_country_FormattedResult {
  }
  interface defra_country_Select {
  }
  interface defra_country_Expand {
  }
  interface defra_country_Filter {
  }
  interface defra_country_Create extends defra_country {
  }
  interface defra_country_Update extends defra_country {
  }
  interface defraexp_commoditytype_Base extends WebEntity {
  }
  interface defraexp_commoditytype_Fixed extends WebEntity_Fixed {
    defraexp_commoditytypeid: string;
  }
  interface defraexp_commoditytype extends defraexp_commoditytype_Base, defraexp_commoditytype_Relationships {
  }
  interface defraexp_commoditytype_Relationships {
  }
  interface defraexp_commoditytype_Result extends defraexp_commoditytype_Base, defraexp_commoditytype_Relationships {
  }
  interface defraexp_commoditytype_FormattedResult {
  }
  interface defraexp_commoditytype_Select {
  }
  interface defraexp_commoditytype_Expand {
  }
  interface defraexp_commoditytype_Filter {
  }
  interface defraexp_commoditytype_Create extends defraexp_commoditytype {
  }
  interface defraexp_commoditytype_Update extends defraexp_commoditytype {
  }
  interface defraimp_apharegion_Base extends WebEntity {
  }
  interface defraimp_apharegion_Fixed extends WebEntity_Fixed {
    defraimp_apharegionid: string;
  }
  interface defraimp_apharegion extends defraimp_apharegion_Base, defraimp_apharegion_Relationships {
  }
  interface defraimp_apharegion_Relationships {
  }
  interface defraimp_apharegion_Result extends defraimp_apharegion_Base, defraimp_apharegion_Relationships {
  }
  interface defraimp_apharegion_FormattedResult {
  }
  interface defraimp_apharegion_Select {
  }
  interface defraimp_apharegion_Expand {
  }
  interface defraimp_apharegion_Filter {
  }
  interface defraimp_apharegion_Create extends defraimp_apharegion {
  }
  interface defraimp_apharegion_Update extends defraimp_apharegion {
  }
  interface defraimp_autonumber_Base extends WebEntity {
  }
  interface defraimp_autonumber_Fixed extends WebEntity_Fixed {
    defraimp_autonumberid: string;
  }
  interface defraimp_autonumber extends defraimp_autonumber_Base, defraimp_autonumber_Relationships {
  }
  interface defraimp_autonumber_Relationships {
  }
  interface defraimp_autonumber_Result extends defraimp_autonumber_Base, defraimp_autonumber_Relationships {
  }
  interface defraimp_autonumber_FormattedResult {
  }
  interface defraimp_autonumber_Select {
  }
  interface defraimp_autonumber_Expand {
  }
  interface defraimp_autonumber_Filter {
  }
  interface defraimp_autonumber_Create extends defraimp_autonumber {
  }
  interface defraimp_autonumber_Update extends defraimp_autonumber {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Base extends WebEntity {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Fixed extends WebEntity_Fixed {
    defraimp_defraimp_importapplication_defraimp_impid: string;
  }
  interface defraimp_defraimp_importapplication_defraimp_imp extends defraimp_defraimp_importapplication_defraimp_imp_Base, defraimp_defraimp_importapplication_defraimp_imp_Relationships {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Relationships {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Result extends defraimp_defraimp_importapplication_defraimp_imp_Base, defraimp_defraimp_importapplication_defraimp_imp_Relationships {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_FormattedResult {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Select {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Expand {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Filter {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Create extends defraimp_defraimp_importapplication_defraimp_imp {
  }
  interface defraimp_defraimp_importapplication_defraimp_imp_Update extends defraimp_defraimp_importapplication_defraimp_imp {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Base extends WebEntity {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Fixed extends WebEntity_Fixed {
    defraimp_defraimp_importapplication_defraimp_itaid: string;
  }
  interface defraimp_defraimp_importapplication_defraimp_ita extends defraimp_defraimp_importapplication_defraimp_ita_Base, defraimp_defraimp_importapplication_defraimp_ita_Relationships {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Relationships {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Result extends defraimp_defraimp_importapplication_defraimp_ita_Base, defraimp_defraimp_importapplication_defraimp_ita_Relationships {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_FormattedResult {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Select {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Expand {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Filter {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Create extends defraimp_defraimp_importapplication_defraimp_ita {
  }
  interface defraimp_defraimp_importapplication_defraimp_ita_Update extends defraimp_defraimp_importapplication_defraimp_ita {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Base extends WebEntity {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Fixed extends WebEntity_Fixed {
    defraimp_defraimp_importinspection_defraimp_sampid: string;
  }
  interface defraimp_defraimp_importinspection_defraimp_samp extends defraimp_defraimp_importinspection_defraimp_samp_Base, defraimp_defraimp_importinspection_defraimp_samp_Relationships {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Relationships {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Result extends defraimp_defraimp_importinspection_defraimp_samp_Base, defraimp_defraimp_importinspection_defraimp_samp_Relationships {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_FormattedResult {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Select {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Expand {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Filter {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Create extends defraimp_defraimp_importinspection_defraimp_samp {
  }
  interface defraimp_defraimp_importinspection_defraimp_samp_Update extends defraimp_defraimp_importinspection_defraimp_samp {
  }
  interface defraimp_docom_Base extends WebEntity {
  }
  interface defraimp_docom_Fixed extends WebEntity_Fixed {
    defraimp_docomid: string;
  }
  interface defraimp_docom extends defraimp_docom_Base, defraimp_docom_Relationships {
  }
  interface defraimp_docom_Relationships {
  }
  interface defraimp_docom_Result extends defraimp_docom_Base, defraimp_docom_Relationships {
  }
  interface defraimp_docom_FormattedResult {
  }
  interface defraimp_docom_Select {
  }
  interface defraimp_docom_Expand {
  }
  interface defraimp_docom_Filter {
  }
  interface defraimp_docom_Create extends defraimp_docom {
  }
  interface defraimp_docom_Update extends defraimp_docom {
  }
  interface defraimp_goldbronzecommodity_Base extends WebEntity {
  }
  interface defraimp_goldbronzecommodity_Fixed extends WebEntity_Fixed {
    defraimp_goldbronzecommodityid: string;
  }
  interface defraimp_goldbronzecommodity extends defraimp_goldbronzecommodity_Base, defraimp_goldbronzecommodity_Relationships {
  }
  interface defraimp_goldbronzecommodity_Relationships {
  }
  interface defraimp_goldbronzecommodity_Result extends defraimp_goldbronzecommodity_Base, defraimp_goldbronzecommodity_Relationships {
  }
  interface defraimp_goldbronzecommodity_FormattedResult {
  }
  interface defraimp_goldbronzecommodity_Select {
  }
  interface defraimp_goldbronzecommodity_Expand {
  }
  interface defraimp_goldbronzecommodity_Filter {
  }
  interface defraimp_goldbronzecommodity_Create extends defraimp_goldbronzecommodity {
  }
  interface defraimp_goldbronzecommodity_Update extends defraimp_goldbronzecommodity {
  }
  interface defraimp_goldbronzecountriesnn_Base extends WebEntity {
  }
  interface defraimp_goldbronzecountriesnn_Fixed extends WebEntity_Fixed {
    defraimp_goldbronzecountriesnnid: string;
  }
  interface defraimp_goldbronzecountriesnn extends defraimp_goldbronzecountriesnn_Base, defraimp_goldbronzecountriesnn_Relationships {
  }
  interface defraimp_goldbronzecountriesnn_Relationships {
  }
  interface defraimp_goldbronzecountriesnn_Result extends defraimp_goldbronzecountriesnn_Base, defraimp_goldbronzecountriesnn_Relationships {
  }
  interface defraimp_goldbronzecountriesnn_FormattedResult {
  }
  interface defraimp_goldbronzecountriesnn_Select {
  }
  interface defraimp_goldbronzecountriesnn_Expand {
  }
  interface defraimp_goldbronzecountriesnn_Filter {
  }
  interface defraimp_goldbronzecountriesnn_Create extends defraimp_goldbronzecountriesnn {
  }
  interface defraimp_goldbronzecountriesnn_Update extends defraimp_goldbronzecountriesnn {
  }
  interface defraimp_importapplication_Base extends WebEntity {
  }
  interface defraimp_importapplication_Fixed extends WebEntity_Fixed {
    defraimp_importapplicationid: string;
  }
  interface defraimp_importapplication extends defraimp_importapplication_Base, defraimp_importapplication_Relationships {
  }
  interface defraimp_importapplication_Relationships {
  }
  interface defraimp_importapplication_Result extends defraimp_importapplication_Base, defraimp_importapplication_Relationships {
  }
  interface defraimp_importapplication_FormattedResult {
  }
  interface defraimp_importapplication_Select {
  }
  interface defraimp_importapplication_Expand {
  }
  interface defraimp_importapplication_Filter {
  }
  interface defraimp_importapplication_Create extends defraimp_importapplication {
  }
  interface defraimp_importapplication_Update extends defraimp_importapplication {
  }
  interface defraimp_importapplication_defra_country_Base extends WebEntity {
  }
  interface defraimp_importapplication_defra_country_Fixed extends WebEntity_Fixed {
    defraimp_importapplication_defra_countryid: string;
  }
  interface defraimp_importapplication_defra_country extends defraimp_importapplication_defra_country_Base, defraimp_importapplication_defra_country_Relationships {
  }
  interface defraimp_importapplication_defra_country_Relationships {
  }
  interface defraimp_importapplication_defra_country_Result extends defraimp_importapplication_defra_country_Base, defraimp_importapplication_defra_country_Relationships {
  }
  interface defraimp_importapplication_defra_country_FormattedResult {
  }
  interface defraimp_importapplication_defra_country_Select {
  }
  interface defraimp_importapplication_defra_country_Expand {
  }
  interface defraimp_importapplication_defra_country_Filter {
  }
  interface defraimp_importapplication_defra_country_Create extends defraimp_importapplication_defra_country {
  }
  interface defraimp_importapplication_defra_country_Update extends defraimp_importapplication_defra_country {
  }
  interface defraimp_importcountrycommodityrisklevel_Base extends WebEntity {
  }
  interface defraimp_importcountrycommodityrisklevel_Fixed extends WebEntity_Fixed {
    defraimp_importcountrycommodityrisklevelid: string;
  }
  interface defraimp_importcountrycommodityrisklevel extends defraimp_importcountrycommodityrisklevel_Base, defraimp_importcountrycommodityrisklevel_Relationships {
  }
  interface defraimp_importcountrycommodityrisklevel_Relationships {
  }
  interface defraimp_importcountrycommodityrisklevel_Result extends defraimp_importcountrycommodityrisklevel_Base, defraimp_importcountrycommodityrisklevel_Relationships {
  }
  interface defraimp_importcountrycommodityrisklevel_FormattedResult {
  }
  interface defraimp_importcountrycommodityrisklevel_Select {
  }
  interface defraimp_importcountrycommodityrisklevel_Expand {
  }
  interface defraimp_importcountrycommodityrisklevel_Filter {
  }
  interface defraimp_importcountrycommodityrisklevel_Create extends defraimp_importcountrycommodityrisklevel {
  }
  interface defraimp_importcountrycommodityrisklevel_Update extends defraimp_importcountrycommodityrisklevel {
  }
  interface defraimp_importinspection_Base extends WebEntity {
  }
  interface defraimp_importinspection_Fixed extends WebEntity_Fixed {
    defraimp_importinspectionid: string;
  }
  interface defraimp_importinspection extends defraimp_importinspection_Base, defraimp_importinspection_Relationships {
  }
  interface defraimp_importinspection_Relationships {
  }
  interface defraimp_importinspection_Result extends defraimp_importinspection_Base, defraimp_importinspection_Relationships {
  }
  interface defraimp_importinspection_FormattedResult {
  }
  interface defraimp_importinspection_Select {
  }
  interface defraimp_importinspection_Expand {
  }
  interface defraimp_importinspection_Filter {
  }
  interface defraimp_importinspection_Create extends defraimp_importinspection {
  }
  interface defraimp_importinspection_Update extends defraimp_importinspection {
  }
  interface defraimp_importnotification_Base extends WebEntity {
  }
  interface defraimp_importnotification_Fixed extends WebEntity_Fixed {
    defraimp_importnotificationid: string;
  }
  interface defraimp_importnotification extends defraimp_importnotification_Base, defraimp_importnotification_Relationships {
  }
  interface defraimp_importnotification_Relationships {
  }
  interface defraimp_importnotification_Result extends defraimp_importnotification_Base, defraimp_importnotification_Relationships {
  }
  interface defraimp_importnotification_FormattedResult {
  }
  interface defraimp_importnotification_Select {
  }
  interface defraimp_importnotification_Expand {
  }
  interface defraimp_importnotification_Filter {
  }
  interface defraimp_importnotification_Create extends defraimp_importnotification {
  }
  interface defraimp_importnotification_Update extends defraimp_importnotification {
  }
  interface defraimp_importquery_Base extends WebEntity {
  }
  interface defraimp_importquery_Fixed extends WebEntity_Fixed {
    activityid: string;
  }
  interface defraimp_importquery extends defraimp_importquery_Base, defraimp_importquery_Relationships {
  }
  interface defraimp_importquery_Relationships {
  }
  interface defraimp_importquery_Result extends defraimp_importquery_Base, defraimp_importquery_Relationships {
  }
  interface defraimp_importquery_FormattedResult {
  }
  interface defraimp_importquery_Select {
  }
  interface defraimp_importquery_Expand {
  }
  interface defraimp_importquery_Filter {
  }
  interface defraimp_importquery_Create extends defraimp_importquery {
  }
  interface defraimp_importquery_Update extends defraimp_importquery {
  }
  interface defraimp_importrisklevel_Base extends WebEntity {
  }
  interface defraimp_importrisklevel_Fixed extends WebEntity_Fixed {
    defraimp_importrisklevelid: string;
  }
  interface defraimp_importrisklevel extends defraimp_importrisklevel_Base, defraimp_importrisklevel_Relationships {
  }
  interface defraimp_importrisklevel_Relationships {
  }
  interface defraimp_importrisklevel_Result extends defraimp_importrisklevel_Base, defraimp_importrisklevel_Relationships {
  }
  interface defraimp_importrisklevel_FormattedResult {
  }
  interface defraimp_importrisklevel_Select {
  }
  interface defraimp_importrisklevel_Expand {
  }
  interface defraimp_importrisklevel_Filter {
  }
  interface defraimp_importrisklevel_Create extends defraimp_importrisklevel {
  }
  interface defraimp_importrisklevel_Update extends defraimp_importrisklevel {
  }
  interface defraimp_inspectioncoveragerule_Base extends WebEntity {
  }
  interface defraimp_inspectioncoveragerule_Fixed extends WebEntity_Fixed {
    defraimp_inspectioncoverageruleid: string;
  }
  interface defraimp_inspectioncoveragerule extends defraimp_inspectioncoveragerule_Base, defraimp_inspectioncoveragerule_Relationships {
  }
  interface defraimp_inspectioncoveragerule_Relationships {
  }
  interface defraimp_inspectioncoveragerule_Result extends defraimp_inspectioncoveragerule_Base, defraimp_inspectioncoveragerule_Relationships {
  }
  interface defraimp_inspectioncoveragerule_FormattedResult {
  }
  interface defraimp_inspectioncoveragerule_Select {
  }
  interface defraimp_inspectioncoveragerule_Expand {
  }
  interface defraimp_inspectioncoveragerule_Filter {
  }
  interface defraimp_inspectioncoveragerule_Create extends defraimp_inspectioncoveragerule {
  }
  interface defraimp_inspectioncoveragerule_Update extends defraimp_inspectioncoveragerule {
  }
  interface defraimp_itahc_Base extends WebEntity {
  }
  interface defraimp_itahc_Fixed extends WebEntity_Fixed {
    defraimp_itahcid: string;
  }
  interface defraimp_itahc extends defraimp_itahc_Base, defraimp_itahc_Relationships {
  }
  interface defraimp_itahc_Relationships {
  }
  interface defraimp_itahc_Result extends defraimp_itahc_Base, defraimp_itahc_Relationships {
  }
  interface defraimp_itahc_FormattedResult {
  }
  interface defraimp_itahc_Select {
  }
  interface defraimp_itahc_Expand {
  }
  interface defraimp_itahc_Filter {
  }
  interface defraimp_itahc_Create extends defraimp_itahc {
  }
  interface defraimp_itahc_Update extends defraimp_itahc {
  }
  interface defraimp_placeoforigin_Base extends WebEntity {
  }
  interface defraimp_placeoforigin_Fixed extends WebEntity_Fixed {
    defraimp_placeoforiginid: string;
  }
  interface defraimp_placeoforigin extends defraimp_placeoforigin_Base, defraimp_placeoforigin_Relationships {
  }
  interface defraimp_placeoforigin_Relationships {
  }
  interface defraimp_placeoforigin_Result extends defraimp_placeoforigin_Base, defraimp_placeoforigin_Relationships {
  }
  interface defraimp_placeoforigin_FormattedResult {
  }
  interface defraimp_placeoforigin_Select {
  }
  interface defraimp_placeoforigin_Expand {
  }
  interface defraimp_placeoforigin_Filter {
  }
  interface defraimp_placeoforigin_Create extends defraimp_placeoforigin {
  }
  interface defraimp_placeoforigin_Update extends defraimp_placeoforigin {
  }
  interface defraimp_sampletest_Base extends WebEntity {
  }
  interface defraimp_sampletest_Fixed extends WebEntity_Fixed {
    defraimp_sampletestid: string;
  }
  interface defraimp_sampletest extends defraimp_sampletest_Base, defraimp_sampletest_Relationships {
  }
  interface defraimp_sampletest_Relationships {
  }
  interface defraimp_sampletest_Result extends defraimp_sampletest_Base, defraimp_sampletest_Relationships {
  }
  interface defraimp_sampletest_FormattedResult {
  }
  interface defraimp_sampletest_Select {
  }
  interface defraimp_sampletest_Expand {
  }
  interface defraimp_sampletest_Filter {
  }
  interface defraimp_sampletest_Create extends defraimp_sampletest {
  }
  interface defraimp_sampletest_Update extends defraimp_sampletest {
  }
  interface ProcessSession_Base extends WebEntity {
  }
  interface ProcessSession_Fixed extends WebEntity_Fixed {
    processsessionid: string;
  }
  interface ProcessSession extends ProcessSession_Base, ProcessSession_Relationships {
  }
  interface ProcessSession_Relationships {
  }
  interface ProcessSession_Result extends ProcessSession_Base, ProcessSession_Relationships {
  }
  interface ProcessSession_FormattedResult {
  }
  interface ProcessSession_Select {
  }
  interface ProcessSession_Expand {
  }
  interface ProcessSession_Filter {
  }
  interface ProcessSession_Create extends ProcessSession {
  }
  interface ProcessSession_Update extends ProcessSession {
  }
  interface SyncError_Base extends WebEntity {
  }
  interface SyncError_Fixed extends WebEntity_Fixed {
    syncerrorid: string;
  }
  interface SyncError extends SyncError_Base, SyncError_Relationships {
  }
  interface SyncError_Relationships {
  }
  interface SyncError_Result extends SyncError_Base, SyncError_Relationships {
  }
  interface SyncError_FormattedResult {
  }
  interface SyncError_Select {
  }
  interface SyncError_Expand {
  }
  interface SyncError_Filter {
  }
  interface SyncError_Create extends SyncError {
  }
  interface SyncError_Update extends SyncError {
  }
  interface SystemUser_Base extends WebEntity {
  }
  interface SystemUser_Fixed extends WebEntity_Fixed {
    systemuserid: string;
  }
  interface SystemUser extends SystemUser_Base, SystemUser_Relationships {
  }
  interface SystemUser_Relationships {
  }
  interface SystemUser_Result extends SystemUser_Base, SystemUser_Relationships {
  }
  interface SystemUser_FormattedResult {
  }
  interface SystemUser_Select {
  }
  interface SystemUser_Expand {
  }
  interface SystemUser_Filter {
  }
  interface SystemUser_Create extends SystemUser {
  }
  interface SystemUser_Update extends SystemUser {
  }
  interface Team_Base extends WebEntity {
  }
  interface Team_Fixed extends WebEntity_Fixed {
    teamid: string;
  }
  interface Team extends Team_Base, Team_Relationships {
  }
  interface Team_Relationships {
  }
  interface Team_Result extends Team_Base, Team_Relationships {
  }
  interface Team_FormattedResult {
  }
  interface Team_Select {
  }
  interface Team_Expand {
  }
  interface Team_Filter {
  }
  interface Team_Create extends Team {
  }
  interface Team_Update extends Team {
  }
  interface TeamMembership_Base extends WebEntity {
  }
  interface TeamMembership_Fixed extends WebEntity_Fixed {
    teammembershipid: string;
  }
  interface TeamMembership extends TeamMembership_Base, TeamMembership_Relationships {
  }
  interface TeamMembership_Relationships {
  }
  interface TeamMembership_Result extends TeamMembership_Base, TeamMembership_Relationships {
  }
  interface TeamMembership_FormattedResult {
  }
  interface TeamMembership_Select {
  }
  interface TeamMembership_Expand {
  }
  interface TeamMembership_Filter {
  }
  interface TeamMembership_Create extends TeamMembership {
  }
  interface TeamMembership_Update extends TeamMembership {
  }
  interface Post_Base extends WebEntity {
  }
  interface Post_Fixed extends WebEntity_Fixed {
    postid: string;
  }
  interface Post extends Post_Base, Post_Relationships {
  }
  interface Post_Relationships {
  }
  interface Post_Result extends Post_Base, Post_Relationships {
  }
  interface Post_FormattedResult {
  }
  interface Post_Select {
  }
  interface Post_Expand {
  }
  interface Post_Filter {
  }
  interface Post_Create extends Post {
  }
  interface Post_Update extends Post {
  }
  interface Connection_Base extends WebEntity {
  }
  interface Connection_Fixed extends WebEntity_Fixed {
    connectionid: string;
  }
  interface Connection extends Connection_Base, Connection_Relationships {
  }
  interface Connection_Relationships {
  }
  interface Connection_Result extends Connection_Base, Connection_Relationships {
  }
  interface Connection_FormattedResult {
  }
  interface Connection_Select {
  }
  interface Connection_Expand {
  }
  interface Connection_Filter {
  }
  interface Connection_Create extends Connection {
  }
  interface Connection_Update extends Connection {
  }
  interface Workflow_Base extends WebEntity {
  }
  interface Workflow_Fixed extends WebEntity_Fixed {
    workflowid: string;
  }
  interface Workflow extends Workflow_Base, Workflow_Relationships {
  }
  interface Workflow_Relationships {
  }
  interface Workflow_Result extends Workflow_Base, Workflow_Relationships {
  }
  interface Workflow_FormattedResult {
  }
  interface Workflow_Select {
  }
  interface Workflow_Expand {
  }
  interface Workflow_Filter {
  }
  interface Workflow_Create extends Workflow {
  }
  interface Workflow_Update extends Workflow {
  }
  interface PostFollow_Base extends WebEntity {
  }
  interface PostFollow_Fixed extends WebEntity_Fixed {
    postfollowid: string;
  }
  interface PostFollow extends PostFollow_Base, PostFollow_Relationships {
  }
  interface PostFollow_Relationships {
  }
  interface PostFollow_Result extends PostFollow_Base, PostFollow_Relationships {
  }
  interface PostFollow_FormattedResult {
  }
  interface PostFollow_Select {
  }
  interface PostFollow_Expand {
  }
  interface PostFollow_Filter {
  }
  interface PostFollow_Create extends PostFollow {
  }
  interface PostFollow_Update extends PostFollow {
  }
}
