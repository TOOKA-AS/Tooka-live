using System;
namespace Live2k.Core.Abstraction
{
    public class RelationshipFootPrint
    {
        private Relationship _relationship;

        private RelationshipFootPrint()
        {

        }

        public RelationshipFootPrint(Relationship relationship) : this()
        {
            this._relationship = relationship ?? throw new ArgumentNullException(nameof(relationship));
            Id = relationship.Id ?? throw new ArgumentException($"Relationship is corrupted, {nameof(relationship.Id)} cannot be null");
            Label = relationship.Label ?? throw new ArgumentException($"Relationship is corrupted, {nameof(relationship.Label)} cannot be null");
            Description = relationship.Description;
            TargetNodeId = relationship.Target.Id ?? throw new ArgumentException($"Relationship is corrupted, {nameof(relationship.Target.Id)} cannot be null");
            TargetNodeLabel = relationship.Target.Label ?? throw new ArgumentException($"Relationship is corrupted, {nameof(relationship.Target.Label)} cannot be null");
            TargetNodeDescription = relationship.Target.Description;
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
