using System;
namespace Live2k.Core.Attributes
{
    public enum RelationshipNodeEnum { Node1, Node2 }

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class RelationshipNodeTypeAttribute : Attribute
    {
        public RelationshipNodeTypeAttribute(RelationshipNodeEnum node, Type type)
        {
            Node = node;
            Type = type;
        }

        public RelationshipNodeEnum Node { get; }
        public Type Type { get; }
    }
}
