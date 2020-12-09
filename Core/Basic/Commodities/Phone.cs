using System;
using Newtonsoft.Json;

namespace Live2k.Core.Basic.Commodities
{
    public class Phone : Commodity
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected Phone(object temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public Phone() : base(nameof(Phone))
        {
            
        }

        protected Phone(string label) : base(label)
        {

        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty("Phone number", "Phone number", typeof(string));
        }

        [JsonIgnore]
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
