﻿using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Live2k.Core.Abstraction
{
    /// <summary>
    /// Base object type
    /// </summary>
    public abstract class Node : Entity
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected Node(object temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        private Node() : base(nameof(Node))
        {

        }

        protected Node(string label) : base(label)
        {

        }

        protected override void InitializeListObjects()
        {
            base.InitializeListObjects();
            Relationships = new List<RelationshipFootPrint>();
        }

        /// <summary>
        /// Relationships owned by current Node (current node as the origin)
        /// </summary>
        public IReadOnlyCollection<RelationshipFootPrint> Relationships { get; set; }

        /// <summary>
        /// Add new relationships
        /// </summary>
        /// <param name="relationships"></param>
        public virtual void AddRelationship(params Relationship[] relationships)
        {
            if (relationships is null)
            {
                throw new ArgumentNullException(nameof(relationships));
            }

            var temp = new List<RelationshipFootPrint>(Relationships);
            temp.AddRange(relationships.Select(a=>new RelationshipFootPrint(a)));
            Relationships = new ReadOnlyCollection<RelationshipFootPrint>(temp);
        }

        /// <summary>
        /// Remove relationship on the index
        /// </summary>
        /// <param name="index"></param>
        public virtual void RemoveRelationship(int index)
        {
            var temp = new List<RelationshipFootPrint>(Relationships);
            temp.Remove(Relationships.ElementAt(index));
            Relationships = new ReadOnlyCollection<RelationshipFootPrint>(temp);
        }

        /// <summary>
        /// Remove relationships
        /// </summary>
        /// <param name="relationships"></param>
        public virtual void RemoveRelationship(params RelationshipFootPrint[] relationships)
        {
            var temp = new List<RelationshipFootPrint>(Relationships.Except(relationships));
            Relationships = new ReadOnlyCollection<RelationshipFootPrint>(temp);
        }

        /// <summary>
        /// Remove relationships
        /// </summary>
        /// <param name="relationships"></param>
        public virtual void RemoveRelationship(params Relationship[] relationships)
        {
            var temp = new List<RelationshipFootPrint>(Relationships);
            foreach (var item in relationships)
            {
                temp.Remove(new RelationshipFootPrint(item));
            }
            Relationships = new ReadOnlyCollection<RelationshipFootPrint>(temp);
        }
    }
}
