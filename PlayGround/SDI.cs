﻿using System;
using Live2k.Core.Basic.Commodities;
using Newtonsoft.Json;

namespace PlayGround
{
    public sealed class SDI : RevisableCommodity
    {
        [JsonConstructor]
        private SDI(object temp)
        {

        }

        public SDI() : base(nameof(SDI))
        {
        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty(nameof(DataCode), "DataCode", typeof(string));
            AddProperty(nameof(Section), "Section", typeof(string));
        }

        [JsonIgnore]
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