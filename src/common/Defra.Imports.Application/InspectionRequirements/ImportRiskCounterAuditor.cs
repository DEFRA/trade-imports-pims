namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Writes a counter history record for each change that a counter manager makes to a counter.
    /// </summary>
    public sealed class ImportRiskCounterAuditor
    {
        private readonly IOrganizationService service;

        /// <summary>Initializes a new instance of the <see cref="ImportRiskCounterAuditor"/> class.</summary>
        /// <param name="service">The current invocation's organisation service, used to create the records.</param>
        public ImportRiskCounterAuditor(IOrganizationService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>Creates a counter history record for a change to a counter.</summary>
        /// <param name="detail">The change.</param>
        public void Audit(CounterTransactionDetail detail)
        {
            if (detail == null)
            {
                throw new ArgumentNullException(nameof(detail));
            }

            var counterHistory = new defraimp_counterhistory
            {
                defraimp_ImportApplicationId = detail.GetImportApplicationReference(),
                defraimp_CounterHistoryType = detail.CounterHistoryType,
                defraimp_Operation = detail.Operation,
                defraimp_Reason = detail.Reason,
                defraimp_PreviousValue = detail.PreviousValue,
                defraimp_CurrentValue = detail.CurrentValue,
            };

            if (detail.CounterHistoryType == defraimp_counterhistory_defraimp_counterhistorytype.AutoNumber)
            {
                counterHistory.defraimp_AutoNumberId = detail.GetRelatedRecordReference();
            }
            else if (detail.CounterHistoryType == defraimp_counterhistory_defraimp_counterhistorytype.PlaceOfOrigin)
            {
                counterHistory.defraimp_PlaceOfOriginId = detail.GetRelatedRecordReference();
            }

            this.service.Create(counterHistory);
        }
    }
}
