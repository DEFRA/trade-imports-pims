namespace Defra.Imports.BusinessLogic.ImporterNotification.FlagRecordsOnWatchList
{
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class FlagRecordsOnWatchListBase
    {
        private IOrganizationService orgService;
        private Entity entityFromContext;
        private ICrmRepository<defraimp_WatchList> watchListRepo;
        private ICrmRepository<defraimp_WatchFlag> watchFlagRepo;

        public FlagRecordsOnWatchListBase(IOrganizationService orgService, Entity entityFromContext, ICrmRepository<defraimp_WatchList> watchListRepo, ICrmRepository<defraimp_WatchFlag> watchFlagRepo)
        {
            this.orgService = orgService;
            this.entityFromContext = entityFromContext;
            this.watchListRepo = watchListRepo;
            this.watchFlagRepo = watchFlagRepo;
        }

        public abstract void FlagRecordIfOnWatchList();

        protected void CheckAndFlagRecordsForParameters(List<FlagRecordParams> flagRecordParamsList)
        {
            DateTime currentDate = DateTime.Now;
            List<defraimp_WatchList> watchRecords = this.GetWatchRecordsForDate(currentDate.Date);
            flagRecordParamsList.ForEach(flagRecordParams => CheckIdentifierAndFlagWatchRecord(flagRecordParams, watchRecords));
        }

        private void CheckIdentifierAndFlagWatchRecord(FlagRecordParams flagRecordParams, List<defraimp_WatchList> watchListRecords)
        {
            if (!string.IsNullOrEmpty(flagRecordParams.Identifier))
            {
                this.CheckAndAddFlag(watchListRecords, flagRecordParams.WatchType, flagRecordParams.WatchListLookupName, flagRecordParams.EconomicOperatorEntityLogicalName, flagRecordParams.EconomicOperatorSearchFieldName, flagRecordParams.Identifier);
            }
        }

        private List<defraimp_WatchList> GetWatchRecordsForDate(DateTime dateTime)
        {
            return this.watchListRepo.Find(
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
                    defraimp_PlaceOfOriginId = e.defraimp_PlaceOfOriginId,
                    defraimp_PlaceOfDestinationId = e.defraimp_PlaceOfDestinationId,
                    defraimp_ConsigneeId = e.defraimp_ConsigneeId,
                    defraimp_TransporterId = e.defraimp_TransporterId,
                    defraimp_VeterinarianId = e.defraimp_VeterinarianId
                }).ToList();
        }

        private void CheckAndAddFlag(List<defraimp_WatchList> watchRecords, defraimp_watchtype watchType, string watchEconomicOperatorAttributeName, string economicOperatorEntityName, string attributeName, object attributeValue)
        {
            // Filter the previously retrieved watch records by the type
            List<defraimp_WatchList> filteredWatchRecords = watchRecords.Where(e =>
                e.defraimp_WatchType == watchType &&
                e.Attributes.Contains(watchEconomicOperatorAttributeName) &&
                e[watchEconomicOperatorAttributeName] != null).ToList();

            if (filteredWatchRecords.Count > 0)
            {
                EntityCollection entitiesMatchingAttributeValue = this.RetrieveEconomicOperator(this.orgService, economicOperatorEntityName, attributeName, attributeValue, new string[] { $"{economicOperatorEntityName}id" });
                if (entitiesMatchingAttributeValue.Entities.Count > 0)
                {
                    Entity foundEconomicOperator = entitiesMatchingAttributeValue.Entities.First();

                    // Check if the retrieved entity is linked to one of the filtered watch records
                    defraimp_WatchList foundWatchRecord = filteredWatchRecords.FirstOrDefault(e => ((EntityReference)e[watchEconomicOperatorAttributeName]).Id == foundEconomicOperator.Id);

                    if (foundWatchRecord != null)
                    {
                        this.CreateWatchFlag(foundWatchRecord);
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

        private void CreateWatchFlag(defraimp_WatchList watchList)
        {
            defraimp_WatchFlag watchFlag = null;

            switch (this.entityFromContext.LogicalName)
            {
                case defraimp_itahc.EntityLogicalName:
                    watchFlag = new defraimp_WatchFlag()
                    {
                        defraimp_Name = $"{watchList.defraimp_Name} - {this.entityFromContext.GetAttributeValue<string>("defraimp_name")}",
                        defraimp_ItahcId = this.entityFromContext.ToEntityReference(),
                        defraimp_WatchListId = watchList.ToEntityReference()
                    };
                    break;
                case defraimp_ImporterNotification.EntityLogicalName:
                    watchFlag = new defraimp_WatchFlag()
                    {
                        defraimp_Name = $"{watchList.defraimp_Name} - {this.entityFromContext.GetAttributeValue<string>("defraimp_name")}",
                        defraimp_ImporterNotificationId = this.entityFromContext.ToEntityReference(),
                        defraimp_WatchListId = watchList.ToEntityReference()
                    };
                    break;
            }

            this.watchFlagRepo.Create(watchFlag);
        }
    }
}
