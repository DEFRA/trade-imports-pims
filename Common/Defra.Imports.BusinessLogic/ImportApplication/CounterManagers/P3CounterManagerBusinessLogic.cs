namespace Defra.Imports.BusinessLogic.ImportApplication
{
    using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
    using Defra.Imports.BusinessLogic.ImportApplication.Factories;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class P3CounterManagerBusinessLogic
    {
        private defraimp_importapplication _preImageImportApplication;
        private defraimp_importapplication _postOperationImportApplication;
        private IAutonumberRepository _autoNumberRepo;
        private ILogWriter _logWriter;

        public P3CounterManagerBusinessLogic(defraimp_importapplication preImageImportApplication, defraimp_importapplication postOperationImportApplication, IAutonumberRepository autoNumberRepo, ILogWriter logWriter)
        {
            _preImageImportApplication = preImageImportApplication;
            _postOperationImportApplication = postOperationImportApplication;
            _autoNumberRepo = autoNumberRepo;
            _logWriter = logWriter;
        }

        public void RunLogic()
        {
            // Update path
            if (_preImageImportApplication != null && _postOperationImportApplication != null)
            {
                if (_postOperationImportApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC)
                {
                    if (_preImageImportApplication.defraimp_PrimaryITAHCId == null && _postOperationImportApplication.defraimp_PrimaryITAHCId != null)
                    {
                        _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.P3_COUNTER_NAME);
                    }
                    else if (_preImageImportApplication.defraimp_PrimaryITAHCId != null && _postOperationImportApplication.defraimp_PrimaryITAHCId == null)
                    {
                        _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.P3_COUNTER_NAME);
                    }
                }
            } // Create Path
            else if (_preImageImportApplication == null && _postOperationImportApplication != null)
            {
                if (_postOperationImportApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC)
                {
                    if (_postOperationImportApplication.defraimp_PrimaryITAHCId != null)
                    {
                        _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.P3_COUNTER_NAME);
                    }
                }
            } // Delete Path
            else if (_preImageImportApplication != null && _postOperationImportApplication == null)
            {
                if (_preImageImportApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC)
                {
                    if (_preImageImportApplication.defraimp_PrimaryITAHCId != null)
                    {
                        _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.P3_COUNTER_NAME);
                    }
                }
            }
        }
    }
}
