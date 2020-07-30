using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defra.Imports.BusinessLogic.Itahc
{
    public class FlagRecordOnWatchListBusinessLogic
    {
        private const string WATCH_DESTINATION_LOOKUP_FIELD_NAME = "defraimp_placeofdestinationid";
        private const string WATCH_CONSIGNEE_LOOKUP_FIELD_NAME = "defraimp_consigneeid";
        private const string WATCH_TRANSPORTER__LOOKUP_FIELD_NAME = "defraimp_transporterid";
        private const string WATCH_VETERINARIAN_LOOKUP_FIELD_NAME = "defraimp_veterinarianid";

        private const string TRADER_SEARCH_FIELD_NAME = "defraimp_name";

        private defraimp_itahc _itahcFromContext;
        private IOrganizationService _orgSvc;
        private ICrmRepository<defraimp_WatchList> _watchListRepo;
        private ICrmRepository<defraimp_WatchFlag> _watchFlagRepo;

        public FlagRecordOnWatchListBusinessLogic(IOrganizationService orgSvc, defraimp_itahc itahcFromContext, ICrmRepository<defraimp_WatchList> watchListRepo, ICrmRepository<defraimp_WatchFlag> watchFlagRepo)
        {
            this._itahcFromContext = itahcFromContext;
            this._orgSvc = orgSvc;
            this._watchListRepo = watchListRepo;
            this._watchFlagRepo = watchFlagRepo;
        }

        public void FlagRecordIfOnWatchList()
        {
            string itahcDestinationIdentifier = _itahcFromContext.defraimp_PlaceOfDestinationName;
            string itahcConsigneeIdentifier = _itahcFromContext.defraimp_ConsigneeName;
            string itahcTransporterIdentifier = _itahcFromContext.defraimp_TransporterName;
            string itahcVeterinarianIdentifier = _itahcFromContext.defraimp_OVName;

            DateTime currentDate = DateTime.Now;

            List<defraimp_WatchList> watchRecords = GetWatchRecordsForDate(currentDate.Date);

            if (!String.IsNullOrEmpty(itahcDestinationIdentifier))
            {
                CheckAndAddFlag(watchRecords, defraimp_watchtype.PlaceofDestination, WATCH_DESTINATION_LOOKUP_FIELD_NAME, "defraimp_placeofdestination", TRADER_SEARCH_FIELD_NAME, itahcDestinationIdentifier);
            }

            if (!String.IsNullOrEmpty(itahcConsigneeIdentifier))
            {
                CheckAndAddFlag(watchRecords, defraimp_watchtype.Consignee, WATCH_CONSIGNEE_LOOKUP_FIELD_NAME, "defraimp_consignee", TRADER_SEARCH_FIELD_NAME, itahcConsigneeIdentifier);
            }

            if (!String.IsNullOrEmpty(itahcTransporterIdentifier))
            {
                CheckAndAddFlag(watchRecords, defraimp_watchtype.Transporter, WATCH_TRANSPORTER__LOOKUP_FIELD_NAME, "defraimp_transporter", TRADER_SEARCH_FIELD_NAME, itahcTransporterIdentifier);
            }

            if(!String.IsNullOrEmpty(itahcVeterinarianIdentifier))
            {
                CheckAndAddFlag(watchRecords, defraimp_watchtype.Veterinarian, WATCH_VETERINARIAN_LOOKUP_FIELD_NAME, "defraimp_veterinarian", TRADER_SEARCH_FIELD_NAME, itahcVeterinarianIdentifier);
            }
        }

        private List<defraimp_WatchList> GetWatchRecordsForDate(DateTime dateTime)
        {
            return _watchListRepo.Find(
                e => (
                    e.statecode == 0 &&
                    e.defraimp_StartDate <= dateTime && (e.defraimp_EndDate >= dateTime || e.defraimp_EndDate == null)
                ),
                e => new defraimp_WatchList()
                {
                    defraimp_WatchListId = e.defraimp_WatchListId,
                    defraimp_Name = e.defraimp_Name,
                    defraimp_StartDate = e.defraimp_StartDate,
                    defraimp_EndDate = e.defraimp_EndDate,
                    defraimp_WatchType = e.defraimp_WatchType,
                    defraimp_PlaceOfDestinationId = e.defraimp_PlaceOfDestinationId,
                    defraimp_ConsigneeId = e.defraimp_ConsigneeId,
                    defraimp_TransporterId = e.defraimp_TransporterId,
                    defraimp_VeterinarianId = e.defraimp_VeterinarianId
                }).ToList();
        }

        private void CheckAndAddFlag(List<defraimp_WatchList> watchRecords, defraimp_watchtype watchType, string watchEconomicOperatorAttributeName, string economicOperatorEntityName,  string attributeName, object attributeValue)
        {
            // Filter the previously retrieved watch records by the type
            List<defraimp_WatchList> filteredWatchRecords = watchRecords.Where(e => 
                e.defraimp_WatchType == watchType &&
                e.Attributes.Contains(watchEconomicOperatorAttributeName) &&
                e[watchEconomicOperatorAttributeName] != null).ToList();

            if (filteredWatchRecords.Count > 0)
            {
                EntityCollection entitiesMatchingAttributeValue = RetrieveEconomicOperator(_orgSvc, economicOperatorEntityName, attributeName, attributeValue, new string[] { $"{economicOperatorEntityName}id" });
                if (entitiesMatchingAttributeValue.Entities.Count > 0)
                {
                    Entity foundEconomicOperator = entitiesMatchingAttributeValue.Entities.First();

                    // Check if the retrieved entity is linked to one of the filtered watch records
                    defraimp_WatchList foundWatchRecord = filteredWatchRecords.FirstOrDefault(e => ((EntityReference)e[watchEconomicOperatorAttributeName]).Id == foundEconomicOperator.Id);

                    if (foundWatchRecord != null)
                    {
                        CreateWatchFlag(_watchFlagRepo, _itahcFromContext, foundWatchRecord);
                    }
                }
            }
        }

        private EntityCollection RetrieveEconomicOperator(IOrganizationService orgSvc, string economicOperatorEntityName, string filterAttributeName, object filterAttributeValue, string[] columns)
        {
            QueryExpression qe = new QueryExpression(economicOperatorEntityName);
            qe.Criteria.AddCondition(new ConditionExpression(filterAttributeName, ConditionOperator.Equal, filterAttributeValue));
            qe.ColumnSet = new ColumnSet(columns);
            EntityCollection entityCol = orgSvc.RetrieveMultiple(qe);
            return entityCol;
        }

        private void CreateWatchFlag(ICrmRepository<defraimp_WatchFlag> watchFlagRepo, defraimp_itahc itahc, defraimp_WatchList watchList)
        {
            defraimp_WatchFlag watchFlag = new defraimp_WatchFlag()
            {
                defraimp_Name = $"{watchList.defraimp_Name} - {itahc.defraimp_name}",
                defraimp_ItahcId = itahc.ToEntityReference(),
                defraimp_WatchListId = watchList.ToEntityReference()
            };
            watchFlagRepo.Create(watchFlag);
        }
    }
}
