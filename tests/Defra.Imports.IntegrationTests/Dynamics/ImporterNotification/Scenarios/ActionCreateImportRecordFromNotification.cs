namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Scenarios
{
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;
    using Microsoft.Xrm.Sdk;
    using System;

    public class ActionCreateImportRecordFromNotification : IExecutableAction<defraimp_ImporterNotification, Guid>
    {
        private ImportsContext context;

        public ActionCreateImportRecordFromNotification(ImportsContext context)
        {
            this.context = context;
        }

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
