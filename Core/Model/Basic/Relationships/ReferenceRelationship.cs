using System;
using Live2k.Core.Base;
using Newtonsoft.Json;

namespace Live2k.Core.Basic.Relationships
{
    public class ReferenceRelationship : Relationship
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected ReferenceRelationship(object temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public ReferenceRelationship() : base(nameof(ReferenceRelationship))
        {

        }

        protected ReferenceRelationship(string label) : base(label)
        {

        }
    }
}
