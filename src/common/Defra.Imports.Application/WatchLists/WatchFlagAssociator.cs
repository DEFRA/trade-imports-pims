namespace Defra.Imports.Application.WatchLists
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Dataverse.Queries;
    using Microsoft.Xrm.Sdk;

    /// <inheritdoc/>
    public sealed class WatchFlagAssociator : IWatchFlagAssociator
    {
        private readonly WatchFlagQueries queries;
        private readonly IOrganizationService service;

        /// <summary>Initializes a new instance of the <see cref="WatchFlagAssociator"/> class.</summary>
        /// <param name="queries">The Dataverse queries used to find the watch flags.</param>
        /// <param name="service">The current invocation's organisation service, used to update the watch flags.</param>
        public WatchFlagAssociator(WatchFlagQueries queries, IOrganizationService service)
        {
            this.queries = queries ?? throw new ArgumentNullException(nameof(queries));
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <inheritdoc/>
        public void AssociateWatchFlags(AssociateItahcWatchFlagsContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.AssociateWatchFlags(
                context.ImportApplicationId,
                context.PrimaryItahcId,
                this.queries.GetItahcWatchFlagsLinkedToImportApplication,
                this.queries.GetWatchFlagsForItahc);
        }

        /// <inheritdoc/>
        public void AssociateWatchFlags(AssociateNotificationWatchFlagsContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.AssociateWatchFlags(
                context.ImportApplicationId,
                context.PrimaryImporterNotificationId,
                this.queries.GetNotificationWatchFlagsLinkedToImportApplication,
                this.queries.GetWatchFlagsForImporterNotification);
        }

        private void AssociateWatchFlags(
            Guid importApplicationId,
            Guid? primaryRecordId,
            Func<Guid, IReadOnlyList<defraimp_WatchFlag>> getLinkedWatchFlags,
            Func<Guid, IReadOnlyList<defraimp_WatchFlag>> getPrimaryRecordWatchFlags)
        {
            // TODO: the links are cleared and then re-read, so a watch flag that stays linked to the import
            // application is updated twice, which triggers downstream plug-ins, flows and audit. This should be
            // investigated, but is preserved by the refactoring.
            // TODO: each read uses only the first page of watch flags that Dataverse returns, with no paging, so any
            // further watch flags are ignored. This should be investigated, but is preserved by the refactoring.
            foreach (var watchFlag in getLinkedWatchFlags(importApplicationId))
            {
                this.UpdateImportApplication(watchFlag, null);
            }

            if (primaryRecordId == null)
            {
                return;
            }

            // TODO: a watch flag of the primary record that is linked to a different import application is moved to
            // this one. This should be investigated, but is preserved by the refactoring.
            foreach (var watchFlag in getPrimaryRecordWatchFlags(primaryRecordId.Value))
            {
                if (watchFlag.defraimp_ImportApplicationId == null || watchFlag.defraimp_ImportApplicationId.Id != importApplicationId)
                {
                    this.UpdateImportApplication(watchFlag, new EntityReference(defraimp_importapplication.EntityLogicalName, importApplicationId));
                }
            }
        }

        // The update holds only the identifier and the link, so the entities that the queries returned are not modified.
        private void UpdateImportApplication(defraimp_WatchFlag watchFlag, EntityReference importApplication)
        {
            this.service.Update(new defraimp_WatchFlag
            {
                defraimp_WatchFlagId = watchFlag.defraimp_WatchFlagId,
                defraimp_ImportApplicationId = importApplication,
            });
        }
    }
}
