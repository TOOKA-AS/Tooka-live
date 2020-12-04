using System;
using System.Linq;

namespace Live2k.Core.Abstraction
{
    /// <summary>
    /// Generic property
    /// </summary>
    public class Property<T> : BaseProperty
    {
        public Property(string title, string description, T value) : base(title, description)
        {
            Value = value;
        }

        public T Value { get; private set; }

        public override object GetValue()
        {
            return Value;
        }

        public override bool HasValue()
        {
            return Value != null;
        }

        public override void SetValue(object value)
        {
            if (value == null)
                return;

            var type = value.GetType();
            if (!type.Equals(typeof(T)) && !type.GetNestedTypes().Contains(typeof(T)))
                throw new FormatException($"Cannot assign value of type {value.GetType()} to proprty of type {typeof(T)}");

            Value = (T)value;
        }
    }
}
