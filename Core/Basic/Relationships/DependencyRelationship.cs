using System;
using Live2k.Core.Abstraction;
using Newtonsoft.Json;

namespace Live2k.Core.Basic.Relationships
{
    public class DependencyRelationship : Relationship
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected DependencyRelationship(object temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public DependencyRelationship() : base(nameof(DependencyRelationship))
        {

        }

        protected DependencyRelationship(string label) : base(label)
        {

        }
    }
}
