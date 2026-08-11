namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public interface IImportRiskCounterAuditor
    {
        void AuditCounterEvent(CounterTransactionDetail counterTransactionDetail);
    }
}
