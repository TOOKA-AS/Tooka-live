using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Live2k.Core.Model
{
    public class ChangeTrackerFoorPrint
    {
        private ChangeTrackerFoorPrint()
        {
        }

        public ChangeTrackerFoorPrint(ChangeTracker tracker)
        {
            ChangeId = tracker.Id;
            ChangeType = tracker.ChangeType;
            UserId = tracker.user.Id;
            UserFullName = tracker.user.FullName;
            ChangedOn = tracker.SaveDate;
            ChangeBody = tracker.Report();
        }

        /// <summary>
        /// Id of the change tracker instance
        /// </summary>
        public string ChangeId { get; set; }

        /// <summary>
        /// Type of the change
        /// </summary>
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public ChangeType ChangeType { get; set; }

        /// <summary>
        /// Id of the user 
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Full name of the user
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// DateTime indication when changes has been done
        /// </summary>
        public DateTime ChangedOn { get; set; }

        /// <summary>
        /// Detail of the changes
        /// </summary>
        public string ChangeBody { get; set; }
    }
}
