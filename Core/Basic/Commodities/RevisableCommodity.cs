using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace Live2k.Core.Basic.Commodities
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

        [JsonIgnore]
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

        [JsonIgnore]
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

        public virtual T Revise<T>() where T: RevisionCommodity
        {
            var constructor = typeof(T).GetConstructor(new Type[0]);

            if (constructor == null)
                throw new InvalidOperationException($"Could not find proper constructor on {typeof(T)}");

            var revision = constructor.Invoke(new object[0]) as T;
            revision.RevisionNumber = ++RevisionsCount;
            AddToListProperty(nameof(Revisions), "Revisions", revision);
            return revision;
        }
    }
}
