using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace Live2k.Core.Basic.Commodities
{
    public class User : Commodity
    {
        private User() : base()
        {
            
        }

        protected User(string label, string description) : base(label, description)
        {
            AddProperty("First name", $"First name of the {nameof(User)}", typeof(string));
            AddProperty("Middle name", $"Middle name of the {nameof(User)}", typeof(string));
            AddProperty("Last name", $"Last name of the {nameof(User)}", typeof(string));
            AddProperty("Birthday", $"Birthday of the {nameof(User)}", typeof(DateTime));
            AddProperty("Phone numbers", "List of phone numbers", typeof(List<Phone>));
            AddProperty("Addresses", "List of addresses", typeof(List<Address>));
        }

        public User(string firstName, string middleName, string lastName) : this(nameof(User), "User object")
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException($"'{nameof(firstName)}' cannot be null or whitespace", nameof(firstName));
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException($"'{nameof(lastName)}' cannot be null or whitespace", nameof(lastName));
            }

            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }

        [JsonIgnore]
        public string FirstName
        {
            get
            {
                return (string)this["First name"];
            }

            set
            {
                this["First name"] = value;
            }
        }

        [JsonIgnore]
        public string MiddleName
        {
            get
            {
                return (string)this["Middle name"];
            }

            set
            {
                this["Middle name"] = value;
            }
        }

        [JsonIgnore]
        public string LastName
        {
            get
            {
                return (string)this["Last name"];
            }

            set
            {
                this["Last name"] = value;
            }
        }

        [JsonIgnore]
        public DateTime Birthday
        {
            get
            {
                return GetPropertyValue<DateTime>("Birthday");
            }

            set
            {
                this["Birthday"] = value;
            }
        }

        [JsonIgnore]
        public string EmailAddress
        {
            get
            {
                return GetPropertyValue<string>("Email address");
            }

            set
            {
                this["Email address"] = value;
            }
        }

        [JsonIgnore]
        public IReadOnlyCollection<Phone> PhoneNumbers
        {
            get
            {
                return new ReadOnlyCollection<Phone>(GetListPropertyValue<Phone>("Phone numbers").ToList());
            }

            set
            {
                this["Phone numbers"] = value;
            }
        }

        [JsonIgnore]
        public IReadOnlyCollection<Address> Addresses
        {
            get
            {
                return new ReadOnlyCollection<Address>(GetListPropertyValue<Address>("Addresses").ToList());
            }

            set
            {
                this["Addresses"] = value;
            }
        }

        [JsonIgnore]
        public Address HomeAddress
        {
            get
            {
                return GetFromListProperty<Address>("Addresses", "Home address");
            }

            set
            {
                AddToListProperty("Addresses", "Home address", value);
            }
        }

        [JsonIgnore]
        public Address ShippingAddress
        {
            get
            {
                return GetFromListProperty<Address>("Addresses", "Shipping address");
            }

            set
            {
                AddToListProperty("Addresses", "Shipping address", value);
            }
        }
    }
}
