using System;
using System.Collections.Generic;
using Live2k.Core.Basic;
using Newtonsoft.Json;

namespace PlayGround
{
    public class ControlObject : Commodity
    {
        private ControlObject() : base()
        {
        }

        private ControlObject(string label, string description)
        {
            AddProperty(nameof(AvevaId), "Aveva reference", typeof(string));
            AddProperty(nameof(Section), "Section in project", typeof(string));
            AddProperty(nameof(Area), "Area in project", typeof(string));
            AddProperty(nameof(ControlObjectCode), "Control object", typeof(string));
            AddProperty(nameof(Status), "Status", typeof(string));
        }

        public ControlObject(string avevaId, string section, string area, string code, string status) : this("Control object", "E3D design context")
        {
            AvevaId = avevaId;
            Section = section;
            Area = area;
            ControlObjectCode = code;
            Status = status;
        }

        [JsonIgnore]
        public string AvevaId
        {
            get
            {
                return (string)this[nameof(AvevaId)];
            }

            set
            {
                this[nameof(AvevaId)] = value;
            }
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

        [JsonIgnore]
        public string Area
        {
            get
            {
                return (string)this[nameof(Area)];
            }

            set
            {
                this[nameof(Area)] = value;
            }
        }

        [JsonIgnore]
        public string ControlObjectCode
        {
            get
            {
                return (string)this[nameof(ControlObjectCode)];
            }

            set
            {
                this[nameof(ControlObjectCode)] = value;
            }
        }

        [JsonIgnore]
        public string Status
        {
            get
            {
                return (string)this[nameof(Status)];
            }

            set
            {
                this[nameof(Status)] = value;
            }
        }
    }
}
