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
        public static event EventHandler<Entity> NewEntityAdded;
        private readonly Mediator mediator;
        private readonly IMongoDatabase _database;

        static Repository()
        {
            
        }

        public Repository(Mediator mediator, IMongoDatabase databse)
        {
            this.mediator = mediator;
            this._database = databse;
        }

        /// <summary>
        /// Add a node
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        public void AddNode<T>(T node) where T: Node
        {
            // validate entity
            ValidateEntity(node);

            // Record history
            RecordHistory(node);

            // Get relevant collection
            var collection = GetCollection(node);

            collection.InsertOne(node);
            NewEntityAdded?.Invoke(this, node);
        }

        /// <summary>
        /// Add an entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relationship"></param>
        public void AddRelationship<T>(T relationship) where T: Relationship
        {
            ValidateEntity(relationship);

            // Get relevant collection
            var collection = GetCollection(relationship);

            collection.InsertOne(relationship);
        }

        /// <summary>
        /// Record history on the node
        /// </summary>
        /// <param name="node"></param>
        private void RecordHistory(Node node)
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

            // Add node history
            node.AddHistory();

            if (trackersToRecord.Count != 0)
                collection.InsertMany(trackersToRecord);
        }

        public T Get<T>(Expression<Func<T, bool>> predicate) where T: Node
        {
            // Get relevant collection
            var collection = GetCollection(typeof(T)).OfType<T>();
            var filter = Builders<T>.Filter.Where(predicate);
            var found = collection.Find(filter).FirstOrDefault();
            var backup = collection.Find(filter).FirstOrDefault();
            found?.AttachTracker(found, backup);
            return found;
        }

        public Node Get(Expression<Func<Node, bool>> predicate)
        {
            var collection = _database.GetCollection<Node>("Nodes");
            var filter = Builders<Node>.Filter.Where(predicate);
            var found = collection.Find(filter).FirstOrDefault();
            var backup = collection.Find(filter).FirstOrDefault();
            found?.AttachTracker(found, backup);
            return found;
        }

        public void Update(Node entity)
        {
            RecordHistory(entity);

            // Get relevant collection
            var collection = _database.GetCollection<Node>("Nodes");
            collection.FindOneAndReplace(a => a.Label == entity.Label, entity);
        }

        public void SaveComment(Node node, Comment comment)
        {
            node.UpdateTopTenComments();

            var comments = _database.GetCollection<Comment>("Comments");
            comments.InsertOne(comment);

            Update(node);
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
