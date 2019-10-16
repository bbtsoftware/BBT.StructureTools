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
        private readonly Func<TModel, TTargetModel> mFunc;

        /// <summary>
        /// Name of compared property.
        /// </summary>
        private readonly string mPropertyName;

        private readonly IComparer<TTargetModel, TIntention> mComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerHelperStrategyCompareModel{TModel,TTargetModel,TIntention}"/> class.
        /// </summary>
        public EqualityComparerHelperStrategyCompareModel(
            Expression<Func<TModel, TTargetModel>> aExpression,
            IComparer<TTargetModel, TIntention> aComparer)
        {
            aExpression.Should().NotBeNull();
            aComparer.Should().NotBeNull();

            this.mFunc = aExpression.Compile();
            this.mPropertyName = EqualityComparerHelperStrategyUtils.GetPropertyName(aExpression);
            this.mComparer = aComparer;
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

            var lValu1 = this.mFunc.Invoke(aCandidate1) as TTargetModel;
            var lValu2 = this.mFunc.Invoke(aCandidate2) as TTargetModel;

            return this.mComparer.Equals(lValu1, lValu2, additionalProcessings, aExclusions);
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperStrategy{TModel}.GetElementHashCode"/>.
        /// </summary>
        public int? GetElementHashCode(TModel aModel)
        {
            if (this.mFunc.Invoke(aModel) is TTargetModel lValue)
            {
                return this.mComparer.GetHashCode(lValue);
            }

            return null;
        }
    }
}