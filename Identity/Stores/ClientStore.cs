using System;
using System.Linq;
using System.Threading.Tasks;
using Identity.DAL.Repository.Services;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Identity.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly IClientService _clientService;
        private readonly IApiScopeService _apiScopeService;
        
        public ClientStore(IClientService clientService, IApiScopeService apiScopeService)
        {
            this._clientService = clientService;
            this._apiScopeService = apiScopeService;
        }
        
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var foundClient = await this._clientService.FindOneAsync(client => client.Name == clientId);
            if (foundClient != null)
            {
                var relatedApiScopes = this._apiScopeService.FindApiScopesByIds(foundClient.ApiScopeIds);
                
                return new Client()
                {
                    ClientId =  foundClient.Name,
                    AccessTokenType = AccessTokenType.Jwt,

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret(Environment.GetEnvironmentVariable("CONTINUEE__CLIENT_SECRET").Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = relatedApiScopes.Select(s => s.Name).ToList(),
                };
            }
            return null;
        }
    }
}