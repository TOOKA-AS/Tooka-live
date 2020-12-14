using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Live2k.Core.Abstraction
{
    /// <summary>
    /// Everything in Live2k is an entity at very high level
    /// </summary>
    public abstract class Entity : IValidatableObject
    {
        /// <summary>
        /// Constructor to be used by JSON/BSON deserializer
        /// </summary>
        /// <param name="temp"></param>
        [JsonConstructor]
        protected Entity(object temp)
        {

        }

        /// <summary>
        /// Constructor which initializes list objects and adds properties
        /// </summary>
        private Entity()
        {
            InitializeListObjects();
            AddProperties();
        }

        /// <summary>
        /// Default constructor to be used to initialize objecr
        /// </summary>
        /// <param name="label"></param>
        protected Entity(string label) : this()
        {
            Label = label;
        }

        /// <summary>
        /// Virtual method to initialize list objects
        /// </summary>
        protected virtual void InitializeListObjects()
        {
            Tags = new List<string>();
            Properties = new List<BaseProperty>();
        }

        /// <summary>
        /// Virtual method to add properties
        /// </summary>
        protected virtual void AddProperties()
        {

        }

        /// <summary>
        /// Unique ID associated with the entity
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Label of the entity (Type)
        /// </summary>
        [Required]
        public string Label { get; set; }

        /// <summary>
        /// Description of the current instance
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
                return prop?.GetValue();
            }

            set
            {
                var prop = GetProperty(propertyTitle) ?? throw new IndexOutOfRangeException($"No property is defined as {propertyTitle}");
                prop.SetValue(value);
            }
        }

        /// <summary>
        /// Get value of a property as expected type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyTitle"></param>
        /// <returns></returns>
        protected virtual T GetPropertyValue<T>(string propertyTitle)
        {
            var value = this[propertyTitle];
            return value == null ? default(T) : (T)value;
        }

        /// <summary>
        /// Get value of a list property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyTitle"></param>
        /// <returns></returns>
        protected virtual IEnumerable<T> GetListPropertyValue<T>(string propertyTitle)
        {
            var value = this[propertyTitle];
            if (value == null) return null;

            // check type of value
            if (!value.GetType().GetGenericArguments()?.FirstOrDefault()?.Equals(typeof(T)) ?? false)
                throw new InvalidOperationException($"Corrupted type of property, {propertyTitle} on {this}");

            return value as IEnumerable<T>;
        }

        /// <summary>
        /// Find property
        /// </summary>
        /// <param name="propertyTitle">Property title</param>
        /// <returns></returns>
        private BaseProperty GetProperty(string propertyTitle)
        {
            var prop = Properties.FirstOrDefault(a => a.Title.Equals(propertyTitle, StringComparison.CurrentCultureIgnoreCase));

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
        protected virtual void AddProperty(string title, string description, object value)
        {
            // Instanciate property
            var prop = BaseProperty.InstanciateNewProperty(title, description, value);
            AddProperty(prop);
        }

        /// <summary>
        /// Adds a new list propery to the current entity
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="valueType"></param>
        protected virtual void AddListProperty(string title, string description, Type valueType)
        {
            // Instanciate property
            var prop = BaseProperty.InstanciateNewListProperty(title, description, valueType);
            AddProperty(prop);
        }

        /// <summary>
        /// Adds a new list property to the current entity
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="value"></param>
        protected virtual void AddListProperty(string title, string description, ICollection value)
        {
            // Instanciate property
            var prop = BaseProperty.InstanciateNewListProperty(title, description, value);
            AddProperty(prop);
        }

        /// <summary>
        /// Adds a new propery to the current entity
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="valueType"></param>
        protected virtual void AddProperty(string title, string description, Type valueType)
        {
            // Instanciate property
            var prop = BaseProperty.InstanciateNewProperty(title, description, valueType);
            AddProperty(prop);
        }

        /// <summary>
        /// Add a member to a list property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listPropTitle"></param>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        public virtual void AddToListProperty<T>(string listPropTitle, string tag, T value) where T: Entity
        {
            if (string.IsNullOrWhiteSpace(listPropTitle))
            {
                throw new ArgumentException($"'{nameof(listPropTitle)}' cannot be null or whitespace", nameof(listPropTitle));
            }

            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException($"'{nameof(tag)}' cannot be null or empty", nameof(tag));
            }

            // Check if value has the tag or assign it
            if (!value.Tags.Contains(tag))
                value.Tags.Add(tag);

            // Get the list property
            var prop = (ICollection<T>)this[listPropTitle] ?? new List<T>();

            // find a value with same tag
            var val = GetFromListProperty<T>(listPropTitle, tag);

            if (val == null)
            {
                prop.Add(value);
            }
            else
            {
                prop.Remove(val);
                prop.Add(value);
            }

            this[listPropTitle] = prop;
        }

        /// <summary>
        /// Get a value from a list property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listPropTitle"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public virtual T GetFromListProperty<T>(string listPropTitle, string tag) where T: Entity
        {
            return ((ICollection<T>)this[listPropTitle] ?? new List<T>()).FirstOrDefault(a => a.Tags.Contains(tag));
        }

        /// <summary>
        /// Validate state of current object
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Get All properties
            var allProperties = GetAllProperties();

            // Validate each property
            foreach (var prop in allProperties)
            {
                yield return ValidateProperty(prop);
            }
        }

        private ValidationResult ValidateProperty(PropertyInfo prop)
        {
            var propValue = prop.GetValue(this);

            // Get All attributes which are validation related
            var attributes = prop.GetCustomAttributes<ValidationAttribute>();

            foreach (var att in attributes)
            {
                if (!att.IsValid(propValue))
                    return new ValidationResult(att.ErrorMessage);
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Get list of all property infos
        /// </summary>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetAllProperties()
        {
            return GetType().GetProperties();
        }
    }
}
