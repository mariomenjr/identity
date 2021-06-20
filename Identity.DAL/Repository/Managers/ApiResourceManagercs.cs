using System;
using System.Collections.Generic;
using System.Linq;
using Identity.DAL.Mongo.Repository;
using Identity.DAL.Mongo.Settings;
using Identity.DAL.Repository.Services;
using Identity.Entities.Model;

namespace Identity.DAL.Repository.Managers
{
    public class ApiResourceManager : MongoRepository<ApiResource>, IApiResourceService
    {
        private readonly IApiScopeService _apiScopeService;

        public ApiResourceManager(IMongoSettings settings, IApiScopeService apiScopeService) : base(settings)
        {
            this._apiScopeService = apiScopeService;
        }

        public IEnumerable<ApiResource> FindApiResourcesByScopeNameWithApiScopes(IEnumerable<string> scopeNames)
        {
            // As there might be significantly less ApiResources than ApiScopes
            // All ApiResources are queried
            var apiResources = this.AsQueryable().ToList();
            var apiResourcesIds = apiResources.Select(s => s.Id);

            // But only those ApiScopes in scopeNames are brought
            var apiScopes = this._apiScopeService.FindApiScopesByNames(scopeNames);

            // Deliver final product
            return apiResources
                .Select(apiResource =>
                {
                    apiResource.ApiScopes = apiScopes.Where(w => apiResourcesIds.Contains(w.Id)).ToList();
                    return apiResource;
                })
                .Where(w => w.ApiScopes.Any());
        }

        public IEnumerable<ApiResource> FindApiResourcesByName(IEnumerable<string> apiResourceNames)
        {
            return this.AsQueryable().Where(aR => apiResourceNames.Contains(aR.Name));
        }
    }
}