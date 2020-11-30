using System;
using Live2k.Core.Abstraction;

namespace Live2k.Core.Validation
{
    public class ValidationResult
    {
        private ValidationResult(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public ValidationResult(object invalidObject, string error = null) : this(false)
        {
            InvalidObject = invalidObject ?? throw new ArgumentNullException(nameof(invalidObject));
            Error = error;
        }

        /// <summary>
        /// Property or relationship which has validation error
        /// </summary>
        public object InvalidObject { get; private set; }

        /// <summary>
        /// Error associated with the object
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Whether validation has been succeeded
        /// </summary>
        public bool Succeeded { get; }

        public static ValidationResult Success => new ValidationResult(true);
    }
}
