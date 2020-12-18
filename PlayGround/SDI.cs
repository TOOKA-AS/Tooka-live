using System;
using System.ComponentModel.DataAnnotations;
using Live2k.Core.Model.Basic.Commodities;
using Live2k.Core.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Live2k.Core.Utilities;

namespace PlayGround
{
    [RevisionType(typeof(SdiRevision))]
    public sealed class SDI : RevisableCommodity
    {
        [JsonConstructor]
        private SDI(object temp) : base(temp)
        {

        }

        public SDI(Mediator mediator) : base(mediator, nameof(SDI))
        {
        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty(nameof(DataCode), "DataCode", typeof(string));
            AddProperty(nameof(Section), "Section", typeof(string));
        }

        [JsonIgnore, BsonIgnore]
        [Required]
        public string DataCode
        {
            get
            {
                return (string)this[nameof(DataCode)];
            }

            set
            {
                this[nameof(DataCode)] = value;
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
    }
}
