// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare
{
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Compare.Helper;
    using FluentAssertions;

    /// <summary>
    /// Generic comparer"/>.
    /// Declaration of attributes to compare is delegated to
    /// <see cref="ICompareRegistrations{TModelToCompare,TCompareIntention}"/>.
    /// </summary>
    /// <typeparam name="T">The model type which shall be compared.</typeparam>
    /// <typeparam name="TIntention">The intention of the comparison.</typeparam>
    public class Comparer<T, TIntention> : IComparer<T, TIntention>
        where T : class
        where TIntention : IBaseComparerIntention
    {
        private readonly IEqualityComparerHelperOperations<T> mEqualityComparerHelper;
        private readonly ICompareHelper mCompareHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="Comparer{TModelToCompare,TCompareIntention}" /> class.
        /// </summary>
        public Comparer(
            ICompareRegistrations<T, TIntention> aCompareRegistrations,
            IEqualityComparerHelperRegistrationFactory factory,
            ICompareHelper aCompareHelper)
        {
            aCompareRegistrations.Should().NotBeNull();
            factory.Should().NotBeNull();
            aCompareHelper.Should().NotBeNull();

            this.mCompareHelper = aCompareHelper;
            var lRegistrations = factory.Create<T>();
            aCompareRegistrations.DoRegistrations(lRegistrations);
            this.mEqualityComparerHelper = lRegistrations.EndRegistrations();
        }

        /// <summary>
        /// See <see cref="IEqualityComparer{T}.Equals(T,T)"/>.
        /// </summary>
        public bool Equals(T aCandidate1, T aCandidate2)
        {
            return this.mEqualityComparerHelper.AreRegistrationsEquals(aCandidate1, aCandidate2);
        }

        /// <summary>
        /// See <see cref="IComparer{T, TComparerIntention}.Equals(T, T, ICollection{IBaseAdditionalProcessing})"/>.
        /// </summary>
        public bool Equals(
            T aCandidate1,
            T aCandidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            var lIsEqual = this.mEqualityComparerHelper.AreRegistrationsEquals(
                aCandidate1,
                aCandidate2,
                additionalProcessings,
                Enumerable.Empty<IComparerExclusion>());
            this.mCompareHelper.DoComparePostProcessing<T, TIntention>(
                aCandidate1,
                aCandidate2,
                additionalProcessings);
            return lIsEqual;
        }

        /// <summary>
        /// See <see cref="IComparer{T, TComparerIntention}.Equals(T, T, ICollection{IBaseAdditionalProcessing}, IEnumerable{IComparerExclusion})"/>.
        /// </summary>
        public bool Equals(
            T aCandidate1,
            T aCandidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings,
            IEnumerable<IComparerExclusion> aExclusions)
        {
            aExclusions.Should().NotBeNull();


            var lIsEqual = this.mEqualityComparerHelper.AreRegistrationsEquals(
                aCandidate1,
                aCandidate2,
                additionalProcessings,
                aExclusions);
            this.mCompareHelper.DoComparePostProcessing<T, TIntention>(
                aCandidate1,
                aCandidate2,
                additionalProcessings);

            return lIsEqual;
        }

        /// <summary>
        /// See <see cref="IEqualityComparer{T}.GetHashCode(T)"/>.
        /// </summary>
        public int GetHashCode(T aCandidate)
        {
            if (aCandidate == null)
            {
                return typeof(T).GetHashCode();
            }

            return this.mEqualityComparerHelper.GetHashCodeOfRegistrations(aCandidate);
        }
    }
}
