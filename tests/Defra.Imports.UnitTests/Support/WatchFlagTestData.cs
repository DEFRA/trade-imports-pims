namespace Defra.Imports.UnitTests.Support
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk;

    // Builds records the way a Create Target presents them: only the attributes that were supplied are present,
    // so any other attribute reads as null.
    internal static class WatchFlagTestData
    {
        public static defraimp_itahc Itahc(
            Guid? id = null,
            string name = null,
            string placeOfOrigin = null,
            string placeOfDestination = null,
            string consignee = null,
            string transporter = null,
            string veterinarian = null)
        {
            var itahc = new defraimp_itahc { Id = id ?? Guid.NewGuid() };
            SetIfSupplied(itahc, defraimp_itahc.Fields.defraimp_name, name);
            SetIfSupplied(itahc, defraimp_itahc.Fields.defraimp_PlaceOfOriginHarvestName, placeOfOrigin);
            SetIfSupplied(itahc, defraimp_itahc.Fields.defraimp_PlaceOfDestinationName, placeOfDestination);
            SetIfSupplied(itahc, defraimp_itahc.Fields.defraimp_ConsigneeName, consignee);
            SetIfSupplied(itahc, defraimp_itahc.Fields.defraimp_TransporterName, transporter);
            SetIfSupplied(itahc, defraimp_itahc.Fields.defraimp_OVName, veterinarian);
            return itahc;
        }

        public static defraimp_ImporterNotification ImporterNotification(
            Guid? id = null,
            string name = null,
            string consignor = null,
            string placeOfDestination = null,
            string importer = null,
            string transporter = null)
        {
            var notification = new defraimp_ImporterNotification { Id = id ?? Guid.NewGuid() };
            SetIfSupplied(notification, defraimp_ImporterNotification.Fields.defraimp_Name, name);
            SetIfSupplied(notification, defraimp_ImporterNotification.Fields.defraimp_consignorcompanyname, consignor);
            SetIfSupplied(notification, defraimp_ImporterNotification.Fields.defraimp_placeofdestinationcompanyname, placeOfDestination);
            SetIfSupplied(notification, defraimp_ImporterNotification.Fields.defraimp_importercompanyname, importer);
            SetIfSupplied(notification, defraimp_ImporterNotification.Fields.defraimp_transportercompanyname, transporter);
            return notification;
        }

        // A Create Target of an import application holds only the primary lookups that were supplied.
        public static defraimp_importapplication ImportApplication(Guid? id = null, Guid? primaryItahcId = null, Guid? primaryImporterNotificationId = null)
        {
            var importApplication = new defraimp_importapplication { Id = id ?? Guid.NewGuid() };
            if (primaryItahcId != null)
            {
                importApplication.defraimp_PrimaryITAHCId = new EntityReference(defraimp_itahc.EntityLogicalName, primaryItahcId.Value);
            }

            if (primaryImporterNotificationId != null)
            {
                importApplication.defraimp_PrimaryImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, primaryImporterNotificationId.Value);
            }

            return importApplication;
        }

        // As the queries return it: only the identifier and the import application lookup are present.
        public static defraimp_WatchFlag WatchFlag(Guid? id = null, Guid? importApplicationId = null)
        {
            var watchFlag = new defraimp_WatchFlag { defraimp_WatchFlagId = id ?? Guid.NewGuid() };
            if (importApplicationId != null)
            {
                watchFlag.defraimp_ImportApplicationId = new EntityReference(defraimp_importapplication.EntityLogicalName, importApplicationId.Value);
            }

            return watchFlag;
        }

        private static void SetIfSupplied(Entity entity, string attributeName, string value)
        {
            if (value != null)
            {
                entity[attributeName] = value;
            }
        }
    }
}
