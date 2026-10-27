namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.ReferenceData;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;

    /// <summary>
    /// The inspection rules for one risk level. A strategy works out whether an import application is inspected
    /// and records the outcome on it, counting the application as it does so.
    /// </summary>
    public abstract class InspectionStrategy
    {
        /// <summary>Initializes a new instance of the <see cref="InspectionStrategy"/> class.</summary>
        /// <param name="queries">The queries used to read configuration and coverage rules.</param>
        /// <param name="updater">The updater that records the outcome.</param>
        protected InspectionStrategy(InspectionQueries queries, InspectionRequirementUpdater updater)
        {
            this.Queries = queries ?? throw new ArgumentNullException(nameof(queries));
            this.Updater = updater ?? throw new ArgumentNullException(nameof(updater));
        }

        /// <summary>Gets the queries used to read configuration and coverage rules.</summary>
        protected InspectionQueries Queries { get; }

        /// <summary>Gets the updater that records the outcome.</summary>
        protected InspectionRequirementUpdater Updater { get; }

        /// <summary>Determines whether an import application is inspected and records the outcome.</summary>
        /// <param name="record">The import application.</param>
        /// <param name="counters">The counter manager for the import application, which may be null.</param>
        public abstract void Execute(InspectionRecord record, RiskCounterManager counters);

        /// <summary>
        /// Determines whether the import application has the primary certificate that its type requires. An ITAHC
        /// needs a primary ITAHC and TRACES to be enabled.
        /// </summary>
        /// <param name="record">The import application.</param>
        /// <returns>True if the import application can be inspected.</returns>
        protected bool ValidImportApplicationTypeForInspection(InspectionRecord record)
        {
            var type = record.Context.ImportApplicationType;

            if ((type == defraimp_importapplication_defraimp_importapplicationtype.ITAHC || type == defraimp_importapplication_defraimp_importapplicationtype.ITAHCLandbridge)
                && record.Context.PrimaryItahc != null)
            {
                // TODO: parsing the configuration parameter fails if it is missing or not a Boolean. This should be investigated, but is preserved by the refactoring.
                return bool.Parse(this.Queries.GetConfigurationParameterValue(ConfigurationParameters.TracesEnabledKey));
            }

            if (type == defraimp_importapplication_defraimp_importapplicationtype.ImportNotification && record.Context.PrimaryImporterNotification != null)
            {
                return true;
            }

            return type == defraimp_importapplication_defraimp_importapplicationtype.CHEDA;
        }

        /// <summary>Records why the inspection cannot be determined when the primary certificate is missing.</summary>
        /// <param name="record">The import application.</param>
        protected void MissingCertificateError(InspectionRecord record)
        {
            var type = record.Context.ImportApplicationType;

            if (type == defraimp_importapplication_defraimp_importapplicationtype.ITAHC || type == defraimp_importapplication_defraimp_importapplicationtype.ITAHCLandbridge)
            {
                if (bool.Parse(this.Queries.GetConfigurationParameterValue(ConfigurationParameters.TracesEnabledKey)))
                {
                    this.Updater.PrimaryItahcMissing(record.Id);
                }
                else
                {
                    this.Updater.TracesDisabled(record.Id);
                }
            }
            else if (type == defraimp_importapplication_defraimp_importapplicationtype.ImportNotification)
            {
                this.Updater.PrimaryImporterNotificationMissing(record.Id);
            }
        }

        /// <summary>Finds a coverage rule by its key. The first match is used if there are several.</summary>
        /// <param name="key">The key of the coverage rule.</param>
        /// <returns>The coverage rule, or null if there is none.</returns>
        protected defraimp_inspectioncoveragerule GetCoverageRule(string key)
        {
            return this.Queries.FindCoverageRuleByKey(key);
        }
    }
}
