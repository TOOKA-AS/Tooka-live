using System;
using System.Collections.Generic;
using Live2k.Core.Validation;

namespace Live2k.Core.Interfaces
{
    public interface IValidatableObject
    {
        IEnumerable<ValidationResult> Validate();
    }
}
