// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare.Helper.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Compare;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IEqualityComparerHelperStrategy{TModel}"/>.
    /// </summary>
    /// <typeparam name="TModel">Owner of the attribute to compare.</typeparam>
    /// <typeparam name="TTargetModel">The attribute to compare.</typeparam>
    /// <typeparam name="TIntention">The compare intention.</typeparam>
    internal class EqualityComparerHelperStrategyCompareModel<TModel, TTargetModel, TIntention>
        : IEqualityComparerHelperStrategy<TModel>
        where TTargetModel : class
        where TIntention : IBaseComparerIntention
    {
        /// <summary>
        /// Function to get the property value.
        /// </summary>
        private readonly Func<TModel, TTargetModel> func;

        /// <summary>
        /// Name of compared property.
        /// </summary>
        private readonly string propertyName;

        private readonly IComparer<TTargetModel, TIntention> comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerHelperStrategyCompareModel{TModel,TTargetModel,TIntention}"/> class.
        /// </summary>
        public EqualityComparerHelperStrategyCompareModel(
            Expression<Func<TModel, TTargetModel>> expression,
            IComparer<TTargetModel, TIntention> comparer)
        {
            expression.Should().NotBeNull();
            comparer.Should().NotBeNull();

            this.func = expression.Compile();
            this.propertyName = EqualityComparerHelperStrategyUtils.GetPropertyName(expression);
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

            var value1 = this.func.Invoke(candidate1) as TTargetModel;
            var value2 = this.func.Invoke(candidate2) as TTargetModel;

            return this.comparer.Equals(value1, value2, additionalProcessings, exclusions);
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperStrategy{TModel}.GetElementHashCode"/>.
        /// </summary>
        public int? GetElementHashCode(TModel model)
        {
            if (this.func.Invoke(model) is TTargetModel value)
            {
                return this.comparer.GetHashCode(value);
            }

            return null;
        }
    }
}