using System.Collections.Generic;

using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Entities.Model
{
    public partial class Client
    {
        [BsonIgnore]
        public virtual ICollection<ApiScope> ApiScopes { get; set; } = new List<ApiScope>();
    }
}