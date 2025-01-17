﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Live2k.Core.Attributes;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Base
{
    public class User : Entity
    {
        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        protected User() : base()
        {

        }

        public User(string emailAddress) : base(null)
        {
            EmailAddress = emailAddress;
            Id = EmailAddress;
        }

        protected override void GenerateLabel()
        {

        }

        protected override void AddProperties()
        {
            AddProperty("First name", $"First name of the {nameof(User)}", typeof(string));
            AddProperty("Middle name", $"Middle name of the {nameof(User)}", typeof(string));
            AddProperty("Last name", $"Last name of the {nameof(User)}", typeof(string));
            AddProperty("Birthday", $"Birthday of the {nameof(User)}", typeof(DateTime));
            AddProperty("Email address", $"Email address of the {nameof(User)}", typeof(string));
            AddListProperty("Phone numbers", "List of phone numbers", typeof(Phone));
            AddListProperty("Addresses", "List of addresses", typeof(Address));
        }

        public override void Save()
        {
            if (this._isFromDb)
                this.mediator.UserRepository.UpdateUser(this);
            else
                this.mediator.UserRepository.AddUser(this);
        }

        [JsonIgnore, BsonIgnore]
        // validation attributes
        [Required]
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

        [JsonIgnore, BsonIgnore]
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

        [JsonIgnore, BsonIgnore]
        // validation attributes
        [Required]
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

        [JsonIgnore, BsonIgnore]
        public string FullName => string.IsNullOrWhiteSpace(MiddleName) ?
            string.Format("{0} {1}", FirstName, LastName) :
            string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);

        [JsonIgnore, BsonIgnore]
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

        [JsonIgnore, BsonIgnore]
        // validation attributes
        [Required, StringFormat("{$FirstName}.{$LastName}@compnay.com")]
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

        [JsonIgnore, BsonIgnore]
        public IReadOnlyCollection<Phone> PhoneNumbers
        {
            get
            {
                return new ReadOnlyCollection<Phone>(GetListPropertyValue<Phone>("Phone numbers")?.ToList() ?? new List<Phone>());
            }

            set
            {
                this["Phone numbers"] = value;
            }
        }

        [JsonIgnore, BsonIgnore]
        public IReadOnlyCollection<Address> Addresses
        {
            get
            {
                return new ReadOnlyCollection<Address>(GetListPropertyValue<Address>("Addresses")?.ToList() ?? new List<Address>());
            }

            set
            {
                this["Addresses"] = value;
            }
        }

        [JsonIgnore, BsonIgnore]
        // validation attributes
        [Required]
        public Address HomeAddress
        {
            get
            {
                return GetFromListProperty<Address>("Addresses", "Home address");
            }

            set
            {
                AddToListProperty("Addresses", value);
            }
        }

        [JsonIgnore, BsonIgnore]
        public Address ShippingAddress
        {
            get
            {
                return GetFromListProperty<Address>("Addresses", "Shipping address");
            }

            set
            {
                AddToListProperty("Addresses", value);
            }
        }

        [JsonIgnore, BsonIgnore]
        // validation attributes
        [Required]
        public Phone MobilePhone
        {
            get
            {
                return GetFromListProperty<Phone>("Phone numbers", "Mobile number");
            }

            set
            {
                AddToListProperty("Phone numbers", value);
            }
        }
    }
}
