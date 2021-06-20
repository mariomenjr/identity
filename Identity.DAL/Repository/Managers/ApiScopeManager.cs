using System;
using System.Collections.Generic;
using System.Linq;
using Identity.DAL.Mongo.Repository;
using Identity.DAL.Mongo.Settings;
using Identity.DAL.Repository.Services;
using Identity.Entities.Model;
using MongoDB.Bson;

namespace Identity.DAL.Repository.Managers
{
    public class ApiScopeManager : MongoRepository<ApiScope>, IApiScopeService
    {
        public ApiScopeManager(IMongoSettings settings) : base(settings)
        {
        }

        public IEnumerable<ApiScope> FindApiScopesByNames(IEnumerable<string> scopeNames)
        {
            return this.AsQueryable().Where(aS => scopeNames.Contains(aS.Name));
        }

        public IEnumerable<ApiScope> FindApiScopesByIds(IEnumerable<ObjectId> apiScopesIds)
        {
            return this.AsQueryable().Where(aS => apiScopesIds.Contains(aS.Id));
        }
    }
}