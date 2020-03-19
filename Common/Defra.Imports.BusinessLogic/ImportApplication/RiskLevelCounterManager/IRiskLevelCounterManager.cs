using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public interface IRiskLevelCounterManager
    {
        void IncrementNumber(string reason);
        void DecrementNumber(string reason);
        void SetNumberValue(string reason, int value);
        void IncrementQuota(string reason);
        void DecrementQuota(string reason);
        void SetQuotaValue(string reason, int value);
    }
}
