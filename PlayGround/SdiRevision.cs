using System;
using Live2k.Core.Basic.Commodities;
using Newtonsoft.Json;

namespace PlayGround
{
    public class SdiRevision : RevisionCommodity
    {
        private SdiRevision() : base()
        {

        }

        private SdiRevision(string label, string description) : base(label, description)
        {
            AddProperty(nameof(NumberOfDocs), "Number of attached documents", typeof(int));
        }

        public SdiRevision(string description, int numberOfDocs) : this(nameof(SdiRevision), description)
        {
            NumberOfDocs = numberOfDocs;
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
