using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace Defra.Imports.Xrm.Workflows
{
    public class RandomRiskAssessment : WorkFlowActivityBase
    {
        #region Input/Output

        [Input("Import Application")]
        [ReferenceTarget("defraimp_importapplication")]
        public InArgument<EntityReference> ImportApplication { get; set; }

        #endregion Input/Output

        string autonumberKey = "defraimp_ImportApplicationRecordCount"; //Key for the Import Application Record Count defraimp_autonumber record
        string configParameterKey = "defraimp_inspectionCoverageCount"; //Key for the Inspection Coverage Count defraexp_configparameter record

        public override void ExecuteCRMWorkFlowActivity(CodeActivityContext executionContext, LocalWorkflowContext crmWorkflowContext)
        {
            if (executionContext == null)
            {
                throw new ArgumentNullException("crmWorkflowContext");
            }

            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            var tracingService = crmWorkflowContext.TracingService;
            tracingService.Trace(string.Format("{0} - RandomRiskAssessment execution started", DateTime.Now));

            try
            {
                //Implement logic
                var crmContext = new ImportsContext(service);
                EntityReference importApplicationReference = ImportApplication.Get(executionContext); //Get the import application we're working with
                defraimp_importapplication importApplication = GetImportApplication(importApplicationReference.Id,service,tracingService);

                EntityReference p1Reference = GetP1RecordReference(service, tracingService); //Get a reference to the P1 Import Risk Level record


                //Check application is not P1. If it is not then
                if (importApplication.defraimp_importrisklevelid != p1Reference)
                {
                    //Get current count of autonumber
                    int currentNumberOfRecords = GetCurrentNumberOfRecords(service, tracingService);

                    //Get the inspection coverage count
                    int inspectionCoverageCount = GetInspectionCoverageCount(crmContext);

                    //Check if the current number of records is greater than the inspection coverage count
                    if (currentNumberOfRecords >= inspectionCoverageCount)
                    {
                        //Set record to P1 and set risk level status of record to "Random assessment"
                        importApplication.defraimp_importrisklevelid = p1Reference;
                        importApplication.defraimp_ImportRiskLevelStatus = defraimp_importapplication_defraimp_importrisklevelstatus.RandomInspection;
                        service.Update(importApplication);
                        //Reset autonumber to 0
                        ResetRecordCount(service, tracingService);
                    }
                }
            }
            catch (Exception e)
            {
                tracingService.Trace(string.Format("{0} - Unhandled exception raised: {1}", DateTime.Now, e));
            }
        }

        int GetCurrentNumberOfRecords(IOrganizationService service, ITracingService tracingService)
        {
            AutonumberRepository autonumberRepository = new AutonumberRepository(service);
            return autonumberRepository.GetAutonumberValue(autonumberKey);
        }

        void ResetRecordCount(IOrganizationService service, ITracingService tracingService)
        {
            AutonumberRepository autonumberRepository = new AutonumberRepository(service);
            autonumberRepository.SetAutonumberValue(autonumberKey, 0);
        }

        int GetInspectionCoverageCount(ImportsContext crmContext)
        {
            //Get config parameter
            ConfigurationParameterRepository configRepo = new ConfigurationParameterRepository(crmContext);
            string value = configRepo.GetConfigurationParameterValueByKey(configParameterKey);
            int inspectionCoverageCount;
            bool parseSuccessful = int.TryParse(value, out inspectionCoverageCount);

            if (parseSuccessful)
            {
                return inspectionCoverageCount;
            }
            else
            {
                throw new InvalidWorkflowException("Please ensure the 'Inspection Coverage Count' value is a number");
            }
        }

        EntityReference GetP1RecordReference(IOrganizationService service, ITracingService tracingService)
        {
            ImportRiskLevelRepository importRiskLevelRepo = new ImportRiskLevelRepository(service, tracingService);
            defraimp_importrisklevel importRiskLevel = importRiskLevelRepo.GetRiskLevelByName("P1");
            return new EntityReference(importRiskLevel.LogicalName, importRiskLevel.Id);
        }

        defraimp_importapplication GetImportApplication(Guid id, IOrganizationService service, ITracingService tracingService)
        {
            ImportApplicationRepository importApplicationRepo = new ImportApplicationRepository(service, tracingService);
            ColumnSet columnSet = new ColumnSet(new string[] {"defraimp_importrisklevelid"} );
            return importApplicationRepo.GetImportApplicationWithID(id, columnSet);
        }
    }
}
