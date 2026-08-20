namespace Defra.Imports.IntegrationTests.Dataverse.ImportApplication.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;
    using Microsoft.Xrm.Sdk;

    public class AssignCommodityAndCountryOfOriginToImportApplication : IExecutableAction<defraimp_importapplication, Guid>
    {
        private readonly ImportsContext context;
        private readonly EntityReference countryOfOrigin;
        private readonly EntityReference commodityType;

        public AssignCommodityAndCountryOfOriginToImportApplication(ImportsContext context, EntityReference countryOfOrigin, EntityReference commodityType)
        {
            this.context = context;
            this.countryOfOrigin = countryOfOrigin;
            this.commodityType = commodityType;
        }

        /// <inheritdoc/>
        public void Execute(Guid id)
        {
            defraimp_importapplication importApplicationToUpdate = new defraimp_importapplication
            {
                Id = id,
                defraimp_CountryofOriginId = this.countryOfOrigin,
                defraimp_CommodityTypeId = this.commodityType,
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
