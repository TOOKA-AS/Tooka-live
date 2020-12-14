using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Abstraction
{
    public class InRelationshipFootPrint
    {
        [JsonIgnore, BsonIgnore]
        private Relationship _relationship;

        private InRelationshipFootPrint()
        {

        }

        public InRelationshipFootPrint(Relationship relationship) : this()
        {
            this._relationship = relationship ?? throw new ArgumentNullException(nameof(relationship));
            Id = relationship.Id ?? throw new ArgumentException($"Relationship is corrupted, {nameof(relationship.Id)} cannot be null");
            Label = relationship.Label ?? throw new ArgumentException($"Relationship is corrupted, {nameof(relationship.Label)} cannot be null");
            Description = relationship.Description;
            OriginNodeId = relationship.Origin.NodeId ?? throw new ArgumentException($"Relationship is corrupted, {nameof(relationship.Target.NodeId)} cannot be null");
            OriginNodeLabel = relationship.Origin.NodeLabel ?? throw new ArgumentException($"Relationship is corrupted, {nameof(relationship.Target.NodeLabel)} cannot be null");
            OriginNodeDescription = relationship.Origin.NodeDescription;
        }

        /// <summary>
        /// Id of the relationship
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Label of the relationship
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Description of the relationship
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Id of the origin node of the relationship
        /// </summary>
        public string OriginNodeId { get; set; }

        /// <summary>
        /// Label of the origin node of the relationship
        /// </summary>
        public string OriginNodeLabel { get; set; }

        /// <summary>
        /// Description of the origin node of the relationship
        /// </summary>
        public string OriginNodeDescription { get; set; }
    }
}
