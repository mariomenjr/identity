using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Entities.Mongo
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }

        public DateTimeOffset CreatedOn { get; }
    }
}