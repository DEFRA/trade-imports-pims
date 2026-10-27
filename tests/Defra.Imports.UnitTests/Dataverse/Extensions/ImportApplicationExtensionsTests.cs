namespace Defra.Imports.UnitTests.Dataverse.Extensions
{
    using System;
    using System.Linq;
    using Defra.Imports.Dataverse.Extensions;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Xunit;

    public sealed class ImportApplicationExtensionsTests
    {
        private const string CommodityTypeLogicalName = "defraexp_commoditytype";

        [Fact]
        public void ToAssociateItahcWatchFlagsContext_NullImportApplication_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((defraimp_importapplication)null).ToAssociateItahcWatchFlagsContext());
        }

        [Fact]
        public void ToAssociateNotificationWatchFlagsContext_NullImportApplication_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((defraimp_importapplication)null).ToAssociateNotificationWatchFlagsContext());
        }

        [Fact]
        public void ToAssociateItahcWatchFlagsContext_ImportApplication_ProjectsItsIdentifierAndThePrimaryItahcIdentifier()
        {
            var itahcId = Guid.NewGuid();
            var importApplication = WatchFlagTestData.ImportApplication(primaryItahcId: itahcId, primaryImporterNotificationId: Guid.NewGuid());

            var context = importApplication.ToAssociateItahcWatchFlagsContext();

            Assert.Equal(importApplication.Id, context.ImportApplicationId);
            Assert.Equal(itahcId, context.PrimaryItahcId);
        }

        [Fact]
        public void ToAssociateNotificationWatchFlagsContext_ImportApplication_ProjectsItsIdentifierAndThePrimaryNotificationIdentifier()
        {
            var notificationId = Guid.NewGuid();
            var importApplication = WatchFlagTestData.ImportApplication(primaryItahcId: Guid.NewGuid(), primaryImporterNotificationId: notificationId);

            var context = importApplication.ToAssociateNotificationWatchFlagsContext();

            Assert.Equal(importApplication.Id, context.ImportApplicationId);
            Assert.Equal(notificationId, context.PrimaryImporterNotificationId);
        }

        [Fact]
        public void ToAssociateItahcWatchFlagsContext_ImportApplicationWithoutThePrimaryItahcSupplied_ProjectsNull()
        {
            var context = WatchFlagTestData.ImportApplication().ToAssociateItahcWatchFlagsContext();

            Assert.Null(context.PrimaryItahcId);
        }

        [Fact]
        public void ToAssociateNotificationWatchFlagsContext_ImportApplicationWithoutThePrimaryNotificationSupplied_ProjectsNull()
        {
            var context = WatchFlagTestData.ImportApplication().ToAssociateNotificationWatchFlagsContext();

            Assert.Null(context.PrimaryImporterNotificationId);
        }

        [Fact]
        public void ToAssociateWatchFlagsContext_ImportApplication_DoesNotModifyTheImportApplication()
        {
            var importApplication = WatchFlagTestData.ImportApplication(primaryItahcId: Guid.NewGuid(), primaryImporterNotificationId: Guid.NewGuid());
            var attributes = importApplication.Attributes.ToDictionary(pair => pair.Key, pair => pair.Value);

            importApplication.ToAssociateItahcWatchFlagsContext();
            importApplication.ToAssociateNotificationWatchFlagsContext();

            Assert.Equal(attributes, importApplication.Attributes.ToDictionary(pair => pair.Key, pair => pair.Value));
        }

        [Fact]
        public void ToInspectionContext_NullImportApplication_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((defraimp_importapplication)null).ToInspectionContext());
        }

        [Fact]
        public void ToInspectionContext_PopulatedImportApplication_ProjectsEveryAttribute()
        {
            var id = Guid.NewGuid();
            var importRiskLevelId = Guid.NewGuid();
            var placeOfOriginId = Guid.NewGuid();
            var previousPlaceOfOriginId = Guid.NewGuid();
            var commodityTypeId = Guid.NewGuid();
            var countryOfOriginId = Guid.NewGuid();
            var primaryItahcId = Guid.NewGuid();
            var primaryImporterNotificationId = Guid.NewGuid();
            var importApplication = new defraimp_importapplication
            {
                Id = id,
                defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ITAHC,
                defraimp_importrisklevelid = new EntityReference(defraimp_importrisklevel.EntityLogicalName, importRiskLevelId),
                defraimp_InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes,
                defraimp_InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP1Inspection,
                defraimp_PlaceofOriginid = new EntityReference(defraimp_placeoforigin.EntityLogicalName, placeOfOriginId),
                defraimp_previousplaceoforiginid = new EntityReference(defraimp_placeoforigin.EntityLogicalName, previousPlaceOfOriginId),
                defraimp_CommodityTypeId = new EntityReference(CommodityTypeLogicalName, commodityTypeId),
                defraimp_CountryofOriginId = new EntityReference(defra_country.EntityLogicalName, countryOfOriginId),
                defraimp_PrimaryITAHCId = new EntityReference(defraimp_itahc.EntityLogicalName, primaryItahcId),
                defraimp_PrimaryImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, primaryImporterNotificationId),
                statecode = defraimp_importapplication_statecode.Active,
                statuscode = defraimp_importapplication_statuscode.Cancelled,
                defraimp_ImportRecordCounted = true,
                defraimp_ManualPostImportCheckDecision = defraimp_importapplication_defraimp_manualpostimportcheckdecision.UseSystemDecision,
            };

            var context = importApplication.ToInspectionContext();

            Assert.Equal(id, context.Id);
            Assert.Equal(defraimp_importapplication_defraimp_importapplicationtype.ITAHC, context.ImportApplicationType);
            Assert.Equal(importRiskLevelId, context.ImportRiskLevel.Id);
            Assert.Equal(defraimp_importapplication_defraimp_inspectionrequired.Yes, context.InspectionRequired);
            Assert.Equal(defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP1Inspection, context.InspectionRequiredReason);
            Assert.Equal(placeOfOriginId, context.PlaceOfOrigin.Id);
            Assert.Equal(previousPlaceOfOriginId, context.PreviousPlaceOfOrigin.Id);
            Assert.Equal(commodityTypeId, context.CommodityType.Id);
            Assert.Equal(countryOfOriginId, context.CountryOfOrigin.Id);
            Assert.Equal(primaryItahcId, context.PrimaryItahc.Id);
            Assert.Equal(primaryImporterNotificationId, context.PrimaryImporterNotification.Id);
            Assert.Equal(defraimp_importapplication_statecode.Active, context.StateCode);
            Assert.Equal(defraimp_importapplication_statuscode.Cancelled, context.StatusCode);
            Assert.True(context.ImportRecordCounted);
            Assert.Equal(defraimp_importapplication_defraimp_manualpostimportcheckdecision.UseSystemDecision, context.ManualPostImportCheckDecision);
        }

        [Fact]
        public void ToInspectionContext_CreateShapedImportApplication_ProjectsEveryUnsuppliedAttributeAsNull()
        {
            var importApplication = new defraimp_importapplication
            {
                Id = Guid.NewGuid(),
                defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ITAHC,
                defraimp_ImportRecordCounted = false,
            };

            var context = importApplication.ToInspectionContext();

            Assert.Equal(defraimp_importapplication_defraimp_importapplicationtype.ITAHC, context.ImportApplicationType);
            Assert.False(context.ImportRecordCounted);
            Assert.Null(context.ImportRiskLevel);
            Assert.Null(context.InspectionRequired);
            Assert.Null(context.InspectionRequiredReason);
            Assert.Null(context.PlaceOfOrigin);
            Assert.Null(context.PreviousPlaceOfOrigin);
            Assert.Null(context.CommodityType);
            Assert.Null(context.CountryOfOrigin);
            Assert.Null(context.PrimaryItahc);
            Assert.Null(context.PrimaryImporterNotification);
            Assert.Null(context.StateCode);
            Assert.Null(context.StatusCode);
            Assert.Null(context.ManualPostImportCheckDecision);
        }

        [Fact]
        public void ToInspectionContext_ImportApplication_DoesNotModifyTheImportApplication()
        {
            var importApplication = new defraimp_importapplication
            {
                Id = Guid.NewGuid(),
                defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ITAHC,
                defraimp_ImportRecordCounted = false,
            };
            var count = importApplication.Attributes.Count;

            importApplication.ToInspectionContext();

            Assert.Equal(count, importApplication.Attributes.Count);
        }
    }
}
