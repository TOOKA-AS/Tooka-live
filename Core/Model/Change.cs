using System;
using Live2k.Core.Model.Base;

namespace Live2k.Core.Model
{
    public class Change
    {
        /// <summary>
        /// Name of changed property
        /// </summary>
        public string Property { get; set; }

        public virtual string Report()
        {
            return $"{Property} has been changed";
        }
    }
}
