// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Compare.Helper.Strategy;
    using FluentAssertions;

    /// <summary>
    /// Helper for the equals and get hash code calculation.
    /// </summary>
    /// <typeparam name="T">Class to compare.</typeparam>
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

        /// <summary>
        /// Register a compare attribute of type <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the attribute to compare.</typeparam>
        public IEqualityComparerHelperRegistration<T> RegisterAttribute<TValue>(Expression<Func<T, TValue>> expression)
        {
            this.registeredStrategies.Add(new EqualityComparerHelperStrategyCompareAttribute<T, TValue>(expression));
            return this;
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperRegistration{T}.RegisterAttributeWithDistinguishedComparer{TComparer,TIntention}"/>.
        /// </summary>
        /// <typeparam name="TComparer">Type of model-comparer"/>.</typeparam>
        /// <typeparam name="TIntention">The compare intention.</typeparam>
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
            comparer.Should().NotBeNull();

            this.registeredStrategies.Add(
                new EqualityComparerHelperStrategyToManyRelationshipComparer<T, TComparer, TComparerIntention>(
                    expression, comparer));

            return this;
        }

        /// <summary>
        /// Register a sub compare for a base class or a implemented interface.
        /// </summary>
        /// <typeparam name="TComparerIntention">The comparer intention.</typeparam>
        public IEqualityComparerHelperRegistration<T> RegisterSubCompare<TComparerIntention>(IComparer<T, TComparerIntention> comparer)
            where TComparerIntention : IBaseComparerIntention
        {
            comparer.Should().NotBeNull();

            this.registeredStrategies.Add(new EqualityComparerHelperStrategySubCompareComparer<T, TComparerIntention>(comparer));

            return this;
        }

        /// <summary>
        /// Ends the registrations and start the operation phase.
        /// </summary>
        public IEqualityComparerHelperOperations<T> EndRegistrations()
        {
            return new EqualityComparerHelperOperations<T>(this.registeredStrategies);
        }
    }
}