using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Live2k.Core.Model
{
    public sealed class ListPropertyChange : Change
    {
        public ListPropertyChange()
        {
            AddedItems = new List<object>();
            RemovedItems = new List<object>();
            UpdatedItems = new List<object>();
        }

        /// <summary>
        /// List of added items to the list property
        /// </summary>
        public ICollection<object> AddedItems { get; set; }

        /// <summary>
        /// List of removed items from the list property
        /// </summary>
        public ICollection<object> RemovedItems { get; set; }

        /// <summary>
        /// List of updated items in the list property
        /// </summary>
        public ICollection<object> UpdatedItems { get; set; }

        public override bool HasChanged => AddedItems.Count != 0 ||
                                           RemovedItems.Count != 0 ||
                                           UpdatedItems.Count != 0;

        public override string Report()
        {
            return $"{Property} has been updated.\n {ReportAdditions()}";
        }

        private string ReportAdditions()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Following items has been added:");
            foreach (var item in AddedItems)
            {
                builder.AppendLine($"\t*\t{item}");
            }
            return builder.ToString();
        }

        internal override void Update(Change change)
        {
            var _change = change as ListPropertyChange;
            var newAddList = new List<object>(AddedItems.Except(_change.RemovedItems).Concat(_change.AddedItems.Except(RemovedItems).Except(AddedItems)));
            var newRemoveList = new List<object>(RemovedItems.Except(_change.AddedItems).Concat(_change.RemovedItems.Except(AddedItems).Except(RemovedItems)));
            AddedItems = newAddList;
            RemovedItems = newRemoveList;
        }
    }
}
