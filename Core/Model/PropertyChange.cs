using System;
using Live2k.Core.Model.Base;

namespace Live2k.Core.Model
{
    public sealed class PropertyChange : Change
    {
        /// <summary>
        /// Previous instance of property
        /// </summary>
        public BaseProperty Previsous { get; set; }

        /// <summary>
        /// Current instance of property
        /// </summary>
        public BaseProperty Current { get; set; }
    }
}
