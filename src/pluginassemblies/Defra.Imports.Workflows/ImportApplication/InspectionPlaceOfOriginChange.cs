namespace Defra.Imports.Workflows.ImportApplication
{
    using System.Activities;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Workflows;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    /// <summary>
    /// Manages a change of place of origin on an import application and how it affects the inspection
    /// requirements. The class name, the activity registration and the argument are retained;
    /// <see cref="InspectionPlaceOfOriginChangeHandler"/> adapts the argument and the shared application service
    /// does the work.
    /// </summary>
    public sealed class InspectionPlaceOfOriginChange : WorkflowActivityBase<InspectionPlaceOfOriginChangeHandler>
    {
        /// <summary>Initializes a new instance of the <see cref="InspectionPlaceOfOriginChange"/> class.</summary>
        public InspectionPlaceOfOriginChange()
            : base(WorkflowCompositionRoot.Root)
        {
        }

        /// <summary>Gets or sets the import application whose place of origin has changed.</summary>
        [Input("Import Application")]
        [ReferenceTarget("defraimp_importapplication")]
        [RequiredArgument]
        public InArgument<EntityReference> ImportApplication { get; set; }

        /// <inheritdoc/>
        protected override void RegisterActivity(PluginContainer scope)
        {
            scope.Register<InspectionPlaceOfOriginChange>(this);
        }
    }
}
