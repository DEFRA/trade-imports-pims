namespace Defra.Imports.Workflows.ImporterNotification
{
    using System;
    using System.Activities;
    using Defra.Imports.BusinessLogic;
    using Defra.Imports.BusinessLogic.ImporterNotification;
    using Defra.Imports.BusinessLogic.Logging;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    /// <summary>
    /// Create or update importer notification and related records from ASB message received.
    /// </summary>
    [CrmPluginRegistration(
        nameof(UpsertImporterNotification),
        "Upsert Importer Notification",
        "Create or update importer notification and related records from an ASB message",
        "Defra.Imports.Workflows.ImporterNotification",
        IsolationModeEnum.Sandbox)]
    public class UpsertImporterNotification : Defra.Imports.Workflows.WorkflowActivity
    {
        /// <summary>
        /// Gets or sets the ASB message received from INS portal.
        /// </summary>
        [Input("ASBMessage")]
        [RequiredArgument]
        public InArgument<string> ASBMessage { get; set; }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        [Output("Response")]
        [RequiredArgument]
        public OutArgument<bool> Response { get; set; }

        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        [Output("Message")]
        [RequiredArgument]
        public OutArgument<string> Message { get; set; }

        /// <summary>
        /// Gets or sets the category of the response.
        /// </summary>
        [Output("Category")]
        [RequiredArgument]
        public OutArgument<string> Category { get; set; }

        /// <inheritdoc />
        internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
        {
            ProcessINSASBMessage processMessage = new ProcessINSASBMessage(orgSvc, logWriter);
            try
            {
                var response = processMessage.UpsertImporterNotification(this.ASBMessage.Get(context));
                logWriter.Log(Severity.Info, "UpsertImporterNotification", $"Response: {response.Item1}, Category: {response.Item2}, Message: {response.Item3}");
                this.Response.Set(context, response.Item1);
                this.Category.Set(context, response.Item2);
                this.Message.Set(context, response.Item3);
            }
            catch (Exception ex)
            {
                var error = $"Error processing ASB message: {ex.Message}";
                logWriter.Log(Severity.Error, "UpsertImporterNotification", error);
                this.Response.Set(context, false);
                this.Category.Set(context, "error");
                this.Message.Set(context, error);
            }
        }
    }
}
