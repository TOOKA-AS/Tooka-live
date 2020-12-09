using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Live2k.Core.Abstraction
{
    /// <summary>
    /// Base object type
    /// </summary>
    public abstract class Node : Entity
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected Node(object temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        private Node() : base(nameof(Node))
        {

        }

        protected Node(string label) : base(label)
        {

        }

        protected override void InitializeListObjects()
        {
            base.InitializeListObjects();
            RelationshipsOut = new List<OutRelationshipFootPrint>();
            RelationshipsIn = new List<InRelationshipFootPrint>();
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
            var rel = temp.FirstOrDefault(a => a.Id == relationship.Id);
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
    }
}
