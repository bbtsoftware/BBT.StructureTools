namespace BBT.StructureTools.Compare
{
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Compare.Helper;
    using FluentAssertions;

    /// <inheritdoc/>
    internal class Comparer<T, TIntention> : IComparer<T, TIntention>
        where T : class
        where TIntention : IBaseComparerIntention
    {
        private readonly IEqualityComparerHelperOperations<T> equalityComparerHelper;
        private readonly ICompareHelper compareHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="Comparer{TModelToCompare,TCompareIntention}" /> class.
        /// </summary>
        internal Comparer(
            ICompareRegistrations<T, TIntention> compareRegistrations,
            IEqualityComparerHelperRegistrationFactory factory,
            ICompareHelper compareHelper)
        {
            compareRegistrations.Should().NotBeNull();
            factory.Should().NotBeNull();
            compareHelper.Should().NotBeNull();

            this.compareHelper = compareHelper;
            var registrations = factory.Create<T>();
            compareRegistrations.DoRegistrations(registrations);
            this.equalityComparerHelper = registrations.EndRegistrations();
        }

        /// <inheritdoc/>
        public bool Equals(T candidate1, T candidate2)
        {
            return this.equalityComparerHelper.AreRegistrationsEquals(candidate1, candidate2);
        }

        /// <inheritdoc/>
        public bool Equals(
            T candidate1,
            T candidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            var isEqual = this.equalityComparerHelper.AreRegistrationsEquals(
                candidate1,
                candidate2,
                additionalProcessings,
                Enumerable.Empty<IComparerExclusion>());
            this.compareHelper.DoComparePostProcessing<T, TIntention>(
                candidate1,
                candidate2,
                additionalProcessings);
            return isEqual;
        }

        /// <inheritdoc/>
        public bool Equals(
            T candidate1,
            T candidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings,
            IEnumerable<IComparerExclusion> exclusions)
        {
            exclusions.Should().NotBeNull();

            var isEqual = this.equalityComparerHelper.AreRegistrationsEquals(
                candidate1,
                candidate2,
                additionalProcessings,
                exclusions);
            this.compareHelper.DoComparePostProcessing<T, TIntention>(
                candidate1,
                candidate2,
                additionalProcessings);

            return isEqual;
        }

        /// <inheritdoc/>
        public int GetHashCode(T candidate)
        {
            if (candidate == null)
            {
                return typeof(T).GetHashCode();
            }

            return this.equalityComparerHelper.GetHashCodeOfRegistrations(candidate);
        }
    }
}
