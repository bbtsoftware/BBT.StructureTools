namespace BBT.StructureTools.Compare
{
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class CompareHelper : ICompareHelper
    {
        /// <inheritdoc/>
        public void DoComparePostProcessing<T, TIntention>(
            T candidate1Nullable,
            T candidate2Nullable,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where T : class
            where TIntention : IBaseComparerIntention
        {
            additionalProcessings.NotNull(nameof(additionalProcessings));

            additionalProcessings
                .OfType<IComparePostProcessing<T, TIntention>>()
                .ToList()
                .ForEach(x => x.DoPostProcessing(candidate1Nullable, candidate2Nullable));
        }
    }
}
