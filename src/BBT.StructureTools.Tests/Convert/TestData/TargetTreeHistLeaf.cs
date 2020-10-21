// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class TargetTreeHistLeaf
    {
        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterMergeLevel"/>.
        /// </summary>
        public TargetTree TargetTree { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterMergeLevel"/>.
        /// </summary>
        public Guid OriginTreeHistLeafId { get; set; }
    }
}
