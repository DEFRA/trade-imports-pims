namespace Defra.Imports.IntegrationTests.Dataverse.ImportApplication.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;

    public class SetManualPostImportCheckDecision : IExecutableAction<defraimp_importapplication, Guid>
    {
        private readonly ImportsContext context;
        private readonly defraimp_importapplication_defraimp_manualpostimportcheckdecision manualImportCheckDecision;

        public SetManualPostImportCheckDecision(ImportsContext context, defraimp_importapplication_defraimp_manualpostimportcheckdecision manualImportCheckDecision)
        {
            this.context = context;
            this.manualImportCheckDecision = manualImportCheckDecision;
        }

        /// <inheritdoc/>
        public void Execute(Guid id)
        {
            defraimp_importapplication importApplicationToUpdate = new defraimp_importapplication
            {
                Id = id,
                defraimp_ManualPostImportCheckDecision = this.manualImportCheckDecision,
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
