// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class SourceDerivedLeaf : SourceBaseLeaf
    {
        /// <summary>
        /// Gets the Id.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();
    }
}
