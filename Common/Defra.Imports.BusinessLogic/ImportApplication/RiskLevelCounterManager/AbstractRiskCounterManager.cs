namespace Defra.Imports.BusinessLogic.ImportApplication
{
    using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
    using Defra.Imports.BusinessLogic.ImportApplication.Factories;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    public abstract class AbstractRiskCounterManager : IRiskLevelCounterManager
    {
        protected ICrmRepository<defraimp_importapplication> _importApplicationRepo;
        protected defraimp_importapplication _importApplication;
        protected ICrmRepository<defraimp_inspectioncoveragerule> _coverageRulesRepo;

        public virtual void IncrementNumber(string reason)
        {
            //Broadcast Stuff
        }

        public virtual void DecrementNumber(string reason)
        {
            //Broadcast Stuff
        }
        public virtual void SetNumberValue(string reason, int value)
        {
            //Broadcast Stuff
        }

        public virtual void IncrementQuota(string reason)
        {
            //Broadcast Stuff
        }

        public virtual void DecrementQuota(string reason)
        {
            //Broadcast Stuff
        }

        public virtual void SetQuotaValue(string reason, int value)
        {
            //Broadcast Stuff
        }

        protected void SetRecordCounted(bool counted)
        {
            defraimp_importapplication updatedImportApplication = new defraimp_importapplication();
            updatedImportApplication.Id = _importApplication.Id;
            updatedImportApplication.defraimp_ImportRecordCounted = counted;
            _importApplicationRepo.Update(updatedImportApplication);

            //Set the local value to ensure we don't do an operation twice
            _importApplication.defraimp_ImportRecordCounted = counted;
        }

    }
}
