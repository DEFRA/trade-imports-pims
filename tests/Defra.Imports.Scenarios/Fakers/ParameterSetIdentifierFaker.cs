namespace Defra.Imports.Scenarios.Fakers
{
    using Defra.Imports.Model;

    /// <summary>
    /// A <see cref="RecordFaker{TEntity}"/> for <see cref="defraimp_parametersetidentifier"/> records - the child record
    /// the EU Imports Notifications Logic App creates per <c>commodities.complementParameterSet.identifiers</c> entry -
    /// see <c>Create_a_new_record_-_Parameter_Set_Identifier</c> in <c>EUImportsNotifications.logicapp.json</c>.
    /// Generates one of the identifier shapes observed across the reviewed payloads: microchip/passport/tattoo (pets),
    /// microchip/passport (horses), microchip/leg ring (birds) and ear tag (cattle). The latter two have no dedicated
    /// column on this entity, so are only reflected in <see cref="defraimp_parametersetidentifier.defraimp_data"/>.
    /// Deliberately does not set the address/individual name/horse name/telephone/email/permanent address fields - these
    /// are unevidenced in any reviewed payload's <c>identifiers</c> entries and appear to belong to a different
    /// identifier shape outside this scenario's scope.
    /// </summary>
    public class ParameterSetIdentifierFaker : RecordFaker<defraimp_parametersetidentifier>
    {
        private sealed class IdentifierProfile
        {
            public string Data { get; set; }

            public string Microchip { get; set; }

            public string Passport { get; set; }

            public string Tattoo { get; set; }
        }

        private static readonly IdentifierProfile[] Profiles =
        {
            new IdentifierProfile { Data = "{\"microchip\":\"123\",\"passport\":\"123\",\"tattoo\":\"123\"}", Microchip = "123", Passport = "123", Tattoo = "123" },
            new IdentifierProfile { Data = "{\"microchip\":\"1\",\"passport\":\"1\"}", Microchip = "1", Passport = "1" },
            new IdentifierProfile { Data = "{\"microchip\":\"123\",\"leg_ring\":\"123\"}", Microchip = "123" },
            new IdentifierProfile { Data = "{\"ear_tag\":\"123\"}" },
        };

        // Set by the first rule and read by the rest so a single Generate() call produces an internally consistent profile.
        private IdentifierProfile profile;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterSetIdentifierFaker"/> class.
        /// </summary>
        public ParameterSetIdentifierFaker()
        {
            this.RuleFor(i => i.defraimp_name, f => f.Random.Int(1, 2).ToString());
            this.RuleFor(i => i.defraimp_data, f => (this.profile = f.PickRandom(Profiles)).Data);
            this.RuleFor(i => i.defraimp_Microchip, f => this.profile.Microchip);
            this.RuleFor(i => i.defraimp_Passport, f => this.profile.Passport);
            this.RuleFor(i => i.defraimp_Tattoo, f => this.profile.Tattoo);
        }
    }
}
