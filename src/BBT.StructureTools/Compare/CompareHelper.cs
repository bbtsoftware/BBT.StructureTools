// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;

    /// <summary>
    /// Provides compare functionality.
    /// </summary>
    public class CompareHelper : ICompareHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompareHelper" /> class.
        /// </summary>
        public CompareHelper()
        {
        }

        /// <summary>
        /// See <see cref="ICompareHelper.DoComparePostProcessing{T, TIntention}"/>.
        /// </summary>
        /// <typeparam name="T">See link above.</typeparam>
        /// <typeparam name="TIntention">See link above.</typeparam>
        public void DoComparePostProcessing<T, TIntention>(
            T aCandidate1Nullable,
            T aCandidate2Nullable,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where T : class
            where TIntention : IBaseComparerIntention
        {
            additionalProcessings.Should().NotBeNull();

            additionalProcessings
                .OfType<IComparePostProcessing<T, TIntention>>()
                .ToList()
                .ForEach(aX => aX.DoPostProcessing(aCandidate1Nullable, aCandidate2Nullable));
        }
    }
}
