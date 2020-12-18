using System;
using Live2k.Core.Model;

namespace Live2k.Core.Events
{
    public class EntityChangeEventArgument : EventArgs
    {
        public Change Change { get; set; }
    }
}
