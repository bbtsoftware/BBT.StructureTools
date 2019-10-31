// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare.Helper.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Compare;

    /// <summary>
    /// See <see cref="IEqualityComparerHelperStrategy{TModel}"/>.
    /// </summary>
    /// <typeparam name="TModel">Owner of the attribute to compare.</typeparam>
    /// <typeparam name="TValue">The value type to compare.</typeparam>
    internal class EqualityComparerHelperStrategyCompareAttribute<TModel, TValue> : IEqualityComparerHelperStrategy<TModel>
    {
        /// <summary>
        /// Function to get the property value.
        /// </summary>
        private readonly Func<TModel, TValue> func;

        /// <summary>
        /// Name of compared property.
        /// </summary>
        private readonly string propertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerHelperStrategyCompareAttribute{TModel,TValue}"/> class.
        /// </summary>
        public EqualityComparerHelperStrategyCompareAttribute(Expression<Func<TModel, TValue>> expression)
        {
            this.func = expression.Compile();
            this.propertyName = EqualityComparerHelperStrategyUtils.GetPropertyName(expression);
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

            var value1 = this.func.Invoke(candidate1);
            var value2 = this.func.Invoke(candidate2);

            var isEqual = EqualityComparer<TValue>.Default.Equals(value1, value2);
            return isEqual;
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperStrategy{TModel}.GetElementHashCode"/>.
        /// </summary>
        public int? GetElementHashCode(TModel model)
        {
            var value = this.func.Invoke(model);
            if (value == null)
            {
                return null;
            }

            return EqualityComparer<TValue>.Default.GetHashCode(value);
        }
    }
}