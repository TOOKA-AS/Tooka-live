using System;
using Live2k.Core.Model.Base;
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
            Signature = tracker.Signature;
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
        /// Signature of the user
        /// </summary>
        public UserSignature Signature { get; set; }

        /// <summary>
        /// Detail of the changes
        /// </summary>
        public string ChangeBody { get; set; }
    }
}
