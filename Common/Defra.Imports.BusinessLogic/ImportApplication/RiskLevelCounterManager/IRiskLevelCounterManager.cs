using System;
using System.Collections.Generic;
using System.Text;
using Defra.Imports.Model;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public interface IRiskLevelCounterManager
    {
        void IncrementNumber(ref defraimp_importapplication importApplication, string reason);
        void DecrementNumber(ref defraimp_importapplication importApplication, string reason);
        void SetNumberValue(ref defraimp_importapplication importApplication, string reason, int value);
        void IncrementQuota(ref defraimp_importapplication importApplication, string reason);
        void DecrementQuota(ref defraimp_importapplication importApplication, string reason);
        void SetQuotaValue(ref defraimp_importapplication importApplication, string reason, int value);
    }
}
