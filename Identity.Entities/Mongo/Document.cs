using System;
using MongoDB.Bson;

namespace Identity.Entities.Mongo
{
    public abstract class Document : IDocument 
    {
        public ObjectId Id { get; set; }

        public DateTimeOffset CreatedOn => Id.CreationTime.ToLocalTime();
    }
}