using System;
using Live2k.Core.Abstraction;
using Live2k.Core.Attributes;
using Live2k.Core.Basic.Commodities;

namespace Live2k.Core.Basic.Relationships
{
    [RelationshipNodeType(RelationshipNodeEnum.Node1, typeof(Company))]
    [RelationshipNodeType(RelationshipNodeEnum.Node1, typeof(User))]
    public class WorksAt : ParentRelationship
    {
        private WorksAt(Company node1, User node2) : base(node1, node2)
        {
            Label = nameof(WorksAt);
            Description = "Working company";
            AddProperty(nameof(Since), "Working in company since", typeof(DateTime));
        }

        public WorksAt(User user, Company company, DateTime since) : this(company, user)
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
