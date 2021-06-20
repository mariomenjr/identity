using System.Collections.Generic;
using Identity.Entities.Mongo;
using Identity.Entities.Utils.Attributes;
using MongoDB.Bson;

namespace Identity.Entities.Model
{
    [BsonCollection("ApiResources")]
    public partial class ApiResource : Document
    {
        public string Name { get; set; }
        public string Secret { get; set; }
        public ICollection<ObjectId> ApiScopeIds { get; set; } 
    }
}