namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Describes one change to a counter, for the counter history. The related record is either an autonumber or a
    /// place of origin.
    /// </summary>
    public sealed class CounterTransactionDetail
    {
        private readonly Guid importApplicationId;
        private readonly defraimp_autonumber autonumber;
        private readonly defraimp_placeoforigin placeOfOrigin;

        private CounterTransactionDetail(
            Guid importApplicationId,
            defraimp_counterhistory_defraimp_counterhistorytype counterHistoryType,
            defraimp_autonumber autonumber,
            defraimp_placeoforigin placeOfOrigin,
            defraimp_counterhistory_defraimp_operation operation,
            defraimp_counterhistory_defraimp_reason reason)
        {
            this.importApplicationId = importApplicationId;
            this.CounterHistoryType = counterHistoryType;
            this.autonumber = autonumber;
            this.placeOfOrigin = placeOfOrigin;
            this.Operation = operation;
            this.Reason = reason;
        }

        /// <summary>Gets the type of counter that changed.</summary>
        public defraimp_counterhistory_defraimp_counterhistorytype CounterHistoryType { get; }

        /// <summary>Gets the operation that was carried out.</summary>
        public defraimp_counterhistory_defraimp_operation Operation { get; }

        /// <summary>Gets the reason for the change.</summary>
        public defraimp_counterhistory_defraimp_reason Reason { get; }

        /// <summary>Gets or sets the value before the change.</summary>
        public int PreviousValue { get; set; }

        /// <summary>Gets or sets the value after the change.</summary>
        public int CurrentValue { get; set; }

        /// <summary>Describes a change to an autonumber.</summary>
        /// <param name="importApplicationId">The identifier of the import application that caused the change.</param>
        /// <param name="autonumber">The autonumber that changed.</param>
        /// <param name="operation">The operation that is carried out.</param>
        /// <param name="reason">The reason for the change.</param>
        /// <returns>The detail, or null if there is no autonumber.</returns>
        public static CounterTransactionDetail ForAutonumber(
            Guid importApplicationId,
            defraimp_autonumber autonumber,
            defraimp_counterhistory_defraimp_operation operation,
            defraimp_counterhistory_defraimp_reason reason)
        {
            return autonumber != null
                ? new CounterTransactionDetail(importApplicationId, defraimp_counterhistory_defraimp_counterhistorytype.AutoNumber, autonumber, null, operation, reason)
                : null;
        }

        /// <summary>Describes a change to a place of origin.</summary>
        /// <param name="importApplicationId">The identifier of the import application that caused the change.</param>
        /// <param name="placeOfOrigin">The place of origin that changed.</param>
        /// <param name="operation">The operation that is carried out.</param>
        /// <param name="reason">The reason for the change.</param>
        /// <returns>The detail, or null if there is no place of origin.</returns>
        public static CounterTransactionDetail ForPlaceOfOrigin(
            Guid importApplicationId,
            defraimp_placeoforigin placeOfOrigin,
            defraimp_counterhistory_defraimp_operation operation,
            defraimp_counterhistory_defraimp_reason reason)
        {
            return placeOfOrigin != null
                ? new CounterTransactionDetail(importApplicationId, defraimp_counterhistory_defraimp_counterhistorytype.PlaceOfOrigin, null, placeOfOrigin, operation, reason)
                : null;
        }

        /// <summary>Creates a new reference to the import application that caused the change.</summary>
        /// <returns>A reference that is not shared with anything else.</returns>
        public EntityReference GetImportApplicationReference()
        {
            return new EntityReference(defraimp_importapplication.EntityLogicalName, this.importApplicationId);
        }

        /// <summary>Creates a new reference to the autonumber or place of origin that changed.</summary>
        /// <returns>A reference that is not shared with anything else.</returns>
        public EntityReference GetRelatedRecordReference()
        {
            if (this.CounterHistoryType == defraimp_counterhistory_defraimp_counterhistorytype.AutoNumber)
            {
                return this.autonumber != null ? new EntityReference(this.autonumber.LogicalName, this.autonumber.Id) : null;
            }

            if (this.CounterHistoryType == defraimp_counterhistory_defraimp_counterhistorytype.PlaceOfOrigin)
            {
                return this.placeOfOrigin != null ? new EntityReference(this.placeOfOrigin.LogicalName, this.placeOfOrigin.Id) : null;
            }

            return null;
        }
    }
}
