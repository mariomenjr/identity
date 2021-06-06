using Identity.DAL.Mongo.Repository;
using Identity.DAL.Mongo.Settings;
using Identity.DAL.Repository.Services;
using Identity.Entities.Model;

namespace Identity.DAL.Repository.Managers
{
    public class ClientManager : MongoRepository<Client>, IClientService
    {
        public ClientManager(IMongoSettings settings) : base(settings)
        {}
    }
}