// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare.Helper
{
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Compare.Helper.Strategy;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IEqualityComparerHelperOperations{TModel}"/>.
    /// </summary>
    /// <typeparam name="TModel">Model type.</typeparam>
    public class EqualityComparerHelperOperations<TModel> : IEqualityComparerHelperOperations<TModel>
        where TModel : class
    {
        /// <summary>
        /// List of registered compare strategies.
        /// </summary>
        private readonly IEnumerable<IEqualityComparerHelperStrategy<TModel>> mRegisteredStrategies;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerHelperOperations{TModel}"/> class.
        /// </summary>
        internal EqualityComparerHelperOperations(IEnumerable<IEqualityComparerHelperStrategy<TModel>> aRegisteredStrategies)
        {
            aRegisteredStrategies.Should().NotBeNull();

            this.mRegisteredStrategies = aRegisteredStrategies;
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperOperations{TModel}.AreRegistrationsEquals(TModel, TModel)"/>.
        /// </summary>
        public bool AreRegistrationsEquals(TModel aCandidate1, TModel aCandidate2)
        {
            return this.AreRegistrationsEquals(
                aCandidate1, aCandidate2, System.Array.Empty<IBaseAdditionalProcessing>(), new List<IComparerExclusion>());
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperOperations{TModel}.AreRegistrationsEquals(TModel,TModel,ICollection{IBaseAdditionalProcessing},IEnumerable{IComparerExclusion})"/>.
        /// </summary>
        public bool AreRegistrationsEquals(
            TModel aCandidate1,
            TModel aCandidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings,
            IEnumerable<IComparerExclusion> aExclusions)
        {
            if (aCandidate1 == null && aCandidate2 == null)
            {
                return true;
            }

            if (aCandidate1 == null || aCandidate2 == null)
            {
                return false;
            }

            foreach (var lRegisteredStrategy in this.mRegisteredStrategies)
            {
                if (!lRegisteredStrategy.IsElementEqualsOrExcluded(aCandidate1, aCandidate2, additionalProcessings, aExclusions))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperOperations{TModel}.GetHashCodeOfRegistrations(TModel)"/>.
        /// </summary>
        public int GetHashCodeOfRegistrations(TModel aModel)
        {
            var lHashCodes = this.mRegisteredStrategies
                .Select(aX => aX.GetElementHashCode(aModel))
                .Where(aX => aX.HasValue)
                .Select(aX => aX.Value)
                .ToList();

            // Hash in case of no attributes
            var lHash = aModel.GetType().GetHashCode();

            for (int lIndex = 0; lIndex < lHashCodes.Count; lIndex++)
            {
                lHash ^= BitOperations.RotateL(lHashCodes.ElementAt(lIndex), lIndex % 32);
            }

            return lHash;
        }
    }
}