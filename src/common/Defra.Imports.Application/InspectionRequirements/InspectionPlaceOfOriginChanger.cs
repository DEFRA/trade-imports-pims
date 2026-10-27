namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse;
    using Defra.Imports.Dataverse.Extensions;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    /// <inheritdoc/>
    public sealed class InspectionPlaceOfOriginChanger : IInspectionPlaceOfOriginChanger
    {
        private readonly InspectionQueries queries;
        private readonly PlaceOfOriginCounters placeOfOriginCounters;
        private readonly InspectionStrategyFactory strategyFactory;
        private readonly IOrganizationService service;

        /// <summary>Initializes a new instance of the <see cref="InspectionPlaceOfOriginChanger"/> class.</summary>
        /// <param name="queries">The queries used to read the gold/bronze rules.</param>
        /// <param name="placeOfOriginCounters">The place of origin counters.</param>
        /// <param name="strategyFactory">The factory that chooses the inspection strategy.</param>
        /// <param name="service">The current invocation's organisation service.</param>
        public InspectionPlaceOfOriginChanger(
            InspectionQueries queries,
            PlaceOfOriginCounters placeOfOriginCounters,
            InspectionStrategyFactory strategyFactory,
            IOrganizationService service)
        {
            this.queries = queries ?? throw new ArgumentNullException(nameof(queries));
            this.placeOfOriginCounters = placeOfOriginCounters ?? throw new ArgumentNullException(nameof(placeOfOriginCounters));
            this.strategyFactory = strategyFactory ?? throw new ArgumentNullException(nameof(strategyFactory));
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <inheritdoc/>
        public void ChangePlaceOfOrigin(Guid importApplicationId)
        {
            var columns = new ColumnSet(
                defraimp_importapplication.Fields.defraimp_importrisklevelid,
                defraimp_importapplication.Fields.defraimp_PreviousImportRiskLevelId,
                defraimp_importapplication.Fields.defraimp_InspectionRequired,
                defraimp_importapplication.Fields.defraimp_PlaceofOriginid,
                defraimp_importapplication.Fields.defraimp_previousplaceoforiginid,
                defraimp_importapplication.Fields.defraimp_CommodityTypeId,
                defraimp_importapplication.Fields.defraimp_CountryofOriginId,
                defraimp_importapplication.Fields.defraimp_InspectionRequiredReason);

            var importApplication = this.service
                .Retrieve(defraimp_importapplication.EntityLogicalName, importApplicationId, columns)
                .ToEntity<defraimp_importapplication>();

            var record = new InspectionRecord(importApplication.ToInspectionContext());

            if (record.Context.ImportRiskLevel != null && record.Context.ImportRiskLevel.Name.ToLowerInvariant() == InspectionConstants.P1RiskLevelName)
            {
                if (this.queries.IsCoveredByGoldBronze(record.Context))
                {
                    var previousPlaceOfOrigin = record.Context.PreviousPlaceOfOrigin != null
                        ? this.placeOfOriginCounters.Find(record.Context.PreviousPlaceOfOrigin.Id)
                        : null;

                    if (previousPlaceOfOrigin != null
                        && previousPlaceOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Gold
                        && record.Context.InspectionRequiredReason == defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage)
                    {
                        // The quota is increased so that the next record for the place of origin is inspected.
                        this.placeOfOriginCounters.IncrementQuotaCounter(previousPlaceOfOrigin.Id);
                    }
                }

                // TODO: the columns do not include the import application type, so the P1 strategy treats the import application as having no certificate and does nothing. This should be investigated, but is preserved by the refactoring.
                this.strategyFactory.GetStrategy(InspectionConstants.P1RiskLevelName).Execute(record, null);
            }
        }
    }
}
