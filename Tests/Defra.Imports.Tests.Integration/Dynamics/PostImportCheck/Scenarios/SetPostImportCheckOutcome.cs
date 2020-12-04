namespace Defra.Imports.Tests.Integration.Dynamics.PostImportCheck.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;

    class SetPostImportCheckOutcome : IExecutableAction<defraimp_importinspection, Guid>
    {
        private readonly ImportsContext context;
        private readonly defraimp_importinspection postImportCheck;
        private readonly defraimp_importinspection_defraimp_inspectionoutcome outcomeValue;

        public SetPostImportCheckOutcome(ImportsContext context, defraimp_importinspection postImportCheck, defraimp_importinspection_defraimp_inspectionoutcome outcomeValue)
        {
            this.context = context;
            this.postImportCheck = postImportCheck;
            this.outcomeValue = outcomeValue;
        }

        public void Execute(Guid id)
        {
            defraimp_importinspection postImportCheckToUpdate = new defraimp_importinspection
            {
                Id = this.postImportCheck.Id,
                defraimp_InspectionOutcome = this.outcomeValue,
            };

            if (!this.context.IsAttached(postImportCheckToUpdate))
            {
                this.context.Attach(postImportCheckToUpdate);
            }

            this.context.UpdateObject(postImportCheckToUpdate);
            this.context.SaveChanges();
        }
    }
}
