using System;
using Live2k.Core.Model.Base;
using Live2k.Core.Utilities;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Basic
{
    /// <summary>
    /// Representation of progressive and targeted works
    /// </summary>
    public class Assignment : Node
    {
        protected Assignment()
        {

        }

        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected Assignment(object temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public Assignment(Mediator mediator) : base(mediator, nameof(Assignment))
        {

        }

        protected Assignment(Mediator mediator, string label) : base(mediator, label)
        {

        }

    }
}
