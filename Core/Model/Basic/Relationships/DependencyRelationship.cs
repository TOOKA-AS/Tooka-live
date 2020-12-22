using System;
using Live2k.Core.Model.Base;
using Live2k.Core.Utilities;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Basic.Relationships
{
    public class DependencyRelationship : Relationship
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected DependencyRelationship(Guid temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public DependencyRelationship() : base()
        {

        }
    }
}
