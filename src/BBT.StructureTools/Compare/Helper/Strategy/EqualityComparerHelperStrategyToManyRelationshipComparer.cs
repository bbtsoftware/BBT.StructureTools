// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare.Helper.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BBT.StructureTools.Compare;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IEqualityComparerHelperStrategy{TModel}"/>.
    /// </summary>
    /// <typeparam name="TModel">Owner of the attribute to compare.</typeparam>
    /// <typeparam name="TTargetModel">Target model of the relationship.</typeparam>
    /// <typeparam name="TComparerIntention">Comparer intention.</typeparam>
    internal class EqualityComparerHelperStrategyToManyRelationshipComparer<TModel, TTargetModel, TComparerIntention> : IEqualityComparerHelperStrategy<TModel>
        where TTargetModel : class
        where TComparerIntention : IBaseComparerIntention
    {
        /// <summary>
        /// Function to get the attribute value.
        /// </summary>
        private readonly Func<TModel, IEnumerable<TTargetModel>> func;

        /// <summary>
        /// The comparer.
        /// </summary>
        private readonly IComparer<TTargetModel, TComparerIntention> comparer;

        /// <summary>
        /// Name of compared property.
        /// </summary>
        private readonly string propertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerHelperStrategyToManyRelationshipComparer{TModel,TTargetModel,TComparerIntention}"/> class.
        /// </summary>
        public EqualityComparerHelperStrategyToManyRelationshipComparer(
            Expression<Func<TModel, IEnumerable<TTargetModel>>> expression,
            IComparer<TTargetModel, TComparerIntention> comparer)
        {
            expression.Should().NotBeNull();
            comparer.Should().NotBeNull();

            this.func = expression.Compile();
            this.propertyName = EqualityComparerHelperStrategyUtils.GetMethodName(expression);
            this.comparer = comparer;
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperStrategy{TModel}.IsElementEqualsOrExcluded"/>.
        /// </summary>
        public bool IsElementEqualsOrExcluded(
            TModel candidate1,
            TModel candidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings,
            IEnumerable<IComparerExclusion> exclusions)
        {
            if (EqualityComparerHelperStrategyUtils.IsPropertyExcluded(exclusions, typeof(TModel), this.propertyName))
            {
                return true;
            }

            var valuesCandidate1 = this.func.Invoke(candidate1);
            var valuesCandidate2 = this.func.Invoke(candidate2);

            var result = EqualityComparerHelperStrategyUtils.AreistEquivalent(
                valuesCandidate1,
                valuesCandidate2,
                (x, y) => this.comparer.Equals(x, y, additionalProcessings, exclusions));

            return result;
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperStrategy{TModel}.GetElementHashCode"/>.
        /// </summary>
        public int? GetElementHashCode(TModel model)
        {
            var models = this.func.Invoke(model);
            if (!models.Any())
            {
                return null;
            }

            var hash = models.Count().GetHashCode();
            foreach (var childModel in models)
            {
                hash ^= this.comparer.GetHashCode(childModel);
            }

            return hash;
        }
    }
}