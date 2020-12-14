using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Basic.Commodities
{
    public class Address : Location
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected Address(object temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public Address() : base(nameof(Address))
        {

        }

        protected Address(string label) : base(label)
        {

        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty(nameof(Provience), "Provience", typeof(string));
            AddProperty(nameof(City), "City", typeof(string));
            AddProperty(nameof(Street), "Street", typeof(string));
            AddProperty("Postal code", "Postal code", typeof(string));
        }

        [JsonIgnore, BsonIgnore]
        public string Provience
        {
            get
            {
                return (string)this[nameof(Provience)];
            }

            set
            {
                this[nameof(Provience)] = value;
            }
        }

        [JsonIgnore, BsonIgnore]
        public string City
        {
            get
            {
                return (string)this[nameof(City)];
            }

            set
            {
                this[nameof(City)] = value;
            }
        }

        [JsonIgnore, BsonIgnore]
        public string Street
        {
            get
            {
                return (string)this[nameof(Street)];
            }

            set
            {
                this[nameof(Street)] = value;
            }
        }

        [JsonIgnore, BsonIgnore]
        public string PostalCode
        {
            get
            {
                return (string)this["Postal code"];
            }

            set
            {
                this["Postal code"] = value;
            }
        }
    }
}
