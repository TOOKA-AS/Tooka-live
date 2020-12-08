using System;
using System.Collections;
using System.Linq;

namespace Live2k.Core.Abstraction
{
    /// <summary>
    /// Property associated with an <see cref="Entity">Entity</see>
    /// </summary>
    public abstract class BaseProperty
    {
        protected BaseProperty(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public string Title { get; set; }
        public string Description { get; set; }

        public abstract bool HasValue();
        public abstract object GetValue();
        public abstract void SetValue(object value);

        /// <summary>
        /// Make a new generic instance of the property
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static BaseProperty InstanciateNewProperty(string title, string description, object value)
        {
            // value type
            var valueType = value.GetType();

            var prop = InstanciateNewProperty(title, description, valueType);
            prop.SetValue(value);
            return prop;
        }

        internal static BaseProperty InstanciateNewProperty(string title, string description, Type valueType)
        {
            // Get property type
            var propType = typeof(Property<>).MakeGenericType(valueType);

            return Activator.CreateInstance(propType, title, description, null) as BaseProperty;
        }

        internal static BaseProperty InstanciateNewListProperty(string title, string description, IEnumerable value)
        {
            // value type
            var valueType = value.GetType().GetGenericArguments()?.FirstOrDefault() ??
                throw new InvalidOperationException($"Cannot make a list property of type {value.GetType()}");

            var prop = InstanciateNewListProperty(title, description, valueType);
            prop.SetValue(value);
            return prop;
        }

        internal static BaseProperty InstanciateNewListProperty(string title, string description, Type valueType)
        {
            // Get property type
            var propType = typeof(ListProperty<>).MakeGenericType(valueType);

            return Activator.CreateInstance(propType, title, description, null) as BaseProperty;
        }
    }
}
