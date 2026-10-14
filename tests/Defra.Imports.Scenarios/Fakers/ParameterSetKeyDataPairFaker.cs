namespace Defra.Imports.Scenarios.Fakers
{
    using Defra.Imports.Model;

    /// <summary>
    /// A <see cref="RecordFaker{TEntity}"/> for <see cref="defraimp_parametersetkeydatapair"/> records - the child record
    /// the EU Imports Notifications Logic App creates per <c>commodities.complementParameterSet.keyDataPair</c> entry -
    /// see <c>Create_a_new_record_-_Parameter_Set_Key_Data_Pair</c> in <c>EUImportsNotifications.logicapp.json</c>.
    /// Every reviewed payload has exactly one key/data pair: key <c>imp_number_animal</c> with the species count as its data.
    /// </summary>
    public class ParameterSetKeyDataPairFaker : RecordFaker<defraimp_parametersetkeydatapair>
    {
        private static readonly string[] SpeciesNames = { "Canis familiaris", "Falconiformes", "Equus asinus", "Bos taurus" };

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterSetKeyDataPairFaker"/> class.
        /// </summary>
        public ParameterSetKeyDataPairFaker()
        {
            this.RuleFor(k => k.defraimp_name, f => "imp_number_animal");
            this.RuleFor(k => k.defraimp_data, f => f.Random.Int(1, 25).ToString());

            // Denormalised copy of the parent parameter set's species - not evidenced in the reviewed payloads/Logic App
            // snapshot, included defensively in case it was added since.
            this.RuleFor(k => k.defraimp_SpeciesID, f => f.Random.Int(10000, 999999).ToString());
            this.RuleFor(k => k.defraimp_SpeciesName, f => f.PickRandom(SpeciesNames));
        }
    }
}
