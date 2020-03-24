using System;
using System.Collections.Generic;
using System.Text;
using Defra.Imports.Model;

namespace Defra.Imports.BusinessLogic.ImportApplication.Factories
{
    public class CounterTransactionDetailFactory : AbstractCounterTransactionDetailFactory
    {
        public override CounterTransactionDetail GetCounterTransactionDetail(defraimp_importapplication importApplication, defraimp_autonumber autoNumberRecord, defraimp_counterhistory_defraimp_operation counterOperation, defraimp_counterhistory_defraimp_reason reason)
        {
            if (autoNumberRecord != null)
            {
                return new CounterTransactionDetail(importApplication, autoNumberRecord, counterOperation, reason, 0, 0);
            }
            else
            {
                return null;
            }
        }

        public override CounterTransactionDetail GetCounterTransactionDetail(defraimp_importapplication importApplication, defraimp_placeoforigin placeOfOriginRecord, defraimp_counterhistory_defraimp_operation counterOperation, defraimp_counterhistory_defraimp_reason reason)
        {
            if (placeOfOriginRecord != null)
            {
                return new CounterTransactionDetail(importApplication, placeOfOriginRecord, counterOperation, reason, 0, 0);
            }
            else
            {
                return null;
            }
        }
    }
}
