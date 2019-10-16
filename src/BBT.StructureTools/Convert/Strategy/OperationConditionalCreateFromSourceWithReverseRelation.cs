// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StrategyPattern;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Strategy;
    using FluentAssertions;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TBaseSource">See link above.</typeparam>
    /// <typeparam name="TBaseTarget">See link above.</typeparam>
    /// <typeparam name="TIntention">See link above.</typeparam>
    public class OperationConditionalCreateFromSourceWithReverseRelation<TSource, TTarget, TBaseSource, TBaseTarget, TIntention>
        : IOperationConditionalCreateFromSourceWithReverseRelation<TSource, TTarget, TBaseSource, TBaseTarget, TIntention>
            where TSource : class
            where TTarget : class
            where TBaseSource : class
            where TBaseTarget : class
            where TIntention : IBaseConvertIntention
    {
        private readonly IConvertStrategyProvider<TBaseSource, TBaseTarget, TIntention> mConvertStrategyProvider;
        private readonly IGenericStrategyProvider<ICreateByBaseAsCriterionStrategy<TBaseSource, TBaseTarget>, TBaseSource> mInstanceCreationStrategyProvider;

        /// <summary>
        /// Function which declares the base source value (e.g. LiBaseCover).
        /// </summary>
        private Func<TSource, TBaseSource> mBaseSourceFunc;

        /// <summary>
        /// Expression which declares the target value (created target class, e.g. LiClaimCover).
        /// </summary>
        private Expression<Func<TTarget, TBaseTarget>> mTargetValueExpression;

        /// <summary>
        /// Expression which declares the target parent (e.g. LiClaimCoverWrapper).
        /// </summary>
        private Expression<Func<TBaseTarget, TTarget>> mTargetParentExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConditionalCreateFromSourceWithReverseRelation{TSource, TTarget, TTargetValue, TBaseTarget, TIntention}" /> class.
        /// </summary>
        public OperationConditionalCreateFromSourceWithReverseRelation(
            IConvertStrategyProvider<TBaseSource, TBaseTarget, TIntention> aConvertStrategyProvider,
            IGenericStrategyProvider<ICreateByBaseAsCriterionStrategy<TBaseSource, TBaseTarget>, TBaseSource> aInstanceCreationStrategyProvider)
        {
            aConvertStrategyProvider.Should().NotBeNull();
            aInstanceCreationStrategyProvider.Should().NotBeNull();

            this.mConvertStrategyProvider = aConvertStrategyProvider;
            this.mInstanceCreationStrategyProvider = aInstanceCreationStrategyProvider;
        }

        /// <summary>
        /// See <see cref="IOperationConditionalCreateFromSourceWithReverseRelation{TSource, TTarget, TTargetValue, TBaseTarget, TIntention}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, TBaseSource> aBaseSourceFunc,
            Expression<Func<TTarget, TBaseTarget>> targetValueExpression,
            Expression<Func<TBaseTarget, TTarget>> targetParentExpression)
        {
            aBaseSourceFunc.Should().NotBeNull();
            targetValueExpression.Should().NotBeNull();
            targetParentExpression.Should().NotBeNull();

            this.mBaseSourceFunc = aBaseSourceFunc;
            this.mTargetValueExpression = targetValueExpression;
            this.mTargetParentExpression = targetParentExpression;
        }

        /// <summary>
        /// See <see cref="IConvertOperation{TSource,TTarget}.Execute"/>.
        /// </summary>
        public void Execute(
            TSource source,
            TTarget targetParent,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();

            targetParent.Should().NotBeNull();

            var lBaseSource = this.mBaseSourceFunc(source);

            var lInstanceCreationStrategy = this.mInstanceCreationStrategyProvider.GetStrategy(lBaseSource);
            var lTarget = lInstanceCreationStrategy.CreateInstance();

            var lStrategy = this.mConvertStrategyProvider.GetConvertStrategyFromSource(lBaseSource);

            // Sets reference to the child on the parent class
            targetParent.SetPropertyValue(this.mTargetValueExpression, lTarget);

            // Sets reference to the parent on the child class
            lTarget.SetPropertyValue(this.mTargetParentExpression, targetParent);

            lStrategy.Convert(lBaseSource, lTarget, additionalProcessings);
        }
    }
}
