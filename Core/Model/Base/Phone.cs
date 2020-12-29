using System;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Base
{
    public class Phone : Entity
    {
        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public Phone() : base()
        {
            
        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty("Phone number", "Phone number", typeof(string));
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        [JsonIgnore, BsonIgnore]
        public string PhoneNumber
        {
            get
            {
                return (string)this["Phone number"];
            }

            set
            {
                this["Phone number"] = value;
            }
        }
    }
}
