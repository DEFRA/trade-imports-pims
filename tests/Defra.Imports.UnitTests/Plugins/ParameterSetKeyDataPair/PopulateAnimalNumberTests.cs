namespace Defra.Imports.UnitTests.Plugins.ParameterSetKeyDataPair
{
    using System;
    using Defra.Imports.Model;
    using Defra.Imports.Plugins.ParameterSetKeyDataPair;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="PopulateAnimalNumber"/>.
    /// </summary>
    public class PopulateAnimalNumberTests
    {
        private readonly Mock<IServiceProvider> serviceProviderMock;
        private readonly Mock<ITracingService> tracingServiceMock;
        private readonly Mock<IPluginExecutionContext> pluginContextMock;
        private readonly Mock<IOrganizationServiceFactory> orgSvcFactoryMock;
        private readonly Mock<IOrganizationService> orgSvcMock;
        private readonly PopulateAnimalNumber sut;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulateAnimalNumberTests"/> class.
        /// </summary>
        public PopulateAnimalNumberTests()
        {
            this.serviceProviderMock = new Mock<IServiceProvider>();
            this.tracingServiceMock = new Mock<ITracingService>();
            this.pluginContextMock = new Mock<IPluginExecutionContext>();
            this.orgSvcFactoryMock = new Mock<IOrganizationServiceFactory>();
            this.orgSvcMock = new Mock<IOrganizationService>();

            this.orgSvcFactoryMock
                .Setup(f => f.CreateOrganizationService(It.IsAny<Guid?>()))
                .Returns(this.orgSvcMock.Object);

            this.serviceProviderMock
                .Setup(s => s.GetService(typeof(ITracingService)))
                .Returns(this.tracingServiceMock.Object);
            this.serviceProviderMock
                .Setup(s => s.GetService(typeof(IPluginExecutionContext)))
                .Returns(this.pluginContextMock.Object);
            this.serviceProviderMock
                .Setup(s => s.GetService(typeof(IOrganizationServiceFactory)))
                .Returns(this.orgSvcFactoryMock.Object);

            this.sut = new PopulateAnimalNumber();
        }

        /// <summary>
        /// Tests that on Create, when the key data pair is for the number of animals, the
        /// business logic is invoked using the Target entity and the commodity complement is updated.
        /// </summary>
        [Fact]
        public void Execute_OnCreateWithNumberAnimalKey_UpdatesCommodityComplement()
        {
            // Arrange
            var importerNotificationId = Guid.NewGuid();
            var commodityComplementId = Guid.NewGuid();

            var target = new defraimp_parametersetkeydatapair
            {
                defraimp_name = "number_animal",
                defraimp_ImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, importerNotificationId),
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, Guid.NewGuid()) { Name = "42" },
                defraimp_data = "7",
            };

            this.pluginContextMock.Setup(c => c.MessageName).Returns("Create");
            this.pluginContextMock.Setup(c => c.InputParameters).Returns(new ParameterCollection { { "Target", target } });

            var existingComplement = new defraimp_commoditycomplement { Id = commodityComplementId };

            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.IsAny<Microsoft.Xrm.Sdk.Query.QueryExpression>()))
                .Returns(new EntityCollection(new System.Collections.Generic.List<Entity> { existingComplement }));

            defraimp_commoditycomplement updated = null;
            this.orgSvcMock
                .Setup(o => o.Update(It.IsAny<Entity>()))
                .Callback<Entity>(e => updated = (defraimp_commoditycomplement)e);

            // Act
            this.sut.Execute(this.serviceProviderMock.Object);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(commodityComplementId, updated.Id);
            Assert.Equal("7", updated.defraimp_NumberofAnimals);
        }

        /// <summary>
        /// Tests that on Create, when the key data pair's name is not "number_animal", no
        /// commodity complement update is attempted.
        /// </summary>
        [Fact]
        public void Execute_OnCreateWithDifferentKey_DoesNotUpdateCommodityComplement()
        {
            // Arrange
            var target = new defraimp_parametersetkeydatapair
            {
                defraimp_name = "some_other_key",
                defraimp_ImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, Guid.NewGuid()),
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, Guid.NewGuid()) { Name = "42" },
                defraimp_data = "7",
            };

            this.pluginContextMock.Setup(c => c.MessageName).Returns("Create");
            this.pluginContextMock.Setup(c => c.InputParameters).Returns(new ParameterCollection { { "Target", target } });

            // Act
            this.sut.Execute(this.serviceProviderMock.Object);

            // Assert
            this.orgSvcMock.Verify(o => o.RetrieveMultiple(It.IsAny<Microsoft.Xrm.Sdk.Query.QueryExpression>()), Times.Never);
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
        }

        /// <summary>
        /// Tests that on Update, when the key data pair is for the number of animals, the
        /// business logic uses the pre-image entity and updates the commodity complement.
        /// </summary>
        [Fact]
        public void Execute_OnUpdateWithNumberAnimalKey_UsesPreImageAndUpdatesCommodityComplement()
        {
            // Arrange
            var importerNotificationId = Guid.NewGuid();
            var commodityComplementId = Guid.NewGuid();

            var target = new defraimp_parametersetkeydatapair
            {
                defraimp_data = "15",
            };

            var preImage = new defraimp_parametersetkeydatapair
            {
                defraimp_name = "number_animal",
                defraimp_ImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, importerNotificationId),
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, Guid.NewGuid()) { Name = "99" },
            };

            this.pluginContextMock.Setup(c => c.MessageName).Returns("Update");
            this.pluginContextMock.Setup(c => c.InputParameters).Returns(new ParameterCollection { { "Target", target } });
            this.pluginContextMock.Setup(c => c.PreEntityImages).Returns(new EntityImageCollection { { "PreImage", preImage } });

            var existingComplement = new defraimp_commoditycomplement { Id = commodityComplementId };

            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.IsAny<Microsoft.Xrm.Sdk.Query.QueryExpression>()))
                .Returns(new EntityCollection(new System.Collections.Generic.List<Entity> { existingComplement }));

            defraimp_commoditycomplement updated = null;
            this.orgSvcMock
                .Setup(o => o.Update(It.IsAny<Entity>()))
                .Callback<Entity>(e => updated = (defraimp_commoditycomplement)e);

            // Act
            this.sut.Execute(this.serviceProviderMock.Object);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(commodityComplementId, updated.Id);
            Assert.Equal("15", updated.defraimp_NumberofAnimals);
        }

        /// <summary>
        /// Tests that on Update, when the key data pair is for the number of animals, the
        /// business logic uses the pre-image entity (for the reference fields) combined with
        /// the Target's defraimp_data value, and updates the commodity complement.
        /// </summary>
        [Fact]
        public void Execute_OnUpdateWithNumberAnimalKey_MergesPreImageWithTargetDataAndUpdatesCommodityComplement()
        {
            // Arrange
            var importerNotificationId = Guid.NewGuid();
            var commodityComplementId = Guid.NewGuid();

            var target = new defraimp_parametersetkeydatapair
            {
                defraimp_name = "number_animal",
                defraimp_data = "15",
            };

            var preImage = new defraimp_parametersetkeydatapair
            {
                defraimp_name = "number_animal",
                defraimp_ImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, importerNotificationId),
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, Guid.NewGuid()) { Name = "99" },

                // Deliberately different to the Target's value, to prove the Target's defraimp_data takes precedence.
                defraimp_data = "8",
            };

            this.pluginContextMock.Setup(c => c.MessageName).Returns("Update");
            this.pluginContextMock.Setup(c => c.InputParameters).Returns(new ParameterCollection { { "Target", target } });
            this.pluginContextMock.Setup(c => c.PreEntityImages).Returns(new EntityImageCollection { { "PreImage", preImage } });

            var existingComplement = new defraimp_commoditycomplement { Id = commodityComplementId };

            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.IsAny<Microsoft.Xrm.Sdk.Query.QueryExpression>()))
                .Returns(new EntityCollection(new System.Collections.Generic.List<Entity> { existingComplement }));

            defraimp_commoditycomplement updated = null;
            this.orgSvcMock
                .Setup(o => o.Update(It.IsAny<Entity>()))
                .Callback<Entity>(e => updated = (defraimp_commoditycomplement)e);

            // Act
            this.sut.Execute(this.serviceProviderMock.Object);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(commodityComplementId, updated.Id);
            Assert.Equal("15", updated.defraimp_NumberofAnimals);
        }

        /// <summary>
        /// Tests that on Update, when there is no pre-image available, the merged key data pair
        /// is null and no commodity complement update is attempted.
        /// </summary>
        [Fact]
        public void Execute_OnUpdateWithNoPreImage_DoesNotUpdateCommodityComplement()
        {
            // Arrange
            var target = new defraimp_parametersetkeydatapair
            {
                defraimp_name = "number_animal",
                defraimp_data = "15",
            };

            this.pluginContextMock.Setup(c => c.MessageName).Returns("Update");
            this.pluginContextMock.Setup(c => c.InputParameters).Returns(new ParameterCollection { { "Target", target } });
            this.pluginContextMock.Setup(c => c.PreEntityImages).Returns(new EntityImageCollection());

            // Act
            var exception = Record.Exception(() => this.sut.Execute(this.serviceProviderMock.Object));

            // Assert
            Assert.Null(exception);
            this.orgSvcMock.Verify(o => o.RetrieveMultiple(It.IsAny<Microsoft.Xrm.Sdk.Query.QueryExpression>()), Times.Never);
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
        }

        /// <summary>
        /// Tests that an exception thrown during execution is caught and logged via the tracing service
        /// rather than propagating out of the plugin.
        /// </summary>
        [Fact]
        public void Execute_WhenExceptionThrown_LogsErrorAndDoesNotThrow()
        {
            // Arrange
            // Omitting "Target" from InputParameters causes a null reference when cast, triggering the catch block.
            this.pluginContextMock.Setup(c => c.MessageName).Returns("Create");
            this.pluginContextMock.Setup(c => c.InputParameters).Returns(new ParameterCollection());

            // Act
            var exception = Record.Exception(() => this.sut.Execute(this.serviceProviderMock.Object));

            // Assert
            Assert.Null(exception);
            this.tracingServiceMock.Verify(t => t.Trace(It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
        }
    }
}