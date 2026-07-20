namespace Defra.Imports.IntegrationTests.Dynamics.ImportApplication.Scenarios
{
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;
    using System;

    public class AssignPlaceOfOriginToImportApplication : IExecutableAction<defraimp_importapplication, Guid>
    {
        private readonly ImportsContext context;
        private readonly defraimp_placeoforigin placeOfOrigin;

        public AssignPlaceOfOriginToImportApplication(ImportsContext context, defraimp_placeoforigin placeOfOrigin)
        {
            this.context = context;
            this.placeOfOrigin = placeOfOrigin;
        }

        public void Execute(Guid id)
        {
            defraimp_importapplication importApplicationToUpdate = new defraimp_importapplication
            {
                Id = id,
                defraimp_PlaceofOriginid = new Microsoft.Xrm.Sdk.EntityReference(this.placeOfOrigin.LogicalName, this.placeOfOrigin.Id),
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
