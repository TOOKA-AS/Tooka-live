using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Live2k.Core.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Basic.Commodities
{
    public class RevisableCommodity : Commodity
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected RevisableCommodity(object temp) : base(temp)
        {

        }

        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        public RevisableCommodity() : base(nameof(RevisableCommodity))
        {
        }

        protected RevisableCommodity(string label) : base(label)
        {
            
        }

        protected override void AddProperties()
        {
            base.AddProperties();
            AddProperty(nameof(RevisionsCount), "Number of revisions", typeof(int));
            AddListProperty(nameof(Revisions), "Revisions", typeof(RevisionCommodity));
        }

        [JsonIgnore, BsonIgnore]
        public int RevisionsCount
        {
            get
            {
                return (int)this[nameof(RevisionsCount)];
            }

            set
            {
                this[nameof(RevisionsCount)] = value;
            }
        }

        [JsonIgnore, BsonIgnore]
        public IReadOnlyCollection<RevisionCommodity> Revisions
        {
            get
            {
                return new ReadOnlyCollection<RevisionCommodity>(GetListPropertyValue<RevisionCommodity>(nameof(Revisions))?.ToList() ?? new List<RevisionCommodity>());
            }

            set
            {
                this[nameof(Revisions)] = value;
            }
        }

        /// <summary>
        /// Make a new revision of the entity based on the defined revision type
        /// </summary>
        /// <returns></returns>
        public virtual RevisionCommodity Revise()
        {
            // find relevant revision type
            var custromAttribute = Attribute.GetCustomAttribute(GetType(), typeof(RevisionTypeAttribute)) ??
                throw new InvalidOperationException($"No revision type found for {GetType()}");
            var revisionType = (custromAttribute as RevisionTypeAttribute).RevisionType;
            return Revise(revisionType);
        }

        /// <summary>
        /// Make an instance of the revision
        /// </summary>
        /// <param name="revisionType"></param>
        /// <returns></returns>
        private RevisionCommodity Revise(Type revisionType)
        {
            var constructor = revisionType.GetConstructor(new Type[0]);

            if (constructor == null)
                throw new InvalidOperationException($"Could not find proper constructor on {revisionType}");

            var revision = constructor.Invoke(new object[0]) as RevisionCommodity ??
                throw new InvalidOperationException($"Could not make an instance of {revisionType} and cast to {typeof(RevisionCommodity)}");
            revision.RevisionNumber = ++RevisionsCount;
            AddToListProperty(nameof(Revisions), "Revisions", revision);
            return revision;
        }
    }
}
