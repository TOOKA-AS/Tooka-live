using System;
using System.Collections.Generic;

namespace Live2k.Core.Abstraction
{
    /// <summary>
    /// Base object type
    /// </summary>
    public abstract class Node : Entity
    {
        protected Node() : base()
        {

        }

        protected Node(string label, string description) : base(label, description)
        {

        }

        protected override void InitializeListObjects()
        {
            base.InitializeListObjects();
            ChildRelationships = new List<Relationship>();
            ParentRelationships = new List<Relationship>();
            NeutralRelationships = new List<Relationship>();
        }

        /// <summary>
        /// Relationships targeting to child nodes
        /// </summary>
        public ICollection<Relationship> ChildRelationships { get; set; }

        /// <summary>
        /// Relationships targeting to parent nodes
        /// </summary>
        public ICollection<Relationship> ParentRelationships { get; set; }

        /// <summary>
        /// Relationships of type neutral
        /// </summary>
        public ICollection<Relationship> NeutralRelationships { get; set; }


    }
}
