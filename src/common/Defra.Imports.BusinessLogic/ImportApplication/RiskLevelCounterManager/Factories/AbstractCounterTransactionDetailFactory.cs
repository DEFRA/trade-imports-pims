using Defra.Imports.Model;

namespace Defra.Imports.BusinessLogic.ImportApplication.Factories
{
    public abstract class AbstractCounterTransactionDetailFactory
    {
        public abstract CounterTransactionDetail GetCounterTransactionDetail(defraimp_importapplication importApplication, defraimp_autonumber autoNumberRecord, defraimp_counterhistory_defraimp_operation counterOperation, defraimp_counterhistory_defraimp_reason reason);

        public abstract CounterTransactionDetail GetCounterTransactionDetail(defraimp_importapplication importApplication, defraimp_placeoforigin placeOfOriginRecord, defraimp_counterhistory_defraimp_operation counterOperation, defraimp_counterhistory_defraimp_reason reason);
    }
}
