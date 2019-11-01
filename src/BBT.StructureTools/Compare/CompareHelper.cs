namespace BBT.StructureTools.Compare
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;

    /// <inheritdoc/>
    public class CompareHelper : ICompareHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompareHelper" /> class.
        /// </summary>
        public CompareHelper()
        {
        }

        /// <inheritdoc/>
        public void DoComparePostProcessing<T, TIntention>(
            T candidate1Nullable,
            T candidate2Nullable,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where T : class
            where TIntention : IBaseComparerIntention
        {
            additionalProcessings.Should().NotBeNull();

            additionalProcessings
                .OfType<IComparePostProcessing<T, TIntention>>()
                .ToList()
                .ForEach(x => x.DoPostProcessing(candidate1Nullable, candidate2Nullable));
        }
    }
}
