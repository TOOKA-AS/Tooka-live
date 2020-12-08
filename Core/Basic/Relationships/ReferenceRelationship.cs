using System;
using Live2k.Core.Abstraction;

namespace Live2k.Core.Basic.Relationships
{
    public class ReferenceRelationship : Relationship
    {
        private ReferenceRelationship() : base()
        {

        }

        public ReferenceRelationship(string description, Node origin, Node target) : base("Reference relationship", description, origin, target)
        {

        }
    }
}
