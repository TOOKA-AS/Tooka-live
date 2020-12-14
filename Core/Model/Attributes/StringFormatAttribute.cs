using System;
using System.ComponentModel.DataAnnotations;

namespace Live2k.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class StringFormatAttribute : ValidationAttribute
    {
        public string Pattern { get; }

        public StringFormatAttribute(string pattern)
        {
            Pattern = pattern;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }

        public override bool Match(object obj)
        {
            return base.Match(obj);
        }
    }
}
