namespace Defra.Imports.Dataverse.Projections
{
    using System;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// The values of an ITAHC that are needed to flag it on the watch lists. Use
    /// <see cref="Defra.Imports.Dataverse.Extensions.ItahcExtensions.ToCreateWatchFlagsContext(Defra.Imports.Dataverse.Model.defraimp_itahc)"/>
    /// to project an ITAHC onto it.
    /// </summary>
    public sealed class CreateItahcWatchFlagsContext
    {
        /// <summary>Initializes a new instance of the <see cref="CreateItahcWatchFlagsContext"/> class.</summary>
        /// <param name="itahc">A reference to the ITAHC that the watch flags are created for.</param>
        /// <param name="name">The name of the ITAHC, which is used in the name of each watch flag.</param>
        /// <param name="placeOfOrigin">The name of the place of origin (harvest), if any.</param>
        /// <param name="placeOfDestination">The name of the place of destination, if any.</param>
        /// <param name="consignee">The name of the consignee, if any.</param>
        /// <param name="transporter">The name of the transporter, if any.</param>
        /// <param name="veterinarian">The name of the official veterinarian, if any.</param>
        public CreateItahcWatchFlagsContext(
            EntityReference itahc,
            string name,
            string placeOfOrigin,
            string placeOfDestination,
            string consignee,
            string transporter,
            string veterinarian)
        {
            this.Itahc = itahc ?? throw new ArgumentNullException(nameof(itahc));
            this.Name = name;
            this.PlaceOfOrigin = placeOfOrigin;
            this.PlaceOfDestination = placeOfDestination;
            this.Consignee = consignee;
            this.Transporter = transporter;
            this.Veterinarian = veterinarian;
        }

        /// <summary>Gets a reference to the ITAHC that the watch flags are created for.</summary>
        public EntityReference Itahc { get; }

        /// <summary>Gets the name of the ITAHC.</summary>
        public string Name { get; }

        /// <summary>Gets the name of the place of origin (harvest).</summary>
        public string PlaceOfOrigin { get; }

        /// <summary>Gets the name of the place of destination.</summary>
        public string PlaceOfDestination { get; }

        /// <summary>Gets the name of the consignee.</summary>
        public string Consignee { get; }

        /// <summary>Gets the name of the transporter.</summary>
        public string Transporter { get; }

        /// <summary>Gets the name of the official veterinarian.</summary>
        public string Veterinarian { get; }
    }
}
