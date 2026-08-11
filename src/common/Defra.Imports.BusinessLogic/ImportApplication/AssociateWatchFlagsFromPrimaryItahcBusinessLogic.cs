using Defra.Imports.BusinessLogic.ImportApplication.AssociateWatchFlags;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public class AssociateWatchFlagsFromPrimaryItahcBusinessLogic : AssociateWatchFlagsBase
    {
        private defraimp_importapplication _target;
        private ICrmRepository<defraimp_WatchFlag> _watchFlagRepo;

        public AssociateWatchFlagsFromPrimaryItahcBusinessLogic(defraimp_importapplication target, ICrmRepository<defraimp_WatchFlag> watchFlagRepo)
            : base(target, watchFlagRepo)
        {
            this._target = target;
            this._watchFlagRepo = watchFlagRepo;
        }

        public override void RunLogic()
        {
            this.DisassociateExistingWatchFlags("defraimp_itahcid");
            this.AssociateWatchFlagsAssociatedToLinkedEntity();
        }

        private void AssociateWatchFlagsAssociatedToLinkedEntity()
        {
            EntityReference primaryItahcRef = _target.defraimp_PrimaryITAHCId;

            if (primaryItahcRef != null)
            {
                // Retrieve the associated watch flags for this primary ITAHC
                IEnumerable<defraimp_WatchFlag> primaryItahcWatchFlags = this.GetWatchFlagsForItahcRef(primaryItahcRef);

                foreach (defraimp_WatchFlag currentWatchFlag in primaryItahcWatchFlags)
                {
                    if (currentWatchFlag.defraimp_ImportApplicationId == null || (currentWatchFlag.defraimp_ImportApplicationId.Id != _target.Id))
                    {
                        this.AssociateFlagToTarget(currentWatchFlag);
                    }
                }
            }
        }

        private IEnumerable<defraimp_WatchFlag> GetWatchFlagsForItahcRef(EntityReference itahcRef)
        {
            return this._watchFlagRepo.Find(
                    e => e.defraimp_ItahcId.Id == itahcRef.Id,
                    e => new defraimp_WatchFlag()
                    {
                        defraimp_WatchFlagId = e.defraimp_WatchFlagId,
                        defraimp_ImportApplicationId = e.defraimp_ImportApplicationId
                    });
        }
    }
}
