using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Live2k.Core.Abstraction
{
    public class ListProperty<T> : BaseProperty
    {
        public ListProperty(string title, string description, ICollection<T> value) : base(title, description)
        {
            Value = value;
        }

        public ICollection<T> Value { get; set; }

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
            if (value == null || (value as ICollection).Count == 0)
                return;

            var type = value.GetType();
            if (!ValidateType(type))
                throw new FormatException($"Cannot assign value of type {value.GetType()} to proprty of type {typeof(T)}");

            Value = new List<T>(((ICollection)value).Cast<T>());
        }

        private bool ValidateType(Type type)
        {
            var isEnumerable = type.GetInterface(nameof(IEnumerable)) != null;

            if (type.Equals(typeof(string)) || !isEnumerable)
                return false;

            var genericArg = type.GenericTypeArguments?.FirstOrDefault();

            if (genericArg.Equals(typeof(T))) return true;

            if (genericArg.IsSubclassOf(typeof(T))) return true;

            return false;
        }
    }
}
