using Defra.Imports.Model;

namespace Defra.Imports.BusinessLogic.ImporterNotification.FlagRecordsOnWatchList
{
    public class FlagRecordParams
    {

        public FlagRecordParams(string identifier, defraimp_watchtype watchType, string watchListLookupName, string economicOperatorEntityLogicalName, string economicOperatorSearchFieldName, string economicOperatorSearchFieldValue)
        {
            this.Identifier = identifier;
            this.WatchType = watchType;
            this.WatchListLookupName = watchListLookupName;
            this.EconomicOperatorEntityLogicalName = economicOperatorEntityLogicalName;
            this.EconomicOperatorSearchFieldName = economicOperatorSearchFieldName;
            this.EconomicOperatorSearchFieldValue = economicOperatorSearchFieldValue;
        }

        public string Identifier { get; }

        public defraimp_watchtype WatchType { get; }

        public string WatchListLookupName { get; }

        public string EconomicOperatorEntityLogicalName { get; }

        public string EconomicOperatorSearchFieldName { get; }

        public string EconomicOperatorSearchFieldValue { get; }
    }
}
