namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class TargetDerivedLeaf : TargetBaseLeaf
    {
        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateToOneFromGenericStrategyWithReverseRelation"/>.
        /// </summary>
        public Guid OriginId { get; set; }
    }
}
