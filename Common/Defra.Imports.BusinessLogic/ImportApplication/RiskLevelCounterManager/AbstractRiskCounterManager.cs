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
        protected ICrmRepository<defraimp_inspectioncoveragerule> _coverageRulesRepo;
        protected IAutonumberRepository _autoNumberRepo;
        protected ILogWriter _logWriter;

        public delegate void IncrementNumberDelegate(string reason);
        public event IncrementNumberDelegate IncrementNumberEvent;

        public delegate void DecrementNumberDelegate(string reason);
        public event IncrementNumberDelegate DecrementNumberEvent;

        public delegate void SetNumberDelegate(string reason, int value);
        public event SetNumberDelegate SetNumberEvent;

        public delegate void IncrementQuotaDelegate(string reason);
        public event IncrementNumberDelegate IncrementQuotaEvent;

        public delegate void DecrementQuotaDelegate(string reason);
        public event IncrementNumberDelegate DecrementQuotaEvent;

        public delegate void SetQuotaDelegate(string reason, int value);
        public event SetQuotaDelegate SetQuotaEvent;

        public virtual void IncrementNumber(ref defraimp_importapplication importApplication, string reason)
        {
            // Call the increment number event
            IncrementNumberEvent(reason);

            //Broadcast Stuff
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Increment because: " + reason);
        }

        public virtual void DecrementNumber(ref defraimp_importapplication importApplication, string reason)
        {
            // Call the decrement number event
            DecrementNumberEvent(reason);

            //Broadcast Stuff
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Decrement because: " + reason);
        }
        public virtual void SetNumberValue(ref defraimp_importapplication importApplication, string reason, int value)
        {
            // Call the set number event
            SetNumberEvent(reason, value);

            //Broadcast Stuff
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Set number because: " + reason);
        }

        public virtual void IncrementQuota(ref defraimp_importapplication importApplication, string reason)
        {
            // Call the increment quota event
            IncrementQuotaEvent(reason);

            //Broadcast Stuff
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Increment Quota because: " + reason);
        }

        public virtual void DecrementQuota(ref defraimp_importapplication importApplication, string reason)
        {
            // Call the decrement quota event
            DecrementQuotaEvent(reason);

            //Broadcast Stuff
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Decrement Quota because: " + reason);
        }

        public virtual void SetQuotaValue(ref defraimp_importapplication importApplication, string reason, int value)
        {
            // Call the set quota event
            SetQuotaEvent(reason, value);

            //Broadcast Stuff
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Set Quota because: " + reason);
        }

        protected void SetRecordCounted(ref defraimp_importapplication importApplication, bool counted)
        {
            defraimp_importapplication updatedImportApplication = new defraimp_importapplication();
            updatedImportApplication.Id = importApplication.Id;
            updatedImportApplication.defraimp_ImportRecordCounted = counted;
            _importApplicationRepo.Update(updatedImportApplication);

            //Set the local value to ensure we don't do an operation twice
            importApplication.defraimp_ImportRecordCounted = counted;
        }

    }
}
