using System;
namespace Live2k.Core.Basic.Commodities
{
    public class EmailAddress : Commodity
    {
        public EmailAddress()
        {
            Label = nameof(EmailAddress);
            AddProperty("Address", "Email address", typeof(string));
        }

        public EmailAddress(string address, string description)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException($"'{nameof(address)}' cannot be null or whitespace", nameof(address));
            }

            Address = address;
            Description = description;
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
