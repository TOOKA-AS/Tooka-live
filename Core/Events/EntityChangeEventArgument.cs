using System;
using System.Collections.Generic;
using Live2k.Core.Model;
using Live2k.Core.Model.Base;

namespace Live2k.Core.Events
{
    public enum EntityListPropertyChangeTypeEnum { Add, Remove, Update }

    public class EntityChangeEventArgument : EventArgs
    {
        public EntityChangeEventArgument(string propName, bool isMainProperty, object previousValue, object currentValue)
        {
            Change = new PropertyChange(isMainProperty) { Property = propName, PrevisousValue = previousValue, CurrentValue = currentValue };
        }

        public EntityChangeEventArgument(string propName, bool isMainProperty, EntityListPropertyChangeTypeEnum type, params object[] items)
        {
            if (string.IsNullOrWhiteSpace(propName))
            {
                throw new ArgumentException($"'{nameof(propName)}' cannot be null or whitespace", nameof(propName));
            }

            if (items is null || items.Length == 0)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var change = new ListPropertyChange(isMainProperty)
            {
                Property = propName
            };

            switch (type)
            {
                case EntityListPropertyChangeTypeEnum.Add:
                    (change.AddedItems as List<object>).AddRange(items);
                    break;
                case EntityListPropertyChangeTypeEnum.Remove:
                    (change.RemovedItems as List<object>).AddRange(items);
                    break;
                case EntityListPropertyChangeTypeEnum.Update:
                    (change.UpdatedItems as List<object>).AddRange(items);
                    break;
                default:
                    throw new InvalidOperationException("Not defined or not recognized type");
            }

            Change = change;
        }

        public Change Change { get; set; }
    }
}
