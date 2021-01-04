using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq.Expressions;

namespace Live2k.Core.Model.Base
{
    /// <summary>
    /// Base object type
    /// </summary>
    public abstract class Node : Entity
    {
        private bool _isTracked = false;
        private ICollection<Comment> _sessionComments;

        protected Node(Mediator mediator) : base(mediator)
        {
            CreatedBy = new UserSignature(mediator.SessionUser);
        }

        protected Node(Mediator mediator, bool isFromDb) : base(mediator, isFromDb)
        {

        }

        /// <summary>
        /// Method to create a tracker for this entity
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        internal void AttachTracker(Node current, Node previous)
        {
            if (!_isTracked)
            {
                var tracker = new ChangeTracker(this.mediator);
                tracker.Track(current, previous);
                ChangeTracker = tracker;
                _isTracked = true;
            }
            AttachTrackersSubNodes(current, previous);
        }

        /// <summary>
        /// Attach dedicated tracker to each of the subnodes
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        private void AttachTrackersSubNodes(Node current, Node previous)
        {
            // Get subnodes of current
            var currentSubnodes = current.GetSubNodes();

            // Get subnodes of preevious
            var previousSubNodes = previous?.GetSubNodes() ?? new Node[0];

            for (int i = 0; i < previousSubNodes.Count(); i++)
            {
                currentSubnodes.ElementAt(i).AttachTracker(currentSubnodes.ElementAt(i), previousSubNodes.ElementAt(i));
            }
        }

        /// <summary>
        /// Get all the subnodes
        /// </summary>
        /// <returns></returns>
        private IReadOnlyCollection<Node> GetSubNodes()
        {
            var subnodes = new List<Node>();

            // Add plain subnodes
            var plainSubNodes = Properties.Where(a => a.GetValue()?.GetType().IsSubclassOf(typeof(Node)) ?? false)
                .Select(a => a.GetValue() as Node).ToList();
            subnodes.AddRange(plainSubNodes);

            // Add nodes in list properties
            var listNodes = Properties.Where(a =>
            {
                var valueType = a.GetValue()?.GetType();
                if (valueType == null || !valueType.IsGenericType) return false;
                var genericConstraints = valueType.GetGenericArguments();
                foreach (var type in genericConstraints)
                {
                    if (type.IsSubclassOf(typeof(Node)))
                        return true;
                }

                return false;
            }).Select(a => (a.GetValue() as ICollection).GetEnumerator()).SelectMany(a =>
            {
                List<Node> result = new List<Node>();
                while (a != null && a.MoveNext())
                {
                    result.Add(a.Current as Node);
                }
                return result;
            }).ToList();
            subnodes.AddRange(listNodes);
            return subnodes.AsReadOnly();
        }

        /// <summary>
        /// Add a record in TopTenHistory list
        /// </summary>
        /// <param name="changeTracker"></param>
        internal void AddHistory()
        {
            if (ChangeTracker.IsChanged)
            {
                if (TopTenHistory.Count() == 10)
                {
                    var first = TopTenHistory.First();
                    TopTenHistory.Remove(first);
                }
                TopTenHistory.Add(new ChangeTrackerFoorPrint(ChangeTracker));
            }

            if (ChangeTracker.HasMainPropertyChange)
                LastModifiedBy = ChangeTracker.Signature;

            // Add history for sub nodes (if any)
            AddSubNodesHistory();
        }

        /// <summary>
        /// Add history for subnodes
        /// </summary>
        private void AddSubNodesHistory()
        {
            // get all subnodes
            var subnodes = GetSubNodes();
            foreach (var node in subnodes)
            {
                node.AddHistory();
            }
        }

        /// <summary>
        /// Return associated change tracker
        /// </summary>
        /// <returns></returns>
        internal IReadOnlyCollection<ChangeTracker> GetTrackers()
        {
            var trackers = new List<ChangeTracker>();

            // Add tracker associated with current node
            trackers.Add(ChangeTracker);

            // Add trackers associated with sub nodes
            trackers.AddRange(GetSubNodesTrackers());

            return trackers.AsReadOnly();
        }

        /// <summary>
        /// Get dedicated trackers to subnodes
        /// </summary>
        /// <returns></returns>
        private IReadOnlyCollection<ChangeTracker> GetSubNodesTrackers()
        {
            var trackers = new List<ChangeTracker>();
            var subnodes = GetSubNodes();
            foreach (var node in subnodes)
            {
                trackers.AddRange(node.GetTrackers());
            }
            return trackers.AsReadOnly();
        }

        /// <summary>
        /// Initialize list properties
        /// </summary>
        protected override void InitializeListObjects()
        {
            base.InitializeListObjects();
            RelationshipsOut = new List<OutRelationshipFootPrint>();
            RelationshipsIn = new List<InRelationshipFootPrint>();
            TopTenHistory = new List<ChangeTrackerFoorPrint>();
            TopTenComments = new List<Comment>();
            this._sessionComments = new List<Comment>();
        }

        /// <summary>
        /// Outward relationships owned by current Node (current node as the origin)
        /// </summary>
        public IReadOnlyCollection<OutRelationshipFootPrint> RelationshipsOut { get; set; }

        /// <summary>
        /// In relationships owned by current Node (current node as the origin)
        /// </summary>
        public IReadOnlyCollection<InRelationshipFootPrint> RelationshipsIn { get; set; }

        /// <summary>
        /// Instance of change tracker
        /// </summary>
        [BsonIgnore]
        private ChangeTracker ChangeTracker { get; set; }

        /// <summary>
        /// Foot print of the user who has created this entity
        /// </summary>
        public UserSignature CreatedBy { get; set; }

