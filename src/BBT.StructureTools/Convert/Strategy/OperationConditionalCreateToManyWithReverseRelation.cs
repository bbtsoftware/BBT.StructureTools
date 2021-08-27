namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StrategyPattern;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Strategy;

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
        private Expression<Func<TTarget, ICollection<TBaseTarget>>> targetParentExpression;
        private Expression<Func<TBaseTarget, TTarget>> reverseRelationOnTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConditionalCreateToManyWithReverseRelation{TSource, TTarget, TBaseSource, TBaseTarget, TIntention}"/> class.
        /// </summary>
        public OperationConditionalCreateToManyWithReverseRelation(
            IConvertStrategyProvider<TBaseSource, TBaseTarget, TIntention> convertStrategyProvider,
            IGenericStrategyProvider<ICreateByBaseAsCriterionStrategy<TBaseSource, TBaseTarget>, TBaseSource> createInstanceStrategyProvider)
        {
            convertStrategyProvider.NotNull(nameof(convertStrategyProvider));
            createInstanceStrategyProvider.NotNull(nameof(createInstanceStrategyProvider));

            this.convertStrategyProvider = convertStrategyProvider;
            this.createInstanceStrategyProvider = createInstanceStrategyProvider;
        }

        /// <inheritdoc/>
        public void Execute(TSource source, TTarget target, ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            var sourceChildren = this.source.Invoke(source);
            var childTargets = new List<TBaseTarget>();
            foreach (var sourceChildElement in sourceChildren)
            {
                var instanceCreationStrategy = this.createInstanceStrategyProvider.GetStrategy(sourceChildElement);
                var childTarget = instanceCreationStrategy.CreateInstance();

                var strategy = this.convertStrategyProvider.GetConvertStrategyFromSource(sourceChildElement);
                strategy.Convert(sourceChildElement, childTarget, additionalProcessings);

                // set reverse relation
                childTarget.SetPropertyValue(this.reverseRelationOnTarget, target);
                childTargets.Add(childTarget);
            }

            target.AddRangeFilterNullValues(
                this.targetParentExpression,
                childTargets);
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, IEnumerable<TBaseSource>> source,
            Expression<Func<TTarget, ICollection<TBaseTarget>>> targetParent,
            Expression<Func<TBaseTarget, TTarget>> reverseRelationOnTarget)
        {
            source.NotNull(nameof(source));
            targetParent.NotNull(nameof(targetParent));
            reverseRelationOnTarget.NotNull(nameof(reverseRelationOnTarget));

            this.source = source;
            this.targetParentExpression = targetParent;
            this.reverseRelationOnTarget = reverseRelationOnTarget;
        }
    }
}
