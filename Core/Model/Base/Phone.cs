using System;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Base
{
    public class Phone : Entity
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected Phone(Guid temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public Phone() : base()
        {
            
        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty("Phone number", "Phone number", typeof(string));
        }

        [JsonIgnore, BsonIgnore]
        public string PhoneNumber
        {
            get
            {
                return (string)this["Phone number"];
            }

            set
            {
                this["Phone number"] = value;
            }
        }
    }
}
