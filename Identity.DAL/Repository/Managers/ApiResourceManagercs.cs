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
        public ApiResourceManager(IMongoSettings settings) : base(settings)
        {
        }
        
        public IEnumerable<ApiResource> FindApiResourcesByScopeName(IEnumerable<string> scopeNames)
        {
            return this.AsQueryable(); // TODO: Relate both ApiScope and ApiResource entities
        }

        public IEnumerable<ApiResource> FindApiResourcesByName(IEnumerable<string> apiResourceNames)
        {
            return this.AsQueryable().Where(aR => apiResourceNames.Contains(aR.Name));
        }
    }
}