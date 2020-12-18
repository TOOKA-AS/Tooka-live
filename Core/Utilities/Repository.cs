using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Live2k.Core.Model;
using Live2k.Core.Model.Base;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Live2k.MongoDb
{
    public class Repository
    {
        private readonly Mediator mediator;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        static Repository()
        {
            
        }

        public Repository(Mediator mediator, MongoClient client)
        {
            this.mediator = mediator;
            this._client = client;
            this._database = _client.GetDatabase("Live2K");
        }

        public void Add<T>(T node) where T: Node
        {
            // validate entity
            ValidateEntity(node);

            // Record history
            RecordHistory(node);

            // Get relevant collection
            var collection = GetCollection(node);

            collection.InsertOne(node);
        }

        public void AddEntity<T>(T entity) where T: Entity
        {
            ValidateEntity(entity);

            // Get relevant collection
            var collection = GetCollection(entity);

            collection.InsertOne(entity);
        }

        private void RecordHistory<T>(T node) where T : Node
        {
            // Get history collection
            var collection = this._database.GetCollection<ChangeTracker>("History");
            var trackers = node.GetTrackers();
            var trackersToRecord = new List<ChangeTracker>();
            foreach (var tracker in trackers)
            {
                tracker.StopTracking();
                if (tracker.IsChanged)
                    trackersToRecord.Add(tracker);
            }
            collection.InsertMany(trackersToRecord);

            // Add node history
            node.AddHistory();
        }

        public T Get<T>(Expression<Func<T, bool>> predicate) where T: Node
        {
            // Get relevant collection
            var collection = GetCollection(typeof(T)).OfType<T>();
            var filter = Builders<T>.Filter.Where(predicate);
            var found = collection.Find(filter).FirstOrDefault();
            var backup = collection.Find(filter).FirstOrDefault();
            found?.AttachTracker(mediator, found, backup);
            return found;
        }

        public void Update<T>(T entity) where T: Node
        {
            RecordHistory(entity);

            // Get relevant collection
            var collection = GetCollection(typeof(T)).OfType<T>();
            collection.FindOneAndReplace(a => a.Id == entity.Id, entity);
        }

        public IEnumerable<T> GetAll<T>() where T: Node
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
