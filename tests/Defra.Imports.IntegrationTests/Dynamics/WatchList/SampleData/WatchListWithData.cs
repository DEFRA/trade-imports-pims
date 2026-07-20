namespace Defra.Imports.IntegrationTests.Dynamics.WatchList.SampleData
{
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using System;

    public class WatchListWithData
    {
        public WatchListWithData(Guid economicOperatorId, defraimp_watchtype watchType)
        {
            Guid recordId = Guid.NewGuid();

            this.WatchList = new defraimp_WatchList
            {
                Id = recordId,
                defraimp_Name = $"INT TEST Watch List - {recordId}",
                defraimp_WatchType = watchType,
                defraimp_StartDate = DateTime.Now.AddDays(-1),
                defraimp_EndDate = DateTime.Now.AddDays(1),
            };

            this.InjectEconomicOperator(watchType, economicOperatorId);
        }

        private void InjectEconomicOperator(defraimp_watchtype watchType, Guid economicOperatorId)
        {
            switch(watchType)
            {
                case defraimp_watchtype.PlaceofOrigin:
                    this.WatchList.defraimp_PlaceOfOriginId = new EntityReference(defraimp_placeoforigin.EntityLogicalName, economicOperatorId);
                    break;
                case defraimp_watchtype.PlaceofDestination:
                    this.WatchList.defraimp_PlaceOfDestinationId = new EntityReference(defraimp_PlaceOfDestination.EntityLogicalName, economicOperatorId);
                    break;
                case defraimp_watchtype.Consignee:
                    this.WatchList.defraimp_ConsigneeId = new EntityReference(defraimp_Consignee.EntityLogicalName, economicOperatorId);
                    break;
                case defraimp_watchtype.Transporter:
                    this.WatchList.defraimp_TransporterId = new EntityReference(defraimp_Transporter.EntityLogicalName, economicOperatorId);
                    break;
                default:
                    throw new ArgumentException("Invalid watchtype for integration tests");
            }
        }

        public defraimp_WatchList WatchList { get; }
    }
}
