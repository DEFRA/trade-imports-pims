namespace Defra.Imports.BusinessLogic.ImportApplication
{
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;

    class PrimaryITAHCCountManagerBusinessLogic
    {
        defraimp_importapplication preImageImportApplication;
        defraimp_importapplication postOperationImportApplication;
        IPlaceOfOriginRepository placeOfOriginRepo;
        ILogWriter logWriter;
        ImportsContext crmContext;

        public PrimaryITAHCCountManagerBusinessLogic(defraimp_importapplication preImageImportApplication, defraimp_importapplication postOperationImportApplication, ImportsContext crmContext, IPlaceOfOriginRepository placeOfOriginRepo, ILogWriter logWriter)
        {
            this.preImageImportApplication = preImageImportApplication;
            this.postOperationImportApplication = postOperationImportApplication;
            this.crmContext = crmContext;
            this.placeOfOriginRepo = placeOfOriginRepo;
            this.logWriter = logWriter;
        }

        public void RunLogic()
        {
            ConfigurationParameterRepository configurationParameterRepository = new ConfigurationParameterRepository(crmContext);

            bool tracesEnabled = bool.Parse(configurationParameterRepository.GetConfigurationParameterValueByKey("defraimp_traces_enabled"));

            // Is traces enabled?
            if (tracesEnabled)
            {
                // Ensure we have a pre-image import application. We won't receive this on create.
                defraimp_placeoforigin preImagePlaceOfOrigin = preImageImportApplication?.defraimp_PlaceofOriginid != null ? placeOfOriginRepo.Find(preImageImportApplication.defraimp_PlaceofOriginid.Id) : null;
                // Check we have a post-operation import application. We should always receive this except on delete.
                defraimp_placeoforigin postOperationPlaceOfOrigin = postOperationImportApplication?.defraimp_PlaceofOriginid != null ? placeOfOriginRepo.Find(postOperationImportApplication.defraimp_PlaceofOriginid.Id) : null;

                // Does both the PreImage and Post Operation Import Application have Places of Origin?
                if (preImagePlaceOfOrigin != null && postOperationPlaceOfOrigin != null)
                {
                    // Did the record have a Health Certificate but no longer has one?
                    if (preImageImportApplication.defraimp_PrimaryITAHCId != null && postOperationImportApplication.defraimp_PrimaryITAHCId == null)
                    {
                        // Decrement the number of Health Certificates
                        placeOfOriginRepo.DecrementHealthCertificateCounter(preImagePlaceOfOrigin.Id);
                    } // Have we added a Health Certificate?
                    else if (preImageImportApplication.defraimp_PrimaryITAHCId == null && postOperationImportApplication.defraimp_PrimaryITAHCId != null)
                    {
                        // increment the counter
                        placeOfOriginRepo.IncrementHealthCertificateCounter(postOperationPlaceOfOrigin.Id);
                    } // Else if both have a Health Certificate
                    else if (preImageImportApplication.defraimp_PrimaryITAHCId != null && postOperationImportApplication.defraimp_PrimaryITAHCId != null)
                    {
                        // Has the Place of Origin changed?
                        if (preImagePlaceOfOrigin.Id != postOperationPlaceOfOrigin.Id)
                        {
                            placeOfOriginRepo.DecrementHealthCertificateCounter(preImagePlaceOfOrigin.Id);
                            placeOfOriginRepo.IncrementHealthCertificateCounter(postOperationPlaceOfOrigin.Id);
                        }
                    }
                } // Have we added a new place of origin on this create/update?
                else if (preImagePlaceOfOrigin == null && postOperationPlaceOfOrigin != null)
                {
                    // Do we currently have a Health Certificate?
                    if (postOperationImportApplication.defraimp_PrimaryITAHCId != null)
                    {
                        // We've added a new place of origin to a record with a valid Health Certificate, increment the counter
                        placeOfOriginRepo.IncrementHealthCertificateCounter(postOperationPlaceOfOrigin.Id);
                    }
                } // Else have removed a place of origin?
                else if (preImagePlaceOfOrigin != null && postOperationPlaceOfOrigin == null)
                {
                    // Did we have a Health Certificate?
                    if (preImageImportApplication.defraimp_PrimaryITAHCId != null)
                    {
                        // We've removed the place of origin and we had a Health Certificate previously, so decrement the counter
                        placeOfOriginRepo.DecrementHealthCertificateCounter(preImagePlaceOfOrigin.Id);
                    }
                }
            }
        }
    }
}
