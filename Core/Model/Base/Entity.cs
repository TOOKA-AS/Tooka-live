using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Live2k.Core.Events;
using Live2k.Core.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Live2k.Core.Model.Base
{
    /// <summary>
    /// Everything in Live2k is an entity at very high level
    /// </summary>
    [BsonIgnoreExtraElements]
    public abstract class Entity : IValidatableObject
    {
        private string label;
        private string description;
        private bool listPropertyUpdateInProgress = false;
        protected readonly Mediator mediator;
        protected readonly bool _isFromDb;

        internal event EventHandler<EntityChangeEventArgument> entityChangedEventHandler;

        protected Entity()
        {

        }

        protected Entity(Mediator mediator) : this()
        {
            this.mediator = mediator;
            Id = Guid.NewGuid().ToString();
            ActualType = GetType().FullName;
            InitializeListObjects();
            AddProperties();
        }

        protected Entity(Mediator mediator, bool isFromDb) : this(mediator)
        {
            this._isFromDb = isFromDb;
        }

        /// <summary>
        /// Method to generate unique ID
        /// <p>By default GUID is used</p>
        /// </summary>
        protected virtual void GenerateLabel()
        {
            Label = string.IsNullOrWhiteSpace(Label) ? GetType().Name :
                string.Format("{0}-{1}",Label,
                mediator.CounterReposity.Count(GetType()) + 1);
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
        /// Actual type of the entity
        /// </summary>
        public string ActualType { get; protected set; }

        /// <summary>
        /// Unique ID associated with the entity
        /// </summary>
        [Required]
        public string Id { get; protected set; }

        /// <summary>
        /// Label of the entity (Type)
        /// </summary>
        [Required]
        public string Label
        {
            get => label;
            set
            {
                FireEntityChangedEventHandelr(
                    new EntityChangeEventArgument(nameof(Label), true, this.label, value));
                this.label = value;
            }
        }

        /// <summary>
        /// Description of the current instance
        /// </summary>
        public string Description
        {
            get => description;
            set
            {
                FireEntityChangedEventHandelr(
                    new EntityChangeEventArgument(nameof(Description), true, this.description, value));
                this.description = value;
            }
        }

        /// <summary>
        /// List of associated labels
        /// </summary>
        public IReadOnlyCollection<string> Tags { get; protected set; }

        /// <summary>
        /// List of all properties
        /// </summary>
        public IReadOnlyCollection<BaseProperty> Properties { get; protected set; }

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
                if (!listPropertyUpdateInProgress)
                    FireEntityChangedEventHandelr(new EntityChangeEventArgument(propertyTitle, true, prop.GetValue(), value));
                    
                prop.SetValue(value);
            }
        }

        /// <summary>
        /// Add a new tag
        /// </summary>
        /// <param name="tag"></param>
        public void AddTag(params string[] tags)
        {
            Tags = new List<string>(Tags.Concat(tags));
            FireEntityChangedEventHandelr(
                new EntityChangeEventArgument(nameof(Tags), true, EntityListPropertyChangeTypeEnum.Add, tags));
        }

        /// <summary>
        /// Remove a tag
        /// </summary>
        /// <param name="tag"></param>
        public void RemoveTag(params string[] tags)
        {
            Tags = new List<string>(Tags.Except(tags));
            FireEntityChangedEventHandelr(
                new EntityChangeEventArgument(nameof(Tags), true, EntityListPropertyChangeTypeEnum.Remove, tags));
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

            Properties = new List<BaseProperty>(Properties.Append(property));
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
        /// <param name="label"></param>
        /// <param name="value"></param>
        public virtual void AddToListProperty<T>(string listPropTitle, T value) where T : Entity
        {
            if (string.IsNullOrWhiteSpace(listPropTitle))
            {
                throw new ArgumentException($"'{nameof(listPropTitle)}' cannot be null or whitespace", nameof(listPropTitle));
            }

            // Get the list property
            var prop = (ICollection<T>)this[listPropTitle] ?? new List<T>();

            // find a value with same label
            var val = GetFromListProperty<T>(listPropTitle, value.Label);
            var changeType = val == null ? EntityListPropertyChangeTypeEnum.Add : EntityListPropertyChangeTypeEnum.Update;

            if (val == null)
            {
                prop.Add(value);
            }
            else
            {
                prop.Remove(val);
                prop.Add(value);
            }

            FireEntityChangedEventHandelr(new EntityChangeEventArgument(listPropTitle, true, changeType, value));

            // flag list property update
            listPropertyUpdateInProgress = true;

            this[listPropTitle] = prop;

            listPropertyUpdateInProgress = false;
        }

        /// <summary>
        /// Get a value from a list property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listPropTitle"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public virtual T GetFromListProperty<T>(string listPropTitle, string label) where T : Entity
        {
            return ((ICollection<T>)this[listPropTitle] ?? new List<T>()).FirstOrDefault(a => a.Label == label);
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

        /// <summary>
        /// Validate property against current object
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        private ValidationResult ValidateProperty(PropertyInfo prop)
        {
            // Get All attributes which are validation related
            var attributes = prop.GetCustomAttributes<ValidationAttribute>();

            if (attributes != null && attributes.Count() != 0)
            {
                var propValue = prop.GetValue(this);
                foreach (var att in attributes)
                {
                    try
                    {
                        att.Validate(propValue, prop.Name);
                    }
                    catch (Exception ex)
                    {
                        return new ValidationResult(ex.Message);
                    }
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Fire changed event handler
        /// </summary>
        /// <param name="arg"></param>
        protected void FireEntityChangedEventHandelr(EntityChangeEventArgument arg)
        {
            entityChangedEventHandler?.Invoke(this, arg);
        }

        /// <summary>
        /// Get list of all property infos
        /// </summary>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetAllProperties()
        {
            return GetType().GetProperties();
        }

        public abstract void Save();
    }
}
