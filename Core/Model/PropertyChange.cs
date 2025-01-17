﻿using System;
using Live2k.Core.Model.Base;

namespace Live2k.Core.Model
{
    public sealed class PropertyChange : Change
    {
        public PropertyChange(bool isMainPropertyChange) : base(isMainPropertyChange)
        {

        }

        /// <summary>
        /// Previous instance of property
        /// </summary>
        public object PrevisousValue { get; set; }

        /// <summary>
        /// Current instance of property
        /// </summary>
        public object CurrentValue { get; set; }

        public override bool HasChanged => CurrentValue == null ||
                                           PrevisousValue == null ||
                                           !PrevisousValue.Equals(CurrentValue);

        public override string Report()
        {
            var fromBase =  base.Report();
            return string.Format("{0}\n\tOld value: {1}\n\tNew value: {2}", fromBase, PrevisousValue, CurrentValue);
        }

        internal override void Update(Change change)
        {
            CurrentValue = (change as PropertyChange).CurrentValue;
        }
    }
}
