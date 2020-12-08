using System;
using Newtonsoft.Json;

namespace Live2k.Core.Basic.Commodities
{
    public class Phone : Commodity
    {
        private Phone() : base()
        {
            
        }

        public Phone(string number, string description) : base(nameof(Phone), description)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentException($"'{nameof(number)}' cannot be null or whitespace", nameof(number));
            }

            AddProperty("Phone number", "Phone number", typeof(string));
            PhoneNumber = number;
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
