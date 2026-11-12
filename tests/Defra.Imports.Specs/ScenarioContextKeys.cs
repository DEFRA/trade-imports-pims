namespace Defra.Imports.Specs
{
    /// <summary>
    /// Constants for an Account record.
    /// </summary>
    public class ScenarioContextKeys
    {
        /// <summary>
        /// The name of the subgrid that a new record is being added to.
        /// </summary>
        public const string AddNewToSubgridName = nameof(AddNewToSubgridName);

        /// <summary>
        /// The total row count of the subgrid that a new record is being added to.
        /// </summary>
        public const string AddNewToSubgridTotalRowCount = nameof(AddNewToSubgridTotalRowCount);

        /// <summary>
        /// A key for a nested subgrid control.
        /// </summary>
        public const string NestedSubgrid = nameof(NestedSubgrid);

        /// <summary>
        /// A key for the type of nested subgrid control.
        /// </summary>
        public const string NestedSubgridType = nameof(NestedSubgridType);

        /// <summary>
        /// A key for the entity record modal dialog.
        /// </summary>
        public const string EntityRecordModal = nameof(EntityRecordModal);

        /// <summary>
        /// A key for the selected row in a grid.
        /// </summary>
        public const string SelectedRow = nameof(SelectedRow);

        /// <summary>
        /// A key for the active business process flow.
        /// </summary>
        public const string ActiveBusinessProcessFlow = nameof(ActiveBusinessProcessFlow);

        /// <summary>
        /// A key for the ID of the Importer Notification most recently opened by a step.
        /// </summary>
        public const string ImporterNotificationId = nameof(ImporterNotificationId);

        /// <summary>
        /// A key for the Submission Date of the Importer Notification most recently opened by a step.
        /// </summary>
        public const string ImporterNotificationSubmissionDate = nameof(ImporterNotificationSubmissionDate);

        /// <summary>
        /// A key for the Import Record prepared with searchable field values.
        /// </summary>
        public const string SearchableImportRecord = nameof(SearchableImportRecord);

        /// <summary>
        /// A key for the search fields that did not return the prepared Import Record.
        /// </summary>
        public const string FieldsWithoutSearchResults = nameof(FieldsWithoutSearchResults);

        /// <summary>
        /// A key for the name of the ITAHC created for the scenario.
        /// </summary>
        public const string ItahcName = nameof(ItahcName);
    }
}