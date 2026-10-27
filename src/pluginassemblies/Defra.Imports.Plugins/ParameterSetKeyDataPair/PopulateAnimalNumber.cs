namespace Defra.Imports.Plugins.ParameterSetKeyDataPair
{
    using Defra.Imports.Infrastructure.Plugins;

    /// <summary>
    /// Applies parameter set key data pairs (currently "number_animal") to the matching attribute of a commodity
    /// complement. The class name is retained for the registered steps; <see cref="PopulateAnimalNumberHandler"/>
    /// adapts the step and the shared application service does the work.
    /// Registered as post-operation, synchronous steps on Create (no filtering attributes) and Update (filtering on
    /// defraimp_data, with a pre-image of defraimp_commoditycomplementparameterset, defraimp_importernotificationid
    /// and defraimp_name) of defraimp_parametersetkeydatapair.
    /// </summary>
    public sealed class PopulateAnimalNumber : PluginBase<PopulateAnimalNumberHandler>
    {
        /// <summary>Initializes a new instance of the <see cref="PopulateAnimalNumber"/> class.</summary>
        public PopulateAnimalNumber()
            : base(PluginCompositionRoot.Root)
        {
        }
    }
}
