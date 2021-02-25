// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class SourceTreeLeaf
    {
        /// <summary>
        /// Gets the Id.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets Tree.
        /// </summary>
        public SourceTree Tree { get; set; }

        /// <summary>
        /// Gets LeafMasterData.
        /// </summary>
        public ICollection<MasterData> MasterDatas { get; } = new List<MasterData>();

        /// <summary>
        /// Gets TreeHistLeaf.
        /// </summary>
        public ICollection<SourceTreeHistLeaf> TreeHistLeafs { get; } = new List<SourceTreeHistLeaf>();

        /// <summary>
        /// Gets or sets LeafName.
        /// </summary>
        public string LeafName { get; set; }

        /// <summary>
        /// Gets or sets TemporalDataRefDate.
        /// </summary>
        public DateTime TemporalDataRefDate { get; set; }
    }
}
