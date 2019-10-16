namespace BBT.StructureTools.Compare.Helper.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class EqualityComparerHelperStrategySubCompareComparer<T, TIntention> : IEqualityComparerHelperStrategy<T>
        where T : class
        where TIntention : IBaseComparerIntention
    {
        private readonly IComparer<T, TIntention> comparer;
        private readonly Type subComparerType;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerHelperStrategySubCompareComparer{T,TComparerIntention}"/> class.
        /// </summary>
        public EqualityComparerHelperStrategySubCompareComparer(IComparer<T, TIntention> comparer)
        {
            comparer.NotNull(nameof(comparer));

            this.comparer = comparer;
            this.subComparerType = EqualityComparerHelperStrategyUtils.GetCompareType(comparer);
        }

        /// <inheritdoc/>
        public bool IsElementEqualsOrExcluded(
            T candidate1,
            T candidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings,
            IEnumerable<IComparerExclusion> exclusions)
        {
            if (exclusions.Any(x => x.TypeOfComparerExclusion == TypeOfComparerExclusion.SubInterface &&
                                      x.ExcludedModelType == this.subComparerType))
            {
                return true;
            }

            return this.comparer.Equals(candidate1, candidate2, additionalProcessings, exclusions);
        }

        /// <inheritdoc/>
        public int? GetElementHashCode(T model)
        {
            return this.comparer.GetHashCode(model);
        }
    }
}