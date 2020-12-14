using System;
using Live2k.Core.Abstraction;
using Newtonsoft.Json;

namespace Live2k.Core.Basic
{
    /// <summary>
    /// Representation of progressive and targeted works
    /// </summary>
    public class Assignment : Node
    {
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
        public Assignment() : base(nameof(Assignment))
        {

        }

        protected Assignment(string label) : base(label)
        {

        }

    }
}
