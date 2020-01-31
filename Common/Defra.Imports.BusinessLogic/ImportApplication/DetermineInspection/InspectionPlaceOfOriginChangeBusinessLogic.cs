using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers;
using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies;
using Defra.Imports.BusinessLogic.ImportApplication.Factories;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public class InspectionPlaceOfOriginChangeBusinessLogic
    {
        private defraimp_importapplication _importApplication;
        private ICrmRepository<defraimp_importapplication> _importApplicationRepo;
        private ICrmRepository<defraimp_inspectioncoveragerule> _coverageRulesRepo;
        private IAutonumberRepository _autoNumberRepo;
        private IPlaceOfOriginRepository _placeOfOriginRepo;
        private IRepositoryFactory _repositoryFactory;
        private ILogWriter _logWriter;
        private DetermineInspectionContext _determineInspectionContext;

        public InspectionPlaceOfOriginChangeBusinessLogic(defraimp_importapplication importApplication, ICrmRepository<defraimp_importapplication> importAppRepo, ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, IAutonumberRepository autonumberRepo, IPlaceOfOriginRepository placeoforiginRepo, IRepositoryFactory repositoryFactory, ILogWriter logWriter)
        {
            _importApplication = importApplication;
            _importApplicationRepo = importAppRepo;
            _coverageRulesRepo = coverageRulesRepo;
            _autoNumberRepo = autonumberRepo;
            _placeOfOriginRepo = placeoforiginRepo;
            _repositoryFactory = repositoryFactory;
            _logWriter = logWriter;

            _determineInspectionContext = new DetermineInspectionContext()
            {
                ImportApplication = _importApplication,
                ImportApplicationRepo = _importApplicationRepo,
                CoverageRulesRepo = _coverageRulesRepo,
                AutoNumberRepo = _autoNumberRepo,
                PlaceOfOriginRepo = _placeOfOriginRepo,
                RepositoryFactory = _repositoryFactory
            };
        }

        public void RunLogic()
        {
            // Record is P1 and subject to Gold/Bronze rule?
            if (_importApplication.defraimp_importrisklevelid != null && _importApplication.defraimp_importrisklevelid.Name.ToLower() == ImportApplicationConstants.P1_RISK_LEVEL_NAME)
            {
                //Do Gold/Bronze rules apply for this record?
                if (CoveredByGoldBronze())
                {
                    defraimp_placeoforigin previousPlaceOfOrigin = GetPreviousPlaceOfOrigin();

                    // Is previous place of origin != null?
                    if (previousPlaceOfOrigin != null)
                    {
                        //If previos Place of Origin Gold?
                        if (previousPlaceOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Gold)
                        {
                            //Were we supposed to inspect the consignment on the import record?
                            if (_importApplication.defraimp_InspectionRequiredReason == defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage)
                            {
                                //Increment the quota counter so that the next record for this place of origin is inspected
                                _placeOfOriginRepo.IncrementQuotaCounter(previousPlaceOfOrigin.Id);
                            }
                        }
                    }
                }

                // For the new record, run inspection logic for P1 path. It wil handle nulls.
                P1Inspection();
            }
        }

        private bool CoveredByGoldBronze()
        {
            // Make sure we have an import application and country of origin
            if (_importApplication.defraimp_CommodityTypeId != null && _importApplication.defraimp_CountryofOriginId != null)
            {
                CommodityHelper commodityHelper = new CommodityHelper(_importApplication.defraimp_CommodityTypeId, _repositoryFactory);

                if (commodityHelper.IsCommodityCoveredByGoldBronze(_importApplication.defraimp_CountryofOriginId))
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

        private void P1Inspection()
        {
            // Get the risk level from the Import Risk Level and then retrieve the correct determine inspection for the risk level
            DetermineInspectionAbstractFatory determineInspectionFactory = new DetermineInspectionFactory();
            AbstractDetermineInspection determineInspection;

            // We already know this path is exclusive to P1, and we only want it to fire if it's P1
            determineInspection = determineInspectionFactory.GetDetermineInspection(ImportApplicationConstants.P1_RISK_LEVEL_NAME);

            if (determineInspection != null)
            {
                // Execute the determine inspection logic for the specific risk level
                determineInspection.ExecuteInspection(_determineInspectionContext);
            }
        }

        private defraimp_placeoforigin GetPreviousPlaceOfOrigin()
        {
            if (_importApplication.defraimp_previousplaceoforiginid != null)
            {
                defraimp_placeoforigin placeOfOrigin = _placeOfOriginRepo.Find(_importApplication.defraimp_previousplaceoforiginid.Id);

                return placeOfOrigin;
            }
            else
            {
                return null;
            }
        }
    }
}
