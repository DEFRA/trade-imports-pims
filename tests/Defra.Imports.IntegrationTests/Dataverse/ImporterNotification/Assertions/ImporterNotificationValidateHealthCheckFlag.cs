namespace Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Assertions
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Assertions.Validators;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Messages;

    class ImporNotificationValidateHealthCheckFlag : BaseValidator<Guid, defraimp_ImporterNotification>
    {
        private readonly ImportsContext context;
        private readonly bool expectedValue;

        public ImporNotificationValidateHealthCheckFlag(ImportsContext context, bool expectedValue)
        {
            this.context = context;
            this.expectedValue = expectedValue;
        }

        /// <inheritdoc/>
        public override defraimp_ImporterNotification GetRecord(Guid id)
        {
            return (this.context.Execute(new RetrieveRequest() { Target = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, id), ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(true) } ) as RetrieveResponse).Entity.ToEntity<defraimp_ImporterNotification>();
        }

        /// <inheritdoc/>
        public override List<ISpecificationValidator<defraimp_ImporterNotification>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_ImporterNotification>>
            {
                new ImportNotificationHasHealthCertificateFlag(this.expectedValue)
            };
        }
    }
}
