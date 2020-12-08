using System;
namespace Live2k.Core.Basic.Commodities
{
    public class EmailAddress : Commodity
    {
        protected EmailAddress() : base()
        {
            
        }

        public EmailAddress(string address, string description) : base(nameof(EmailAddress), description)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException($"'{nameof(address)}' cannot be null or whitespace", nameof(address));
            }

            AddProperty("Address", "Email address", typeof(string));
            Address = address;
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
