using System;
using Live2k.Core.Abstraction;

namespace Live2k.Core.Basic
{
    public class NeutralRelationship : Relationship
    {
        public NeutralRelationship(Node node1, Node node2) : base(node1, node2)
        {
        }

        public override Node GetChildNode() => throw new NotSupportedException($"{this} is not directional");

        public override Node GetOwnerNode() => throw new NotSupportedException($"{this} is not directional");

        public override bool IsDirectional() => false;
    }
}
