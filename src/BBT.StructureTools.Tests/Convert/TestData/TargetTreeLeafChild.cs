// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class TargetTreeLeafChild
    {
        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateFromSourceWithReverseRelation"/>.
        /// </summary>
        public TargetTreeLeaf TargetTreeLeaf { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateFromSourceWithReverseRelation"/>.
        /// </summary>
        public Guid OriginId { get; set; }
    }
}
