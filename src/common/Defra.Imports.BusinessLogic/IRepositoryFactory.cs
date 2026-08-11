namespace Defra.Imports.BusinessLogic
{
    using Defra.Imports.Repositories;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;

    /// <summary>
    /// Factory for repositories.
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Gets the organization service.
        /// </summary>
        IOrganizationService OrganizationService { get; }

        /// <summary>
        /// Get a repository for the given entity.
        /// </summary>
        /// <typeparam name="TContext">The context.</typeparam>
        /// <typeparam name="TEntity">The entity.</typeparam>
        /// <returns>A repository for the given entity.</returns>
        ICrmRepository<TEntity> GetRepository<TContext, TEntity>()
            where TEntity : Entity, new()
            where TContext : OrganizationServiceContext;

        /// <summary>
        /// Gets a repository for the entity passed in.
        /// </summary>
        /// <param name="entityLogicalName">The logical name of the entity to get a repository for.</param>
        /// <returns>A repository for the given entity.</returns>
        ICrmRepository GetRepository(string entityLogicalName);
    }
}