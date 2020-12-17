using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Live2k.Core.Model.Base;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Live2k.MongoDb
{
    public class Repository
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        static Repository()
        {
            
        }

        public Repository(MongoClient client)
        {
            this._client = client;
            this._database = _client.GetDatabase("Live2K");
        }

        public void Add(Entity entity)
        {
            // validate entity
            ValidateEntity(entity);

            // Get relevant collection
            var collection = GetCollection(entity);

            collection.InsertOne(entity);
        }

        public IEnumerable<T> GetAll<T>() where T: Entity
        {
            // Get relevant collection
            var collection = GetCollection(typeof(T)).OfType<T>();

            // filter
            var filter = Builders<T>.Filter.Eq(a => a.Label, typeof(T).Name);
            return collection.Find(filter).ToList();
        }

        private void ValidateEntity(Entity entity)
        {
            var result = entity.Validate(null);
            var fails = result.Where(a => a != ValidationResult.Success);
            if (fails.Count() != 0)
            {
                throw new ValidationException(fails.First(), null, entity);
            }
        }

        private IMongoCollection<Entity> GetCollection(Type type)
        {
            if (type.IsSubclassOf(typeof(Node)))
                return _database.GetCollection<Entity>("Nodes");
            else if (type.IsSubclassOf(typeof(Relationship)))
                return _database.GetCollection<Entity>("Relationships");
            else
                throw new EntryPointNotFoundException($"Could not find relevant collections for {type}");
        }

        private IMongoCollection<Entity> GetCollection(Entity entity)
        {
            if (entity is Node)
                return _database.GetCollection<Entity>("Nodes");

            else if (entity is Relationship)
                return _database.GetCollection<Entity>("Relationships");

            else
                throw new EntryPointNotFoundException($"Could not find relevant collections for {entity}");
        }
    }
}
