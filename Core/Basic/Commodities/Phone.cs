using System;
namespace Live2k.Core.Basic.Commodities
{
    public class Phone : Commodity
    {
        public Phone()
        {
            Label = nameof(Phone);
            AddProperty("Phone number", "Phone number", typeof(string));
        }

        public Phone(string number, string description)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentException($"'{nameof(number)}' cannot be null or whitespace", nameof(number));
            }

            PhoneNumber = number;
            Description = description;
        }

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
