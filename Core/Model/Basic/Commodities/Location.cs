using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Basic.Commodities
{
    public class Location : Commodity
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected Location(object temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public Location() : base(nameof(Location))
        {

        }

        protected Location(string label) : base(label)
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
