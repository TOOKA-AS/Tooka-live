using System;
using Newtonsoft.Json;

namespace Live2k.Core.Abstraction
{
    public class OutRelationshipFootPrint
    {
        [JsonIgnore]
        private Relationship _relationship;

        private OutRelationshipFootPrint()
        {

        }

        public OutRelationshipFootPrint(Relationship relationship) : this()
        {
            this._relationship = relationship ?? throw new ArgumentNullException(nameof(relationship));
            Id = relationship.Id ?? throw new ArgumentException($"Relationship is corrupted, {nameof(relationship.Id)} cannot be null");
            Label = relationship.Label ?? throw new ArgumentException($"Relationship is corrupted, {nameof(relationship.Label)} cannot be null");
            Description = relationship.Description;
            TargetNodeId = relationship.Target.NodeId ?? throw new ArgumentException($"Relationship is corrupted, {nameof(relationship.Target.NodeId)} cannot be null");
            TargetNodeLabel = relationship.Target.NodeLabel ?? throw new ArgumentException($"Relationship is corrupted, {nameof(relationship.Target.NodeLabel)} cannot be null");
            TargetNodeDescription = relationship.Target.NodeDescription;
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
        /// Id of the node targeted by the relationship
        /// </summary>
        public string TargetNodeId { get; set; }

        /// <summary>
        /// Label of the node targeted by the relationship
        /// </summary>
        public string TargetNodeLabel { get; set; }

        /// <summary>
        /// Description of the node targeted by the relationship
        /// </summary>
        public string TargetNodeDescription { get; set; }
    }
}
