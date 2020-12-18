using System;
using Live2k.Core.Model.Base;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Basic
{
    /// <summary>
    /// Representation of real object (physically or virtually)
    /// <p>Examples: Document, File, Location, ...</p>
    /// </summary>
    public class Commodity : Node
    {
        protected Commodity()
        {

        }

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
        public Commodity(Mediator mediator) : base(mediator, nameof(Commodity))
        {

        }

        protected Commodity(Mediator mediator, string label) : base(mediator, label)
        {

        }
    }
}
