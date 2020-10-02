namespace BBT.StructureTools.Compare
{
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools;
    using BBT.StructureTools.Extension;

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
            additionalProcessings.NotNull(nameof(additionalProcessings));

            foreach (var additionalProcessing in additionalProcessings.OfType<IComparePostProcessing<T, TIntention>>())
            {
                additionalProcessing.DoPostProcessing(candidate1Nullable, candidate2Nullable);
            }
        }
    }
}
