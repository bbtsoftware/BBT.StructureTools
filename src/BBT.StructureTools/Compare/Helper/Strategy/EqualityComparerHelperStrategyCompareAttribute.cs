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
        private readonly Func<TModel, TValue> mFunc;

        /// <summary>
        /// Name of compared property.
        /// </summary>
        private readonly string mPropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerHelperStrategyCompareAttribute{TModel,TValue}"/> class.
        /// </summary>
        public EqualityComparerHelperStrategyCompareAttribute(Expression<Func<TModel, TValue>> aExpression)
        {
            this.mFunc = aExpression.Compile();
            this.mPropertyName = EqualityComparerHelperStrategyUtils.GetPropertyName(aExpression);
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperStrategy{TModel}.IsElementEqualsOrExcluded"/>.
        /// </summary>
        public bool IsElementEqualsOrExcluded(
            TModel aCandidate1,
            TModel aCandidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings,
            IEnumerable<IComparerExclusion> aExclusions)
        {
            if (EqualityComparerHelperStrategyUtils.IsPropertyExcluded(aExclusions, typeof(TModel), this.mPropertyName))
            {
                return true;
            }

            var lValue1 = this.mFunc.Invoke(aCandidate1);
            var lValue2 = this.mFunc.Invoke(aCandidate2);

            var lIsEqual = EqualityComparer<TValue>.Default.Equals(lValue1, lValue2);
            return lIsEqual;
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperStrategy{TModel}.GetElementHashCode"/>.
        /// </summary>
        public int? GetElementHashCode(TModel aModel)
        {
            var lValue = this.mFunc.Invoke(aModel);
            if (lValue == null)
            {
                return null;
            }

            return EqualityComparer<TValue>.Default.GetHashCode(lValue);
        }
    }
}