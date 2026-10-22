namespace Defra.Imports.Dataverse.Extensions
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;

    /// <summary>
    /// Extensions for the parameter set key data pair entity.
    /// </summary>
    public static class ParameterSetKeyDataPairExtensions
    {
        /// <summary>
        /// Projects a key data pair onto the values needed to apply it to a commodity complement. The key data pair is
        /// not modified. An attribute that the key data pair does not hold is projected as null.
        /// </summary>
        /// <param name="keyDataPair">The key data pair.</param>
        /// <returns>The context holding the key, the value and the references to the importer notification and the commodity complement parameter set.</returns>
        public static ApplyCommodityComplementKeyDataContext ToApplyCommodityComplementKeyDataContext(this defraimp_parametersetkeydatapair keyDataPair)
        {
            if (keyDataPair == null)
            {
                throw new ArgumentNullException(nameof(keyDataPair));
            }

            return new ApplyCommodityComplementKeyDataContext(
                keyDataPair.defraimp_name,
                keyDataPair.defraimp_data,
                keyDataPair.defraimp_ImporterNotificationId?.Id,
                keyDataPair.defraimp_CommodityComplementParameterSet?.Id,
                keyDataPair.defraimp_CommodityComplementParameterSet?.Name);
        }
    }
}
