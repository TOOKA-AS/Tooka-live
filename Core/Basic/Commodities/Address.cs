using System;
namespace Live2k.Core.Basic.Commodities
{
    public class Address : Location
    {
        private Address(string description) : base(description)
        {
            AddProperty(nameof(Provience), "Provience", typeof(string));
            AddProperty(nameof(City), "City", typeof(string));
            AddProperty(nameof(Street), "Street", typeof(string));
            AddProperty(nameof(PostalCode), "Postal code", typeof(string));
        }

        public Address(string provience, string city, string street, string postalCode) : this("Address")
        {
            Label = nameof(Address);
            Provience = provience;
            City = city;
            Street = street;
            PostalCode = postalCode;
        }

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
