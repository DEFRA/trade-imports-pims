namespace Defra.Imports.IntegrationTests.Dataverse.ImportApplication.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;

    public class SetApplicationInspectionOutcomeValue : IExecutableAction<defraimp_importapplication, Guid>
    {
        private readonly ImportsContext context;
        private readonly defraimp_importapplication_defraimp_inspectionoutcome inspectionOutcome;

        public SetApplicationInspectionOutcomeValue(ImportsContext context, defraimp_importapplication_defraimp_inspectionoutcome inspectionOutcome)
        {
            this.context = context;
            this.inspectionOutcome = inspectionOutcome;
        }

        /// <inheritdoc/>
        public void Execute(Guid id)
        {
            defraimp_importapplication importApplicationToUpdate = new defraimp_importapplication
            {
                Id = id,
                defraimp_InspectionOutcome = this.inspectionOutcome,
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
