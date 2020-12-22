using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;

namespace Live2k.Core.Model.Base
{
    /// <summary>
    /// Base object type
    /// </summary>
    public class Node : Entity
    {
        private bool _isTracked = false;

        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected Node(Guid temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        protected Node() : base()
        {

        }

        protected Node(Mediator mediator) : base(mediator)
        {
            CreatedBy = new UserSignature(mediator.SessionUser);
        }

        /// <summary>
        /// Method to create a tracker for this entity
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        internal void AttachTracker(Mediator mediator, Node current, Node previous)
        {
            if (!_isTracked)
            {
                var tracker = new ChangeTracker(mediator);
                tracker.Track(current, previous);
                ChangeTracker = tracker;
                _isTracked = true;
            }
            AttachTrackersSubNodes(mediator, current, previous);
        }

        /// <summary>
        /// Attach dedicated tracker to each of the subnodes
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        private void AttachTrackersSubNodes(Mediator mediator, Node current, Node previous)
        {
            // Get subnodes of current
            var currentSubnodes = current.GetSubNodes();

            // Get subnodes of preevious
            var previousSubNodes = previous?.GetSubNodes() ?? new Node[0];

            for (int i = 0; i < previousSubNodes.Count(); i++)
            {
                currentSubnodes.ElementAt(i).AttachTracker(mediator, currentSubnodes.ElementAt(i), previousSubNodes.ElementAt(i));
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
                    var last = TopTenHistory.Last();
                    TopTenHistory.Remove(last);
                }
                TopTenHistory.Add(new ChangeTrackerFoorPrint(ChangeTracker));
            }

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

        private void RemoveInwardRelationship(Relationship relationship)
        {
            var temp = new List<InRelationshipFootPrint>(RelationshipsIn);
            var rel = temp.FirstOrDefault(a => a.Label == relationship.Id);
            temp.Remove(rel);
            RelationshipsIn = temp.AsReadOnly();
        }

        private void RemoveOutwardRelationship(Relationship relationship)
        {
            var temp = new List<OutRelationshipFootPrint>(RelationshipsOut);
            var rel = temp.FirstOrDefault(a => a.Id == relationship.Id);
            temp.Remove(rel);
            RelationshipsOut = temp.AsReadOnly();
        }

        /// <summary>
        /// Change current object´s type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Cast<T>() where T : Node, new()
        {
            var obj = new T();
            obj.Id = Id;
            obj.Label = Label;
            obj.Description = Description;
            obj.Tags = Tags;
            obj.Properties = Properties;
            obj.ChangeTracker = ChangeTracker;
            obj.CreatedBy = CreatedBy;
            obj.LastModifiedBy = LastModifiedBy;
            obj.TopTenHistory = TopTenHistory;

            return obj;
        }
    }
}
