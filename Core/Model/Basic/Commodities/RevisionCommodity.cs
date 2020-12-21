using System;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Basic.Commodities
{
    public class RevisionCommodity : Commodity
    {
        protected RevisionCommodity()
        {

        }

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
        public RevisionCommodity(Mediator mediator) : base(mediator, nameof(RevisionCommodity))
        {
        }

        protected RevisionCommodity(Mediator mediator, string label) : base(mediator, label)
        {
            
        }

        protected override void GenerateLabel()
        {
            Label = string.Format("{0}-{1}",
                "Revision",
                RevisionNumber);
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
