// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class SourceTreeHist : ITemporalData
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets Begin.
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// Gets or sets End.
        /// </summary>
        public DateTime To { get; set; }

        /// <summary>
        /// Gets or sets the TreeHistLeaf.
        /// </summary>
        public ICollection<SourceTreeHistLeaf> TreeHistLeafs { get; set; } = new Collection<SourceTreeHistLeaf>();
    }
}
