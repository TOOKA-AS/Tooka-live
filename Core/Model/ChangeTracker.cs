using System;
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
        public readonly User user;

        private ChangeTracker()
        {
            Id = Guid.NewGuid().ToString();
            Changes = new List<Change>();
        }

        internal ChangeTracker(Mediator mediator) : this()
        {
            if (mediator is null)
            {
                throw new ArgumentNullException(nameof(mediator));
            }

            user = mediator.SessionUser;
        }

        /// <summary>
        /// Start tracking the entity
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        public void Track(Node current, Node previous)
        {
            if (this._isTracking)
                throw new InvalidOperationException($"Already tracking: {Node.NodeId}");

            Node = current != null ? new NodeFootPrint(current) :
                   previous != null ? new NodeFootPrint(previous) :
                   throw new ArgumentNullException("Current and previous cannot be null at same time");

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
            // Check if there is a change
            if (!e.Change.HasChanged)
                return;

            // check if there is already a change registered for the property
            var alreadyRegistered = Changes.FirstOrDefault(a => a.Equals(e.Change));
            if (alreadyRegistered == null)
            {
                Changes.Add(e.Change);
            }
            else
            {
                alreadyRegistered.Update(e.Change);
            }
        }

        /// <summary>
        /// Set the save date on the tracker
        /// </summary>
        internal void StopTracking()
        {
            RemoveRedundantChanges();
            Signature = new UserSignature(this.user);
        }

        /// <summary>
        /// Remove change instances with not changed status
        /// </summary>
        private void RemoveRedundantChanges()
        {
            var changes = Changes.ToList();
            if (changes.RemoveAll(a => !a.HasChanged) != 0)
                Changes = changes;
        }

        /// <summary>
        /// Identification of the change history
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Footprint of the tracked Node
        /// </summary>
        public NodeFootPrint Node { get; set; }

        /// <summary>
        /// Signature of the user
        /// </summary>
        public UserSignature Signature { get; set; }

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
        public bool HasMainPropertyChange => Changes.Any(a => a.IsMainPropertyChange);

        [BsonIgnore]
        public bool IsChanged => ChangeType != ChangeType.Update || Changes.Any(a => a.HasChanged);

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

        /// <summary>
        /// Report as create
        /// </summary>
        /// <returns></returns>
        private string CreateReport()
        {
            return string.Format("'{0}' has been created. \nBy {1}\nOn {2}", Node.NodeId, Signature.UserId, Signature.SignedOn);
        }

        /// <summary>
        /// Report as update
        /// </summary>
        /// <returns></returns>
        private string UpdateReport()
        {
            var report = new StringBuilder();
            report.Append($"{Node.NodeId} has been updated");
            report.AppendLine($"By {Signature.UserId}");
            report.AppendLine($"On {Signature.SignedOn}");
            Changes.ToList().ForEach(a => report.AppendLine(a.Report()));
            return report.ToString();
        }

        /// <summary>
        /// Report as delete
        /// </summary>
        /// <returns></returns>
        private string DeleteReport()
        {
            return string.Format("{0} has been deleted by {1} on {2}", Node.NodeId, Signature.UserId, Signature.SignedOn);
        }
    }
}
