using System;
namespace Live2k.Core.Model
{
    public sealed class ListPropertyChange : Change
    {
        /// <summary>
        /// List of added items to the list property
        /// </summary>
        public object[] AddedItems { get; set; }

        /// <summary>
        /// List of removed items from the list property
        /// </summary>
        public object[] RemovedItems { get; set; }

        /// <summary>
        /// List of updated items in the list property
        /// </summary>
        public object[] UpdatedItems { get; set; }
    }
}
