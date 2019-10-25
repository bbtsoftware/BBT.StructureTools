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
        private readonly Func<TModel, IEnumerable<TTargetModel>> mFunc;

        /// <summary>
        /// The comparer.
        /// </summary>
        private readonly IComparer<TTargetModel, TComparerIntention> mComparer;

        /// <summary>
        /// Name of compared property.
        /// </summary>
        private readonly string mPropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerHelperStrategyToManyRelationshipComparer{TModel,TTargetModel,TComparerIntention}"/> class.
        /// </summary>
        public EqualityComparerHelperStrategyToManyRelationshipComparer(
            Expression<Func<TModel, IEnumerable<TTargetModel>>> aExpression,
            IComparer<TTargetModel, TComparerIntention> aComparer)
        {
            aExpression.Should().NotBeNull();
            aComparer.Should().NotBeNull();

            this.mFunc = aExpression.Compile();
            this.mPropertyName = EqualityComparerHelperStrategyUtils.GetMethodName(aExpression);
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

            var lValuesCandidate1 = this.mFunc.Invoke(aCandidate1);
            var lValuesCandidate2 = this.mFunc.Invoke(aCandidate2);

            var lResult = EqualityComparerHelperStrategyUtils.AreListEquivalent(
                lValuesCandidate1,
                lValuesCandidate2,
                (aX, aY) => this.mComparer.Equals(aX, aY, additionalProcessings, aExclusions));

            return lResult;
        }

        /// <summary>
        /// See <see cref="IEqualityComparerHelperStrategy{TModel}.GetElementHashCode"/>.
        /// </summary>
        public int? GetElementHashCode(TModel aModel)
        {
            var lModels = this.mFunc.Invoke(aModel);
            if (!lModels.Any())
            {
                return null;
            }

            var lHash = lModels.Count().GetHashCode();
            foreach (var lModel in lModels)
            {
                lHash ^= this.mComparer.GetHashCode(lModel);
            }

            return lHash;
        }
    }
}