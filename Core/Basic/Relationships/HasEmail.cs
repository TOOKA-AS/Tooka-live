using System;
using Live2k.Core.Abstraction;
using Live2k.Core.Attributes;
using Live2k.Core.Basic.Commodities;

namespace Live2k.Core.Basic.Relationships
{
    [RelationshipNodeType(RelationshipNodeEnum.Node1, typeof(User))]
    [RelationshipNodeType(RelationshipNodeEnum.Node2, typeof(EmailAddress))]
    public class HasEmail : ChildRelationship
    {
        public HasEmail(User node1, EmailAddress node2) : base(node1, node2)
        {
            Label = nameof(HasEmail);
            Description = "Email address";
        }
    }
}
