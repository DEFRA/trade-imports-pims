using System;
using System.Collections.Generic;
using System.Text;
using Defra.Imports.Model;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public interface IRiskLevelCounterManager
    {
        void IncrementNumber(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason);
        void DecrementNumber(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason);
        void SetNumberValue(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason, int value);
        void IncrementQuota(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason);
        void DecrementQuota(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason);
        void SetQuotaValue(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason, int value);
        void IncrementGlobalCounter(defraimp_importapplication importApplication);
        void DecrementGlobalCounter(defraimp_importapplication importApplication);
    }
}
