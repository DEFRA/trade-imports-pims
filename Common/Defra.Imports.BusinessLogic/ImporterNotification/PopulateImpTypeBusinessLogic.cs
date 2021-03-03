namespace Defra.Imports.BusinessLogic.ImporterNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;

    /// <summary>
    /// A class containing business logic for populating a Importer notifications Imp Type.
    /// </summary>
    public class PopulateImpTypeBusinessLogic
    {
        private defraimp_ImporterNotification target;
        private ICrmRepository<defraimp_imptype> impTypeRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulateImpTypeBusinessLogic"/> class.
        /// </summary>
        /// <param name="target">The target record to set the imp type lookup on</param>
        /// <param name="impTypeRepo">The repository to use to retrieve the imp types with same code as the target</param>
        public PopulateImpTypeBusinessLogic(defraimp_ImporterNotification target, ICrmRepository<defraimp_imptype> impTypeRepo)
        {
            this.target = target;
            this.impTypeRepo = impTypeRepo;
        }

        /// <summary>
        /// A method to intiate the populate imp type logic.
        /// </summary>
        public void RunLogic()
        {
            if (this.target.defraimp_ImpType != null)
            {
                // Retrieve the imptypes with matching imptype code on the notification
                List<defraimp_imptype> impTypes =
                    this.impTypeRepo.Find(
                        x => x.defraimp_code == this.target.defraimp_ImpType,
                        x => new defraimp_imptype()
                        {
                            defraimp_imptypeId = x.defraimp_imptypeId,
                        }).ToList();

                if (impTypes != null && impTypes.Count > 0)
                {
                    // Populate the imp type id lookup
                    this.target.defraimp_imptypeid = impTypes.First().ToEntityReference();
                }
            }
        }
    }
}
