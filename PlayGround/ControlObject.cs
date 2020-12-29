using System;
using System.Collections.Generic;
using Live2k.Core.Model.Basic;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace PlayGround
{
    public class ControlObject : Commodity
    {
        private ControlObject(Mediator mediator) : base(mediator)
        {

        }

        private ControlObject(Mediator mediator, bool isFromDb) : base(mediator, isFromDb)
        {

        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty(nameof(AvevaId), "Aveva reference", typeof(string));
            AddProperty(nameof(Section), "Section in project", typeof(string));
            AddProperty(nameof(Area), "Area in project", typeof(string));
            AddProperty(nameof(ControlObjectCode), "Control object", typeof(string));
            AddProperty(nameof(Status), "Status", typeof(string));
        }

        protected override void GenerateLabel()
        {

        }

        [JsonIgnore, BsonIgnore]
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

        [JsonIgnore, BsonIgnore]
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

        [JsonIgnore, BsonIgnore]
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

        [JsonIgnore, BsonIgnore]
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

        [JsonIgnore, BsonIgnore]
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
