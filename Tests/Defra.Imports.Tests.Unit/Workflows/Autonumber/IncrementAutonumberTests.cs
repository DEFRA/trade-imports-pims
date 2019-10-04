using Defra.Imports.Model;
using Defra.Imports.Workflows;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using System.Collections.Generic;

using Xunit;

namespace Defra.Imports.Tests.Unit.Workflows.Autonumber
{
    public class IncrementAutonumberTests : WorkflowActivityTests<IncrementAutonumber>
    {
        [Fact]
        public void Execute_WithAName_ShouldCallReposIncrementMethodWithTheName()
        {
            // Arrange
            defraimp_autonumber configEntityStub = new defraimp_autonumber();
            EntityCollection entityColStub = new EntityCollection();
            entityColStub.Entities.Add(configEntityStub);

            // Act
            this.OrgSvcMock.Setup(o => o.RetrieveMultiple(It.IsAny<QueryExpression>())).Returns(() => {
                return entityColStub;
            });

            this.WorkflowInvoker.Invoke(this.GetInputs("test"));

            // Assert
            this.OrgSvcMock.Verify(
                o => o.RetrieveMultiple(It.Is<QueryExpression>(qe =>
                qe.Criteria.Conditions[0].AttributeName == "defraimp_key" &&
                (string)qe.Criteria.Conditions[0].Values[0] == "test")),Times.Once);

            this.OrgSvcMock.Verify(
                o => o.Update(configEntityStub));
        }

        [Fact]
        public void Execute_WithAValidName_ShouldIncrementTheRetrievedEntitiesCounterByOne()
        {
            // Arrange
            defraimp_autonumber configEntityStub = new defraimp_autonumber()
            {
                defraimp_CurrentNumber = 0,
            };
            EntityCollection entityColStub = new EntityCollection();
            entityColStub.Entities.Add(configEntityStub);

            // Act
            this.OrgSvcMock.Setup(o => o.RetrieveMultiple(It.IsAny<QueryExpression>())).Returns(() => {
                return entityColStub;
            });

            this.WorkflowInvoker.Invoke(this.GetInputs("test"));

            // Assert
            Assert.Equal(1, configEntityStub.defraimp_CurrentNumber);
        }

        private IDictionary<string, object> GetInputs(string keyName) 
        {
            return new Dictionary<string, object>()
            {
                { "AutonumberCounterName", keyName },
            };
        }
    }
}

