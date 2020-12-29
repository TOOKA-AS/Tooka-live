using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Live2k.Core.Attributes;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Basic.Commodities
{
    public class RevisableCommodity : Commodity
    {
        /// <summary>
        /// Default constructor to be used to initialize object
        /// </summary>
        protected RevisableCommodity(Mediator mediator) : base(mediator)
        {

        }

        protected RevisableCommodity(Mediator mediator, bool isFromDb) : base(mediator, isFromDb)
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
            var createMethod = this.mediator.Factory.GetType().GetMethod("CreateNew", 1,
                new Type[] { typeof(string), typeof(string), typeof(Tuple<string, object>[]) }) ??
                throw new InvalidOperationException($"Could not find proper create methos in {this.mediator.Factory.GetType()}");

            var genericCreateMethod = createMethod.MakeGenericMethod(revisionType);
            var revision = genericCreateMethod.Invoke(this.mediator.Factory,
                new object[] { null, null, new Tuple<string, object>[]
                { new Tuple<string, object>("RevisionNumber", ++RevisionsCount),
                  new Tuple<string, object>("OwnerLabel", Label) } }) as RevisionCommodity;

            AddToListProperty(nameof(Revisions), revision);
            return revision;
        }
    }
}
