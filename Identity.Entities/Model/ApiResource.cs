using Identity.Entities.Mongo;
using Identity.Entities.Utils.Attributes;

namespace Identity.Entities.Model
{
    [BsonCollection("ApiResources")]
    public class ApiResource : Document
    {
        public string Name { get; set; }
    }
}