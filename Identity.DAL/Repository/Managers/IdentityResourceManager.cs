using System.Collections.Generic;
using System.Linq;
using Identity.DAL.Mongo.Repository;
using Identity.DAL.Mongo.Settings;
using Identity.DAL.Repository.Services;
using Identity.Entities.Model;

namespace Identity.DAL.Repository.Managers
{
    public class IdentityResourceManager : MongoRepository<IdentityResource>, IIdentityResourceService
    {
        public IdentityResourceManager(IMongoSettings settings) : base(settings)
        {
        }

        public IEnumerable<IdentityResource> FindIdentityResourcesByScopeNames(IEnumerable<string> scopeNames)
        {
            return this.AsQueryable()
                .Where(iR => scopeNames.Contains(iR.Name));
        }
    }
}