using Defra.Imports.Model;
using Defra.Imports.Workflows.ImportApplication;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Defra.Imports.Tests.Unit.Workflows.ImportApplication
{
    public class DetermineInspectionRequirementTests : WorkflowActivityTests<DetermineInspectionRequirement>
    {

        [Fact]
        public void Execute_AValidImportApplication_RunsSuccessfully()
        {
            // Arrange
            Guid importApplicationId = Guid.NewGuid();
            Guid countryOfOriginId = Guid.NewGuid();
            Guid commodityTypeId = new Guid();
            IDictionary<string, object> inputs = GetInputs(importApplicationId);

            SetupOrgServiceMock(importApplicationId);

            // Act
            this.WorkflowInvoker.Invoke(inputs);

            // Assert that workflow completed execution
        }

        private void SetupOrgServiceMock(Guid importApplicationId)
        {
            SetupOrgServiceMockRetrieve(importApplicationId);
            SetupOrgServiceMockRetrieveMultiple();
        }

        private void SetupOrgServiceMockRetrieve(Guid importApplicationId)
        {
            OrgSvcMock.Setup(o => o.Retrieve(defraimp_importapplication.EntityLogicalName, importApplicationId, It.IsAny<ColumnSet>())).Returns(() =>
            {
                return new defraimp_importapplication()
                {
                    defraimp_name = "test import application",
                    Id = importApplicationId,
                    defraimp_importapplicationId = importApplicationId,
                };
            });
        }

        private void SetupOrgServiceMockRetrieveMultiple()
        {
            OrgSvcMock.Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_autonumber.EntityLogicalName))).Returns(() =>
            {
                defraimp_autonumber autonumberEntity = new defraimp_autonumber();
                EntityCollection col = new EntityCollection(new List<Entity>() { autonumberEntity });

                return col;
            });
        }

        private IDictionary<string, object> GetInputs(Guid importApplicationId)
        {
            EntityReference importApplicationReference = new EntityReference(defraimp_importapplication.EntityLogicalName, importApplicationId);

            return new Dictionary<string, object>()
            {
                { "ImportApplication", importApplicationReference },
            };
        }

    }
}
