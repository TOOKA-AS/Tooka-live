using System;

namespace Live2k.Core.Abstraction
{
    /// <summary>
    /// Represents the relationship between nodes
    /// </summary>
    public abstract class Relationship : Entity
    {
        protected Relationship() : base()
        {

        }

        protected Relationship(string label, string description) : base(label, description)
        {

        }

        protected Relationship(string label, string description, Node origin, Node target) : this(label, description)
        {
            Origin = origin ?? throw new ArgumentNullException(nameof(origin));
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public virtual Node Origin { get; set; }
        public virtual Node Target { get; set; }
    }
}
