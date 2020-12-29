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
        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        protected Commodity(Mediator mediator) : base(mediator)
        {

        }

        protected Commodity(Mediator mediator, bool isFromDb) : base(mediator, isFromDb)
        {

        }
    }
}
