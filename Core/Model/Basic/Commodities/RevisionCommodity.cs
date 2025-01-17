﻿using System;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Basic.Commodities
{
    public class RevisionCommodity : Commodity
    {

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        protected RevisionCommodity(Mediator mediator) : base(mediator)
        {

        }

        protected RevisionCommodity(Mediator mediator, bool isFromDb) : base(mediator, isFromDb)
        {

        }

        protected override void GenerateLabel()
        {
            Label = string.Format("{0}_Rev.{1}",
                OwnerLabel,
                RevisionNumber);
        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty(nameof(OwnerLabel), "Owner label", typeof(string));
            AddProperty(nameof(RevisionNumber), "Revision number", typeof(int));
        }

        [JsonIgnore, BsonIgnore]
        public string OwnerLabel
        {
            get
            {
                return (string)this[nameof(OwnerLabel)];
            }

            set
            {
                this[nameof(OwnerLabel)] = value;
            }
        }

        [JsonIgnore, BsonIgnore]
        public int RevisionNumber
        {
            get
            {
                return (int)this[nameof(RevisionNumber)];
            }

            set
            {
                this[nameof(RevisionNumber)] = value;
            }
        }
    }
}
