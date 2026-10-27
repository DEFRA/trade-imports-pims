namespace Defra.Imports.Workflows.ImportApplication
{
    using System.Activities;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Workflows;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    /// <summary>
    /// Determines whether an import application should be inspected, as if it had just been created. The class
    /// name, the activity registration and the argument are retained;
    /// <see cref="DetermineInspectionRequirementHandler"/> adapts the argument and the shared application service
    /// does the work.
    /// </summary>
    public sealed class DetermineInspectionRequirement : WorkflowActivityBase<DetermineInspectionRequirementHandler>
    {
        /// <summary>Initializes a new instance of the <see cref="DetermineInspectionRequirement"/> class.</summary>
        public DetermineInspectionRequirement()
            : base(WorkflowCompositionRoot.Root)
        {
        }

        /// <summary>Gets or sets the import application whose inspection requirement is determined.</summary>
        [Input("Import Application")]
        [ReferenceTarget("defraimp_importapplication")]
        [RequiredArgument]
        public InArgument<EntityReference> ImportApplication { get; set; }

        /// <inheritdoc/>
        protected override void RegisterActivity(PluginContainer scope)
        {
            scope.Register<DetermineInspectionRequirement>(this);
        }
    }
}