        /// <summary>
        /// Footprint of the user who has modified this entity
        /// </summary>
        public UserSignature LastModifiedBy { get; set; }

        /// <summary>
        /// List of ten most recent changes
        /// </summary>
        public ICollection<ChangeTrackerFoorPrint> TopTenHistory { get; set; }

        /// <summary>
        /// List of ten most recent comments
        /// </summary>
        public ICollection<Comment> TopTenComments { get; set; }

        /// <summary>
        /// Comments added in the current session
        /// </summary>
        public IReadOnlyCollection<Comment> SessionComments
        {
            get
            {
                return new List<Comment>(this._sessionComments.Concat(GetSubNodes().SelectMany(a => a.SessionComments))).AsReadOnly();
            }
        }

        /// <summary>
        /// Add new outwards relationships
        /// </summary>
        /// <param name="relationships"></param>
        internal virtual void AddOutwardRelationship(Relationship relationship)
        {
            if (relationship is null)
            {
                throw new ArgumentNullException(nameof(relationship));
            }

            var temp = new List<OutRelationshipFootPrint>(RelationshipsOut);
            temp.Add(new OutRelationshipFootPrint(relationship));
            RelationshipsOut = temp.AsReadOnly();
        }

        /// <summary>
        /// Add new inwards relationship
        /// </summary>
        /// <param name="relationship"></param>
        internal void AddInwardRelationship(Relationship relationship)
        {
            if (relationship is null)
            {
                throw new ArgumentNullException(nameof(relationship));
            }

            var temp = new List<InRelationshipFootPrint>(RelationshipsIn);
            temp.Add(new InRelationshipFootPrint(relationship));
            RelationshipsIn = temp.AsReadOnly();
        }

        /// <summary>
        /// Remove relationship
        /// </summary>
        /// <param name="relationship"></param>
        internal void RemoveRelationship(Relationship relationship)
        {
            if (relationship is null)
            {
                throw new ArgumentNullException(nameof(relationship));
            }

            RemoveInwardRelationship(relationship);
            RemoveOutwardRelationship(relationship);
        }

        /// <summary>
        /// Remove from inward relationships
        /// </summary>
        /// <param name="relationship"></param>
        private void RemoveInwardRelationship(Relationship relationship)
        {
            var temp = new List<InRelationshipFootPrint>(RelationshipsIn);
            var rel = temp.FirstOrDefault(a => a.Label == relationship.Id);
            temp.Remove(rel);
            RelationshipsIn = temp.AsReadOnly();
        }

        /// <summary>
        /// Remove from outward relationships
        /// </summary>
        /// <param name="relationship"></param>
        private void RemoveOutwardRelationship(Relationship relationship)
        {
            var temp = new List<OutRelationshipFootPrint>(RelationshipsOut);
            var rel = temp.FirstOrDefault(a => a.Id == relationship.Id);
            temp.Remove(rel);
            RelationshipsOut = temp.AsReadOnly();
        }

        /// <summary>
        /// Register session comment
        /// </summary>
        /// <param name="comment"></param>
        internal void RegisterSessionComment(Comment comment)
        {
            this._sessionComments.Add(comment);
            FireEntityChangedEventHandelr(
                new Events.EntityChangeEventArgument("Comments", false,
                Events.EntityListPropertyChangeTypeEnum.Add,
                comment));
        }

        /// <summary>
        /// Remove comment from session comments
        /// </summary>
        /// <param name="comment"></param>
        internal void RemoveSessionComment(Comment comment)
        {
            this._sessionComments.Remove(comment);
            FireEntityChangedEventHandelr(
                new Events.EntityChangeEventArgument("Comments", false,
                Events.EntityListPropertyChangeTypeEnum.Remove,
                comment));
        }

        /// <summary>
        /// Adds session comments into TopTenComments
        /// </summary>
        internal void UpdateTopTenComments()
        {
            var topTenComments_copy = new List<Comment>(TopTenComments);
            foreach (var comment in this._sessionComments)
            {
                if (topTenComments_copy.Count() == 10)
                {
                    var first = topTenComments_copy.First();
                    topTenComments_copy.Remove(first);
                }
                topTenComments_copy.Add(comment);
            }
            TopTenComments = topTenComments_copy.AsReadOnly();

            // Update top ten comments for subnodes
            UpdateTopTenCommentsForSubnodes();
        }

        /// <summary>
        /// Runs UpdateTopTenComments method on subnodes
        /// </summary>
        private void UpdateTopTenCommentsForSubnodes()
        {
            var subnodes = GetSubNodes();
            foreach (var node in subnodes)
            {
                node.UpdateTopTenComments();
            }
        }

        public override void Save()
        {
            // Check if the object is tracked
            if (!this._isTracked)
                throw new NotSupportedException("Cannot save not tracked entities.");

            // Check if the object is from databse
            if (this._isFromDb)
                this.mediator.NodeRepository.Update(this);
            else
                this.mediator.NodeRepository.AddNode(this);
        }

        public static T NewNode<T>(Mediator mediator, string label, string description, params Tuple<string, object>[] properties) where T : Entity
        {
            return mediator.Factory.CreateNew<T>(label, description, properties);
        }

        public static Node GetFromDatabase(Mediator mediator, Expression<Func<Node, bool>> predicate)
        {
            return mediator.NodeRepository.GetAsNode(predicate);
        }

        public static T GetFromDatabase<T>(Mediator mediator, Expression<Func<T, bool>> predicate) where T: Node
        {
            return mediator.NodeRepository.GetAsActualType(predicate);
        }
    }
}
