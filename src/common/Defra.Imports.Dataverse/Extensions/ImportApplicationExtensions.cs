namespace Defra.Imports.Dataverse.Extensions
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;

    /// <summary>
    /// Extensions for the import application entity.
    /// </summary>
    public static class ImportApplicationExtensions
    {
        /// <summary>
        /// Projects an import application onto the values needed to associate it with the watch flags of its primary
        /// ITAHC. The import application is not modified. An attribute that it does not hold, such as one that a
        /// Create did not supply, is projected as null.
        /// </summary>
        /// <param name="importApplication">The import application.</param>
        /// <returns>The context holding the identifiers of the import application and of its primary ITAHC.</returns>
        public static AssociateItahcWatchFlagsContext ToAssociateItahcWatchFlagsContext(this defraimp_importapplication importApplication)
        {
            if (importApplication == null)
            {
                throw new ArgumentNullException(nameof(importApplication));
            }

            return new AssociateItahcWatchFlagsContext(
                importApplication.Id,
                importApplication.defraimp_PrimaryITAHCId?.Id);
        }

        /// <summary>
        /// Projects an import application onto the values needed to associate it with the watch flags of its primary
        /// importer notification. The import application is not modified. An attribute that it does not hold, such as
        /// one that a Create did not supply, is projected as null.
        /// </summary>
        /// <param name="importApplication">The import application.</param>
        /// <returns>The context holding the identifiers of the import application and of its primary importer notification.</returns>
        public static AssociateNotificationWatchFlagsContext ToAssociateNotificationWatchFlagsContext(this defraimp_importapplication importApplication)
        {
            if (importApplication == null)
            {
                throw new ArgumentNullException(nameof(importApplication));
            }

            return new AssociateNotificationWatchFlagsContext(
                importApplication.Id,
                importApplication.defraimp_PrimaryImporterNotificationId?.Id);
        }

        /// <summary>
        /// Projects an import application onto the values needed to determine its inspection requirement. The import
        /// application is not modified. An attribute that it does not hold, such as one that an image or a retrieve
        /// did not include, is projected as null.
        /// </summary>
        /// <param name="importApplication">The import application.</param>
        /// <returns>The context holding the values that the inspection requirement depends on.</returns>
        public static InspectionImportApplicationContext ToInspectionContext(this defraimp_importapplication importApplication)
        {
            if (importApplication == null)
            {
                throw new ArgumentNullException(nameof(importApplication));
            }

            return new InspectionImportApplicationContext(
                importApplication.Id,
                importApplication.defraimp_ImportApplicationType,
                importApplication.defraimp_importrisklevelid,
                importApplication.defraimp_InspectionRequired,
                importApplication.defraimp_InspectionRequiredReason,
                importApplication.defraimp_PlaceofOriginid,
                importApplication.defraimp_previousplaceoforiginid,
                importApplication.defraimp_CommodityTypeId,
                importApplication.defraimp_CountryofOriginId,
                importApplication.defraimp_PrimaryITAHCId,
                importApplication.defraimp_PrimaryImporterNotificationId,
                importApplication.statecode,
                importApplication.statuscode,
                importApplication.defraimp_ImportRecordCounted,
                importApplication.defraimp_ManualPostImportCheckDecision);
        }
    }
}
