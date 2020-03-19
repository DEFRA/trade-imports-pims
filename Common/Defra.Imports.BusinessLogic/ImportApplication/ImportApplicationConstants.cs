using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public class ImportApplicationConstants
    {
        public const string P1_COUNTER_NAME = "p1_record_count";
        public const string P2_COUNTER_NAME = "p2_record_count";
        public const string P3_COUNTER_NAME = "p3_record_count";

        public const string P1_QUOTA_COUNTER_NAME = "p1_quota_record_count";
        public const string P2_QUOTA_COUNTER_NAME = "p2_quota_record_count";
        public const string P3_QUOTA_COUNTER_NAME = "p3_quota_record_count";

        public const string P1_RISK_LEVEL_NAME = "p1";
        public const string P2_RISK_LEVEL_NAME = "p2";
        public const string P3_RISK_LEVEL_NAME = "p3";

        public const string GB_COVERAGE_RULE_KEY = "p1gb";
        public const string P1_COVERAGE_RULE_KEY = "p1";
        public const string P2_COVERAGE_RULE_KEY = "p2";
        public const string P3_COVERAGE_RULE_KEY = "p3";


        public static string GetCounterName(string riskLevel)
        {
            riskLevel = string.IsNullOrEmpty(riskLevel) ? string.Empty : riskLevel.ToLower();

            switch (riskLevel)
            {
                case "p1":
                    {
                        return P1_COUNTER_NAME;
                    }
                case "p2":
                    {
                        return P2_COUNTER_NAME;
                    }
                case "p3":
                    {
                        return P3_COUNTER_NAME;
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        public static string GetQuotaCounterName(string riskLevel)
        {
            riskLevel = string.IsNullOrEmpty(riskLevel) ? string.Empty : riskLevel.ToLower();

            switch (riskLevel)
            {
                case "p1":
                    {
                        return P1_QUOTA_COUNTER_NAME;
                    }
                case "p2":
                    {
                        return P2_QUOTA_COUNTER_NAME;
                    }
                case "p3":
                    {
                        return P3_QUOTA_COUNTER_NAME;
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }
    }
}
