namespace Defra.Imports.UnitTests.Dataverse.Extensions
{
    using System;
    using System.Linq;
    using Defra.Imports.Dataverse.Extensions;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Xunit;

    public sealed class ImporterNotificationExtensionsTests
    {
        [Fact]
        public void ToCreateWatchFlagsContext_NullNotification_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((defraimp_ImporterNotification)null).ToCreateWatchFlagsContext());
        }

        [Fact]
        public void ToCreateWatchFlagsContext_Notification_ProjectsTheReferenceNameAndEachCompanyName()
        {
            var notification = new defraimp_ImporterNotification
            {
                Id = Guid.NewGuid(),
                defraimp_Name = "Notification",
                defraimp_consignorcompanyname = "consignor",
                defraimp_placeofdestinationcompanyname = "destination",
                defraimp_importercompanyname = "importer",
                defraimp_transportercompanyname = "transporter",
            };

            var context = notification.ToCreateWatchFlagsContext();

            Assert.Equal(notification.Id, context.Notification.Id);
            Assert.Equal(defraimp_ImporterNotification.EntityLogicalName, context.Notification.LogicalName);
            Assert.Equal("Notification", context.Name);
            Assert.Equal("consignor", context.Consignor);
            Assert.Equal("destination", context.PlaceOfDestination);
            Assert.Equal("importer", context.Importer);
            Assert.Equal("transporter", context.Transporter);
        }

        [Fact]
        public void ToCreateWatchFlagsContext_NotificationWithoutTheAttributesSupplied_ProjectsNulls()
        {
            var context = new defraimp_ImporterNotification { Id = Guid.NewGuid() }.ToCreateWatchFlagsContext();

            Assert.Null(context.Name);
            Assert.Null(context.Consignor);
            Assert.Null(context.PlaceOfDestination);
            Assert.Null(context.Importer);
            Assert.Null(context.Transporter);
        }

        [Fact]
        public void ToCreateWatchFlagsContext_NotificationWithSomeAttributesSupplied_ProjectsTheRestAsNull()
        {
            var context = new defraimp_ImporterNotification { Id = Guid.NewGuid(), defraimp_importercompanyname = "importer" }.ToCreateWatchFlagsContext();

            Assert.Equal("importer", context.Importer);
            Assert.Null(context.Name);
            Assert.Null(context.Transporter);
        }

        [Fact]
        public void ToCreateWatchFlagsContext_Notification_DoesNotModifyTheNotification()
        {
            var notification = new defraimp_ImporterNotification { Id = Guid.NewGuid(), defraimp_Name = "Notification", defraimp_importercompanyname = "importer" };
            var attributes = notification.Attributes.ToDictionary(pair => pair.Key, pair => pair.Value);

            notification.ToCreateWatchFlagsContext();

            Assert.Equal(attributes, notification.Attributes.ToDictionary(pair => pair.Key, pair => pair.Value));
        }

        [Fact]
        public void CreateNotificationWatchFlagsContext_NullReference_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CreateNotificationWatchFlagsContext(null, "name", null, null, null, null));
        }
    }
}
