namespace Defra.Imports.Scenarios
{
    /// <summary>
    /// A user persona to use when running tests.
    /// </summary>
    public enum Persona
    {
        /// <summary>
        /// A caseworker.
        /// </summary>
        Caseworker,

        /// <summary>
        /// A business rules administrator.
        /// </summary>
        BusinessRulesAdmin,

        /// <summary>
        /// A team leader.
        /// </summary>
        TeamLeader,

        /// <summary>
        /// A caseworker with permissions to export to Excel.
        /// </summary>
        ExcelExporter,

        /// <summary>
        /// The application user that the EU Imports Notifications Logic App authenticates as (the "EU Imports Dynamics Application User") when it submits Importer Notifications received from IPAFFS.
        /// </summary>
        LogicApp,
    }
}