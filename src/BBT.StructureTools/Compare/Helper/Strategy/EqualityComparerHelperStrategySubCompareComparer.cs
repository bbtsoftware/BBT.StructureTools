// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare.Helper.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Compare;

    /// <summary>
    /// See <see cref="IEqualityComparerHelperStrategy{T}"/>.
    /// </summary>
    /// <typeparam name="T">Subtype of <see cref="IBaseModel"/>.</typeparam>
    /// <typeparam name="TIntention">Comparer intention.</typeparam>
    internal class EqualityComparerHelperStrategySubCompareComparer<T, TIntention> : IEqualityComparerHelperStrategy<T>
        where T : class
        where TIntention : IBaseComparerIntention
    {
        private readonly IComparer<T, TIntention> mComparer;
        private readonly Type mSubComparerType;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerHelperStrategySubCompareComparer{T,TComparerIntention}"/> class.
        /// </summary>
        public EqualityComparerHelperStrategySubCompareComparer(IComparer<T, TIntention> aComparer)
        {
            aComparer.Should().NotBeNull();

            this.mComparer = aComparer;
            this.mSubComparerType = EqualityComparerHelperStrategyUtils.GetCompareType(aComparer);
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperStrategy{T}.IsElementEqualsOrExcluded"/>.
        /// </summary>
        public bool IsElementEqualsOrExcluded(
            T aCandidate1,
            T aCandidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings,
            IEnumerable<IComparerExclusion> aExclusions)
        {
            if (aExclusions.Any(aX => aX.TypeOfComparerExclusion == TypeOfComparerExclusion.SubInterface &&
                                      aX.ExcludedModelType == this.mSubComparerType))
            {
                return true;
            }

            return this.mComparer.Equals(aCandidate1, aCandidate2, additionalProcessings, aExclusions);
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperStrategy{T}.GetElementHashCode"/>.
        /// </summary>
        public int? GetElementHashCode(T aModel)
        {
            return this.mComparer.GetHashCode(aModel);
        }
    }
}