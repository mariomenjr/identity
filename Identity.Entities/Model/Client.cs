using Identity.Entities.Mongo;
using Identity.Entities.Utils.Attributes;

namespace Identity.Entities.Model
{
    [BsonCollection("Clients")]
    public class Client : Document
    {
        public string Name { get; set; }
    }
}