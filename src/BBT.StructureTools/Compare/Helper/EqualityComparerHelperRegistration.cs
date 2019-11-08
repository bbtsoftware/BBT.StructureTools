namespace BBT.StructureTools.Compare.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Compare.Helper.Strategy;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    public class EqualityComparerHelperRegistration<T> : IEqualityComparerHelperRegistration<T>
        where T : class
    {
        private readonly ICollection<IEqualityComparerHelperStrategy<T>> registeredStrategies;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerHelperRegistration{T}"/> class.
        /// </summary>
        public EqualityComparerHelperRegistration()
        {
            this.registeredStrategies = new List<IEqualityComparerHelperStrategy<T>>();
        }

        /// <inheritdoc/>
        public IEqualityComparerHelperRegistration<T> RegisterAttribute<TValue>(Expression<Func<T, TValue>> expression)
        {
            this.registeredStrategies.Add(new EqualityComparerHelperStrategyCompareAttribute<T, TValue>(expression));
            return this;
        }

        /// <inheritdoc/>
        public IEqualityComparerHelperRegistration<T> RegisterAttributeWithDistinguishedComparer<TComparer, TIntention>(
            Expression<Func<T, TComparer>> expression,
            IComparer<TComparer, TIntention> comparer)
            where TComparer : class
            where TIntention : IBaseComparerIntention
        {
            this.registeredStrategies.Add(new EqualityComparerHelperStrategyCompareModel<T, TComparer, TIntention>(expression, comparer));
            return this;
        }

        /// <summary>
        /// Register a to many relationship.
        /// </summary>
        /// <typeparam name="TComparer">Type of combined-comparer"/>.</typeparam>
        /// <typeparam name="TComparerIntention">The comparer intention.</typeparam>
        public IEqualityComparerHelperRegistration<T> RegisterToManyRelationship<TComparer, TComparerIntention>(
            Expression<Func<T, IEnumerable<TComparer>>> expression, IComparer<TComparer, TComparerIntention> comparer)
            where TComparer : class
            where TComparerIntention : IBaseComparerIntention
        {
            comparer.NotNull(nameof(comparer));

            this.registeredStrategies.Add(
                new EqualityComparerHelperStrategyToManyRelationshipComparer<T, TComparer, TComparerIntention>(
                    expression, comparer));

            return this;
        }

        /// <inheritdoc/>
        public IEqualityComparerHelperRegistration<T> RegisterSubCompare<TComparerIntention>(IComparer<T, TComparerIntention> comparer)
            where TComparerIntention : IBaseComparerIntention
        {
            comparer.NotNull(nameof(comparer));

            this.registeredStrategies.Add(new EqualityComparerHelperStrategySubCompareComparer<T, TComparerIntention>(comparer));

            return this;
        }

        /// <inheritdoc/>
        public IEqualityComparerHelperOperations<T> EndRegistrations()
        {
            return new EqualityComparerHelperOperations<T>(this.registeredStrategies);
        }
    }
}