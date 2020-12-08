using System;
using Newtonsoft.Json;

namespace Live2k.Core.Basic.Commodities
{
    public class RevisionCommodity : Commodity
    {
        protected RevisionCommodity() : base()
        {
        }

        protected RevisionCommodity(string label, string description) : base(label, description)
        {
            AddProperty(nameof(RevisionNumber), "Revision number", typeof(int));
        }

        public RevisionCommodity(RevisableCommodity owner) : this(nameof(RevisableCommodity), $"Revision nr. {owner.RevisionsCount + 1} for {owner}")
        {
            if (owner is null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            RevisionNumber = owner.RevisionsCount + 1;
        }

        [JsonIgnore]
        public int RevisionNumber
        {
            get
            {
                return (int)this[nameof(RevisionNumber)];
            }

            set
            {
                this[nameof(RevisionNumber)] = value;
            }
        }
    }
}
