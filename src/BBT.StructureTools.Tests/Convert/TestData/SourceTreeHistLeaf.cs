namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class SourceTreeHistLeaf
    {
        /// <summary>
        /// Gets or sets TreeHist.
        /// </summary>
        public SourceTreeHist TreeHist { get; set; }

        /// <summary>
        /// Gets or sets TreeLeaf.
        /// </summary>
        public SourceTreeLeaf Leaf { get; set; }

        /// <summary>
        /// Gets or sets TreeHistLeafId.
        /// </summary>
        public Guid TreeHistLeafId { get; set; }
    }
}
