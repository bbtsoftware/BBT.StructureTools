// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public interface ITemporalData
    {
        /// <summary>
        /// Gets or sets From.
        /// </summary>
        DateTime From { get; set; }

        /// <summary>
        /// Gets or sets To.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "To", Justification = "OK")]
        DateTime To { get; set; }
    }
}
