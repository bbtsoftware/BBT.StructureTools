namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Test data for convert tests.
    /// </summary>
    public class TargetTreeHist : ITemporalData
    {
        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateToOneHistWithCondition"/>.
        /// </summary>
        public TargetTree TargetTree { get; set; }

        /// <summary>
        /// Gets or sets data
        /// used to test <see cref="IConvertRegistration{TSource, TTarget}.RegisterCreateToOneHistWithCondition"/>.
        /// </summary>
        public Guid OriginId { get; set; }

        /// <summary>
        /// Gets or sets Begin.
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// Gets or sets End.
        /// </summary>
        public DateTime To { get; set; }
    }
}
