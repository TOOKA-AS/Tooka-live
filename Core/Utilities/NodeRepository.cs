using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Live2k.Core.Model;
using Live2k.Core.Model.Base;
using MongoDB.Driver;

namespace Live2k.Core.Utilities
{
    public class NodeRepository
    {
        public static event EventHandler<Entity> NewNodeAdded;

        private readonly Mediator _mediator;
        private readonly IMongoDatabase _database;

        public NodeRepository(Mediator mediator, IMongoDatabase database)
        {
            this._mediator = mediator;
            this._database = database;
        }

        private IMongoCollection<Node> Collection => this._database.GetCollection<Node>("Nodes");
        private IMongoCollection<T> GenericColleciton<T>() => this._database.GetCollection<T>("Nodes");

        /// <summary>
        /// Get a single node
        /// <p>Note that if several nodes found with given criteria only one will be returned</p>
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Node GetAsNode(Expression<Func<Node, bool>> predicate)
        {
            var found = GetAllAsNode(predicate).FirstOrDefault();
            var backup = GetAllAsNode(predicate).FirstOrDefault();
            found?.AttachTracker(found, backup);
            return found;
        }

        /// <summary>
        /// Get a single node by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Node GetAsNodeById(string id)
        {
            return GetAsNode(a => a.Id == id);
        }

        /// <summary>
        /// Get a single node by label
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public Node GetAsNodeByLabel(string label)
        {
            return GetAsNode(a => a.Label == label);
        }

        /// <summary>
        /// Get all nodes which saticfies the given criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<Node> GetAllAsNode(Expression<Func<Node, bool>> predicate = null)
        {
            try
            {
                return Collection.Find(GetFilter(predicate)).ToList();
            }
            catch (InvalidOperationException)
            {
                return Collection.Find(GetFilter()).ToList().Where(predicate.Compile());
            }
        }

        /// <summary>
        /// Get a node by its actual type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T GetAsActualType<T>(Expression<Func<T, bool>> predicate) where T: Node
        {
            var found = GetAllAsActualType(predicate).FirstOrDefault();
            var backup = GetAllAsActualType(predicate).FirstOrDefault();
            found?.AttachTracker(found, backup);
            return found;
        }

        /// <summary>
        /// Get a node as its actual type by its id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetAsActualTypeById<T>(string id) where T: Node
        {
            return GetAsActualType<T>(a => a.Id == id);
        }

        /// <summary>
        /// Get a node as its actual type by its label
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label"></param>
        /// <returns></returns>
        public T GetAsActualTypeByLabel<T>(string label) where T: Node
        {
            return GetAsActualType<T>(a => a.Label == label);
        }

        /// <summary>
        /// Get all nodes as actual type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAllAsActualType<T>(Expression<Func<T, bool>> predicate = null) where T: Node
        {
            return GenericColleciton<T>().Find(GetFilter(predicate)).ToList();
        }

        /// <summary>
        /// Add a new node
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(Node node)
        {
            // validate entity
            ValidateEntity(node);

            // Record history
            RecordHistory(node);

            // Recored comments
            RecoredComments(node);

            Collection.InsertOne(node);
            NewNodeAdded?.Invoke(this, node);
        }

        /// <summary>
        /// Update a node
        /// </summary>
        /// <param name="node"></param>
        public void Update(Node node)
        {
            RecordHistory(node);

            // Recored comments
            RecoredComments(node);

            Collection.FindOneAndReplace(a => a.Label == node.Label, node);
        }

        /// <summary>
        /// Build a filter for the passed in predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private FilterDefinition<Node> GetFilter(Expression<Func<Node, bool>> predicate = null)
        {
            return predicate == null ?
                   Builders<Node>.Filter.Empty :
                   Builders<Node>.Filter.Where(predicate);
        }

        /// <summary>
        /// Build a generic filter for the passed in predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private FilterDefinition<T> GetFilter<T>(Expression<Func<T, bool>> predicate = null)
            where T: Node
        {
            var typeFilter = Builders<T>.Filter.Eq(a => a.ActualType, typeof(T).FullName);

            if (predicate == null)
                return typeFilter;

            else
            {
                var predicateFilter = Builders<T>.Filter.Where(predicate);
                return Builders<T>.Filter.And(typeFilter, predicateFilter);
            }
        }

        private void ValidateEntity(Node node)
        {
            var result = node.Validate(null);
            var fails = result.Where(a => a != ValidationResult.Success);
            if (fails.Count() != 0)
            {
                throw new ValidationException(fails.First(), null, node);
            }
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

        private void RecoredComments(Node node)
        {
            // If there is no session comment on the node
            if (node.SessionComments == null || node.SessionComments.Count == 0)
                return;

            node.UpdateTopTenComments();

            // save seesion comments
            this._mediator.CommentRepository.Add(node.SessionComments);
        }
    }
}
