namespace Defra.Imports.Dataverse.Projections
{
    using System;

    /// <summary>
    /// The values of a parameter set key data pair that are needed to apply it to a commodity complement. Use
    /// <see cref="Defra.Imports.Dataverse.Extensions.ParameterSetKeyDataPairExtensions.ToApplyCommodityComplementKeyDataContext(Defra.Imports.Dataverse.Model.defraimp_parametersetkeydatapair)"/>
    /// to project a key data pair onto it.
    /// </summary>
    public sealed class ApplyCommodityComplementKeyDataContext
    {
        /// <summary>Initializes a new instance of the <see cref="ApplyCommodityComplementKeyDataContext"/> class.</summary>
        /// <param name="key">The key (name) of the key data pair, if any.</param>
        /// <param name="data">The value of the key data pair, if any.</param>
        /// <param name="importerNotificationId">The identifier of the importer notification that the key data pair belongs to, if any.</param>
        /// <param name="commodityComplementParameterSetId">The identifier of the commodity complement parameter set that the key data pair belongs to, if any.</param>
        /// <param name="commodityComplementParameterSetName">
        /// The name of the commodity complement parameter set, if the reference carried one. It holds the complement
        /// identifier. A reference set through an <c>@odata.bind</c> on Create has no name.
        /// </param>
        public ApplyCommodityComplementKeyDataContext(
            string key,
            string data,
            Guid? importerNotificationId,
            Guid? commodityComplementParameterSetId,
            string commodityComplementParameterSetName)
        {
            this.Key = key;
            this.Data = data;
            this.ImporterNotificationId = importerNotificationId;
            this.CommodityComplementParameterSetId = commodityComplementParameterSetId;
            this.CommodityComplementParameterSetName = commodityComplementParameterSetName;
        }

        /// <summary>Gets the key (name) of the key data pair.</summary>
        public string Key { get; }

        /// <summary>Gets the value of the key data pair.</summary>
        public string Data { get; }

        /// <summary>Gets the identifier of the importer notification that the key data pair belongs to.</summary>
        public Guid? ImporterNotificationId { get; }

        /// <summary>Gets the identifier of the commodity complement parameter set that the key data pair belongs to.</summary>
        public Guid? CommodityComplementParameterSetId { get; }

        /// <summary>Gets the name of the commodity complement parameter set, if the reference carried one.</summary>
        public string CommodityComplementParameterSetName { get; }
    }
}
