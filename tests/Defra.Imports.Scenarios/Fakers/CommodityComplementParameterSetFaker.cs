namespace Defra.Imports.Scenarios.Fakers
{
    using Defra.Imports.Model;

    /// <summary>
    /// A <see cref="RecordFaker{TEntity}"/> for <see cref="defraimp_commoditycomplementparameterset"/> records - the child
    /// record the EU Imports Notifications Logic App creates per <c>commodities.complementParameterSet</c> entry - see
    /// <c>Create_a_new_record_-_Commodity_Complement_Parameter_Set</c> in <c>EUImportsNotifications.logicapp.json</c>.
    /// Also creates the <see cref="defraimp_parametersetidentifier"/> and <see cref="defraimp_parametersetkeydatapair"/>
    /// child records nested under it, via the early-bound deep-insert relationship properties - one key/data pair and one
    /// identifier by default, matching most reviewed payloads (a couple had two identifiers for a multi-animal consignment).
    /// Note: the complement/species IDs generated here are not cross-correlated with a sibling <see cref="CommodityComplementFaker"/>-
    /// generated record - the reviewed payloads match them by value, but each faker generates its own independently.
    /// </summary>
    public class CommodityComplementParameterSetFaker : RecordFaker<defraimp_commoditycomplementparameterset>
    {
        private static readonly string[] SpeciesNames = { "Canis familiaris", "Falconiformes", "Equus asinus", "Bos taurus" };

        /// <summary>
        /// Initializes a new instance of the <see cref="CommodityComplementParameterSetFaker"/> class.
        /// </summary>
        /// <param name="identifierFaker">
        /// The faker used to generate the identifier child record(s). Defaults to a plain <see cref="ParameterSetIdentifierFaker"/>,
        /// generating a single record.
        /// </param>
        /// <param name="keyDataPairFaker">
        /// The faker used to generate the key/data pair child record(s). Defaults to a plain <see cref="ParameterSetKeyDataPairFaker"/>,
        /// generating a single record.
        /// </param>
        public CommodityComplementParameterSetFaker(
            ParameterSetIdentifierFaker identifierFaker = null,
            ParameterSetKeyDataPairFaker keyDataPairFaker = null)
        {
            var identifiers = (identifierFaker ?? new ParameterSetIdentifierFaker()).Generate(1);
            var keyDataPairs = (keyDataPairFaker ?? new ParameterSetKeyDataPairFaker()).Generate(1);

            this.RuleFor(p => p.defraimp_name, f => f.Random.Int(1, 999999).ToString());
            this.RuleFor(p => p.defraimp_speciesid, f => f.Random.Int(10000, 999999).ToString());

            // Not evidenced in the reviewed payloads/Logic App snapshot - included defensively in case it was added since.
            this.RuleFor(p => p.defraimp_SpeciesName, f => f.PickRandom(SpeciesNames));
            this.RuleFor(p => p.defraimp_SpeciesQuantity, f => f.Random.Int(1, 25).ToString());

            this.RuleFor(p => p.defraimp_parameterset_defraimp_parametersetidentifier_CommodityComplementParameterSet, f => identifiers);
            this.RuleFor(p => p.defraimp_parameterset_defraimp_parametersetkeydatapair_CommodityComplementParameterSet, f => keyDataPairs);
        }
    }
}
