using System;
namespace Live2k.Core.Basic.Commodities
{
    public class Location : Commodity
    {
        private Location() : base()
        {
            this.Label = "Location";
            AddProperty(nameof(Latitude), "Latitude of location", typeof(double));
            AddProperty(nameof(Langtitude), "Latitude of location", typeof(double));
        }

        public Location(string description) : this()
        {
            this.Description = description;
        }

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
