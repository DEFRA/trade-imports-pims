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

        private static void SetIfSupplied(Entity entity, string attributeName, string value)
        {
            if (value != null)
            {
                entity[attributeName] = value;
            }
        }
    }
}
