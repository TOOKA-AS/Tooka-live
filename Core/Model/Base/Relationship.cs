﻿using System;
using Live2k.Core.Utilities;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Base
{
    /// <summary>
    /// Represents the relationship between nodes
    /// </summary>
    public abstract class Relationship : Entity
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected Relationship(Guid temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        protected Relationship() : base()
        {

        }

        public virtual void SetNodes(Node origin, Node target)
        {
            Origin = new NodeFootPrint(origin);
            Target = new NodeFootPrint(target);
            origin.AddOutwardRelationship(this);
            target.AddInwardRelationship(this);
        }

        public NodeFootPrint Origin { get; private set; }

        public NodeFootPrint Target { get; private set; }
    }
}