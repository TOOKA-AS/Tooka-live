using System;
using Live2k.Core.Model.Base;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Basic
{
    /// <summary>
    /// Representation of progressive and targeted works
    /// </summary>
    public class Assignment : Node
    {
        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        protected Assignment(Mediator mediator) : base(mediator)
        {

        }

        protected Assignment(Mediator mediator, bool isFromDb) : base(mediator, isFromDb)
        {

        }

    }
}
