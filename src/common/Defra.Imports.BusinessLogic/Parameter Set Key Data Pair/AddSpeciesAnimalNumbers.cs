namespace Defra.Imports.BusinessLogic.ParameterSetKeyDataPair
{
    using System;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    /// <summary>
    /// Adding the species animal numbers to the species entity.
    /// </summary>
    public sealed class AddSpeciesAnimalNumbers
    {
        private readonly IOrganizationService orgSvc;
        private readonly ILogWriter logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddSpeciesAnimalNumbers"/> class.
        /// </summary>
        /// <param name="orgSvc">Org service.</param>
        /// <param name="logger">Logger.</param>
        public AddSpeciesAnimalNumbers(IOrganizationService orgSvc, ILogWriter logger)
        {
            this.orgSvc = orgSvc;
            this.logger = logger;
        }

        /// <summary>
        /// Finds the commodity complement related to the given parameter set key data pair and
        /// populates its number of animals field with the key data pair's value.
        /// </summary>
        /// <param name="keyDataPair">Parameter set key data pair containing the number of animals value.</param>
        public void AddAnimalNumbersToSpecies(defraimp_parametersetkeydatapair keyDataPair)
        {
            if (keyDataPair?.defraimp_ImporterNotificationId == null || keyDataPair.defraimp_CommodityComplementParameterSet == null)
            {
                this.logger.Log(Severity.Warning, nameof(AddSpeciesAnimalNumbers), "Cannot populate animal numbers - key data pair is missing the importer notification or commodity complement parameter set reference.");
                return;
            }

            var complementSetName = this.ResolveComplementParameterSetName(keyDataPair.defraimp_CommodityComplementParameterSet);

            if (string.IsNullOrWhiteSpace(complementSetName) || !int.TryParse(complementSetName, out var complementId))
            {
                this.logger.Log(Severity.Warning, nameof(AddSpeciesAnimalNumbers), $"Cannot populate animal numbers - commodity complement parameter set name '{complementSetName}' is not a valid complement ID.");
                return;
            }

            var commodityComplement = this.FindMatchingCommodityComplement(keyDataPair.defraimp_ImporterNotificationId.Id, complementId);

            if (commodityComplement == null)
            {
                this.logger.Log(Severity.Warning, nameof(AddSpeciesAnimalNumbers), $"No matching commodity complement found for importer notification {keyDataPair.defraimp_ImporterNotificationId.Id} and complement ID {complementId}.");
                return;
            }

            commodityComplement.defraimp_NumberofAnimals = keyDataPair.defraimp_data;
            this.orgSvc.Update(commodityComplement);
        }

        /// <summary>
        /// Resolves the commodity complement parameter set's name. The lookup's Name property is
        /// only populated when the reference was resolved from a stored record (e.g. an image or a
        /// retrieved entity) - when a lookup is set via an @odata.bind on Create, the Name is not
        /// populated, so the parameter set record is retrieved to reliably obtain its name.
        /// </summary>
        /// <param name="commodityComplementParameterSet">Reference to the commodity complement parameter set.</param>
        /// <returns>The commodity complement parameter set's name, or null if it could not be resolved.</returns>
        private string ResolveComplementParameterSetName(EntityReference commodityComplementParameterSet)
        {
            if (!string.IsNullOrWhiteSpace(commodityComplementParameterSet.Name))
            {
                return commodityComplementParameterSet.Name;
            }

            var parameterSet = this.orgSvc.Retrieve(
                defraimp_commoditycomplementparameterset.EntityLogicalName,
                commodityComplementParameterSet.Id,
                new ColumnSet(defraimp_commoditycomplementparameterset.Fields.defraimp_name)).ToEntity<defraimp_commoditycomplementparameterset>();

            return parameterSet?.defraimp_name;
        }

        private defraimp_commoditycomplement FindMatchingCommodityComplement(Guid importerNotificationId, int complementId)
        {
            var query = new QueryExpression(defraimp_commoditycomplement.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(defraimp_commoditycomplement.Fields.defraimp_NumberofAnimals),
            };

            query.Criteria.AddCondition(new ConditionExpression(defraimp_commoditycomplement.Fields.defraimp_ImporterNotificationId, ConditionOperator.Equal, importerNotificationId));
            query.Criteria.AddCondition(new ConditionExpression(defraimp_commoditycomplement.Fields.defraimp_complementid, ConditionOperator.Equal, complementId));

            var results = this.orgSvc.RetrieveMultiple(query);

            return results.Entities.Count > 0
                ? results.Entities[0].ToEntity<defraimp_commoditycomplement>()
                : null;
        }
    }
}
