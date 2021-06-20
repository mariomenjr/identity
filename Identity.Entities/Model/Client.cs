using System.Collections.Generic;
using Identity.Entities.Mongo;
using Identity.Entities.Utils.Attributes;
using MongoDB.Bson;

namespace Identity.Entities.Model
{
    [BsonCollection("Clients")]
    public partial class Client : Document
    {
        public string Name { get; set; }
        public ICollection<ObjectId> ApiScopeIds { get; set; } 
    }
}