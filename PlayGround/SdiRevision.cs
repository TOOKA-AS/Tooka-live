using System;
using Live2k.Core.Basic.Commodities;
using Newtonsoft.Json;

namespace PlayGround
{
    public sealed class SdiRevision : RevisionCommodity
    {
        public SdiRevision() : base(nameof(SdiRevision))
        {

        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty(nameof(NumberOfDocs), "Number of attached documents", typeof(int));
        }

        [JsonIgnore]
        public int NumberOfDocs
        {
            get
            {
                return (int)this[nameof(NumberOfDocs)];
            }

            set
            {
                this[nameof(NumberOfDocs)] = value;
            }
        }
    }
}
