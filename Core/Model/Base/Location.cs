using System;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Base
{
    public class Location : Entity
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected Location(Guid temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public Location() : base()
        {

        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty(nameof(Latitude), "Latitude of location", typeof(double));
            AddProperty(nameof(Langtitude), "Latitude of location", typeof(double));
        }

        [JsonIgnore, BsonIgnore]
        public double Latitude
        {
            get
            {
                return (double)this[nameof(Latitude)];
            }

            set
            {
                this[nameof(Latitude)] = value;
            }
        }

        [JsonIgnore, BsonIgnore]
        public double Langtitude
        {
            get
            {
                return (double)this[nameof(Langtitude)];
            }

            set
            {
                this[nameof(Langtitude)] = value;
            }
        }

    }
}
