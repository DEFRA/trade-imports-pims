namespace Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;
    using Microsoft.Xrm.Sdk.Messages;

    public class CreateIPAFFSHealthCheckDocument : IExecutableAction<defraimp_ImporterNotification, Guid>
    {
        private readonly ImportsContext context;

        public CreateIPAFFSHealthCheckDocument(ImportsContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public void Execute(Guid id)
        {
            var doc = new defraimp_ipaffsdocument();
            doc.defraimp_DocumentUrl = "http://someurl.com";
            doc.defraimp_DocumentType = defraimp_ipaffsdocument_defraimp_documenttype.Healthcertificate;
            doc.defraimp_ImporterNotificationId = new Microsoft.Xrm.Sdk.EntityReference(defraimp_ImporterNotification.EntityLogicalName, id);
            this.context.Execute(new CreateRequest() {Target = doc });
        }
    }
}
