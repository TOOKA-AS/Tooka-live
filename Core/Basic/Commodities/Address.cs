using System;
using Newtonsoft.Json;

namespace Live2k.Core.Basic.Commodities
{
    public class Address : Location
    {
        private Address(string description) : base(description)
        {
            AddProperty(nameof(Provience), "Provience", typeof(string));
            AddProperty(nameof(City), "City", typeof(string));
            AddProperty(nameof(Street), "Street", typeof(string));
            AddProperty("Postal code", "Postal code", typeof(string));
        }

        public Address(string provience, string city, string street, string postalCode) : this("Address")
        {
            Label = nameof(Address);
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
