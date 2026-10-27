namespace Defra.Imports.Workflows.ImportApplication
{
    using System;
    using System.Activities;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Infrastructure.Workflows;

    /// <summary>
    /// Adapts the import application argument of the <see cref="DetermineInspectionRequirement"/> activity to the
    /// <see cref="IInspectionRequirementDeterminer"/>. Any failure is not caught, as the legacy activity did not
    /// catch it.
    /// </summary>
    public sealed class DetermineInspectionRequirementHandler : IWorkflowActivityHandler
    {
        private readonly DetermineInspectionRequirement activity;
        private readonly CodeActivityContext context;
        private readonly IInspectionRequirementDeterminer determiner;

        /// <summary>Initializes a new instance of the <see cref="DetermineInspectionRequirementHandler"/> class.</summary>
        /// <param name="activity">The exact host-created activity instance for this invocation.</param>
        /// <param name="context">The current workflow execution context.</param>
        /// <param name="determiner">The shared inspection requirement determiner.</param>
        public DetermineInspectionRequirementHandler(
            DetermineInspectionRequirement activity,
            CodeActivityContext context,
            IInspectionRequirementDeterminer determiner)
        {
            this.activity = activity ?? throw new ArgumentNullException(nameof(activity));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.determiner = determiner ?? throw new ArgumentNullException(nameof(determiner));
        }

        /// <inheritdoc/>
        public void Execute()
        {
            // The retrieve and the determination are one application command.
            this.determiner.DetermineForImportApplication(this.activity.ImportApplication.Get(this.context).Id);
        }
    }
}
