using System;
namespace Live2k.Core.Basic.Commodities
{
    public class Company : Commodity
    {
        public Company() : base()
        {
            Label = nameof(Company);
            AddProperty(nameof(Name), "Name of company", typeof(string));
            AddProperty(nameof(RegistrationCode), "Registration code of company", typeof(string));
        }

        public Company(string name, string registrationCode) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
            }

            Name = name;
            RegistrationCode = registrationCode;
        }    

        public string Name
        {
            get
            {
                return (string)this[nameof(Name)];
            }

            set
            {
                this[nameof(Name)] = value;
            }
        }

        public string RegistrationCode
        {
            get
            {
                return (string)this["Registration code"];
            }

            set
            {
                this["Registration code"] = value;
            }
        }
    }
}
