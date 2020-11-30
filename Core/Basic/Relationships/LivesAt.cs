using System;
using Live2k.Core.Abstraction;
using Live2k.Core.Attributes;
using Live2k.Core.Basic.Commodities;

namespace Live2k.Core.Basic.Relationships
{
    [RelationshipNodeType(RelationshipNodeEnum.Node1, typeof(User))]
    [RelationshipNodeType(RelationshipNodeEnum.Node2, typeof(Address))]
    public class LivesAt : ChildRelationship
    {
        private LivesAt(User node1, Address node2) : base(node1, node2)
        {
            Label = nameof(LivesAt);
            Description = "Living address";
            AddProperty(nameof(Since), "Lives at location since ...", typeof(DateTime));
        }

        public LivesAt(User user, Address address, DateTime since) : this(user, address)
        {
            Since = since;
        }

        public DateTime Since
        {
            get
            {
                return (DateTime)this[nameof(Since)];
            }

            set
            {
                this[nameof(Since)] = value;
            }
        }
    }
}
