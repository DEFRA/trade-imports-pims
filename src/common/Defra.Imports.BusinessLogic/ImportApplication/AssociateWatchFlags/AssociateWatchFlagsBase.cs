using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace Defra.Imports.BusinessLogic.ImportApplication.AssociateWatchFlags
{
    public abstract class AssociateWatchFlagsBase
    {
        private defraimp_importapplication target;
        private ICrmRepository<defraimp_WatchFlag> watchFlagRepo;

        public AssociateWatchFlagsBase(defraimp_importapplication target, ICrmRepository<defraimp_WatchFlag> watchFlagRepo)
        {
            this.target = target;
            this.watchFlagRepo = watchFlagRepo;
        }

        public abstract void RunLogic();

        protected void DisassociateExistingWatchFlags(string watchFlaglookupFieldName)
        {
            // Retrieve watch flags currently linked to the import record
            IEnumerable<defraimp_WatchFlag> existingWatchFlags = this.GetWatchFlagsForImportRecordWithLookupPopulated(this.target.ToEntityReference(), watchFlaglookupFieldName);

            foreach (defraimp_WatchFlag currentExistingFlag in existingWatchFlags)
            {
                this.DissociateFlagFromImportRecord(currentExistingFlag);
            }
        }

        protected IEnumerable<defraimp_WatchFlag> GetWatchFlagsForImportRecordWithLookupPopulated(EntityReference importApplicationRef, string lookupFieldName)
        {
            return this.watchFlagRepo.Find(
                    e => e.defraimp_ImportApplicationId.Id == importApplicationRef.Id && e.GetAttributeValue<EntityReference>(lookupFieldName) != null,
                    e => new defraimp_WatchFlag()
                    {
                        defraimp_WatchFlagId = e.defraimp_WatchFlagId,
                        defraimp_ImportApplicationId = e.defraimp_ImportApplicationId
                    });
        }

        protected void AssociateFlagToTarget(defraimp_WatchFlag flag)
        {
            flag.defraimp_ImportApplicationId = this.target.ToEntityReference();
            this.watchFlagRepo.Update(flag);
        }

        private void DissociateFlagFromImportRecord(defraimp_WatchFlag flag)
        {
            flag.defraimp_ImportApplicationId = null;
            this.watchFlagRepo.Update(flag);
        }
    }
}
