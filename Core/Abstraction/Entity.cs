using System;
using System.Collections.Generic;
using System.Linq;
using Live2k.Core.Interfaces;
using Live2k.Core.Validation;

namespace Live2k.Core.Abstraction
{
    /// <summary>
    /// Everything in Live2k is an entity at very high level
    /// </summary>
    public abstract class Entity : IValidatableObject
    {
        protected Entity()
        {
            Tags = new List<string>();
            Properties = new List<BaseProperty>();
        }

        /// <summary>
        /// Unique ID associated with the entity
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Label of the entity (Type)
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Description of the current entity
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of associated labels
        /// </summary>
        public ICollection<string> Tags { get; set; }

        /// <summary>
        /// List of all properties
        /// </summary>
        public ICollection<BaseProperty> Properties { get; set; }


        /// <summary>
        /// Indexer to get or set properties based on the given key
        /// </summary>
        /// <param name="propertyTitle">Key: Title of target property</param>
        /// <returns></returns>
        public object this[string propertyTitle]
        {
            get
            {
                var prop = GetProperty(propertyTitle);
                return prop.GetValue();
            }
            set
            {
                var prop = GetProperty(propertyTitle);
                prop.SetValue(value);
            }
        }

        /// <summary>
        /// Find property
        /// </summary>
        /// <param name="propertyTitle">Property title</param>
        /// <returns></returns>
        private BaseProperty GetProperty(string propertyTitle)
        {
            var prop = Properties.FirstOrDefault(a => a.Title.Equals(propertyTitle, StringComparison.CurrentCultureIgnoreCase)) ??
                throw new IndexOutOfRangeException($"No property is defined as {propertyTitle} on {this}");

            return prop;
        }

        /// <summary>
        /// Checks existance of the property
        /// </summary>
        /// <param name="propertyTitle"></param>
        /// <returns></returns>
        public bool HasProperty(string propertyTitle) => GetProperty(propertyTitle) != null;

        /// <summary>
        /// Adds a new property
        /// </summary>
        /// <param name="property"></param>
        private void AddProperty(BaseProperty property)
        {
            if (HasProperty(property.Title))
                throw new InvalidOperationException($"{this} has already a property with title {property.Title}");

            Properties.Add(property);
        }

        /// <summary>
        /// Adds a new property to the current entity
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="value"></param>
        public virtual void AddProperty(string title, string description, object value)
        {
            // Instanciate property
            var prop = BaseProperty.InstanciateNew(title, description, value);
            AddProperty(prop);
        }

        /// <summary>
        /// Adds a new propery to the current entity
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="valueType"></param>
        public virtual void AddProperty(string title, string description, Type valueType)
        {
            // Instanciate property
            var prop = BaseProperty.InstanciateNew(title, description, valueType);
            AddProperty(prop);
        }

        public IEnumerable<ValidationResult> Validate()
        {
            throw new NotImplementedException();
        }
    }
}
