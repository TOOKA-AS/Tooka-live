﻿using System;
using System.ComponentModel.DataAnnotations;
using Live2k.Core.Model.Basic.Commodities;
using Live2k.Core.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Live2k.Core.Utilities;
using System.Linq;

namespace PlayGround
{
    [RevisionType(typeof(SdiRevision))]
    public sealed class SDI : RevisableCommodity
    {
        [JsonConstructor]
        private SDI(Guid temp) : base(temp)
        {

        }

        public SDI(Mediator mediator, Factory factory) : base(mediator, factory)
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

        [JsonIgnore, BsonIgnore]
        public SdiRevision ActiveRevision => Revisions.FirstOrDefault(a => a.RevisionNumber == RevisionsCount) as SdiRevision;
    }
}
