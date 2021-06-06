using Identity.DAL.Mongo.Repository;
using Identity.Entities.Model;

namespace Identity.DAL.Repository.Services
{
    public interface IClientService : IMongoRepository<Client>
    {}
}