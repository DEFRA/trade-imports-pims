using Defra.Imports.Model;
using Defra.Imports.Workflows.Autonumber;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Defra.Imports.Tests.Unit.Workflows.Autonumber
{
    public class ResetAutonumberTests : WorkflowActivityTests<ResetAutonumber>
    {
        [Fact]
        public void Execute_WithAValidName_ShouldResetTheAutoNumbersCounterToZero()
        {
            // Arrange
            defraimp_autonumber configEntityStub = new defraimp_autonumber()
            {
                defraimp_CurrentNumber = 10
            };
            EntityCollection entityColStub = new EntityCollection();
            entityColStub.Entities.Add(configEntityStub);

            // Act
            this.OrgSvcMock.Setup(o => o.RetrieveMultiple(It.IsAny<QueryExpression>())).Returns(() => {
                return entityColStub;
            });

            this.WorkflowInvoker.Invoke(this.GetInputs("test_autonumber_record"));

            // Assert
            Assert.Equal(configEntityStub.defraimp_CurrentNumber, 0);
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
