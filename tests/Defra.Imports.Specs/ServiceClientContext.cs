namespace Defra.Imports.Specs
{
    using System.Collections.Generic;
    using Defra.Imports.Scenarios;
    using Microsoft.PowerPlatform.Dataverse.Client;

    /// <summary>
    /// Shared context for accessing <see cref="ServiceClient"/> instances.
    /// </summary>
    public class ServiceClientContext
    {
        private readonly ServiceClientFactory clientFactory;
        private readonly Dictionary<Persona, ServiceClient> personaClients;

        private ServiceClient appUserClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClientContext"/> class.
        /// </summary>
        /// <param name="clientFactory">The <see cref="ServiceClientFactory"/>.</param>
        public ServiceClientContext(ServiceClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            this.personaClients = new Dictionary<Persona, ServiceClient>();
        }

        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance authenticated with the configured application user.
        /// </summary>
        /// <returns>A <see cref="ServiceClient"/> instance authenticated as the configured application user.</returns>
        public ServiceClient GetAppUserClient()
        {
            if (this.appUserClient == null)
            {
                this.appUserClient = this.clientFactory.GetAppUserClient();
            }

            return this.appUserClient;
        }

        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance authenticated as the given persona.
        /// </summary>
        /// <param name="persona">The user persona to authenticate.</param>
        /// <param name="useExisting">Whether to use a previously instantiated service client for this persona.</param>
        /// <returns>A <see cref="ServiceClient"/> instance authenticated as the given persona.</returns>
        public ServiceClient GetClient(Persona persona, bool useExisting = true)
        {
            if (useExisting && this.personaClients.ContainsKey(persona))
            {
                return this.personaClients[persona];
            }

            var client = this.clientFactory.GetClient(persona);

            if (this.personaClients.ContainsKey(persona))
            {
                this.personaClients[persona] = client;
            }
            else
            {
                this.personaClients.Add(persona, client);
            }

            return client;
        }
    }
}
