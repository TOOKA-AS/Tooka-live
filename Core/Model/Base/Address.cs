using System;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Base
{
    public class Address : Entity
    {
        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public Address() : base()
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

        public override void Save()
        {
            throw new NotImplementedException();
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
