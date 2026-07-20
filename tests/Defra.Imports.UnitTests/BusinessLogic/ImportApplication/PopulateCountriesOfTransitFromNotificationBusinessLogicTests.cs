namespace Defra.Imports.UnitTests.BusinessLogic.ImportApplication
{
    using Defra.Imports.BusinessLogic.ImportApplication;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using System;
    using Xunit;

    public class PopulateCountriesOfTransitFromNotificationBusinessLogicTests
    {
        private Mock<IOrganizationService> mockOrgService;
        private PopulateCountriesOfTransitFromNotificationBusinessLogic populateCountriesOfTransitBusinessLogic;

        public PopulateCountriesOfTransitFromNotificationBusinessLogicTests()
        {
            this.mockOrgService = new Mock<IOrganizationService>();
            this.populateCountriesOfTransitBusinessLogic = new PopulateCountriesOfTransitFromNotificationBusinessLogic(mockOrgService.Object);
        }

        [Fact]
        public void PopulateCountriesOfTransit_ApplicationWithExistingTransitCountries_ShouldDissasociateTheExistingCountries()
        {
            // Arrange
            var stubbedApplicationRef = new EntityReference(defraimp_importapplication.EntityLogicalName, Guid.NewGuid());
            var stubbedNotificationRef = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, Guid.NewGuid());

            var stubbedExistingCountry = new Entity(defraimp_importapplication_defra_country.EntityLogicalName, Guid.NewGuid());
            stubbedExistingCountry.Attributes.Add("defra_countryid", Guid.NewGuid());
            var stubbedExistingTransitCountries = new EntityCollection();
            stubbedExistingTransitCountries.Entities.Add(stubbedExistingCountry);
            this.SetupMockOrgServiceForDissassociate(stubbedExistingTransitCountries, stubbedApplicationRef.Id);

            var stubbedNotificationTransitCountries = new EntityCollection();
            this.SetupMockOrgServiceForAssociate(stubbedNotificationTransitCountries, stubbedNotificationRef.Id);

            // Act
            this.populateCountriesOfTransitBusinessLogic.PopulateCountriesOfTransit(stubbedNotificationRef, stubbedApplicationRef);

            // Assert
            this.mockOrgService.Verify(
                x => x.Disassociate(
                    stubbedApplicationRef.LogicalName,
                    stubbedApplicationRef.Id,
                    It.Is<Relationship>(r => r.SchemaName == "defraimp_defraimp_importapplication_defra_country"),
                    It.Is<EntityReferenceCollection>(col => col[0].Id == (Guid) stubbedExistingTransitCountries[0]["defra_countryid"])
                ), Times.Once);
        }

        [Fact]
        public void PopulateCountriesOfTransit_NotificationWithTransitCountries_ShouldAssociateTransitCountriesToApplication()
        {
            // Arrange
            var stubbedApplicationRef = new EntityReference(defraimp_importapplication.EntityLogicalName, Guid.NewGuid());
            var stubbedNotificationRef = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, Guid.NewGuid());

            var stubbedExistingTransitCountries = new EntityCollection();
            this.SetupMockOrgServiceForDissassociate(stubbedExistingTransitCountries, stubbedApplicationRef.Id);

            var stubbedNotificationCountry = new Entity(defraimp_importapplication_defra_country.EntityLogicalName, Guid.NewGuid());
            stubbedNotificationCountry.Attributes.Add("defra_countryid", Guid.NewGuid());
            var stubbedNotificationTransitCountries = new EntityCollection();
            stubbedNotificationTransitCountries.Entities.Add(stubbedNotificationCountry);
            this.SetupMockOrgServiceForAssociate(stubbedNotificationTransitCountries, stubbedNotificationRef.Id);

            // Act
            this.populateCountriesOfTransitBusinessLogic.PopulateCountriesOfTransit(stubbedNotificationRef, stubbedApplicationRef);

            // Assert
            this.mockOrgService.Verify(
                x => x.Associate(
                    stubbedApplicationRef.LogicalName,
                    stubbedApplicationRef.Id,
                    It.Is<Relationship>(r => r.SchemaName == "defraimp_defraimp_importapplication_defra_country"),
                    It.Is<EntityReferenceCollection>(col => col[0].Id == (Guid)stubbedNotificationTransitCountries[0]["defra_countryid"])
                ), Times.Once);
        }

        private void SetupMockOrgServiceForDissassociate(EntityCollection stubbedExistingTransitCountries, Guid applicationId)
        {
            this.mockOrgService.Setup(x => x.RetrieveMultiple(
                It.Is<QueryExpression>(
                    q =>
                        q.EntityName == defraimp_importapplication_defra_country.EntityLogicalName &&
                        q.Criteria.Conditions[0].AttributeName == "defraimp_importapplicationid" &&
                        (Guid) q.Criteria.Conditions[0].Values[0] == applicationId)))
                .Returns(stubbedExistingTransitCountries);
        }

        private void SetupMockOrgServiceForAssociate(EntityCollection stubbedImporterTransitCountries, Guid notificationId)
        {
            this.mockOrgService.Setup(x => x.RetrieveMultiple(
                It.Is<QueryExpression>(
                    q =>
                        q.EntityName == defraimp_ImporterNotification_CountriesofTransit.EntityLogicalName &&
                        q.Criteria.Conditions[0].AttributeName == "defraimp_importernotificationid" &&
                        (Guid) q.Criteria.Conditions[0].Values[0] == notificationId)))
                .Returns(stubbedImporterTransitCountries);
        }
    }
}
