namespace Defra.Imports.Dataverse.Queries
{
    using System;
    using System.Linq;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk.Client;

    /// <summary>
    /// Read-only queries over the autonumber records that hold the inspection counters.
    /// </summary>
    public class AutonumberQueries
    {
        private readonly ImportsContext context;

        /// <summary>Initializes a new instance of the <see cref="AutonumberQueries"/> class.</summary>
        /// <param name="context">The invocation-local context, which these read-only queries stop tracking.</param>
        public AutonumberQueries(ImportsContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            // These queries are read-only, so avoid change tracking on the invocation-local context.
            this.context.MergeOption = MergeOption.NoTracking;
        }

        /// <summary>
        /// Finds an autonumber by its key. The lookup is not constrained to a single record, so the first match
        /// returned is used.
        /// </summary>
        /// <param name="key">The key of the autonumber.</param>
        /// <returns>The identifier, key and current number of the autonumber, or null if there is none.</returns>
        public defraimp_autonumber FindByKey(string key)
        {
            var query =
                from autonumber in this.context.defraimp_autonumberSet
                where autonumber.defraimp_Key == key
                select new defraimp_autonumber
                {
                    defraimp_autonumberId = autonumber.defraimp_autonumberId,
                    defraimp_Key = autonumber.defraimp_Key,
                    defraimp_CurrentNumber = autonumber.defraimp_CurrentNumber,
                };

            return query.FirstOrDefault();
        }
    }
}
