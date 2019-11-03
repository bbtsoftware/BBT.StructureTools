namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StrategyPattern;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Strategy;
    using FluentAssertions;

    /// <inheritdoc/>
    internal class OperationConditionalCreateFromSourceWithReverseRelation<TSource, TTarget, TBaseSource, TBaseTarget, TIntention>
        : IOperationConditionalCreateFromSourceWithReverseRelation<TSource, TTarget, TBaseSource, TBaseTarget, TIntention>
            where TSource : class
            where TTarget : class
            where TBaseSource : class
            where TBaseTarget : class
            where TIntention : IBaseConvertIntention
    {
        private readonly IConvertStrategyProvider<TBaseSource, TBaseTarget, TIntention> convertStrategyProvider;
        private readonly IGenericStrategyProvider<ICreateByBaseAsCriterionStrategy<TBaseSource, TBaseTarget>, TBaseSource> instanceCreationStrategyProvider;
        private Func<TSource, TBaseSource> baseSourceFunc;
        private Expression<Func<TTarget, TBaseTarget>> targetValueExpression;
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
