using System;
using Live2k.Core.Attributes;
using Live2k.Core.Basic.Commodities;

namespace Live2k.Core.Basic.Relationships
{
    [RelationshipNodeType(RelationshipNodeEnum.Origin, typeof(RevisableCommodity))]
    [RelationshipNodeType(RelationshipNodeEnum.Target, typeof(RevisionCommodity))]
    public class RevisionRelationship : DependencyRelationship
    {
        private RevisionRelationship() : base()
        {
        }
    }
}
