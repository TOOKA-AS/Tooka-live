﻿using System;
using Live2k.Core.Abstraction;
using Newtonsoft.Json;

namespace Live2k.Core.Basic
{
    /// <summary>
    /// Representation of real object (physically or virtually)
    /// <p>Examples: Document, File, Location, ...</p>
    /// </summary>
    public class Commodity : Node
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected Commodity(object temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public Commodity() : base(nameof(Commodity))
        {

        }

        protected Commodity(string label) : base(label)
        {

        }
    }
}