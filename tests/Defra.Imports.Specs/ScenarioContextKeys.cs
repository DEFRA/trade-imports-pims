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
        /// A key for a precondition Importer Notification record reference number.
        /// </summary>
        public const string CreatedImporterNotificationReferenceNumber = nameof(CreatedImporterNotificationReferenceNumber);

        /// <summary>
        /// A key for a precondition Importer Notification search token.
        /// </summary>
        public const string CreatedImporterNotificationSearchToken = nameof(CreatedImporterNotificationSearchToken);

        /// <summary>
        /// A key for a precondition Importer Notification record ID.
        /// </summary>
        public const string CreatedImporterNotificationId = nameof(CreatedImporterNotificationId);

        /// <summary>
        /// A key for the free text search values seeded against a precondition Importer Notification, keyed by the field name used in US-003 AC-3.
        /// </summary>
        public const string CreatedImporterNotificationSearchValues = nameof(CreatedImporterNotificationSearchValues);

        /// <summary>
        /// A key for a precondition APHA Region record name.
        /// </summary>
        public const string CreatedAphaRegionName = nameof(CreatedAphaRegionName);
    }
}