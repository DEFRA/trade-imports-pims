namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// The import application that an inspection requirement is determined for. It holds the values the application
    /// was projected with and tracks whether the application has been counted, which the counter managers change as
    /// they work. The projected context itself is never modified.
    /// </summary>
    public sealed class InspectionRecord
    {
        /// <summary>Initializes a new instance of the <see cref="InspectionRecord"/> class.</summary>
        /// <param name="context">The values of the import application.</param>
        public InspectionRecord(InspectionImportApplicationContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.ImportRecordCounted = context.ImportRecordCounted;
        }

        /// <summary>Gets the values of the import application.</summary>
        public InspectionImportApplicationContext Context { get; }

        /// <summary>Gets the identifier of the import application.</summary>
        public Guid Id => this.Context.Id;

        /// <summary>Gets or sets whether the import application has been counted, as tracked by this run.</summary>
        public bool? ImportRecordCounted { get; set; }

        /// <summary>Creates a new reference to the import application.</summary>
        /// <returns>A reference that is not shared with anything else.</returns>
        public EntityReference ToEntityReference()
        {
            return new EntityReference(defraimp_importapplication.EntityLogicalName, this.Id);
        }
    }
}
