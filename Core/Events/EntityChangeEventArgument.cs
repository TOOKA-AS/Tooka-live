using System;
using System.Collections.Generic;
using Live2k.Core.Model;
using Live2k.Core.Model.Base;

namespace Live2k.Core.Events
{
    public enum EntityListPropertyChangeTypeEnum { Add, Remove, Update }

    public class EntityChangeEventArgument : EventArgs
    {
        public EntityChangeEventArgument(string propName, object previousValue, object currentValue)
        {
            Change = new PropertyChange() { Property = propName, PrevisousValue = previousValue, CurrentValue = currentValue };
        }

        public EntityChangeEventArgument(string propName, object item, EntityListPropertyChangeTypeEnum type)
        {
            if (string.IsNullOrWhiteSpace(propName))
            {
                throw new ArgumentException($"'{nameof(propName)}' cannot be null or whitespace", nameof(propName));
            }

            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var change = new ListPropertyChange()
            {
                Property = propName
            };

            switch (type)
            {
                case EntityListPropertyChangeTypeEnum.Add:
                    change.AddedItems.Add(item);
                    break;
                case EntityListPropertyChangeTypeEnum.Remove:
                    change.RemovedItems.Add(item);
                    break;
                case EntityListPropertyChangeTypeEnum.Update:
                    change.UpdatedItems.Add(item);
                    break;
                default:
                    throw new InvalidOperationException("Not defined or not recognized type");
            }

            Change = change;
        }

        public Change Change { get; set; }
    }
}
