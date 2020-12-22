using System;
using Live2k.Core.Model.Basic.Commodities;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace PlayGround
{
    public sealed class SdiRevision : RevisionCommodity
    {
        public SdiRevision() : base()
        {

        }

        [JsonConstructor]
        private SdiRevision(Guid temp) : base(temp)
        {

        }

        public SdiRevision(Mediator mediator) : base(mediator)
        {

        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty(nameof(NumberOfDocs), "Number of attached documents", typeof(int));
        }

        [JsonIgnore, BsonIgnore]
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
