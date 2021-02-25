namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class TargetTreeLeaf
    {
        /// <summary>
        /// Gets the Id.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets TargetTree.
        /// </summary>
        public TargetTree TargetTree { get; set; }

        /// <summary>
        /// Gets or sets OriginLeaf.
        /// </summary>
        public SourceTreeLeaf OriginLeaf { get; set; }

        /// <summary>
        /// Gets or sets OriginId.
        /// </summary>
        public Guid OriginId { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateFromSourceWithReverseRelation"/>.
        /// </summary>
        public TargetTreeLeafChild Child { get; set; }

        /// <summary>
        /// Gets or sets MasterDataName.
        /// </summary>
        public Guid MasterDataId { get; set; }

        /// <summary>
        /// Gets or sets LeafName.
        /// </summary>
        public string LeafName { get; set; }

        /// <summary>
        /// Gets or sets OriginTreeHistLeafId.
        /// </summary>
        public Guid OriginTreeHistLeafId { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateToManyWithRelation"/>.
        /// </summary>
        public MasterData RelationOnTarget { get; set; }
    }
}
