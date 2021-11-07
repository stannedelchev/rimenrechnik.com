using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace RhymeDictionary
{
    public class IdentityServerConfig
    {
        private readonly IConfiguration authConfig;

        public IdentityServerConfig(IConfiguration authConfig)
        {
            this.authConfig = authConfig;
        }

        public IEnumerable<ApiResource> GetApiResources()
        {
            var apiName = this.authConfig["IdentityServerOptions:ApiName"];
            var apiDisplayName = this.authConfig["IdentityServerOptions:ApiDisplayName"];

            if (!ValidateStrings(apiName, apiDisplayName))
            {
                return Enumerable.Empty<ApiResource>();
            }

            return new List<ApiResource>
            {
                new ApiResource(apiName, apiDisplayName)
            };
        }

        public IEnumerable<Client> GetClients()
        {
            var clientId = this.authConfig["Shared:AdminClient:ClientId"];
            var clientSecret = this.authConfig["Shared:AdminClient:Secret"];

            if (!ValidateStrings(clientId, clientSecret))
            {
                return Enumerable.Empty<Client>();
            }

            return new List<Client>
            {
                new Client
                {
                    ClientId = clientId,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret(clientSecret.Sha256())
                    },
                    AllowedScopes =
                    {
                        this.authConfig["IdentityServerOptions:ApiName"]
                    }
                }
            };
        }

        public void GetAuthOptions(IdentityServerAuthenticationOptions options)
        {
            var idsSection = this.authConfig.GetSection("IdentityServerOptions");

            var authority = idsSection["Authority"];
            var requireHttpsMetadata = idsSection.GetValue("RequireHttpsMetadata", false);
            var apiName = idsSection["ApiName"];

            if (!ValidateStrings(authority, apiName))
            {
                return;
            }

            options.Authority = authority;
            options.RequireHttpsMetadata = requireHttpsMetadata;
            options.ApiName = apiName;
        }

        private static bool ValidateStrings(params string[] strings)
        {
            return strings.All(str => !string.IsNullOrEmpty(str));
        }
    }
}