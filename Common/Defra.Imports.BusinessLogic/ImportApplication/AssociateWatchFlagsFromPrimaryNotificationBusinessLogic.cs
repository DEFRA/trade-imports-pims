using Defra.Imports.BusinessLogic.ImportApplication.AssociateWatchFlags;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public class AssociateWatchFlagsFromPrimaryNotificationBusinessLogic : AssociateWatchFlagsBase
    {
        private defraimp_importapplication target;
        private ICrmRepository<defraimp_WatchFlag> watchFlagRepo;

        public AssociateWatchFlagsFromPrimaryNotificationBusinessLogic(defraimp_importapplication target, ICrmRepository<defraimp_WatchFlag> watchFlagRepo)
            : base(target, watchFlagRepo)
        {
            this.target = target;
            this.watchFlagRepo = watchFlagRepo;
        }

        public override void RunLogic()
        {
            this.DisassociateExistingWatchFlags("defraimp_importernotificationid");
            this.AssociateWatchFlagsAssociatedToLinkedEntity();
        }

        private void AssociateWatchFlagsAssociatedToLinkedEntity()
        {
            EntityReference primaryNotificationRef = this.target.defraimp_PrimaryImporterNotificationId;

            if (primaryNotificationRef != null)
            {
                // Retrieve the associated watch flags for this primary ITAHC
                IEnumerable<defraimp_WatchFlag> primaryNotificationWatchFlags = this.GetWatchFlagsForNotificationRef(primaryNotificationRef);

                foreach (defraimp_WatchFlag currentWatchFlag in primaryNotificationWatchFlags)
                {
                    if (currentWatchFlag.defraimp_ImportApplicationId == null || (currentWatchFlag.defraimp_ImportApplicationId.Id != this.target.Id))
                    {
                        this.AssociateFlagToTarget(currentWatchFlag);
                    }
                }
            }
        }

        private IEnumerable<defraimp_WatchFlag> GetWatchFlagsForNotificationRef(EntityReference notificationRef)
        {
            return this.watchFlagRepo.Find(
                    e => e.defraimp_ImporterNotificationId.Id == notificationRef.Id,
                    e => new defraimp_WatchFlag()
                    {
                        defraimp_WatchFlagId = e.defraimp_WatchFlagId,
                        defraimp_ImportApplicationId = e.defraimp_ImportApplicationId,
                    });
        }
    }
}
