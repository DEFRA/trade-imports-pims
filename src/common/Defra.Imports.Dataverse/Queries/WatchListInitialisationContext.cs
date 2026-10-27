namespace Defra.Imports.Dataverse.Queries
{
    using System;

    /// <summary>
    /// ILLUSTRATIVE EXAMPLE of the pattern in architecture/adr/0001-plugin-workflow-di-scaffold.md: a multi-table
    /// query projection (the owning user and, optionally, the transporter) that is not an SDK entity. It is not a
    /// real business process. It must not be registered with Dataverse or included in a solution, and must not be
    /// extended or copied as a template for entity or business choices.
    /// </summary>
    public sealed class WatchListInitialisationContext
    {
        /// <summary>Initializes a new instance of the <see cref="WatchListInitialisationContext"/> class.</summary>
        /// <param name="ownerId">The identifier of the owning user.</param>
        /// <param name="ownerName">The full name of the owning user.</param>
        /// <param name="ownerEmail">The internal email address of the owning user.</param>
        /// <param name="transporterName">The transporter name, or <see langword="null"/> if there is no transporter.</param>
        public WatchListInitialisationContext(
            Guid ownerId, string ownerName, string ownerEmail, string transporterName)
        {
            this.OwnerId = ownerId;
            this.OwnerName = ownerName;
            this.OwnerEmail = ownerEmail;
            this.TransporterName = transporterName;
        }

        /// <summary>Gets the identifier of the owning user.</summary>
        public Guid OwnerId { get; }

        /// <summary>Gets the full name of the owning user.</summary>
        public string OwnerName { get; }

        /// <summary>Gets the internal email address of the owning user.</summary>
        public string OwnerEmail { get; }

        /// <summary>Gets the transporter name, or <see langword="null"/> if there is no transporter.</summary>
        public string TransporterName { get; }
    }
}
