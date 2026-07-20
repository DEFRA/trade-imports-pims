using Defra.Imports.Model;
using Defra.Imports.Workflows.Autonumber;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Defra.Imports.UnitTests.Workflows.Autonumber
{
    public class DecrementAutonumberTests : WorkflowActivityTests<DecrementAutonumber>
    {
        [Fact]
        public void Execute_WithAName_ShouldCallReposDecrementMethodWithTheName()
        {
            // Arrange
            int originalCount = 1;
            defraimp_autonumber autonumberStub = new defraimp_autonumber();
            autonumberStub.defraimp_CurrentNumber = originalCount;

            EntityCollection entityColStub = new EntityCollection();
            entityColStub.Entities.Add(autonumberStub);

            this.OrgSvcMock.Setup(o => o.RetrieveMultiple(It.IsAny<QueryExpression>())).Returns(entityColStub);

            // Act
            this.WorkflowInvoker.Invoke(this.GetInputs("test"));

            // Assert
            this.OrgSvcMock.Verify(o => o.Update(autonumberStub), Times.Once);
            Assert.Equal(0, autonumberStub.defraimp_CurrentNumber);
        }

        private Dictionary<string, object> GetInputs(string keyname)
        {
            return new Dictionary<string, object>()
            {
                { "AutonumberCounterName", keyname }
            };
        }
    }
}
