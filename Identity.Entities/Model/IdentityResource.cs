using Identity.Entities.Mongo;
using Identity.Entities.Utils.Attributes;

namespace Identity.Entities.Model
{
    [BsonCollection("IdentityResources")]
    public class IdentityResource : Document
    {
        public string Name { get; set; }
    }
}