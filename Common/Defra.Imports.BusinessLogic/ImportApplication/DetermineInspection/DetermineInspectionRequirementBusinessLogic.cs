namespace Defra.Imports.BusinessLogic.ImportApplication
{
    using System;
    using System.Linq;
    using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies;
    using Defra.Imports.BusinessLogic.ImportApplication.Factories;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;

    public class DetermineInspectionRequirementBusinessLogic
    {
        private defraimp_importapplication preImageImportApplication;
        private defraimp_importapplication postImageImportApplication;
        private ICrmRepository<defraimp_importapplication> importApplicationRepo;
        private ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo;
        private ICrmRepository<defraimp_importrisklevel> importRiskLevelRepo;
        private IAutonumberRepository autoNumberRepo;
        private IPlaceOfOriginRepository placeOfOriginRepo;
        private IRepositoryFactory repositoryFactory;
        private ILogWriter logWriter;
        private DetermineInspectionContext determineInspectionContext;
        private AbstractRiskCounterManager previousRiskLevelCounterManager;
        private IImportRiskCounterAuditor importRiskCounterAuditor;
        private ICrmRepository<defraimp_counterhistory> counterHistoryRepo;
        private ConfigurationParameterRepository configurationParameterRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DetermineInspectionRequirementBusinessLogic"/> class.
        /// </summary>
        /// <param name="preImageImportApplication">The Import Application image before changes</param>
        /// <param name="postImageImportApplication">The Import Application image after changes</param>
        /// <param name="repositoryFactory">The Repository Factory.</param>
        /// <param name="logWriter">The Log Writer.</param>
        public DetermineInspectionRequirementBusinessLogic(defraimp_importapplication preImageImportApplication, defraimp_importapplication postImageImportApplication, IRepositoryFactory repositoryFactory, ILogWriter logWriter)
        {
            ImportsContext importsContext = new ImportsContext(repositoryFactory.OrganizationService);
            this.repositoryFactory = repositoryFactory;
            this.preImageImportApplication = preImageImportApplication;
            this.postImageImportApplication = postImageImportApplication;
            this.autoNumberRepo = new AutonumberRepository(repositoryFactory.OrganizationService);
            this.placeOfOriginRepo = new PlaceOfOriginRepository(repositoryFactory.OrganizationService);
            this.importApplicationRepo = this.repositoryFactory.GetRepository<ImportsContext, defraimp_importapplication>();
            this.coverageRulesRepo = this.repositoryFactory.GetRepository<ImportsContext, defraimp_inspectioncoveragerule>();
            this.importApplicationRepo = this.repositoryFactory.GetRepository<ImportsContext, defraimp_importapplication>();
            this.importRiskLevelRepo = this.repositoryFactory.GetRepository<ImportsContext, defraimp_importrisklevel>();
            this.counterHistoryRepo = this.repositoryFactory.GetRepository<ImportsContext, defraimp_counterhistory>();
            this.configurationParameterRepo = new ConfigurationParameterRepository(importsContext);
            this.logWriter = logWriter;

            this.determineInspectionContext = new DetermineInspectionContext()
            {
                ImportApplication = this.postImageImportApplication,
                ImportApplicationRepo = this.importApplicationRepo,
                CoverageRulesRepo = this.coverageRulesRepo,
                AutoNumberRepo = this.autoNumberRepo,
                PlaceOfOriginRepo = this.placeOfOriginRepo,
                RepositoryFactory = this.repositoryFactory,
                ConfigurationParameterRepo = this.configurationParameterRepo,
            };
        }

        /// <summary>
        /// Runs the business logic.
        /// </summary>
        public void RunLogic()
        {
            string currentRiskLevel = postImageImportApplication != null && postImageImportApplication?.defraimp_importrisklevelid != null ? postImageImportApplication.defraimp_importrisklevelid.Name : string.Empty;
            string previousRiskLevel = preImageImportApplication != null && preImageImportApplication?.defraimp_importrisklevelid != null ? GetRiskLevelName(preImageImportApplication.defraimp_importrisklevelid.Id) : string.Empty;  // We need this method because the "Name" primary field does not work correctly on a pre-image

            previousRiskLevelCounterManager = preImageImportApplication != null ? SetupRiskLevelCounterManager(preImageImportApplication) : null;
            importRiskCounterAuditor = previousRiskLevelCounterManager != null ? new ImportRiskCounterAuditor(previousRiskLevelCounterManager, counterHistoryRepo) : null;

            // Create (Only post)
            if (preImageImportApplication == null && postImageImportApplication != null)
            {
                DealWithDeterminingInspection();
            } // Update
            else if (preImageImportApplication != null && postImageImportApplication != null)
            {
                // Update of active record
                if (preImageImportApplication.statecode == defraimp_importapplicationState.Active && postImageImportApplication.statecode == defraimp_importapplicationState.Active)
                {
                    //Check if the record has not been overriden for a manual post import check
                    if (postImageImportApplication.defraimp_ManualPostImportCheckDecision == defraimp_importapplication_defraimp_manualpostimportcheckdecision.UseSystemDecision)
                    {
                        ManageRiskLevelChange(previousRiskLevel, currentRiskLevel);

                        if (preImageImportApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC
                            || preImageImportApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHCLandbridge)
                        {
                            ManageITAHCRemoval(previousRiskLevel);
                        }
                        else if (preImageImportApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ImportNotification)
                        {
                            ManageImporterNotificationRemoval(previousRiskLevel);
                        }

                        ManagePlaceOfOriginChange(previousRiskLevel);
                        DealWithDeterminingInspection();
                    }
                } // Deactivate when the state was previously active but has been moved to a status of inactive (Note that we use state and statuscode here as we don't want this logic to run on Application Completion status reason)
                else if (preImageImportApplication.statecode == defraimp_importapplicationState.Active && postImageImportApplication.statuscode == defraimp_importapplication_statuscode.Cancelled)
                {
                    ManageRecordDeactivation(currentRiskLevel);
                }
                else if (preImageImportApplication.statecode == defraimp_importapplicationState.Active && postImageImportApplication.statuscode == defraimp_importapplication_statuscode.NoITAHCReceived)
                {
                    ManageRecordDeactivation(currentRiskLevel);
                }
                else if (preImageImportApplication.statuscode == defraimp_importapplication_statuscode.Cancelled && postImageImportApplication.statecode == defraimp_importapplicationState.Active)
                {
                    ManageRecordReactivation();
                }
                else if (preImageImportApplication.statuscode == defraimp_importapplication_statuscode.NoITAHCReceived && postImageImportApplication.statecode == defraimp_importapplicationState.Active)
                {
                    ManageRecordReactivation();
                }
            }
            else if (preImageImportApplication != null && postImageImportApplication == null)
            {
                ManageRecordDeletion(previousRiskLevel);
            }
        }

        AbstractRiskCounterManager SetupRiskLevelCounterManager(defraimp_importapplication importApplication)
        {
            if (importApplication != null)
            {
                string riskLevel = importApplication?.defraimp_importrisklevelid != null ? riskLevel = GetRiskLevelName(importApplication.defraimp_importrisklevelid.Id) : string.Empty;

                if (!string.IsNullOrEmpty(riskLevel))
                {
                    // Does this record work with Gold/Bronze
                    if (CommodityCoveredByGoldBronze(importApplication))
                    {
                        return new PlaceOfOriginRiskLevelCounterManager(importApplicationRepo, autoNumberRepo, importApplication, placeOfOriginRepo, coverageRulesRepo, logWriter);
                    }
                    else if (riskLevel.ToLower() == ImportApplicationConstants.TB_RISK_LEVEL_NAME)
                    {
                        return new OnlyGlobalCounterRiskLevelManager(importApplicationRepo, autoNumberRepo, riskLevel, coverageRulesRepo, logWriter);
                    }
                    else
                    {
                        return new AutonumberRiskCounterManager(importApplicationRepo, autoNumberRepo, riskLevel, coverageRulesRepo, logWriter);

                    }
                }
            }

            return null; // If we make it here, we should return null.
        }

        void ManageRiskLevelChange(string previousRiskLevel, string currentRiskLevel)
        {
            if (previousRiskLevel != currentRiskLevel)
            {
                // Make sure the previous risk level was not empty
                if (!string.IsNullOrEmpty(previousRiskLevel))
                {
                    if (CommodityCoveredByGoldBronze(preImageImportApplication))
                    {
                        // Does the pre image have a place of origin?
                        if (preImageImportApplication.defraimp_PlaceofOriginid != null)
                        {
                            // Get the Place of Origin
                            defraimp_placeoforigin placeOfOrigin = placeOfOriginRepo.Find(preImageImportApplication.defraimp_PlaceofOriginid.Id);

                            //If previous Place of Origin Gold?
                            if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Gold)
                            {
                                // Manage the counts for the place of origin record we're replacing
                                previousRiskLevelCounterManager.DecrementNumber(ref preImageImportApplication, defraimp_counterhistory_defraimp_reason.RiskLevelChangedRemoved);
                                postImageImportApplication.defraimp_ImportRecordCounted = false; // Update the local copy of the post image to reflect the change to the count state
                            }
                            else if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Bronze)
                            {
                                // We need to decrement the global counter
                                previousRiskLevelCounterManager.DecrementGlobalCounter(preImageImportApplication);
                                postImageImportApplication.defraimp_ImportRecordCounted = false; // Update the local copy of the post image to reflect the change to the count state
                            }
                        }
                    }
                    else
                    {
                        previousRiskLevelCounterManager.DecrementNumber(ref preImageImportApplication, defraimp_counterhistory_defraimp_reason.RiskLevelChangedRemoved);
                        postImageImportApplication.defraimp_ImportRecordCounted = false; // Update the local copy of the post image to reflect the change to the count state
                    }
                }
                else
                {
                    logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Null string");
                }
            }
        }

        void ManageITAHCRemoval(string previousRiskLevel)
        {
            if (preImageImportApplication.defraimp_PrimaryITAHCId != null && postImageImportApplication.defraimp_PrimaryITAHCId == null)
            {
                // Make sure the previous risk level was not empty
                if (!string.IsNullOrEmpty(previousRiskLevel))
                {
                    if (CommodityCoveredByGoldBronze(preImageImportApplication))
                    {
                        // Does the pre image have a place of origin?
                        if (preImageImportApplication.defraimp_PlaceofOriginid != null)
                        {
                            // Get the Place of Origin
                            defraimp_placeoforigin placeOfOrigin = placeOfOriginRepo.Find(preImageImportApplication.defraimp_PlaceofOriginid.Id);

                            //If previous Place of Origin Gold?
                            if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Gold)
                            {
                                // Manage the counts for the place of origin record we're replacing
                                previousRiskLevelCounterManager.DecrementNumber(ref preImageImportApplication, defraimp_counterhistory_defraimp_reason.ITAHCRemoved);
                                postImageImportApplication.defraimp_ImportRecordCounted = false; // Update the local copy of the post image to reflect the change to the count state
                            }
                            else if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Bronze)
                            {
                                // We need to decrement the global counter
                                previousRiskLevelCounterManager.DecrementGlobalCounter(preImageImportApplication);
                                postImageImportApplication.defraimp_ImportRecordCounted = false; // Update the local copy of the post image to reflect the change to the count state
                            }
                        }
                    }
                    else
                    {
                        // Manage the counts for the place of origin record we're replacing
                        previousRiskLevelCounterManager.DecrementNumber(ref preImageImportApplication, defraimp_counterhistory_defraimp_reason.ITAHCRemoved);
                        postImageImportApplication.defraimp_ImportRecordCounted = false; // Update the local copy of the post image to reflect the change to the count state
                    }
                }
            }
        }

        void ManageImporterNotificationRemoval(string previousRiskLevel)
        {
            if (preImageImportApplication.defraimp_PrimaryImporterNotificationId != null && postImageImportApplication.defraimp_PrimaryImporterNotificationId == null)
            {
                // Make sure the previous risk level was not empty
                if (!string.IsNullOrEmpty(previousRiskLevel))
                {
                    if (CommodityCoveredByGoldBronze(preImageImportApplication))
                    {
                        // Does the pre image have a place of origin?
                        if (preImageImportApplication.defraimp_PlaceofOriginid != null)
                        {
                            // Get the Place of Origin
                            defraimp_placeoforigin placeOfOrigin = placeOfOriginRepo.Find(preImageImportApplication.defraimp_PlaceofOriginid.Id);

                            //If previous Place of Origin Gold?
                            if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Gold)
                            {
                                // Manage the counts for the place of origin record we're replacing
                                previousRiskLevelCounterManager.DecrementNumber(ref preImageImportApplication, defraimp_counterhistory_defraimp_reason.ImporterNotificationRemoved);
                                postImageImportApplication.defraimp_ImportRecordCounted = false; // Update the local copy of the post image to reflect the change to the count state
                            }
                            else if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Bronze)
                            {
                                // We need to decrement the global counter
                                previousRiskLevelCounterManager.DecrementGlobalCounter(preImageImportApplication);
                                postImageImportApplication.defraimp_ImportRecordCounted = false; // Update the local copy of the post image to reflect the change to the count state
                            }
                        }
                    }
                    else
                    {
                        // Manage the counts for the place of origin record we're replacing
                        previousRiskLevelCounterManager.DecrementNumber(ref preImageImportApplication, defraimp_counterhistory_defraimp_reason.ImporterNotificationRemoved);
                        postImageImportApplication.defraimp_ImportRecordCounted = false; // Update the local copy of the post image to reflect the change to the count state
                    }
                }
            }
        }

        void ManagePlaceOfOriginChange(string previousRiskLevel)
        {
            // Record is P1 and subject to Gold/Bronze rule?
            if (!string.IsNullOrEmpty(previousRiskLevel) && previousRiskLevel.ToLower() == ImportApplicationConstants.P1_RISK_LEVEL_NAME)
            {
                // Was the commodity a Gold/Bronze commodity
                if (CommodityCoveredByGoldBronze(preImageImportApplication))
                {
                    // Was there previously a place of origin?
                    if (preImageImportApplication.defraimp_PlaceofOriginid != null)
                    {
                        // Has the place of origin changed?
                        if (preImageImportApplication.defraimp_PlaceofOriginid != postImageImportApplication.defraimp_PlaceofOriginid)
                        {
                            // Get the Place of Origin
                            defraimp_placeoforigin placeOfOrigin = placeOfOriginRepo.Find(preImageImportApplication.defraimp_PlaceofOriginid.Id);

                            //If previous Place of Origin Gold?
                            if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Gold)
                            {
                                // Manage the counts for the place of origin record we're replacing
                                previousRiskLevelCounterManager.DecrementNumber(ref preImageImportApplication, defraimp_counterhistory_defraimp_reason.GBPlaceOfOriginRemoved);
                                postImageImportApplication.defraimp_ImportRecordCounted = false; // Update the local copy of the post image to reflect the change to the count state
                            }
                            else if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Bronze)
                            {
                                // We need to decrement the global counter
                                previousRiskLevelCounterManager.DecrementGlobalCounter(preImageImportApplication);
                                postImageImportApplication.defraimp_ImportRecordCounted = false; // Update the local copy of the post image to reflect the change to the count state
                            }
                        }
                    }
                }
            }
        }

        private void ManageRecordDeactivation(string riskLevel)
        {
            previousRiskLevelCounterManager.DecrementNumber(ref preImageImportApplication, defraimp_counterhistory_defraimp_reason.RecordDeactivated);
        }

        private void ManageRecordDeletion(string riskLevel)
        {
            previousRiskLevelCounterManager.DecrementNumber(ref preImageImportApplication, defraimp_counterhistory_defraimp_reason.RecordDeactivated);
        }

        private void ManageRecordReactivation()
        {
            DealWithDeterminingInspection();
        }

        private bool CommodityCoveredByGoldBronze(defraimp_importapplication importApplication)
        {
            // Make sure we have an import application and country of origin
            if (importApplication.defraimp_CommodityTypeId != null && importApplication.defraimp_CountryofOriginId != null)
            {
                CommodityHelper commodityHelper = new CommodityHelper(importApplication.defraimp_CommodityTypeId, repositoryFactory);

                if (commodityHelper.IsCommodityCoveredByGoldBronze(importApplication.defraimp_CountryofOriginId))
                {
                    return true;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        private void DealWithDeterminingInspection()
        {
            // Set up counter manager to manage incrementing. We need seperate counter managers to support a change in risk levels. 
            // We put this in the determine inspection context for risk strategies to use, and set it up here so it can capture any prior changes made by the previous counter
            determineInspectionContext.RiskLevelCounterManager = SetupRiskLevelCounterManager(postImageImportApplication);

            // Set up an auditor for the counter manager
            if (determineInspectionContext.RiskLevelCounterManager != null)
            {
                determineInspectionContext.ImportRiskCounterAuditor = new ImportRiskCounterAuditor(determineInspectionContext.RiskLevelCounterManager, counterHistoryRepo);
            }

            // Get the risk level from the Import Risk Level and then retrieve the correct determine inspection for the risk level
            DetermineInspectionAbstractFatory determineInspectionFactory = new DetermineInspectionFactory();
            AbstractDetermineInspection determineInspection;

            // Make sure we have a risk level as we need to access the name, otherwise pass in an empty string
            string riskLevel = postImageImportApplication?.defraimp_importrisklevelid != null ? postImageImportApplication.defraimp_importrisklevelid.Name : string.Empty;
            determineInspection = determineInspectionFactory.GetDetermineInspection(riskLevel);

            if (determineInspection != null)
            {
                // Execute the determine inspection logic for the specific risk level
                determineInspection.ExecuteInspection(determineInspectionContext);
            }
        }

        private string GetRiskLevelName(Guid riskLevelId)
        {
            defraimp_importrisklevel previousRiskLevelId = importRiskLevelRepo.Find<defraimp_importrisklevel>(
            rule => rule.Id.Equals(riskLevelId),
            e => new defraimp_importrisklevel()
            {
                defraimp_name = e.defraimp_name
            }
            ).FirstOrDefault();

            return previousRiskLevelId != null ? previousRiskLevelId.defraimp_name : string.Empty;
        }
    }
}
