namespace Defra.Imports.Scenarios.Fakers
{
    using Defra.Imports.Model;

    /// <summary>
    /// A <see cref="RecordFaker{TEntity}"/> for <see cref="defraimp_commoditycomplement"/> records - the child record the
    /// EU Imports Notifications Logic App creates per <c>commodities.commodityComplement</c> entry - see
    /// <c>Create_a_new_record_-_Commodity_Complement</c> in <c>EUImportsNotifications.logicapp.json</c>.
    /// Picks one of a handful of commodity/species combinations observed across the reviewed notification payloads, so
    /// that the related fields stay internally consistent rather than being randomised independently.
    /// Deliberately does not set <see cref="defraimp_commoditycomplement.defraimp_speciesfamily"/>/
    /// <see cref="defraimp_commoditycomplement.defraimp_speciesfamilyname"/> (not populated in any reviewed payload) or
    /// <see cref="defraimp_commoditycomplement.defraimp_NumberofAnimals"/>/<see cref="defraimp_commoditycomplement.defraimp_NumberofPackages"/>
    /// (not set by the Logic App's commodity complement creation action).
    /// </summary>
    public class CommodityComplementFaker : RecordFaker<defraimp_commoditycomplement>
    {
        private sealed class SpeciesProfile
        {
            public string CommodityId { get; set; }

            public string CommodityDescription { get; set; }

            public int ComplementId { get; set; }

            public string ComplementName { get; set; }

            public string SpeciesId { get; set; }

            public string SpeciesName { get; set; }

            public string SpeciesNomination { get; set; }

            public string SpeciesCommonName { get; set; }

            public string SpeciesType { get; set; }

            public string SpeciesTypeName { get; set; }

            public string SpeciesClass { get; set; }

            public string SpeciesClassName { get; set; }
        }

        private static readonly SpeciesProfile[] Profiles =
        {
            new SpeciesProfile
            {
                CommodityId = "01061900", CommodityDescription = "Dogs", ComplementId = 106400, ComplementName = "Canis familiaris",
                SpeciesId = "22392", SpeciesName = "Canis familiaris", SpeciesNomination = "Canis familiaris", SpeciesCommonName = "Dogs",
                SpeciesType = "2", SpeciesClass = "106400", SpeciesClassName = "Carnivora",
            },
            new SpeciesProfile
            {
                CommodityId = "01063100", CommodityDescription = "Birds of Prey - Falcons", ComplementId = 2, ComplementName = "Falconiformes",
                SpeciesId = "113311", SpeciesName = "Falconiformes", SpeciesNomination = "Falconiformes", SpeciesCommonName = "Birds of Prey - Falcons",
                SpeciesType = "2",
            },
            new SpeciesProfile
            {
                CommodityId = "0101", CommodityDescription = "Live horses, asses, mules and hinnies", ComplementId = 26602, ComplementName = "Equus asinus",
                SpeciesId = "35403", SpeciesName = "Equus asinus", SpeciesNomination = "Equus asinus", SpeciesType = "2", SpeciesClass = "26602",
            },
            new SpeciesProfile
            {
                CommodityId = "0102", CommodityDescription = "Cattle", ComplementId = 16, ComplementName = "Bos taurus",
                SpeciesId = "121693", SpeciesName = "Bos taurus", SpeciesNomination = "Bos taurus", SpeciesCommonName = "Cattle",
                SpeciesType = "16", SpeciesTypeName = "Domestic",
            },
        };

        // Set by the first rule and read by the rest so a single Generate() call produces an internally consistent profile.
        private SpeciesProfile profile;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommodityComplementFaker"/> class.
        /// </summary>
        public CommodityComplementFaker()
        {
            this.RuleFor(c => c.defraimp_commodityid, f => (this.profile = f.PickRandom(Profiles)).CommodityId);
            this.RuleFor(c => c.defraimp_commoditydescription, f => this.profile.CommodityDescription);
            this.RuleFor(c => c.defraimp_complementid, f => this.profile.ComplementId);
            this.RuleFor(c => c.defraimp_name, f => this.profile.ComplementName);
            this.RuleFor(c => c.defraimp_speciesid, f => this.profile.SpeciesId);
            this.RuleFor(c => c.defraimp_speciesname, f => this.profile.SpeciesName);
            this.RuleFor(c => c.defraimp_speciesnomination, f => this.profile.SpeciesNomination);
            this.RuleFor(c => c.defraimp_speciescommonname, f => this.profile.SpeciesCommonName);
            this.RuleFor(c => c.defraimp_speciestype, f => this.profile.SpeciesType);
            this.RuleFor(c => c.defraimp_speciestypename, f => this.profile.SpeciesTypeName);
            this.RuleFor(c => c.defraimp_speciesclass, f => this.profile.SpeciesClass);
            this.RuleFor(c => c.defraimp_speciesclassname, f => this.profile.SpeciesClassName);
        }
    }
}
