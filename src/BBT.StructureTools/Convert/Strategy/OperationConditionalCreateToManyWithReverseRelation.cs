namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StrategyPattern;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Strategy;
    using FluentAssertions;

    /// <inheritdoc/>
    internal class OperationConditionalCreateToManyWithReverseRelation<TSource, TTarget, TBaseSource, TBaseTarget, TIntention>
        : IOperationConditionalCreateToManyWithReverseRelation<TSource, TTarget, TBaseSource, TBaseTarget, TIntention>
            where TSource : class
            where TTarget : class
            where TBaseSource : class
            where TBaseTarget : class
            where TIntention : IBaseConvertIntention
    {
        private readonly IConvertStrategyProvider<TBaseSource, TBaseTarget, TIntention> convertStrategyProvider;
        private readonly IGenericStrategyProvider<ICreateByBaseAsCriterionStrategy<TBaseSource, TBaseTarget>, TBaseSource> createInstanceStrategyProvider;

        private Func<TSource, IEnumerable<TBaseSource>> source;
        private Expression<Func<TTarget, ICollection<TBaseTarget>>> targetParent;
        private Expression<Func<TBaseTarget, TTarget>> reverseRelationOnTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConditionalCreateToManyWithReverseRelation{TSource, TTarget, TBaseSource, TBaseTarget, TIntention}"/> class.
        /// </summary>
        public OperationConditionalCreateToManyWithReverseRelation(
            IConvertStrategyProvider<TBaseSource, TBaseTarget, TIntention> convertStrategyProvider,
            IGenericStrategyProvider<ICreateByBaseAsCriterionStrategy<TBaseSource, TBaseTarget>, TBaseSource> createInstanceStrategyProvider)
        {
            convertStrategyProvider.Should().NotBeNull();
            createInstanceStrategyProvider.Should().NotBeNull();

            this.convertStrategyProvider = convertStrategyProvider;
            this.createInstanceStrategyProvider = createInstanceStrategyProvider;
        }

        /// <inheritdoc/>
        public void Execute(TSource source, TTarget target, ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            var sourceChildren = this.source.Invoke(source);
            foreach (var sourceChildElement in sourceChildren)
            {
                var instanceCreationStrategy = this.createInstanceStrategyProvider.GetStrategy(sourceChildElement);
                var childTarget = instanceCreationStrategy.CreateInstance();

                var strategy = this.convertStrategyProvider.GetConvertStrategyFromSource(sourceChildElement);
                strategy.Convert(sourceChildElement, childTarget, additionalProcessings);

                // set reverse relation
                this.reverseRelationOnTarget.Compile().Invoke(childTarget);

                // Add to target collection
                this.targetParent.Compile().Invoke(target).Add(childTarget);
            }
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, IEnumerable<TBaseSource>> source,
            Expression<Func<TTarget, ICollection<TBaseTarget>>> targetParent,
            Expression<Func<TBaseTarget, TTarget>> reverseRelationOnTarget)
        {
            source.Should().NotBeNull();
            targetParent.Should().NotBeNull();
            reverseRelationOnTarget.Should().NotBeNull();

            this.source = source;
            this.targetParent = targetParent;
            this.reverseRelationOnTarget = reverseRelationOnTarget;
        }
    }
}
