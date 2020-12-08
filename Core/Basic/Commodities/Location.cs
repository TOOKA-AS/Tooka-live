using System;
using Newtonsoft.Json;

namespace Live2k.Core.Basic.Commodities
{
    public class Location : Commodity
    {
        protected Location() : base()
        {

        }

        protected Location(string label, string description, double latitude, double langtitude) : base(label, description)
        {
            AddProperty(nameof(Latitude), "Latitude of location", typeof(double));
            AddProperty(nameof(Langtitude), "Latitude of location", typeof(double));
            Latitude = latitude;
            Langtitude = langtitude;
        }

        public Location(string description, double latitude, double langtitude) : this(nameof(Location), description, latitude, langtitude)
        {
            
        }

        [JsonIgnore]
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

        [JsonIgnore]
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
