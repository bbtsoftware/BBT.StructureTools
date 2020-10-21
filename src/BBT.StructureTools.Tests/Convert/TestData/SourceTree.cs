namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class SourceTree : BaseData
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets Root.
        /// </summary>
        public SourceRoot Root { get; set; }

        /// <summary>
        /// Gets or sets MasterData.
        /// </summary>
        public MasterData MasterData { get; set; }

        /// <summary>
        /// Gets Leafs.
        /// </summary>
        public ICollection<SourceTreeLeaf> Leafs { get; } = new List<SourceTreeLeaf>();

        /// <summary>
        /// Gets TemporalLeafs.
        /// </summary>
        public ICollection<SourceTreeHist> Hists { get; } = new List<SourceTreeHist>();

        /// <summary>
        /// Gets or sets the expected hist returned by reference date filter.
        /// </summary>
        public SourceTreeHist ExpectedFilteredHist { get; set; }

        /// <summary>
        /// Gets or sets TreeName.
        /// </summary>
        public string TreeName { get; set; }
    }
}
