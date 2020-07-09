using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public class AssociateWatchFlagsFromPrimaryItahcBusinessLogic
    {
        private defraimp_importapplication _target;
        private ICrmRepository<defraimp_WatchFlag> _watchFlagRepo;

        public AssociateWatchFlagsFromPrimaryItahcBusinessLogic(defraimp_importapplication target, ICrmRepository<defraimp_WatchFlag> watchFlagRepo)
        {
            this._target = target;
            this._watchFlagRepo = watchFlagRepo;
        }

        public void RunLogic()
        {
            DisassociateExistingWatchFlags();
            AssociateWatchFlagsAssociatedToPrimaryItahc();
        }

        private void DisassociateExistingWatchFlags()
        {
            // Retrieve watch flags currently linked to the import record
            IEnumerable<defraimp_WatchFlag> existingWatchFlags = GetWatchFlagsForImportRecordRef(_target.ToEntityReference());

            foreach (defraimp_WatchFlag currentExistingFlag in existingWatchFlags)
            {
                DissociateFlagFromImportRecord(currentExistingFlag);
            }
        }

        private void AssociateWatchFlagsAssociatedToPrimaryItahc()
        {
            EntityReference primaryItahcRef = _target.defraimp_PrimaryITAHCId;

            if (primaryItahcRef != null)
            {
                // Retrieve the associated watch flags for this primary ITAHC
                IEnumerable<defraimp_WatchFlag> primaryItahcWatchFlags = GetWatchFlagsForItahcRef(primaryItahcRef);

                foreach (defraimp_WatchFlag currentWatchFlag in primaryItahcWatchFlags)
                {
                    if (currentWatchFlag.defraimp_ImportApplicationId == null || (currentWatchFlag.defraimp_ImportApplicationId.Id != _target.Id))
                    {
                        AssociateFlagToTarget(currentWatchFlag);
                    }
                }
            }
        }

        private void DissociateFlagFromImportRecord(defraimp_WatchFlag flag)
        {
            flag.defraimp_ImportApplicationId = null;
            _watchFlagRepo.Update(flag);
        }

        private void AssociateFlagToTarget(defraimp_WatchFlag flag)
        {
            flag.defraimp_ImportApplicationId = _target.ToEntityReference();
            _watchFlagRepo.Update(flag);
        }

        private IEnumerable<defraimp_WatchFlag> GetWatchFlagsForItahcRef(EntityReference itahcRef)
        {
            return _watchFlagRepo.Find(
                    e => e.defraimp_ItahcId.Id == itahcRef.Id,
                    e => new defraimp_WatchFlag()
                    {
                        defraimp_WatchFlagId = e.defraimp_WatchFlagId,
                        defraimp_ImportApplicationId = e.defraimp_ImportApplicationId
                    });
        }

        private IEnumerable<defraimp_WatchFlag> GetWatchFlagsForImportRecordRef(EntityReference importApplicationRef)
        {
            return _watchFlagRepo.Find(
                    e => e.defraimp_ImportApplicationId.Id == importApplicationRef.Id,
                    e => new defraimp_WatchFlag()
                    {
                        defraimp_WatchFlagId = e.defraimp_WatchFlagId,
                        defraimp_ImportApplicationId = e.defraimp_ImportApplicationId
                    });
        }
    }
}
