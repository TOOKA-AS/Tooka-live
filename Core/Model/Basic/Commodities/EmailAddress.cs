using System;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Basic.Commodities
{
    public class EmailAddress : Commodity
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected EmailAddress(object temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public EmailAddress() : base(nameof(EmailAddress))
        {
            
        }

        protected EmailAddress(string label) : base(label)
        {

        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty("Address", "Email address", typeof(string));
        }

        public string Address
        {
            get
            {
                return (string)this[nameof(Address)];
            }

            set
            {
                this[nameof(Address)] = value;
            }
        }
    }
}
