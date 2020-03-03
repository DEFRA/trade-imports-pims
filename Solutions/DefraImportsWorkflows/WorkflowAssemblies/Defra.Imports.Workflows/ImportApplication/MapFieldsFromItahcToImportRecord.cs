using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Extensions;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic.Utils;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Workflows.ImportApplication
{
  [CrmPluginRegistration(
    nameof(MapFieldsFromItahcToImportRecord),
    "Maps fields from one record to another using a mapping web resource",
    "Maps fields from one record to another using a xml mapping web resource",
    "Defra.Imports.Workflows.ImportApplication",
    IsolationModeEnum.Sandbox)]
  public class MapFieldsFromItahcToImportRecord : WorkflowActivity
  {
    [Input("ITAHC Record to map fields from")]
    [RequiredArgument]
    [ReferenceTarget("defraimp_itahc")]
    public InArgument<EntityReference> ITAHCToMapFrom { get; set; }

    [Input("Import Record to map fields to")]
    [RequiredArgument]
    [ReferenceTarget("defraimp_importapplication")]
    public InArgument<EntityReference> ImportRecordToMapTo { get; set; }

    [Input("Field Mapping XML Web Resource")]
    [RequiredArgument]
    [ReferenceTarget("webresource")]
    public InArgument<EntityReference> FieldMappingResource { get; set; }

    [Input("Overwrite existing fields?")]
    [RequiredArgument]
    public InArgument<bool> OverwriteExistingFields { get; set; }

    internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
    {
      EntityReference itahcToMapFromRef = ITAHCToMapFrom.GetRequired(context, nameof(this.ITAHCToMapFrom));
      EntityReference importRecordToMapToRef = ImportRecordToMapTo.GetRequired(context, nameof(this.ImportRecordToMapTo));
      EntityReference fieldMappingResourceRef = FieldMappingResource.GetRequired(context, nameof(this.FieldMappingResource));
      bool shouldOverwriteExisting = OverwriteExistingFields.GetRequired(context, nameof(this.OverwriteExistingFields));

      IRepositoryFactory repoFactory = new RepositoryFactory(orgSvc);

      MapFieldsFromOneRecordToAnotherBusinessLogic mapFieldsBusinessLogic = new MapFieldsFromOneRecordToAnotherBusinessLogic(repoFactory, itahcToMapFromRef, importRecordToMapToRef, fieldMappingResourceRef, shouldOverwriteExisting);
      mapFieldsBusinessLogic.RunLogic();
    }
  }
}
