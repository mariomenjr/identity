using System.Collections.Generic;
using Identity.Entities.Mongo;
using Identity.Entities.Utils.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Entities.Model
{
    public partial class ApiResource : Document
    {
        [BsonIgnore]
        public ICollection<ApiScope> ApiScopes { get; set; } 
    }
}