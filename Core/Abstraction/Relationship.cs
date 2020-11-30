using System;

namespace Live2k.Core.Abstraction
{
    /// <summary>
    /// Represents the relationship between nodes
    /// </summary>
    public abstract class Relationship : Entity
    {
        protected readonly Node _node1;
        protected readonly Node _node2;

        public Relationship(Node node1, Node node2)
        {
            this._node1 = node1 ?? throw new ArgumentNullException(nameof(node1));
            this._node2 = node2 ?? throw new ArgumentNullException(nameof(node2));
        }

        public abstract bool IsDirectional();
        public abstract Node GetOwnerNode();
        public abstract Node GetChildNode();
    }
}
