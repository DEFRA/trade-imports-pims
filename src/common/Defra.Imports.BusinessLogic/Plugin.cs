namespace Defra.Imports.BusinessLogic
{
    using Defra.Imports.BusinessLogic.Logging;
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Linq;

    /// <summary>
    /// Dynamics 365 plugin.
    /// </summary>
    public abstract class Plugin : IPlugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        public Plugin()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        /// <param name="unsecureConfig">The unsecure configuration.</param>
        /// <param name="secureConfig">The secure configuration.</param>
        public Plugin(string unsecureConfig, string secureConfig)
            : this()
        {
            this.UnsecureConfig = unsecureConfig;
            this.SecureConfig = secureConfig;
        }

        /// <summary>
        /// Gets the plugin step's unsecure configuration.
        /// </summary>
        protected string UnsecureConfig { get; private set; }

        /// <summary>
        /// Gets the plugin step's secure configuration.
        /// </summary>
        protected string SecureConfig { get; private set; }

        /// <inheritdoc/>
        public void Execute(IServiceProvider serviceProvider)
        {
            var tracingSvc = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var orgSvc = serviceFactory.CreateOrganizationService(Guid.Empty);
            var repositoryFactory = new RepositoryFactory(orgSvc);
            var logWriter = new TracingServiceLogWriter(tracingSvc, true);

            this.Execute(context, orgSvc, logWriter, repositoryFactory);
        }

        /// <summary>
        /// Execute the plugin.
        /// </summary>
        /// <param name="context">The plugin execution context.</param>
        /// <param name="orgSvc">The organization service.</param>
        /// <param name="logWriter">The log writer.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        protected abstract void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory);

        /// <summary>
        /// Check if the root contexts message name is contained in the custom message names array
        /// </summary>
        /// <param name="context">The execution context</param>
        /// <param name="customMessageNames">An array of custom message names to search for</param>
        /// <returns>True if the root context message name is in the custom message names array</returns>
        protected bool IsRootContextCustomMessage(IPluginExecutionContext context, string[] customMessageNames)
        {
            IPluginExecutionContext currentContext = context;
            while (currentContext.ParentContext != null)
            {
                currentContext = currentContext.ParentContext;
            }

            return this.IsMessageContextCustomMessage(currentContext, customMessageNames);
        }

        /// <summary>
        /// Check if the message context name is contained in the custom message names array
        /// </summary>
        /// <param name="context">The execution context</param>
        /// <param name="customMessageNames">An array of custom message names to search for</param>
        /// <returns>True if the context message name is in the custom message names array</returns>
        protected bool IsMessageContextCustomMessage(IPluginExecutionContext context, string[] customMessageNames)
        {
            return customMessageNames.Contains(context.MessageName);
        }

        /// <summary>
        /// Retrieve a post image entity
        /// </summary>
        /// <typeparam name="T">The entity type of the post image</typeparam>
        /// <param name="context">The plugin execution context</param>
        /// <param name="postImageName">The name of the post image</param>
        /// <returns>The specified post image if it's found</returns>
        protected T GetPostImage<T>(IPluginExecutionContext context, string postImageName) where T : Entity
        {
            return GetImageEntity<T>(postImageName, context.PostEntityImages);
        }

        /// <summary>
        /// Retrieve a pre image entity
        /// </summary>
        /// <typeparam name="T">The entity type of the post image</typeparam>
        /// <param name="context">The plugin execution context</param>
        /// <param name="preImageName">The name of the pre image</param>
        /// <returns>The specified pre image if it's found</returns>
        protected T GetPreImage<T>(IPluginExecutionContext context, string preImageName) where T : Entity
        {
            return GetImageEntity<T>(preImageName, context.PreEntityImages);
        }

        private T GetImageEntity<T>(string imageName, EntityImageCollection entityImages) where T : Entity
        {
            T imageEntity = null;
            if (entityImages.Contains(imageName))
            {
                Entity image = (Entity)entityImages[imageName];
                imageEntity = image.ToEntity<T>();
            }
            return imageEntity;
        }
    }
}
