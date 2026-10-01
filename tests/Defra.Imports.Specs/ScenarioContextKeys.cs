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
    }
}