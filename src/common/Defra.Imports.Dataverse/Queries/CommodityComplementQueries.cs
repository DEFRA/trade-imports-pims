namespace Defra.Imports.Dataverse.Queries
{
    using System;
    using System.Linq;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk.Client;

    /// <summary>
    /// Read-only queries over commodity complements and their commodity complement parameter sets.
    /// </summary>
    public class CommodityComplementQueries
    {
        private readonly ImportsContext context;

        /// <summary>Initializes a new instance of the <see cref="CommodityComplementQueries"/> class.</summary>
        /// <param name="context">The invocation-local context, which these read-only queries stop tracking.</param>
        public CommodityComplementQueries(ImportsContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            // These queries are read-only, so avoid change tracking on the invocation-local context.
            this.context.MergeOption = MergeOption.NoTracking;
        }

        /// <summary>Retrieves the name of a commodity complement parameter set.</summary>
        /// <param name="parameterSetId">The identifier of the commodity complement parameter set.</param>
        /// <returns>The name of the parameter set, which is null if the record has no name.</returns>
        /// <exception cref="InvalidOperationException">The parameter set does not exist.</exception>
        public string GetParameterSetName(Guid parameterSetId)
        {
            var query =
                from parameterSet in this.context.defraimp_commoditycomplementparametersetSet
                where parameterSet.defraimp_commoditycomplementparametersetId == parameterSetId
                select new { parameterSet.defraimp_name };

            var result = query.FirstOrDefault()
                ?? throw new InvalidOperationException("The commodity complement parameter set was not found.");

            return result.defraimp_name;
        }

        /// <summary>
        /// Finds the commodity complement of an importer notification that has the given complement identifier.
        /// The lookup is not constrained to a single record, so the first match returned is used.
        /// </summary>
        /// <param name="importerNotificationId">The identifier of the importer notification.</param>
        /// <param name="complementId">The complement identifier held by the commodity complement.</param>
        /// <returns>The identifier of the matching commodity complement, or null if there is none.</returns>
        public Guid? FindCommodityComplementId(Guid importerNotificationId, int complementId)
        {
            var query =
                from commodityComplement in this.context.defraimp_commoditycomplementSet
                where commodityComplement.defraimp_ImporterNotificationId.Id == importerNotificationId
                    && commodityComplement.defraimp_complementid == complementId
                select new { commodityComplement.defraimp_commoditycomplementId };

            return query.FirstOrDefault()?.defraimp_commoditycomplementId;
        }
    }
}
