namespace Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;
    using Microsoft.Xrm.Sdk;

    public class ActionCreateImportRecordFromNotification : IExecutableAction<defraimp_ImporterNotification, Guid>
    {
        private ImportsContext context;

        public ActionCreateImportRecordFromNotification(ImportsContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public void Execute(Guid id)
        {
            var request = new defraimp_CreateImportRecordFromNotificationRequest()
            {
                Target = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, id),
            };

            this.context.Execute(request);
        }
    }
}
