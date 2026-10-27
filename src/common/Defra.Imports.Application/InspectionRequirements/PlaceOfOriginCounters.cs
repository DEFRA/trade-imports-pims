namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Reads and changes the application and quota counters of a place of origin. Each change reads the place of
    /// origin and then sends an explicit update that holds only the counter.
    /// </summary>
    public sealed class PlaceOfOriginCounters
    {
        private readonly PlaceOfOriginQueries queries;
        private readonly IOrganizationService service;

        /// <summary>Initializes a new instance of the <see cref="PlaceOfOriginCounters"/> class.</summary>
        /// <param name="queries">The queries used to find places of origin.</param>
        /// <param name="service">The current invocation's organisation service, used for updates.</param>
        public PlaceOfOriginCounters(PlaceOfOriginQueries queries, IOrganizationService service)
        {
            this.queries = queries ?? throw new ArgumentNullException(nameof(queries));
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>Finds a place of origin, with all of its columns.</summary>
        /// <param name="placeOfOriginId">The identifier of the place of origin.</param>
        /// <returns>The place of origin, or null if there is none.</returns>
        public defraimp_placeoforigin Find(Guid placeOfOriginId)
        {
            return this.queries.Find(placeOfOriginId);
        }

        /// <summary>Gets the application counter of a place of origin.</summary>
        /// <param name="placeOfOriginId">The identifier of the place of origin.</param>
        /// <returns>The counter, or zero if it has none or the place of origin is not found.</returns>
        public int GetApplicationCounterValue(Guid placeOfOriginId)
        {
            var placeOfOrigin = this.Find(placeOfOriginId);
            return placeOfOrigin != null ? placeOfOrigin.defraimp_ApplicationCounter ?? 0 : 0;
        }

        /// <summary>Gets the inspection quota counter of a place of origin.</summary>
        /// <param name="placeOfOriginId">The identifier of the place of origin.</param>
        /// <returns>The counter, or zero if it has none or the place of origin is not found.</returns>
        public int GetQuotaCounterValue(Guid placeOfOriginId)
        {
            var placeOfOrigin = this.Find(placeOfOriginId);
            return placeOfOrigin != null ? placeOfOrigin.defraimp_InspectionQuotaCounter ?? 0 : 0;
        }

        /// <summary>Increases the application counter by one and updates the place of origin.</summary>
        /// <param name="placeOfOriginId">The identifier of the place of origin.</param>
        public void IncrementApplicationCounter(Guid placeOfOriginId)
        {
            this.SetApplicationCounter(placeOfOriginId, this.GetApplicationCounterValue(placeOfOriginId) + 1);
        }

        /// <summary>Decreases the application counter by one and updates the place of origin.</summary>
        /// <param name="placeOfOriginId">The identifier of the place of origin.</param>
        public void DecrementApplicationCounter(Guid placeOfOriginId)
        {
            this.SetApplicationCounter(placeOfOriginId, this.GetApplicationCounterValue(placeOfOriginId) - 1);
        }

        /// <summary>Sets the application counter and updates the place of origin.</summary>
        /// <param name="placeOfOriginId">The identifier of the place of origin.</param>
        /// <param name="value">The new counter value.</param>
        public void SetApplicationCounter(Guid placeOfOriginId, int value)
        {
            this.service.Update(new defraimp_placeoforigin
            {
                Id = placeOfOriginId,
                defraimp_ApplicationCounter = value,
            });
        }

        /// <summary>Increases the inspection quota counter by one and updates the place of origin.</summary>
        /// <param name="placeOfOriginId">The identifier of the place of origin.</param>
        public void IncrementQuotaCounter(Guid placeOfOriginId)
        {
            this.SetQuotaCounter(placeOfOriginId, this.GetQuotaCounterValue(placeOfOriginId) + 1);
        }

        /// <summary>Decreases the inspection quota counter by one and updates the place of origin.</summary>
        /// <param name="placeOfOriginId">The identifier of the place of origin.</param>
        public void DecrementQuotaCounter(Guid placeOfOriginId)
        {
            this.SetQuotaCounter(placeOfOriginId, this.GetQuotaCounterValue(placeOfOriginId) - 1);
        }

        private void SetQuotaCounter(Guid placeOfOriginId, int value)
        {
            this.service.Update(new defraimp_placeoforigin
            {
                Id = placeOfOriginId,
                defraimp_InspectionQuotaCounter = value,
            });
        }
    }
}
