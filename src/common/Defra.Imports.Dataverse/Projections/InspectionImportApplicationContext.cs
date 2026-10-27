namespace Defra.Imports.Dataverse.Projections
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// The values of an import application that are needed to determine its inspection requirement. Use
    /// <see cref="Defra.Imports.Dataverse.Extensions.ImportApplicationExtensions.ToInspectionContext(Defra.Imports.Dataverse.Model.defraimp_importapplication)"/>
    /// to project an import application onto it.
    /// </summary>
    public sealed class InspectionImportApplicationContext
    {
        /// <summary>Initializes a new instance of the <see cref="InspectionImportApplicationContext"/> class.</summary>
        /// <param name="id">The identifier of the import application.</param>
        /// <param name="importApplicationType">The type of the import application, if any.</param>
        /// <param name="importRiskLevel">The import risk level, if any.</param>
        /// <param name="inspectionRequired">Whether an inspection is required, if known.</param>
        /// <param name="inspectionRequiredReason">The reason that an inspection is or is not required, if known.</param>
        /// <param name="placeOfOrigin">The place of origin, if any.</param>
        /// <param name="previousPlaceOfOrigin">The previous place of origin, if any.</param>
        /// <param name="commodityType">The commodity type, if any.</param>
        /// <param name="countryOfOrigin">The country of origin, if any.</param>
        /// <param name="primaryItahc">The primary ITAHC, if any.</param>
        /// <param name="primaryImporterNotification">The primary importer notification, if any.</param>
        /// <param name="stateCode">The state of the import application, if known.</param>
        /// <param name="statusCode">The status of the import application, if known.</param>
        /// <param name="importRecordCounted">Whether the import application has been counted, if known.</param>
        /// <param name="manualPostImportCheckDecision">The manual post import check decision, if any.</param>
        public InspectionImportApplicationContext(
            Guid id,
            defraimp_importapplication_defraimp_importapplicationtype? importApplicationType,
            EntityReference importRiskLevel,
            defraimp_importapplication_defraimp_inspectionrequired? inspectionRequired,
            defraimp_importapplication_defraimp_inspectionrequiredreason? inspectionRequiredReason,
            EntityReference placeOfOrigin,
            EntityReference previousPlaceOfOrigin,
            EntityReference commodityType,
            EntityReference countryOfOrigin,
            EntityReference primaryItahc,
            EntityReference primaryImporterNotification,
            defraimp_importapplication_statecode? stateCode,
            defraimp_importapplication_statuscode? statusCode,
            bool? importRecordCounted,
            defraimp_importapplication_defraimp_manualpostimportcheckdecision? manualPostImportCheckDecision)
        {
            this.Id = id;
            this.ImportApplicationType = importApplicationType;
            this.ImportRiskLevel = importRiskLevel;
            this.InspectionRequired = inspectionRequired;
            this.InspectionRequiredReason = inspectionRequiredReason;
            this.PlaceOfOrigin = placeOfOrigin;
            this.PreviousPlaceOfOrigin = previousPlaceOfOrigin;
            this.CommodityType = commodityType;
            this.CountryOfOrigin = countryOfOrigin;
            this.PrimaryItahc = primaryItahc;
            this.PrimaryImporterNotification = primaryImporterNotification;
            this.StateCode = stateCode;
            this.StatusCode = statusCode;
            this.ImportRecordCounted = importRecordCounted;
            this.ManualPostImportCheckDecision = manualPostImportCheckDecision;
        }

        /// <summary>Gets the identifier of the import application.</summary>
        public Guid Id { get; }

        /// <summary>Gets the type of the import application.</summary>
        public defraimp_importapplication_defraimp_importapplicationtype? ImportApplicationType { get; }

        /// <summary>Gets the import risk level, whose name is populated only if the source held it.</summary>
        public EntityReference ImportRiskLevel { get; }

        /// <summary>Gets whether an inspection is required.</summary>
        public defraimp_importapplication_defraimp_inspectionrequired? InspectionRequired { get; }

        /// <summary>Gets the reason that an inspection is or is not required.</summary>
        public defraimp_importapplication_defraimp_inspectionrequiredreason? InspectionRequiredReason { get; }

        /// <summary>Gets the place of origin.</summary>
        public EntityReference PlaceOfOrigin { get; }

        /// <summary>Gets the previous place of origin.</summary>
        public EntityReference PreviousPlaceOfOrigin { get; }

        /// <summary>Gets the commodity type.</summary>
        public EntityReference CommodityType { get; }

        /// <summary>Gets the country of origin.</summary>
        public EntityReference CountryOfOrigin { get; }

        /// <summary>Gets the primary ITAHC.</summary>
        public EntityReference PrimaryItahc { get; }

        /// <summary>Gets the primary importer notification.</summary>
        public EntityReference PrimaryImporterNotification { get; }

        /// <summary>Gets the state of the import application.</summary>
        public defraimp_importapplication_statecode? StateCode { get; }

        /// <summary>Gets the status of the import application.</summary>
        public defraimp_importapplication_statuscode? StatusCode { get; }

        /// <summary>Gets whether the import application has been counted.</summary>
        public bool? ImportRecordCounted { get; }

        /// <summary>Gets the manual post import check decision.</summary>
        public defraimp_importapplication_defraimp_manualpostimportcheckdecision? ManualPostImportCheckDecision { get; }
    }
}
