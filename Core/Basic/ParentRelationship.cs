using System;
using Live2k.Core.Abstraction;

namespace Live2k.Core.Basic
{
    public class ParentRelationship : Relationship
    {
        public ParentRelationship(Node node1, Node node2) : base(node1, node2)
        {
        }

        public override Node GetChildNode() => this._node1;

        public override Node GetOwnerNode() => this._node2;

        public override bool IsDirectional() => true;
    }
}
