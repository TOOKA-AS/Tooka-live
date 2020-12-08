using System;
using Newtonsoft.Json;

namespace Live2k.Core.Basic.Commodities
{
    public class Address : Location
    {
        protected Address() : base()
        {

        }

        protected Address(string description, double latitude, double langtitude) : base(nameof(Address), description, latitude, langtitude)
        {

        }

        public Address(string description, string provience, string city, string street, string postalCode, double latitude = double.NaN, double langtitude = double.NaN)
            : this(description, latitude, langtitude)
        {
            AddProperty(nameof(Provience), "Provience", typeof(string));
            AddProperty(nameof(City), "City", typeof(string));
            AddProperty(nameof(Street), "Street", typeof(string));
            AddProperty("Postal code", "Postal code", typeof(string));
            Provience = provience;
            City = city;
            Street = street;
            PostalCode = postalCode;
        }

        [JsonIgnore]
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

        [JsonIgnore]
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

        [JsonIgnore]
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

        [JsonIgnore]
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
