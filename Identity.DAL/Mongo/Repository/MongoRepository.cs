using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Identity.DAL.Mongo.Settings;
using Identity.Entities.Mongo;
using Identity.Entities.Utils.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Identity.DAL.Mongo.Repository
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IMongoSettings settings)
        {
            ConventionRegistry.Register("camelCase", new ConventionPack {new CamelCaseElementNameConvention()}, t => true);
            
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            
            var collectionName = this.GetCollectionName(typeof(TDocument));
            this._collection = database.GetCollection<TDocument>(collectionName);
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute) documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }
        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TDocument FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual IEnumerable<TDocument> Find()
        {
            return _collection.Find(Builders<TDocument>.Filter.Empty).ToList();
        }

        public virtual Task<IEnumerable<TDocument>> FindAsync()
        {
            return Task.Run(() =>
            {
                return _collection.Find(Builders<TDocument>.Filter.Empty).ToListAsync().ContinueWith(task => task.Result.AsEnumerable());
            });
        }

        public virtual void InsertOne(TDocument document)
        {
            _collection.InsertOne(document);
        }

        public virtual Task InsertOneAsync(TDocument document)
        {
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            _collection.InsertMany(documents);
        }


        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        } 
    }
}

// protected readonly IMongoContext MongoContext;
// protected IMongoCollection<TEntity> DbCollection;
//
// protected MongoRepository(IMongoContext mongoContext)
// {
//     this.MongoContext = mongoContext;
//     this.DbCollection = this.MongoContext.GetCollection<TEntity>(typeof(TEntity).Name);
// }
//
// public async Task Create(TEntity obj)
// {
//     if (obj == null) throw new ArgumentNullException($"{typeof(TEntity).Name} object is NULL");
//
//     await this.DbCollection.InsertOneAsync(obj);
// }
//
// public virtual void Update(TEntity obj)
// {
//     this.DbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.Id), obj);
// }
//
// public virtual void Delete(string id)
// {
//     var objectId = new ObjectId(id);
//     this.DbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id));
// }
//
// public async Task<TEntity> Get(string id)
// {
//     var objectId = new ObjectId(id);
//     var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
//     
//     return await this.DbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
// }
//
// public async Task<IEnumerable<TEntity>> Get()
// {
//     var enumeration = await this.DbCollection.FindAsync(Builders<TEntity>.Filter.Empty);
//     return await enumeration.ToListAsync();
// }