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
  interface defraimp_ImporterNotification_Base extends WebEntity {
  }
  interface defraimp_ImporterNotification_Fixed extends WebEntity_Fixed {
    defraimp_importernotificationid: string;
  }
  interface defraimp_ImporterNotification extends defraimp_ImporterNotification_Base, defraimp_ImporterNotification_Relationships {
  }
  interface defraimp_ImporterNotification_Relationships {
  }
  interface defraimp_ImporterNotification_Result extends defraimp_ImporterNotification_Base, defraimp_ImporterNotification_Relationships {
  }
  interface defraimp_ImporterNotification_FormattedResult {
  }
  interface defraimp_ImporterNotification_Select {
  }
  interface defraimp_ImporterNotification_Expand {
  }
  interface defraimp_ImporterNotification_Filter {
  }
  interface defraimp_ImporterNotification_Create extends defraimp_ImporterNotification {
  }
  interface defraimp_ImporterNotification_Update extends defraimp_ImporterNotification {
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
  interface defraimp_matchrecord_Base extends WebEntity {
  }
  interface defraimp_matchrecord_Fixed extends WebEntity_Fixed {
    defraimp_matchrecordid: string;
  }
  interface defraimp_matchrecord extends defraimp_matchrecord_Base, defraimp_matchrecord_Relationships {
  }
  interface defraimp_matchrecord_Relationships {
  }
  interface defraimp_matchrecord_Result extends defraimp_matchrecord_Base, defraimp_matchrecord_Relationships {
  }
  interface defraimp_matchrecord_FormattedResult {
  }
  interface defraimp_matchrecord_Select {
  }
  interface defraimp_matchrecord_Expand {
  }
  interface defraimp_matchrecord_Filter {
  }
  interface defraimp_matchrecord_Create extends defraimp_matchrecord {
  }
  interface defraimp_matchrecord_Update extends defraimp_matchrecord {
  }
  interface defraimp_PotentiallyRelatedImportRecords_Base extends WebEntity {
  }
  interface defraimp_PotentiallyRelatedImportRecords_Fixed extends WebEntity_Fixed {
    defraimp_potentiallyrelatedimportrecordsid: string;
  }
  interface defraimp_PotentiallyRelatedImportRecords extends defraimp_PotentiallyRelatedImportRecords_Base, defraimp_PotentiallyRelatedImportRecords_Relationships {
  }
  interface defraimp_PotentiallyRelatedImportRecords_Relationships {
  }
  interface defraimp_PotentiallyRelatedImportRecords_Result extends defraimp_PotentiallyRelatedImportRecords_Base, defraimp_PotentiallyRelatedImportRecords_Relationships {
  }
  interface defraimp_PotentiallyRelatedImportRecords_FormattedResult {
  }
  interface defraimp_PotentiallyRelatedImportRecords_Select {
  }
  interface defraimp_PotentiallyRelatedImportRecords_Expand {
  }
  interface defraimp_PotentiallyRelatedImportRecords_Filter {
  }
  interface defraimp_PotentiallyRelatedImportRecords_Create extends defraimp_PotentiallyRelatedImportRecords {
  }
  interface defraimp_PotentiallyRelatedImportRecords_Update extends defraimp_PotentiallyRelatedImportRecords {
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
}
