namespace Defra.Imports.IntegrationTests.Dataverse.ImportApplication.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;

    public class AssignImporterNotificationToImportApplication : IExecutableAction<defraimp_importapplication, Guid>
    {
        private readonly ImportsContext context;
        private readonly defraimp_ImporterNotification importerNotification;

        public AssignImporterNotificationToImportApplication(ImportsContext context, defraimp_ImporterNotification importerNotification)
        {
            this.context = context;
            this.importerNotification = importerNotification;
        }

        /// <inheritdoc/>
        public void Execute(Guid id)
        {
            defraimp_importapplication importApplicationToUpdate = new defraimp_importapplication
            {
                Id = id,
                defraimp_PrimaryImporterNotificationId = new Microsoft.Xrm.Sdk.EntityReference(this.importerNotification.LogicalName, this.importerNotification.Id),
            };

            if (!this.context.IsAttached(importApplicationToUpdate))
            {
                this.context.Attach(importApplicationToUpdate);
            }

            this.context.UpdateObject(importApplicationToUpdate);
            this.context.SaveChanges();
        }
    }
}
