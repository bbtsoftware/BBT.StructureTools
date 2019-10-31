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
        /// ist of registered compare strategies.
        /// </summary>
        private readonly IEnumerable<IEqualityComparerHelperStrategy<TModel>> registeredStrategies;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerHelperOperations{TModel}"/> class.
        /// </summary>
        internal EqualityComparerHelperOperations(IEnumerable<IEqualityComparerHelperStrategy<TModel>> registeredStrategies)
        {
            registeredStrategies.Should().NotBeNull();

            this.registeredStrategies = registeredStrategies;
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperOperations{TModel}.AreRegistrationsEquals(TModel, TModel)"/>.
        /// </summary>
        public bool AreRegistrationsEquals(TModel candidate1, TModel candidate2)
        {
            return this.AreRegistrationsEquals(
                candidate1, candidate2, System.Array.Empty<IBaseAdditionalProcessing>(), new List<IComparerExclusion>());
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperOperations{TModel}.AreRegistrationsEquals(TModel,TModel,ICollection{IBaseAdditionalProcessing},IEnumerable{IComparerExclusion})"/>.
        /// </summary>
        public bool AreRegistrationsEquals(
            TModel candidate1,
            TModel candidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings,
            IEnumerable<IComparerExclusion> exclusions)
        {
            if (candidate1 == null && candidate2 == null)
            {
                return true;
            }

            if (candidate1 == null || candidate2 == null)
            {
                return false;
            }

            foreach (var registeredStrategy in this.registeredStrategies)
            {
                if (!registeredStrategy.IsElementEqualsOrExcluded(candidate1, candidate2, additionalProcessings, exclusions))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperOperations{TModel}.GetHashCodeOfRegistrations(TModel)"/>.
        /// </summary>
        public int GetHashCodeOfRegistrations(TModel model)
        {
            var hashCodes = this.registeredStrategies
                .Select(x => x.GetElementHashCode(model))
                .Where(x => x.HasValue)
                .Select(x => x.Value)
                .ToList();

            // Hash in case of no attributes
            var hash = model.GetType().GetHashCode();

            for (var index = 0; index < hashCodes.Count; index++)
            {
                hash ^= BitOperations.RotateL(hashCodes.ElementAt(index), index % 32);
            }

            return hash;
        }
    }
}