namespace Defra.Imports.UnitTests.Dataverse.Extensions
{
    using System;
    using System.Linq;
    using Defra.Imports.Dataverse.Extensions;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.UnitTests.Support;
    using Xunit;

    public sealed class ImportApplicationExtensionsTests
    {
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
    }
}
