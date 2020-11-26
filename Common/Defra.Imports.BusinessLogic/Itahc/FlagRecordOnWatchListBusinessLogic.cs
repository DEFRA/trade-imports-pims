using Defra.Imports.BusinessLogic.ImporterNotification.FlagRecordsOnWatchList;
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
    public class FlagRecordOnWatchListBusinessLogic : FlagRecordsOnWatchListBase
    {
        private defraimp_itahc itahcFromContext;

        public FlagRecordOnWatchListBusinessLogic(IOrganizationService orgSvc, defraimp_itahc itahcFromContext, ICrmRepository<defraimp_WatchList> watchListRepo, ICrmRepository<defraimp_WatchFlag> watchFlagRepo)
            : base(orgSvc, itahcFromContext, watchListRepo, watchFlagRepo)
        {
            this.itahcFromContext = itahcFromContext;
        }

        public override void FlagRecordIfOnWatchList()
        {
            string originIdentifier = this.itahcFromContext.defraimp_PlaceOfOriginHarvestName;
            string destinationIdentifier = this.itahcFromContext.defraimp_PlaceOfDestinationName;
            string consigneeIdentifier = this.itahcFromContext.defraimp_ConsigneeName;
            string transporterIdentifier = this.itahcFromContext.defraimp_TransporterName;
            string veterinarianIdentifier = this.itahcFromContext.defraimp_OVName;

            List<FlagRecordParams> flagRecordParamsList = new List<FlagRecordParams>();
            flagRecordParamsList.Add(new FlagRecordParams(originIdentifier, defraimp_watchtype.PlaceofOrigin, "defraimp_placeoforiginid", "defraimp_placeoforigin", "defraimp_name", originIdentifier));
            flagRecordParamsList.Add(new FlagRecordParams(destinationIdentifier, defraimp_watchtype.PlaceofDestination, "defraimp_placeofdestinationid", "defraimp_placeofdestination", "defraimp_name", destinationIdentifier));
            flagRecordParamsList.Add(new FlagRecordParams(consigneeIdentifier, defraimp_watchtype.Consignee, "defraimp_consigneeid", "defraimp_consignee", "defraimp_name", consigneeIdentifier));
            flagRecordParamsList.Add(new FlagRecordParams(transporterIdentifier, defraimp_watchtype.Transporter, "defraimp_transporterid", "defraimp_transporter", "defraimp_name", transporterIdentifier));
            flagRecordParamsList.Add(new FlagRecordParams(veterinarianIdentifier, defraimp_watchtype.Veterinarian, "defraimp_veterinarianid", "defraimp_veterinarian", "defraimp_name", veterinarianIdentifier));

            CheckAndFlagRecordsForParameters(flagRecordParamsList);
        }
    }
}
