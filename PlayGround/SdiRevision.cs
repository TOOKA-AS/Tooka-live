using System;
using Live2k.Core.Model.Basic.Commodities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace PlayGround
{
    public sealed class SdiRevision : RevisionCommodity
    {
        [JsonConstructor]
        private SdiRevision(object temp)
        {

        }

        public SdiRevision() : base(nameof(SdiRevision))
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
