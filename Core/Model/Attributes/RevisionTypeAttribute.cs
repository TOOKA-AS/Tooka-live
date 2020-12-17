using System;
using System.Diagnostics.CodeAnalysis;
using Live2k.Core.Basic.Commodities;

namespace Live2k.Core.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RevisionTypeAttribute : Attribute
    {
        private readonly Type revisionType;

        public RevisionTypeAttribute(Type revisionType)
        {
            this.revisionType = revisionType ?? throw new ArgumentNullException(nameof(revisionType));
        }

        public Type RevisionType => revisionType.IsSubclassOf(typeof(RevisionCommodity)) ? revisionType :
                    throw new InvalidOperationException($"{revisionType} is not of type {typeof(RevisionCommodity)}");
    }
}
