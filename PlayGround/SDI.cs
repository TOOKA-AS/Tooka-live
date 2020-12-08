using System;
using Live2k.Core.Basic.Commodities;
using Newtonsoft.Json;

namespace PlayGround
{
    public class SDI : RevisableCommodity
    {
        private SDI() : base()
        {
        }

        private SDI(string label, string description) : base(label, description)
        {
            AddProperty(nameof(Section), "Section", typeof(string));
        }

        public SDI(string description, int dataCode, string section) : this(nameof(SDI), description)
        {
            DataCode = dataCode;
            Section = section;
        }

        [JsonIgnore]
        public string Section
        {
            get
            {
                return (string)this[nameof(Section)];
            }

            set
            {
                this[nameof(Section)] = value;
            }
        }
    }
}
