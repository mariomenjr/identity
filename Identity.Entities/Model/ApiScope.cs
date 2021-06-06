using Identity.Entities.Mongo;
using Identity.Entities.Utils.Attributes;

namespace Identity.Entities.Model
{
    [BsonCollection("ApiScopes")]
    public class ApiScope : Document
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}