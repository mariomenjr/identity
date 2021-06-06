using System.Collections.Generic;
using Identity.DAL.Mongo.Repository;
using Identity.Entities.Model;

namespace Identity.DAL.Repository.Services
{
    public interface IApiScopeService : IMongoRepository<ApiScope>
    {
        IEnumerable<ApiScope> FindApiScopesByName(IEnumerable<string> scopeNames);
    }
}