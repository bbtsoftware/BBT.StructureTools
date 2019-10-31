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
        private readonly IConvertStrategyProvider<TBaseSource, TBaseTarget, TIntention> convertStrategyProvider;
        private readonly IGenericStrategyProvider<ICreateByBaseAsCriterionStrategy<TBaseSource, TBaseTarget>, TBaseSource> instanceCreationStrategyProvider;

        /// <summary>
        /// Function which declares the base source value (e.g. LiBaseCover).
        /// </summary>
        private Func<TSource, TBaseSource> baseSourceFunc;

        /// <summary>
        /// Expression which declares the target value (created target class, e.g. LiClaicover).
        /// </summary>
        private Expression<Func<TTarget, TBaseTarget>> targetValueExpression;

        /// <summary>
        /// Expression which declares the target parent (e.g. LiClaicoverWrapper).
        /// </summary>
        private Expression<Func<TBaseTarget, TTarget>> targetParentExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConditionalCreateFromSourceWithReverseRelation{TSource, TTarget, TTargetValue, TBaseTarget, TIntention}" /> class.
        /// </summary>
        public OperationConditionalCreateFromSourceWithReverseRelation(
            IConvertStrategyProvider<TBaseSource, TBaseTarget, TIntention> convertStrategyProvider,
            IGenericStrategyProvider<ICreateByBaseAsCriterionStrategy<TBaseSource, TBaseTarget>, TBaseSource> instanceCreationStrategyProvider)
        {
            convertStrategyProvider.Should().NotBeNull();
            instanceCreationStrategyProvider.Should().NotBeNull();

            this.convertStrategyProvider = convertStrategyProvider;
            this.instanceCreationStrategyProvider = instanceCreationStrategyProvider;
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

            this.baseSourceFunc = aBaseSourceFunc;
            this.targetValueExpression = targetValueExpression;
            this.targetParentExpression = targetParentExpression;
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

            var baseSource = this.baseSourceFunc(source);

            var instanceCreationStrategy = this.instanceCreationStrategyProvider.GetStrategy(baseSource);
            var target = instanceCreationStrategy.CreateInstance();

            var strategy = this.convertStrategyProvider.GetConvertStrategyFromSource(baseSource);

            // Sets reference to the child on the parent class
            targetParent.SetPropertyValue(this.targetValueExpression, target);

            // Sets reference to the parent on the child class
            target.SetPropertyValue(this.targetParentExpression, targetParent);

            strategy.Convert(baseSource, target, additionalProcessings);
        }
    }
}
