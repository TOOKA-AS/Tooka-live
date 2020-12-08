using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace Live2k.Core.Basic.Commodities
{
    public class RevisableCommodity : Commodity
    {
        protected RevisableCommodity() : base()
        {
        }

        protected RevisableCommodity(string label, string description) : base(label, description)
        {
            AddProperty(nameof(DataCode), "Data code", typeof(int));
            AddProperty(nameof(RevisionsCount), "Number of revisions", typeof(int));
            AddListProperty(nameof(Revisions), "Revisions", typeof(RevisionCommodity));
        }

        public RevisableCommodity(string description, int dataCode) : this(nameof(RevisableCommodity), description)
        {
            DataCode = dataCode;
        }

        // This is just for demonstration
        [JsonIgnore]
        public int DataCode
        {
            get
            {
                return (int)this[nameof(DataCode)];
            }

            set
            {
                this[nameof(DataCode)] = value;
            }
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
                return new ReadOnlyCollection<RevisionCommodity>(GetListPropertyValue<RevisionCommodity>(nameof(Revisions)).ToList());
            }

            set
            {
                this[nameof(Revisions)] = value;
            }
        }

        public virtual T Revise<T>(params object[] constructorArgs) where T: RevisionCommodity
        {
            var constructor = typeof(T).GetConstructor(constructorArgs.Select(a => a.GetType()).ToArray());

            if (constructor == null)
                throw new InvalidOperationException($"Could not find proper constructor on {typeof(T)}");

            var revision = constructor.Invoke(constructorArgs) as T;
            revision.RevisionNumber = ++RevisionsCount;
            AddToListProperty(nameof(Revisions), "Revisions", revision);
            return revision;
        }
    }
}
