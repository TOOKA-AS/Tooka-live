using System;
using System.Collections;
using System.Linq;

namespace Live2k.Core.Model.Base
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
            if (!ValidateType(type))
                throw new FormatException($"Cannot assign value of type {value.GetType()} to proprty of type {typeof(T)}");

            Value = (T)value;
        }

        private bool ValidateType(Type type)
        {
            if (type.Equals(typeof(T))) return true;

            if (type.GetNestedTypes().Contains(typeof(T))) return true;

            return false;
        }
    }
}
