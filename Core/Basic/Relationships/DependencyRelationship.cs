using System;
using Live2k.Core.Abstraction;

namespace Live2k.Core.Basic.Relationships
{
    public class DependencyRelationship : Relationship
    {
        protected DependencyRelationship() : base()
        {

        }

        public DependencyRelationship(string description, Node origin, Node target) : base("Dependency relationship", description, origin, target)
        {

        }
    }
}
