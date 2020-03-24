using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public interface IImportRiskCounterAuditor
    {
        void AuditCounterEvent(CounterTransactionDetail counterTransactionDetail);
    }
}
