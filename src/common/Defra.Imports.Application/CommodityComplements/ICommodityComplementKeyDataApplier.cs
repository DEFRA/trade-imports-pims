namespace Defra.Imports.Application.CommodityComplements
{
    using Defra.Imports.Dataverse.Model;

    /// <summary>
    /// Applies the value of a parameter set key data pair to the commodity complement that it refers to.
    /// </summary>
    public interface ICommodityComplementKeyDataApplier
    {
        /// <summary>
        /// Finds the commodity complement that the key data pair refers to and updates the attribute that the key maps
        /// to with the key data pair's value, issuing one explicit update. Key data pairs whose key is not one that
        /// the applier understands are skipped. Those that cannot be related to a commodity complement are skipped and
        /// traced as warnings. The key data pair is not modified.
        /// </summary>
        /// <param name="keyDataPair">The key data pair holding the value.</param>
        void Apply(defraimp_parametersetkeydatapair keyDataPair);
    }
}
