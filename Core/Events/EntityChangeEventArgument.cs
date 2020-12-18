using System;
using Live2k.Core.Model;
using Live2k.Core.Model.Base;

namespace Live2k.Core.Events
{
    public class EntityChangeEventArgument : EventArgs
    {
        public EntityChangeEventArgument(string propName, object previousValue, object currentValue)
        {
            Change = new PropertyChange() { Property = propName, PrevisousValue = previousValue, CurrentValue = currentValue };
        }

        public Change Change { get; set; }
    }
}
