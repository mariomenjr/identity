using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.DAL.Repository.Services;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Identity.Stores
{
    public class ResourceStore : IResourceStore
    {
        private readonly IIdentityResourceService _identityResourceService;
        private readonly IApiScopeService _apiScopeService;
        private readonly IApiResourceService _apiResourceService;

        private readonly IdentityResource[] _defaultIdentityResources = new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
        
        public ResourceStore(IIdentityResourceService identityResourceService, IApiScopeService apiScopeService,
            IApiResourceService apiResourceService)
        {
            this._identityResourceService = identityResourceService;
            this._apiScopeService = apiScopeService;
            this._apiResourceService = apiResourceService;
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(
            IEnumerable<string> scopeNames)
        {
            // Online
            var foundIdentityResources = this._identityResourceService
                .FindIdentityResourcesByScopeNames(scopeNames)
                .Select(iR => new IdentityResource {Name = iR.Name});

            // Hardcoded
            return await Task.Run(() => this._defaultIdentityResources.Union(foundIdentityResources));
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            return await Task.Run(() => this._apiScopeService.FindApiScopesByNames(scopeNames).Select(s =>
                new ApiScope()
                {
                    Name = s.Name,
                    DisplayName = s.DisplayName
                }));
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return await Task.Run(() => this._apiResourceService.FindApiResourcesByScopeNameWithApiScopes(scopeNames)
                .Select(apiResource =>
                    new ApiResource()
                    {
                        Name = apiResource.Name,
                        Scopes = apiResource.ApiScopes.Select(s => s.Name).ToList(),
                        ApiSecrets = new List<Secret>
                        {
                            new(apiResource.Secret.Sha256())
                        },
                        UserClaims = new List<string> {"role"} // TODO: From MongoDB
                    }));
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            return await Task.Run(() => this._apiResourceService.FindApiResourcesByNameWithApiScopes(apiResourceNames).Select(apiResource =>
                new ApiResource()
                {
                    Name = apiResource.Name,
                    Scopes = apiResource.ApiScopes.Select(s => s.Name).ToList(),
                    ApiSecrets = new List<Secret>
                    {
                        new(apiResource.Secret.Sha256())
                    },
                    UserClaims = new List<string> {"role"} // TODO: From MongoDB
                }));
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            return await Task.Run(() => new Resources()
            {
                ApiScopes = this._apiScopeService.Find().Select(s => new ApiScope() {Name = s.Name}).ToList(),
                ApiResources = this._apiResourceService.Find().Select(s => new ApiResource {Name = s.Name}).ToList(),
                IdentityResources = this._defaultIdentityResources
                    .Union(this._identityResourceService.Find().Select(s => new IdentityResource() {Name = s.Name}))
                    .ToList()
            });
        }
    }
}