using System.Collections.Generic;
using Identity.DAL.Mongo.Repository;
using Identity.Entities.Model;
using MongoDB.Bson;

namespace Identity.DAL.Repository.Services
{
    public interface IApiScopeService : IMongoRepository<ApiScope>
    {
        IEnumerable<ApiScope> FindApiScopesByNames(IEnumerable<string> scopeNames);
        IEnumerable<ApiScope> FindApiScopesByIds(IEnumerable<ObjectId> apiScopesIds);
    }
}