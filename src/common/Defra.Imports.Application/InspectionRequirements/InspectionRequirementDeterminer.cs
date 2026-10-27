namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse;
    using Defra.Imports.Dataverse.Extensions;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.Logging;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    /// <inheritdoc/>
    public sealed class InspectionRequirementDeterminer : IInspectionRequirementDeterminer
    {
        private readonly InspectionQueries queries;
        private readonly PlaceOfOriginCounters placeOfOriginCounters;
        private readonly RiskCounterManagerFactory counterManagerFactory;
        private readonly InspectionStrategyFactory strategyFactory;
        private readonly IOrganizationService service;
        private readonly ILogWriter logger;

        /// <summary>Initializes a new instance of the <see cref="InspectionRequirementDeterminer"/> class.</summary>
        /// <param name="queries">The queries used to read risk levels and the gold/bronze rules.</param>
        /// <param name="placeOfOriginCounters">The place of origin counters.</param>
        /// <param name="counterManagerFactory">The factory that creates counter managers.</param>
        /// <param name="strategyFactory">The factory that chooses the inspection strategy.</param>
        /// <param name="service">The current invocation's organisation service.</param>
        /// <param name="logger">The log writer.</param>
        public InspectionRequirementDeterminer(
            InspectionQueries queries,
            PlaceOfOriginCounters placeOfOriginCounters,
            RiskCounterManagerFactory counterManagerFactory,
            InspectionStrategyFactory strategyFactory,
            IOrganizationService service,
            ILogWriter logger)
        {
            this.queries = queries ?? throw new ArgumentNullException(nameof(queries));
            this.placeOfOriginCounters = placeOfOriginCounters ?? throw new ArgumentNullException(nameof(placeOfOriginCounters));
            this.counterManagerFactory = counterManagerFactory ?? throw new ArgumentNullException(nameof(counterManagerFactory));
            this.strategyFactory = strategyFactory ?? throw new ArgumentNullException(nameof(strategyFactory));
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public void DetermineForImportApplication(Guid importApplicationId)
        {
            var columns = new ColumnSet(
                defraimp_importapplication.Fields.defraimp_importrisklevelid,
                defraimp_importapplication.Fields.defraimp_PreviousImportRiskLevelId,
                defraimp_importapplication.Fields.defraimp_InspectionRequired,
                defraimp_importapplication.Fields.defraimp_PlaceofOriginid,
                defraimp_importapplication.Fields.defraimp_CommodityTypeId,
                defraimp_importapplication.Fields.defraimp_CountryofOriginId,
                defraimp_importapplication.Fields.defraimp_PrimaryITAHCId);

            var importApplication = this.service
                .Retrieve(defraimp_importapplication.EntityLogicalName, importApplicationId, columns)
                .ToEntity<defraimp_importapplication>();

            // TODO: the columns do not include the import application type, so the strategies treat it as having no certificate and do nothing for P1, P2, P3 and TB. This should be investigated, but is preserved by the refactoring.
            this.Determine(null, importApplication.ToInspectionContext());
        }

        /// <inheritdoc/>
        public void Determine(InspectionImportApplicationContext before, InspectionImportApplicationContext after)
        {
            var run = new Run
            {
                Before = before != null ? new InspectionRecord(before) : null,
                After = after != null ? new InspectionRecord(after) : null,
            };

            var currentRiskLevel = run.After != null && run.After.Context.ImportRiskLevel != null
                ? run.After.Context.ImportRiskLevel.Name
                : string.Empty;

            // The risk level is read by identifier, because the name of a lookup on a pre-image is not reliable.
            var previousRiskLevel = run.Before != null && run.Before.Context.ImportRiskLevel != null
                ? this.queries.GetImportRiskLevelName(run.Before.Context.ImportRiskLevel.Id)
                : string.Empty;

            run.PreviousCounters = run.Before != null ? this.counterManagerFactory.Create(run.Before) : null;

            if (run.Before == null && run.After != null)
            {
                this.DealWithDeterminingInspection(run);
            }
            else if (run.Before != null && run.After != null)
            {
                this.DetermineForUpdate(run, previousRiskLevel, currentRiskLevel);
            }
            else if (run.Before != null)
            {
                // TODO: this fails if the import application had no risk level, because there is then no counter manager. This should be investigated, but is preserved by the refactoring.
                run.PreviousCounters.DecrementNumber(run.Before, defraimp_counterhistory_defraimp_reason.RecordDeactivated);
            }
        }

        private void DetermineForUpdate(Run run, string previousRiskLevel, string currentRiskLevel)
        {
            var before = run.Before.Context;
            var after = run.After.Context;

            if (before.StateCode == defraimp_importapplication_statecode.Active && after.StateCode == defraimp_importapplication_statecode.Active)
            {
                // Nothing is done if the record has been overridden for a manual post import check.
                if (after.ManualPostImportCheckDecision == defraimp_importapplication_defraimp_manualpostimportcheckdecision.UseSystemDecision)
                {
                    this.ManageRiskLevelChange(run, previousRiskLevel, currentRiskLevel);

                    if (before.ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC
                        || before.ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHCLandbridge)
                    {
                        this.ManageItahcRemoval(run, previousRiskLevel);
                    }
                    else if (before.ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ImportNotification)
                    {
                        this.ManageImporterNotificationRemoval(run, previousRiskLevel);
                    }

                    this.ManagePlaceOfOriginChange(run, previousRiskLevel);
                    this.DealWithDeterminingInspection(run);
                }
            }

            // The status is used with the state, so that the logic does not run when an application is completed.
            else if (before.StateCode == defraimp_importapplication_statecode.Active && after.StatusCode == defraimp_importapplication_statuscode.Cancelled)
            {
                this.ManageRecordDeactivation(run);
            }
            else if (before.StateCode == defraimp_importapplication_statecode.Active && after.StatusCode == defraimp_importapplication_statuscode.NoITAHCReceived)
            {
                this.ManageRecordDeactivation(run);
            }
            else if (before.StatusCode == defraimp_importapplication_statuscode.Cancelled && after.StateCode == defraimp_importapplication_statecode.Active)
            {
                this.DealWithDeterminingInspection(run);
            }
            else if (before.StatusCode == defraimp_importapplication_statuscode.NoITAHCReceived && after.StateCode == defraimp_importapplication_statecode.Active)
            {
                this.DealWithDeterminingInspection(run);
            }
        }

        private void ManageRiskLevelChange(Run run, string previousRiskLevel, string currentRiskLevel)
        {
            if (previousRiskLevel != currentRiskLevel)
            {
                if (!string.IsNullOrEmpty(previousRiskLevel))
                {
                    this.RemoveFromPreviousCount(run, defraimp_counterhistory_defraimp_reason.RiskLevelChangedRemoved);
                }
                else
                {
                    this.logger.Log(Severity.Info, InspectionConstants.LogTag, "Null string");
                }
            }
        }

        private void ManageItahcRemoval(Run run, string previousRiskLevel)
        {
            if (run.Before.Context.PrimaryItahc != null && run.After.Context.PrimaryItahc == null)
            {
                if (!string.IsNullOrEmpty(previousRiskLevel))
                {
                    this.RemoveFromPreviousCount(run, defraimp_counterhistory_defraimp_reason.ITAHCRemoved);
                }
            }
        }

        private void ManageImporterNotificationRemoval(Run run, string previousRiskLevel)
        {
            if (run.Before.Context.PrimaryImporterNotification != null && run.After.Context.PrimaryImporterNotification == null)
            {
                if (!string.IsNullOrEmpty(previousRiskLevel))
                {
                    this.RemoveFromPreviousCount(run, defraimp_counterhistory_defraimp_reason.ImporterNotificationRemoved);
                }
            }
        }

        private void ManagePlaceOfOriginChange(Run run, string previousRiskLevel)
        {
            // Only P1 import applications are subject to the gold/bronze rule.
            if (!string.IsNullOrEmpty(previousRiskLevel) && previousRiskLevel.ToLowerInvariant() == InspectionConstants.P1RiskLevelName)
            {
                if (this.queries.IsCoveredByGoldBronze(run.Before.Context) && run.Before.Context.PlaceOfOrigin != null)
                {
                    // TODO: this compares the references, not the places of origin, so it is true unless both are the same object. This should be investigated, but is preserved by the refactoring.
                    if (run.Before.Context.PlaceOfOrigin != run.After.Context.PlaceOfOrigin)
                    {
                        this.RemoveFromPreviousPlaceOfOriginCount(run, defraimp_counterhistory_defraimp_reason.GBPlaceOfOriginRemoved);
                    }
                }
            }
        }

        private void ManageRecordDeactivation(Run run)
        {
            // TODO: this fails if the import application had no risk level, because there is then no counter manager. This should be investigated, but is preserved by the refactoring.
            run.PreviousCounters.DecrementNumber(run.Before, defraimp_counterhistory_defraimp_reason.RecordDeactivated);
        }

        // Takes the import application out of the counts that it was in before the change.
        private void RemoveFromPreviousCount(Run run, defraimp_counterhistory_defraimp_reason reason)
        {
            if (this.queries.IsCoveredByGoldBronze(run.Before.Context))
            {
                if (run.Before.Context.PlaceOfOrigin != null)
                {
                    this.RemoveFromPreviousPlaceOfOriginCount(run, reason);
                }
            }
            else
            {
                run.PreviousCounters.DecrementNumber(run.Before, reason);

                // The local copy of the after values reflects the change to the count state.
                run.After.ImportRecordCounted = false;
            }
        }

        private void RemoveFromPreviousPlaceOfOriginCount(Run run, defraimp_counterhistory_defraimp_reason reason)
        {
            var placeOfOrigin = this.placeOfOriginCounters.Find(run.Before.Context.PlaceOfOrigin.Id);

            // TODO: this fails if the place of origin is not found. This should be investigated, but is preserved by the refactoring.
            if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Gold)
            {
                run.PreviousCounters.DecrementNumber(run.Before, reason);
                run.After.ImportRecordCounted = false;
            }
            else if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Bronze)
            {
                run.PreviousCounters.DecrementGlobalCounter(run.Before);
                run.After.ImportRecordCounted = false;
            }
        }

        private void DealWithDeterminingInspection(Run run)
        {
            // The counter manager is created here, so that it captures any earlier change made by the previous one.
            var counters = this.counterManagerFactory.Create(run.After);

            var riskLevel = run.After.Context.ImportRiskLevel != null
                ? run.After.Context.ImportRiskLevel.Name
                : string.Empty;

            this.strategyFactory.GetStrategy(riskLevel).Execute(run.After, counters);
        }

        private sealed class Run
        {
            public InspectionRecord Before { get; set; }

            public InspectionRecord After { get; set; }

            public RiskCounterManager PreviousCounters { get; set; }
        }
    }
}
