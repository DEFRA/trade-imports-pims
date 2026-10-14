namespace Defra.Imports.UnitTests.BusinessLogic.ParameterSerKeyDataPair
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.BusinessLogic.ParameterSetKeyDataPair;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="AddSpeciesAnimalNumbers"/>.
    /// </summary>
    public class AddSpeciesAnimalNumbersTests
    {
        private readonly Mock<IOrganizationService> orgSvcMock;
        private readonly Mock<ILogWriter> loggerMock;
        private readonly AddSpeciesAnimalNumbers sut;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddSpeciesAnimalNumbersTests"/> class.
        /// </summary>
        public AddSpeciesAnimalNumbersTests()
        {
            this.orgSvcMock = new Mock<IOrganizationService>();
            this.loggerMock = new Mock<ILogWriter>();
            this.sut = new AddSpeciesAnimalNumbers(this.orgSvcMock.Object, this.loggerMock.Object);
        }

        /// <summary>
        /// Tests that when a matching commodity complement is found, its number of animals
        /// field is updated with the key data pair's value.
        /// </summary>
        [Fact]
        public void AddAnimalNumbersToSpecies_WithMatchingCommodityComplement_UpdatesNumberOfAnimals()
        {
            // Arrange
            var importerNotificationId = Guid.NewGuid();
            var commodityComplementId = Guid.NewGuid();

            var keyDataPair = new defraimp_parametersetkeydatapair
            {
                defraimp_ImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, importerNotificationId),
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, Guid.NewGuid()) { Name = "42" },
                defraimp_data = "12",
            };

            var existingComplement = new defraimp_commoditycomplement
            {
                Id = commodityComplementId,
            };

            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_commoditycomplement.EntityLogicalName)))
                .Returns(new EntityCollection(new List<Entity> { existingComplement }));

            defraimp_commoditycomplement updated = null;
            this.orgSvcMock
                .Setup(o => o.Update(It.IsAny<Entity>()))
                .Callback<Entity>(e => updated = (defraimp_commoditycomplement)e);

            // Act
            this.sut.AddAnimalNumbersToSpecies(keyDataPair);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(commodityComplementId, updated.Id);
            Assert.Equal("12", updated.defraimp_NumberofAnimals);

            this.orgSvcMock.Verify(
                o => o.RetrieveMultiple(It.Is<QueryExpression>(qe =>
                    qe.EntityName == defraimp_commoditycomplement.EntityLogicalName &&
                    HasCondition(qe, defraimp_commoditycomplement.Fields.defraimp_ImporterNotificationId, importerNotificationId) &&
                    HasCondition(qe, defraimp_commoditycomplement.Fields.defraimp_complementid, 42))),
                Times.Once);
        }

        /// <summary>
        /// Tests that no update is performed when the key data pair has no importer notification reference.
        /// </summary>
        [Fact]
        public void AddAnimalNumbersToSpecies_WithNullImporterNotificationId_DoesNotUpdateAndLogsWarning()
        {
            // Arrange
            var keyDataPair = new defraimp_parametersetkeydatapair
            {
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, Guid.NewGuid()) { Name = "42" },
                defraimp_data = "12",
            };

            // Act
            this.sut.AddAnimalNumbersToSpecies(keyDataPair);

            // Assert
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
            this.orgSvcMock.Verify(o => o.RetrieveMultiple(It.IsAny<QueryExpression>()), Times.Never);

            this.loggerMock.Verify(
                l => l.Log(
                    Severity.Warning,
                    nameof(AddSpeciesAnimalNumbers),
                    It.Is<string>(m => m.Contains("missing the importer notification"))),
                Times.Once);
        }

        /// <summary>
        /// Tests that no update is performed when the key data pair has no commodity complement
        /// parameter set reference.
        /// </summary>
        [Fact]
        public void AddAnimalNumbersToSpecies_WithNullCommodityComplementParameterSet_DoesNotUpdateAndLogsWarning()
        {
            // Arrange
            var keyDataPair = new defraimp_parametersetkeydatapair
            {
                defraimp_ImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, Guid.NewGuid()),
                defraimp_data = "12",
            };

            // Act
            this.sut.AddAnimalNumbersToSpecies(keyDataPair);

            // Assert
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
            this.orgSvcMock.Verify(o => o.RetrieveMultiple(It.IsAny<QueryExpression>()), Times.Never);

            this.loggerMock.Verify(
                l => l.Log(
                    Severity.Warning,
                    nameof(AddSpeciesAnimalNumbers),
                    It.Is<string>(m => m.Contains("missing the importer notification"))),
                Times.Once);
        }

        /// <summary>
        /// Tests that no update is performed and a warning is logged when the key data pair is null.
        /// </summary>
        [Fact]
        public void AddAnimalNumbersToSpecies_WithNullKeyDataPair_DoesNotUpdateAndLogsWarning()
        {
            // Act
            this.sut.AddAnimalNumbersToSpecies(null);

            // Assert
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
            this.orgSvcMock.Verify(o => o.RetrieveMultiple(It.IsAny<QueryExpression>()), Times.Never);

            this.loggerMock.Verify(
                l => l.Log(
                    Severity.Warning,
                    nameof(AddSpeciesAnimalNumbers),
                    It.Is<string>(m => m.Contains("missing the importer notification"))),
                Times.Once);
        }

        /// <summary>
        /// Tests that no update is performed when the commodity complement parameter set's
        /// display name is null or whitespace.
        /// </summary>
        [Fact]
        public void AddAnimalNumbersToSpecies_WithBlankComplementSetName_DoesNotUpdateAndLogsWarning()
        {
            // Arrange
            var keyDataPair = new defraimp_parametersetkeydatapair
            {
                defraimp_ImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, Guid.NewGuid()),
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, Guid.NewGuid()) { Name = "   " },
                defraimp_data = "12",
            };

            // Act
            this.sut.AddAnimalNumbersToSpecies(keyDataPair);

            // Assert
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
            this.orgSvcMock.Verify(o => o.RetrieveMultiple(It.IsAny<QueryExpression>()), Times.Never);

            this.loggerMock.Verify(
                l => l.Log(
                    Severity.Warning,
                    nameof(AddSpeciesAnimalNumbers),
                    It.Is<string>(m => m.Contains("is not a valid complement ID"))),
                Times.Once);
        }

        /// <summary>
        /// Tests that no update is performed when the commodity complement parameter set's
        /// display name cannot be parsed as an integer.
        /// </summary>
        [Fact]
        public void AddAnimalNumbersToSpecies_WithNonNumericComplementSetName_DoesNotUpdateAndLogsWarning()
        {
            // Arrange
            var keyDataPair = new defraimp_parametersetkeydatapair
            {
                defraimp_ImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, Guid.NewGuid()),
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, Guid.NewGuid()) { Name = "not-a-number" },
                defraimp_data = "12",
            };

            // Act
            this.sut.AddAnimalNumbersToSpecies(keyDataPair);

            // Assert
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
            this.orgSvcMock.Verify(o => o.RetrieveMultiple(It.IsAny<QueryExpression>()), Times.Never);

            this.loggerMock.Verify(
                l => l.Log(
                    Severity.Warning,
                    nameof(AddSpeciesAnimalNumbers),
                    It.Is<string>(m => m.Contains("is not a valid complement ID"))),
                Times.Once);
        }

        /// <summary>
        /// Tests that no update is performed when no matching commodity complement is found.
        /// </summary>
        [Fact]
        public void AddAnimalNumbersToSpecies_WithNoMatchingCommodityComplement_DoesNotUpdateAndLogsWarning()
        {
            // Arrange
            var keyDataPair = new defraimp_parametersetkeydatapair
            {
                defraimp_ImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, Guid.NewGuid()),
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, Guid.NewGuid()) { Name = "42" },
                defraimp_data = "12",
            };

            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_commoditycomplement.EntityLogicalName)))
                .Returns(new EntityCollection());

            // Act
            this.sut.AddAnimalNumbersToSpecies(keyDataPair);

            // Assert
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);

            this.loggerMock.Verify(
                l => l.Log(
                    Severity.Warning,
                    nameof(AddSpeciesAnimalNumbers),
                    It.Is<string>(m => m.Contains("No matching commodity complement found"))),
                Times.Once);
        }

        /// <summary>
        /// Tests that when the commodity complement parameter set reference has no Name (e.g. when
        /// set via an @odata.bind on Create), the parameter set record is retrieved to resolve its
        /// name, and the retrieved value is used to find and update the matching commodity complement.
        /// </summary>
        [Fact]
        public void AddAnimalNumbersToSpecies_WithNoParameterSetReferenceName_RetrievesParameterSetAndUpdatesNumberOfAnimals()
        {
            // Arrange
            var importerNotificationId = Guid.NewGuid();
            var commodityComplementId = Guid.NewGuid();
            var parameterSetId = Guid.NewGuid();

            var keyDataPair = new defraimp_parametersetkeydatapair
            {
                defraimp_ImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, importerNotificationId),
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, parameterSetId),
                defraimp_data = "12",
            };

            var parameterSet = new defraimp_commoditycomplementparameterset
            {
                Id = parameterSetId,
                defraimp_name = "42",
            };

            this.orgSvcMock
                .Setup(o => o.Retrieve(
                    defraimp_commoditycomplementparameterset.EntityLogicalName,
                    parameterSetId,
                    It.Is<ColumnSet>(cs => cs.Columns.Contains(defraimp_commoditycomplementparameterset.Fields.defraimp_name))))
                .Returns(parameterSet);

            var existingComplement = new defraimp_commoditycomplement
            {
                Id = commodityComplementId,
            };

            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_commoditycomplement.EntityLogicalName)))
                .Returns(new EntityCollection(new List<Entity> { existingComplement }));

            defraimp_commoditycomplement updated = null;
            this.orgSvcMock
                .Setup(o => o.Update(It.IsAny<Entity>()))
                .Callback<Entity>(e => updated = (defraimp_commoditycomplement)e);

            // Act
            this.sut.AddAnimalNumbersToSpecies(keyDataPair);

            // Assert
            this.orgSvcMock.Verify(
                o => o.Retrieve(
                    defraimp_commoditycomplementparameterset.EntityLogicalName,
                    parameterSetId,
                    It.IsAny<ColumnSet>()),
                Times.Once);

            Assert.NotNull(updated);
            Assert.Equal(commodityComplementId, updated.Id);
            Assert.Equal("12", updated.defraimp_NumberofAnimals);

            this.orgSvcMock.Verify(
                o => o.RetrieveMultiple(It.Is<QueryExpression>(qe =>
                    qe.EntityName == defraimp_commoditycomplement.EntityLogicalName &&
                    HasCondition(qe, defraimp_commoditycomplement.Fields.defraimp_ImporterNotificationId, importerNotificationId) &&
                    HasCondition(qe, defraimp_commoditycomplement.Fields.defraimp_complementid, 42))),
                Times.Once);
        }

        private static bool HasCondition(QueryExpression query, string attributeName, object expectedValue)
        {
            foreach (var condition in query.Criteria.Conditions)
            {
                if (condition.AttributeName == attributeName &&
                    condition.Operator == ConditionOperator.Equal &&
                    condition.Values.Count == 1 &&
                    Equals(condition.Values[0], expectedValue))
                {
                    return true;
                }
            }

            return false;
        }
    }
}