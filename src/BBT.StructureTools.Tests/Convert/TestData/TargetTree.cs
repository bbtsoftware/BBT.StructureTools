namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class TargetTree : BaseData
    {
        /// <summary>
        /// Gets the Id.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets TargetRoot.
        /// </summary>
        public TargetRoot TargetRoot { get; set; }

        /// <summary>
        /// Gets or sets OriginTree.
        /// </summary>
        public SourceTree OriginTree { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateToOneHistWithCondition"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "ToDo needs refacotrings of structure tools.")]
        public IList<TargetTreeHist> TargetHists { get; set; } = new List<TargetTreeHist>();

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateToManyWithReverseRelation{TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention}(Func{TSource, IEnumerable{TSourceValue}}, Expression{Func{TTarget, ICollection{TTargetValue}}}, ICreateConvertHelper{TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention})"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "ToDo needs refacotrings of structure tools.")]
        public IList<TargetTreeLeaf> TargetLeafs { get; set; } = new List<TargetTreeLeaf>();

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterMergeLevel"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "ToDo needs refacotrings of structure tools.")]
        public Collection<TargetTreeHistLeaf> TargetHistLeafs { get; set; } = new Collection<TargetTreeHistLeaf>();

        /// <summary>
        /// Gets or sets MasterDataName.
        /// </summary>
        public Guid MasterDataId { get; set; }

        /// <summary>
        /// Gets or sets TreeName.
        /// </summary>
        public string TreeName { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCopyFromHist"/>.
        /// </summary>
        public Guid TemporalDataOriginId { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateToOneWithRelation"/>
        /// and <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateToManyWithRelation"/>.
        /// </summary>
        public MasterData RelationOnTarget { get; set; }
    }
}
