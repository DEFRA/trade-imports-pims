namespace Defra.Imports.Dataverse.Contexts
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Execution;

    /// <summary>Creates explicitly identity-bound contexts that remain owned by the caller.</summary>
    public sealed class ImportsContextFactory : IImportsContextFactory
    {
        private readonly IExecutionServiceProvider serviceProvider;

        /// <summary>Initializes a new instance of the <see cref="ImportsContextFactory"/> class.</summary>
        /// <param name="serviceProvider">The current invocation's identity-aware service provider.</param>
        public ImportsContextFactory(IExecutionServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <inheritdoc/>
        public ImportsContext CreateForExecutingUser()
        {
            return new ImportsContext(this.serviceProvider.GetExecutingUserService());
        }

        /// <inheritdoc/>
        public ImportsContext CreateForInitiatingUser()
        {
            return new ImportsContext(this.serviceProvider.GetInitiatingUserService());
        }

        /// <inheritdoc/>
        public ImportsContext CreateForSystem()
        {
            return new ImportsContext(this.serviceProvider.GetSystemService());
        }
    }
}
