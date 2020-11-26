namespace Defra.Imports.Tests.Integration
{
    using System;
    using System.Globalization;
    using System.Net;
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Tooling.Connector;

    /// <summary>
    /// A base class for integration tests ran against a Common Data Service environment.
    /// </summary>
    public class CommonDataServiceFixture : IDisposable
    {
        private const string AdminAlias = "ADMIN";
        private const string EnvironmentVariableUrl = "CDS_TEST_IMPORTS_URL";
        private const string EnvironmentVariableFormatUsername = "CDS_TEST_{0}_USERNAME";
        private const string EnvironmentVariableFormatPassword = "CDS_TEST_{0}_PASSWORD";
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonDataServiceFixture"/> class.
        /// </summary>
        public CommonDataServiceFixture()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            this.AdminTestClient = GetCrmServiceClient(
                EnvironmentVariableUrl,
                AdminAlias);

            IOrganizationService service;
            service = (IOrganizationService)AdminTestClient.OrganizationWebProxyClient != null ? (IOrganizationService)AdminTestClient.OrganizationWebProxyClient : (IOrganizationService)AdminTestClient.OrganizationServiceProxy;

            // Get a system user to send the email (From: field)
            WhoAmIRequest systemUserRequest = new WhoAmIRequest();
            WhoAmIResponse systemUserResponse = (WhoAmIResponse)service.Execute(systemUserRequest);
            this.ExecutingUser = systemUserResponse.UserId;
        }

        public Guid ExecutingUser { get; private set; }

        /// <summary>
        /// Gets a <see cref="CrmServiceClient"/> instance as an admin for the environment under test.
        /// </summary>
        public CrmServiceClient AdminTestClient { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    AdminTestClient.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets a <see cref="CrmServiceClient"/> instance as a given security role.
        /// </summary>
        /// <param name="userAlias">The alias to use for the test.</param>
        /// <returns>A service client authenticated as the provided role.</returns>
        public static CrmServiceClient GetUserTestClient(string userAlias)
        {
            if (userAlias == null)
            {
                throw new ArgumentNullException(nameof(userAlias));
            }

            return GetCrmServiceClient(
                EnvironmentVariableUrl,
                userAlias);
        }

        private static string GetConnectionString(string url, string username, string password)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("You must provide a URL for the connection string.", nameof(url));
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("You must provide a username for the connection string.", nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("You must provide a password for the connection string.", nameof(password));
            }

            return $"Url={url}; Username={username}; Password={password}; authtype=Office365; RequireNewInstance=true";
        }

        private static CrmServiceClient GetCrmServiceClient(string urlEnvironmentVariable, string userAlias)
        {
            if (string.IsNullOrEmpty(urlEnvironmentVariable))
            {
                throw new ArgumentException("You must provide the name of an environment variable containing the URL.", nameof(urlEnvironmentVariable));
            }

            if (string.IsNullOrEmpty(userAlias))
            {
                throw new ArgumentException("You must provide an alias to use for the client.", nameof(userAlias));
            }

            var url = Environment.GetEnvironmentVariable(urlEnvironmentVariable);
            var usernameEnvironmentVariable = string.Format(CultureInfo.InvariantCulture, EnvironmentVariableFormatUsername, userAlias.ToUpper(CultureInfo.InvariantCulture));
            var username = Environment.GetEnvironmentVariable(usernameEnvironmentVariable);
            var passwordEnvironmentVariable = string.Format(CultureInfo.InvariantCulture, EnvironmentVariableFormatPassword, userAlias.ToUpper(CultureInfo.InvariantCulture));
            var password = Environment.GetEnvironmentVariable(passwordEnvironmentVariable);

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new Exception($"One or more of the following environment variables were not set: {urlEnvironmentVariable}, {usernameEnvironmentVariable}, {passwordEnvironmentVariable}.");
            }

            var client = new CrmServiceClient(GetConnectionString(url, username, password));
            if (client.LastCrmException != null)
            {
                throw client.LastCrmException;
            }

            return client;
        }
    }
}
