using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomAuthenticationExtensions.API.Services
{ 
    public class DynamicsClientDomainService : IDynamicsClientDomainService
    {
        private readonly string tableName;
        private readonly string tenantId;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string dataverseUrl;
        private readonly string authorityUrl;
        private readonly ILogger<DynamicsClientDomainService> _logger;

        public DynamicsClientDomainService()
        {
            tableName = Environment.GetEnvironmentVariable(Consts.EnvironmentVariableNames.ClientDomainsTableName);
            tenantId = Environment.GetEnvironmentVariable(Consts.EnvironmentVariableNames.TenantId);
            clientId = Environment.GetEnvironmentVariable(Consts.EnvironmentVariableNames.ClientId);
            clientSecret = Environment.GetEnvironmentVariable(Consts.EnvironmentVariableNames.ClientSecret);
            dataverseUrl = Environment.GetEnvironmentVariable(Consts.EnvironmentVariableNames.DataverseUrl);
            authorityUrl = $"https://login.microsoftonline.com/{tenantId}";
        }

        public async Task QueryData()
        {
            var accessToken = await GetAccessToken();

            if (accessToken == null || string.IsNullOrEmpty(accessToken))
            {
                return;
            }
        }

        private async Task<string> GetAccessToken()
        {
            try
            {
                if (string.IsNullOrEmpty(tenantId) ||
                    string.IsNullOrEmpty(clientId) ||
                    string.IsNullOrEmpty(clientSecret) ||
                    string.IsNullOrEmpty(dataverseUrl) ||
                    string.IsNullOrEmpty(authorityUrl))
                {
                    throw new Exception("Cannot initiate Dynamics client");
                }

                var app = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority(authorityUrl)
                    .Build();

                var result = await app.AcquireTokenForClient(new[] { $"{dataverseUrl}/.default" }).ExecuteAsync();

                return result.AccessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
