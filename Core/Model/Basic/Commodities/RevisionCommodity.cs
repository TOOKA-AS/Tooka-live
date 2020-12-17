using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Basic.Commodities
{
    public class RevisionCommodity : Commodity
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected RevisionCommodity(object temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public RevisionCommodity() : base(nameof(RevisionCommodity))
        {
        }

        protected RevisionCommodity(string label) : base(label)
        {
            
        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty(nameof(RevisionNumber), "Revision number", typeof(int));
        }

        [JsonIgnore, BsonIgnore]
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
