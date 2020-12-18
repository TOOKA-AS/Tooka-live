﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Live2k.Core.Model.Base;
using Live2k.Core.Model.Basic.Commodities;
using Live2k.Core.Utilities;
using MongoDB.Bson.Serialization.Attributes;

namespace Live2k.Core.Model
{
    public enum ChangeType { Create, Update, Delete }

    public sealed class ChangeTracker
    {
        private bool _isTracking = false;
        private bool _isChanged = false;

        private ChangeTracker()
        {
            Changes = new List<Change>();
        }

        internal ChangeTracker(Mediator mediator) : this()
        {
            if (mediator is null)
            {
                throw new ArgumentNullException(nameof(mediator));
            }

            UserId = mediator.SessionUser.Id;
        }

        /// <summary>
        /// Start tracking the entity
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        public void Track(Entity current, Entity previous)
        {
            if (this._isTracking)
                throw new InvalidOperationException($"Already tracking: {EntityId}");

            EntityId = current != null ?
                current.Id :
                previous?.Id ?? throw new ArgumentNullException("Current and previous cannot be null at same time");

            CurrentSnapshot = current;
            PreviousSnapshot = previous;

            AssignTrackingType();

            this._isTracking = true;
        }

        /// <summary>
        /// Set tracking type
        /// </summary>
        private void AssignTrackingType()
        {
            if (CurrentSnapshot == null)
                ChangeType = ChangeType.Delete;
            else if (PreviousSnapshot == null)
            {
                _isChanged = true;
                ChangeType = ChangeType.Create;
            }
            else
            {
                ChangeType = ChangeType.Update;
                CurrentSnapshot.entityChangedEventHandler += CurrentSnapshot_entityChangedEventHandler;
            }
        }

        private void CurrentSnapshot_entityChangedEventHandler(object sender, Events.EntityChangeEventArgument e)
        {
            _isChanged = true;
            Changes.Add(e.Change);
        }

        /// <summary>
        /// Set the save date on the tracker
        /// </summary>
        internal void StopTracking()
        {
            this.SaveDate = DateTime.Now;
        }

        /// <summary>
        /// ID of the tracked entity
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Time when the changed has occured
        /// </summary>
        public DateTime SaveDate { get; set; }

        /// <summary>
        /// User who have done the change
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Type of the change
        /// </summary>
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public ChangeType ChangeType { get; set; }

        /// <summary>
        /// List of all changed
        /// </summary>
        public ICollection<Change> Changes { get; set; }

        /// <summary>
        /// Sanpshot of the entity before updating
        /// </summary>
        public Entity PreviousSnapshot { get; set; }

        /// <summary>
        /// Snapshot of the entity after updating
        /// </summary>
        public Entity CurrentSnapshot { get; set; }

        [BsonIgnore]
        public bool IsChanged => _isChanged;

        /// <summary>
        /// Returns a string reporting change details
        /// </summary>
        /// <returns></returns>
        public string Report()
        {
            switch (ChangeType)
            {
                case ChangeType.Create:
                    return CreateReport();
                case ChangeType.Update:
                    return UpdateReport();
                case ChangeType.Delete:
                    return DeleteReport();
                default:
                    return string.Empty;
            }
        }

        private string CreateReport()
        {
            return string.Format("'{0}' has been created. \nBy {1}\nOn {2}", EntityId, UserId, SaveDate);
        }

        private string UpdateReport()
        {
            var report = new StringBuilder();
            report.Append($"{EntityId} has been updated");
            report.AppendLine($"By {UserId}");
            report.AppendLine($"On {SaveDate}");
            Changes.ToList().ForEach(a => report.AppendLine(a.Report()));
            return report.ToString();
        }

        private string DeleteReport()
        {
            throw new NotImplementedException();
        }
    }
}
