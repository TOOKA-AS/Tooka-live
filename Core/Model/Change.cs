using System;
using Live2k.Core.Model.Base;

namespace Live2k.Core.Model
{
    public class Change
    {
        /// <summary>
        /// Name of changed property
        /// </summary>
        public string Property { get; set; }

        public virtual bool HasChanged { get; }

        public virtual string Report()
        {
            return $"{Property} has been changed";
        }

        public override bool Equals(object obj)
        {
            return obj is Change && (obj as Change).Property.Equals(Property);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Property);
        }

        internal virtual void Update(Change change)
        {
            throw new NotImplementedException();
        }
    }
}
