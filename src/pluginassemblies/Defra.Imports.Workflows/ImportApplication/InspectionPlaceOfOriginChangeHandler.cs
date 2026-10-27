namespace Defra.Imports.Workflows.ImportApplication
{
    using System;
    using System.Activities;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Infrastructure.Workflows;

    /// <summary>
    /// Adapts the import application argument of the <see cref="InspectionPlaceOfOriginChange"/> activity to the
    /// <see cref="IInspectionPlaceOfOriginChanger"/>. Any failure is not caught, as the legacy activity did not
    /// catch it.
    /// </summary>
    public sealed class InspectionPlaceOfOriginChangeHandler : IWorkflowActivityHandler
    {
        private readonly InspectionPlaceOfOriginChange activity;
        private readonly CodeActivityContext context;
        private readonly IInspectionPlaceOfOriginChanger changer;

        /// <summary>Initializes a new instance of the <see cref="InspectionPlaceOfOriginChangeHandler"/> class.</summary>
        /// <param name="activity">The exact host-created activity instance for this invocation.</param>
        /// <param name="context">The current workflow execution context.</param>
        /// <param name="changer">The shared place of origin changer.</param>
        public InspectionPlaceOfOriginChangeHandler(
            InspectionPlaceOfOriginChange activity,
            CodeActivityContext context,
            IInspectionPlaceOfOriginChanger changer)
        {
            this.activity = activity ?? throw new ArgumentNullException(nameof(activity));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.changer = changer ?? throw new ArgumentNullException(nameof(changer));
        }

        /// <inheritdoc/>
        public void Execute()
        {
            // The retrieve and the change are one application command.
            this.changer.ChangePlaceOfOrigin(this.activity.ImportApplication.Get(this.context).Id);
        }
    }
}
