namespace Defra.Imports.UnitTests.Support
{
    using System;
    using Defra.Imports.Infrastructure.Plugins;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.PluginTelemetry;
    using Moq;

    // Simulates the services Dataverse supplies to a single plug-in invocation.
    public sealed class PluginRuntime
    {
        public PluginRuntime()
        {
            this.Context.SetupGet(context => context.UserId).Returns(this.UserId);
            this.Context.SetupGet(context => context.OutputParameters).Returns(new ParameterCollection());
            this.Context.SetupGet(context => context.InputParameters).Returns(this.InputParameters);
            this.Factory.Setup(factory => factory.CreateOrganizationService(this.UserId)).Returns(this.Service.Object);
            this.Provider.Setup(provider => provider.GetService(typeof(IPluginExecutionContext))).Returns(this.Context.Object);
            this.Provider.Setup(provider => provider.GetService(typeof(IOrganizationServiceFactory))).Returns(this.Factory.Object);
            this.Provider.Setup(provider => provider.GetService(typeof(ITracingService))).Returns(this.Tracing);
        }

        public Guid UserId { get; } = Guid.NewGuid();

        public Mock<IServiceProvider> Provider { get; } = new Mock<IServiceProvider>();

        public Mock<IPluginExecutionContext> Context { get; } = new Mock<IPluginExecutionContext>();

        public Mock<IOrganizationServiceFactory> Factory { get; } = new Mock<IOrganizationServiceFactory>();

        public Mock<IOrganizationService> Service { get; } = new Mock<IOrganizationService>(MockBehavior.Strict);

        public RecordingTracingService Tracing { get; } = new RecordingTracingService();

        // The step's input parameters, such as the Target entity of a Create.
        public ParameterCollection InputParameters { get; } = new ParameterCollection();

        // Doubles as the channel through which test handlers report what they observed.
        public ParameterCollection OutputParameters => this.Context.Object.OutputParameters;

        public PluginRuntime ForMessage(string messageName, string entityName, Guid entityId)
        {
            this.Context.SetupGet(context => context.MessageName).Returns(messageName);
            this.Context.SetupGet(context => context.PrimaryEntityName).Returns(entityName);
            this.Context.SetupGet(context => context.PrimaryEntityId).Returns(entityId);
            return this;
        }

        // Describes a step registration: the stage (10 pre-validation, 20 pre-operation, 40 post-operation) and mode (0 synchronous, 1 asynchronous).
        public PluginRuntime ForStep(string messageName, string entityName, int stage, int mode)
        {
            this.Context.SetupGet(context => context.Stage).Returns(stage);
            this.Context.SetupGet(context => context.Mode).Returns(mode);
            return this.ForMessage(messageName, entityName, Guid.Empty);
        }

        // A synchronous pre-operation Create of the entity, the shape the pipeline presents to a stage 20 plug-in.
        public PluginRuntime ForPreOperationCreate(Entity target)
        {
            return this.ForStep("Create", target.LogicalName, 20, 0).WithTarget(target);
        }

        public PluginRuntime WithTarget(Entity target)
        {
            this.InputParameters["Target"] = target;
            return this;
        }

        public PluginRuntime WithoutTarget()
        {
            this.InputParameters.Remove("Target");
            return this;
        }

        public PluginRuntime WithLogger(ILogger logger)
        {
            this.Provider.Setup(provider => provider.GetService(typeof(ILogger))).Returns(logger);
            return this;
        }

        public PluginRuntime WithoutService(Type serviceType)
        {
            this.Provider.Setup(provider => provider.GetService(serviceType)).Returns(null);
            return this;
        }

        public PluginRuntime WithoutOrganizationService()
        {
            this.Factory.Setup(factory => factory.CreateOrganizationService(It.IsAny<Guid?>())).Returns((IOrganizationService)null);
            return this;
        }

        public PluginExecution CreateExecution()
        {
            return new PluginExecution(this.Context.Object, this.Service.Object, this.Tracing);
        }
    }
}
