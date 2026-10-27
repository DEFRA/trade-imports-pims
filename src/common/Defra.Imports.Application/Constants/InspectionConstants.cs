namespace Defra.Imports.Application.Constants
{
    /// <summary>
    /// The autonumber keys, risk level names and coverage rule keys that drive the inspection of an import
    /// application. Risk level names are compared in lower case.
    /// </summary>
    public static class InspectionConstants
    {
        /// <summary>The key of the P1 record counter.</summary>
        public const string P1CounterName = "p1_record_count";

        /// <summary>The key of the P2 record counter.</summary>
        public const string P2CounterName = "p2_record_count";

        /// <summary>The key of the P3 record counter, which also serves as the global counter.</summary>
        public const string P3CounterName = "p3_record_count";

        /// <summary>The key of the import application record counter.</summary>
        public const string ImportApplicationCounterName = "defraimp_ImportApplicationRecordCount";

        /// <summary>The key of the P1 quota counter.</summary>
        public const string P1QuotaCounterName = "p1_quota_record_count";

        /// <summary>The key of the P2 quota counter.</summary>
        public const string P2QuotaCounterName = "p2_quota_record_count";

        /// <summary>The key of the P3 quota counter.</summary>
        public const string P3QuotaCounterName = "p3_quota_record_count";

        /// <summary>The lower case name of the P1 risk level.</summary>
        public const string P1RiskLevelName = "p1";

        /// <summary>The lower case name of the P2 risk level.</summary>
        public const string P2RiskLevelName = "p2";

        /// <summary>The lower case name of the P3 risk level.</summary>
        public const string P3RiskLevelName = "p3";

        /// <summary>The lower case name of the TB risk level.</summary>
        public const string TbRiskLevelName = "tb";

        /// <summary>The key of the gold/bronze coverage rule.</summary>
        public const string GoldBronzeCoverageRuleKey = "p1gb";

        /// <summary>The key of the P1 coverage rule.</summary>
        public const string P1CoverageRuleKey = "p1";

        /// <summary>The key of the P2 coverage rule.</summary>
        public const string P2CoverageRuleKey = "p2";

        /// <summary>The key of the P3 coverage rule.</summary>
        public const string P3CoverageRuleKey = "p3";

        /// <summary>The tag under which the inspection logic writes to the trace log.</summary>
        public const string LogTag = "DetermineInspectionLogic";

        /// <summary>Gets the key of the record counter for a risk level.</summary>
        /// <param name="riskLevel">The name of the risk level, in any case.</param>
        /// <returns>The key, or an empty string if the risk level has no record counter.</returns>
        public static string GetCounterName(string riskLevel)
        {
            switch (string.IsNullOrEmpty(riskLevel) ? string.Empty : riskLevel.ToLowerInvariant())
            {
                case P1RiskLevelName:
                    return P1CounterName;
                case P2RiskLevelName:
                    return P2CounterName;
                case P3RiskLevelName:
                    return P3CounterName;
                default:
                    return string.Empty;
            }
        }

        /// <summary>Gets the key of the quota counter for a risk level.</summary>
        /// <param name="riskLevel">The name of the risk level, in any case.</param>
        /// <returns>The key, or an empty string if the risk level has no quota counter.</returns>
        public static string GetQuotaCounterName(string riskLevel)
        {
            switch (string.IsNullOrEmpty(riskLevel) ? string.Empty : riskLevel.ToLowerInvariant())
            {
                case P1RiskLevelName:
                    return P1QuotaCounterName;
                case P2RiskLevelName:
                    return P2QuotaCounterName;
                case P3RiskLevelName:
                    return P3QuotaCounterName;
                default:
                    return string.Empty;
            }
        }
    }
}
