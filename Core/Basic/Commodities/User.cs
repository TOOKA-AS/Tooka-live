using System;
namespace Live2k.Core.Basic.Commodities
{
    public class User : Commodity
    {
        private User() : base()
        {
            AddProperty("First name", $"First name of the {nameof(User)}", typeof(string));
            AddProperty("Middle name", $"Middle name of the {nameof(User)}", typeof(string));
            AddProperty("Last name", $"Last name of the {nameof(User)}", typeof(string));
            AddProperty("Birthday", $"Birthday of the {nameof(User)}", typeof(DateTime));
        }

        protected User(string label, string description) : this()
        {
            Label = label;
            Description = description;
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

        public DateTime Birthday
        {
            get
            {
                return (DateTime)this["Birthday"];
            }

            set
            {
                this["Birthday"] = value;
            }
        }
    }
}
