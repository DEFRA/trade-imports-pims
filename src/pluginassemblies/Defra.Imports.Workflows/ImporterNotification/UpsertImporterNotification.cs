namespace Defra.Imports.Workflows.ImporterNotification
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using Defra.Imports.BusinessLogic.ImporterNotification;
    using Defra.Imports.BusinessLogic.Logging;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    /// <summary>
    /// Create or update importer notification and related records from ASB message received.
    /// </summary>
    public class UpsertImporterNotification : WorkflowActivity
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
        /// Gets or sets the response.
        /// </summary>
        [Output("Message")]
        [RequiredArgument]
        public OutArgument<string> Message { get; set; }

        /// <inheritdoc />
        internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
        {
            ProcessINSASBMessage processMessage = new ProcessINSASBMessage(orgSvc, logWriter);
            try
            {
                var response = processMessage.UpsertImporterNotification(this.ASBMessage.Get(context));
                logWriter.Log(Severity.Info, "UpsertImporterNotification", $"Response: {response.Item1}, Message: {response.Item2}");
                this.Response.Set(context, response.Item1);
                this.Message.Set(context, response.Item2);
            }
            catch (Exception ex)
            {
                var error = $"Error processing ASB message: {ex.Message}";
                logWriter.Log(Severity.Error, "UpsertImporterNotification", error);
                this.Response.Set(context, false);
                this.Message.Set(context, error);
            }
        }
    }
}
