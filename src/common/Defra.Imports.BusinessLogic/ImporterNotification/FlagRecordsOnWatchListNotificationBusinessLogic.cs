using Defra.Imports.BusinessLogic.ImporterNotification.FlagRecordsOnWatchList;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace Defra.Imports.BusinessLogic.ImporterNotification
{
    public class FlagRecordsOnWatchListNotificationBusinessLogic
        : FlagRecordsOnWatchListBase
    {
        private defraimp_ImporterNotification notificationFromContext;

        public FlagRecordsOnWatchListNotificationBusinessLogic(IOrganizationService orgSvc, defraimp_ImporterNotification notificationFromContext, ICrmRepository<defraimp_WatchList> watchListRepo, ICrmRepository<defraimp_WatchFlag> watchFlagRepo)
            : base(orgSvc, notificationFromContext, watchListRepo, watchFlagRepo)
        {
            this.notificationFromContext = notificationFromContext;
        }

        public override void FlagRecordIfOnWatchList()
        {
            string originIdentifier = this.notificationFromContext.defraimp_consignorcompanyname;
            string destinationIdentifier = this.notificationFromContext.defraimp_placeofdestinationcompanyname;
            string importerIdentifier = this.notificationFromContext.defraimp_importercompanyname;
            string transporterIdentifier = this.notificationFromContext.defraimp_transportercompanyname;

            List<FlagRecordParams> flagRecordParamsList = new List<FlagRecordParams>();
            flagRecordParamsList.Add(new FlagRecordParams(originIdentifier, defraimp_watchtype.PlaceofOrigin, "defraimp_placeoforiginid", "defraimp_placeoforigin", "defraimp_name", originIdentifier));
            flagRecordParamsList.Add(new FlagRecordParams(destinationIdentifier, defraimp_watchtype.PlaceofDestination, "defraimp_placeofdestinationid", "defraimp_placeofdestination", "defraimp_name", destinationIdentifier));
            flagRecordParamsList.Add(new FlagRecordParams(importerIdentifier, defraimp_watchtype.Consignee, "defraimp_consigneeid", "defraimp_consignee", "defraimp_name", importerIdentifier));
            flagRecordParamsList.Add(new FlagRecordParams(transporterIdentifier, defraimp_watchtype.Transporter, "defraimp_transporterid", "defraimp_transporter", "defraimp_name", transporterIdentifier));

            CheckAndFlagRecordsForParameters(flagRecordParamsList);
        }
    }
}
