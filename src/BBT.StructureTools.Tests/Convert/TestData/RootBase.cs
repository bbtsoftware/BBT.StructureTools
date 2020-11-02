// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class RootBase
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used for <see cref="IConvertRegistration{TSource, TTarget}.RegisterTargetSubConvert"/>.
        /// </summary>
        public Guid TargetId { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used for <see cref="IConvertRegistration{TSource, TTarget}.RegisterConvertFromSourceOnDifferentLevels{TSourceValue, TTargetValue, TConvertIntention}"/>.
        /// </summary>
        public Guid TreeId { get; set; }
    }
}
