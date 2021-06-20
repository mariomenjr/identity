using System.Collections.Generic;
using Identity.DAL.Mongo.Repository;
using Identity.Entities.Model;

namespace Identity.DAL.Repository.Services
{
    public interface IApiResourceService : IMongoRepository<ApiResource>
    {
        IEnumerable<ApiResource> FindApiResourcesByScopeNameWithApiScopes(IEnumerable<string> scopeNames);
        IEnumerable<ApiResource> FindApiResourcesByName(IEnumerable<string> apiResourceNames);
    }
}