namespace Defra.Imports.IntegrationTests.Dataverse
{
    using System;
    using System.Linq;
    using Defra.Imports.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Upserts the reference data that the integration tests depend on before any test runs.
    /// </summary>
    [TestClass]
    public static class ReferenceDataHooks
    {
        private static readonly Guid DogGoldBronzeCommodityId = new Guid("1477b550-b6e6-e911-a866-000d3ab0dc71");

        private static readonly Guid RomaniaCountryId = new Guid("9c9cb7ed-b2d3-e911-a861-000d3ab1dad7");

        /// <summary>
        /// Ensures the Dog gold/bronze commodity is related to Romania, creating the relationship only if it does not already exist.
        /// </summary>
        /// <param name="context">The test context.</param>
        [AssemblyInitialize]
        public static void UpsertDogRomaniaGoldBronzeCountry(TestContext context)
        {
            using (var dataverseContext = DataverseFixture.GetAppUserContext())
            {
                var exists = dataverseContext.defraimp_goldbronzecountriesnnSet
                    .Where(r => r.defraimp_goldbronzecommodityid == DogGoldBronzeCommodityId && r.defra_countryid == RomaniaCountryId)
                    .Select(r => r.defraimp_goldbronzecountriesnnId)
                    .ToList()
                    .Any();

                if (exists)
                {
                    return;
                }

                var relationship = new Entity(defraimp_goldbronzecountriesnn.EntityLogicalName)
                {
                    ["defraimp_goldbronzecommodityid"] = DogGoldBronzeCommodityId,
                    ["defra_countryid"] = RomaniaCountryId,
                };

                DataverseFixture.BaseClient.Create(relationship);
            }
        }
    }
}
