namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class SourceRoot : RootBase
    {
        /// <summary>
        /// Gets or sets Tree.
        /// </summary>
        public SourceTree Tree { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets NumberSourceDefault.
        /// </summary>
        public int NumberSourceDefault { get; set; }

        /// <summary>
        /// Gets or sets NumberSourceNotDefault.
        /// </summary>
        public int NumberSourceNotDefault { get; set; }

        /// <summary>
        /// Gets or sets NumberTargetDefault.
        /// </summary>
        public decimal NumberTargetDefault { get; set; }

        /// <summary>
        /// Gets or sets NumberTargetNotDefault.
        /// </summary>
        public decimal NumberTargetNotDefault { get; set; }

        /// <summary>
        /// Gets or sets EnumValue.
        /// </summary>
        public (SourceEnum Source, TargetEnum Target) EnumValue { get; set; }

        /// <summary>
        /// Gets or sets SourceLeaf.
        /// </summary>
        public SourceBaseLeaf Leaf { get; set; }

        /// <summary>
        /// Gets the Leafs.
        /// </summary>
        public ICollection<SourceBaseLeaf> Leafs { get; } = new Collection<SourceBaseLeaf>();

        /// <summary>
        /// Gets or sets MasterData.
        /// </summary>
        public MasterData ExpectedFilteredMasterData { get; set; }

        /// <summary>
        /// Gets or sets MasterDatasToFilter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "ToDo needs refacotrings of structure tools.")]
        public ICollection<MasterData> MasterDatasToFilter { get; set; } = new Collection<MasterData>();
    }
}
