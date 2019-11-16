namespace BBT.StructureTools.Compare.Helper
{
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Compare.Helper.Strategy;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class EqualityComparerHelperOperations<TModel> : IEqualityComparerHelperOperations<TModel>
        where TModel : class
    {
        private readonly IEnumerable<IEqualityComparerHelperStrategy<TModel>> registeredStrategies;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerHelperOperations{TModel}"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is required and needs to be public because of the issue
        /// described in GH-17.
        /// </remarks>
        public EqualityComparerHelperOperations(IEnumerable<IEqualityComparerHelperStrategy<TModel>> registeredStrategies)
        {
            registeredStrategies.NotNull(nameof(registeredStrategies));

            this.registeredStrategies = registeredStrategies;
        }

        /// <inheritdoc/>
        public bool AreRegistrationsEquals(TModel candidate1, TModel candidate2)
        {
            return this.AreRegistrationsEquals(
                candidate1, candidate2, System.Array.Empty<IBaseAdditionalProcessing>(), new List<IComparerExclusion>());
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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