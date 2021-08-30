// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class TargetBaseLeaf
    {
        private TargetRoot root;

        /// <summary>
        /// Gets or sets Root.
        /// </summary>
        public TargetRoot TargetRoot
        {
            get
            {
                return this.root;
            }
            set
            {
                this.root = value;
                if (value != null)
                {
                    value.TargetLeafs.Add(this);
                }
            }
        }
    }
}
