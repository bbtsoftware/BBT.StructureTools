// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class MasterData
    {
        /// <summary>
        /// Gets the Id.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets a value indicating whether IsDefault.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
