namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class TargetRoot : RootBase
    {
        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyAttribute"/>.
        /// </summary>
        public SourceRoot OriginRoot { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateToOneWithReverseRelation"/>.
        /// </summary>
        public TargetTree TargetTree { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used for <see cref="IConvertRegistration{TSource, TTarget}.RegisterConvertFromTargetOnDifferentLevels"/>.
        /// </summary>
        public Guid TargetTreeId { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used for <see cref="IConvertRegistration{TSource, TTarget}.RegisterConvertFromSourceOnDifferentLevels{TSourceValue, TConvertIntention}"/>.
        /// </summary>
        public Guid OriginTreeId { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used for <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyFromMany"/>.
        /// </summary>
        public Guid FilteredMasterDataId { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyAttribute"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyAttributeIfSourceNotDefault"/>.
        /// </summary>
        public int NumberSourceDefault { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyAttributeIfSourceNotDefault"/>.
        /// </summary>
        public int NumberSourceNotDefault { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyAttributeWithLookUp"/>.
        /// </summary>
        public int NumberSourceLookedUp { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyAttributeWithLookUp"/>.
        /// </summary>
        public int NumberSourceNotLookedUp { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyAttributeWithUpperLimit"/>.
        /// </summary>
        public int NumberLimitApplied { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyAttributeWithUpperLimit"/>.
        /// </summary>
        public int NumberLimitNotApplied { get; set; }

        /// <summary>
        /// Gets or sets NumberTargetDefault.
        /// Used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyAttributeIfTargetIsDefault"/>.
        /// </summary>
        public decimal NumberTargetDefault { get; set; }

        /// <summary>
        /// Gets or sets NumberTargetNotDefault.
        /// Used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyAttributeIfTargetIsDefault"/>.
        /// </summary>
        public decimal NumberTargetNotDefault { get; set; }

        /// <summary>
        /// Gets or sets TargetValue.
        /// Used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyAttributeWithMapping"/>.
        /// </summary>
        public TargetEnum EnumValue { get; set; }

        /// <summary>
        /// Gets or sets the reference date
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyAttribute"/>.
        /// </summary>
        public DateTime ReferenceDate { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyAttribute"/>.
        /// </summary>
        public SourceTreeHist FilteredHist { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterSubConvert{TSourceValue, TConvertIntention}()"/>.
        /// </summary>
        public Guid RootId { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateToOneFromGenericStrategyWithReverseRelation"/>.
        /// </summary>
        public TargetBaseLeaf TargetLeaf { get; set; }

        /// <summary>
        /// Gets or sets the TargetLeafs.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "ToDo needs refacotrings of structure tools.")]
        public IList<TargetBaseLeaf> TargetLeafs { get; set; } = new List<TargetBaseLeaf>();

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateToOne"/>.
        /// </summary>
        public TargetMasterData TargetMasterData { get; set; }

        /// <summary>
        /// Gets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateToOneWithRelation"/>.
        /// </summary>
        public MasterData RelationOnTarget { get; } = new MasterData();
    }
}
