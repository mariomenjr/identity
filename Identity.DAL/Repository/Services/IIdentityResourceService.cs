using System.Collections.Generic;
using Identity.DAL.Mongo.Repository;
using Identity.Entities.Model;

namespace Identity.DAL.Repository.Services
{
    public interface IIdentityResourceService : IMongoRepository<IdentityResource>
    {
        IEnumerable<IdentityResource> FindIdentityResourcesByScopeNames(IEnumerable<string> scopeNames);
    }
}