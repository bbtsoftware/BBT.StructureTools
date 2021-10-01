namespace BBT.StructureTools.Extensions.Convert
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StrategyPattern;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    public class CreateConvertFromStrategyHelperReverse<TSource, TTarget, TReverseRelation, TIntention>
        : ICreateConvertFromStrategyHelper<TSource, TTarget, TReverseRelation, TIntention>
        where TSource : class
        where TTarget : class
        where TReverseRelation : class
        where TIntention : IBaseConvertIntention
    {
        private readonly IGenericStrategyProvider<ICreateConvertStrategy<TSource, TTarget, TIntention>, TSource> strategyProvider;
        private Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateConvertFromStrategyHelperReverse{TSource, TTarget, TReverseRelation, TIntention}" /> class.
        /// </summary>
        public CreateConvertFromStrategyHelperReverse(IGenericStrategyProvider<ICreateConvertStrategy<TSource, TTarget, TIntention>, TSource> strategyProvider)
        {
            strategyProvider.NotNull(nameof(strategyProvider));

            this.strategyProvider = strategyProvider;
        }

        /// <inheritdoc/>
        public TTarget CreateTarget(
            TSource source,
            TReverseRelation reverseRelation,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.NotNull(nameof(source));
            reverseRelation.NotNull(nameof(reverseRelation));

            var strategy = this.strategyProvider.GetStrategy(source);
            var concreteTarget = strategy.CreateTarget(source);
            concreteTarget.SetPropertyValue(this.reverseRelationExpr, reverseRelation);
            strategy.Convert(source, concreteTarget, additionalProcessings);
            return concreteTarget;
        }

        /// <inheritdoc/>
        public void SetupReverseRelation(
            Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr)
        {
            reverseRelationExpr.NotNull(nameof(reverseRelationExpr));

            this.reverseRelationExpr = reverseRelationExpr;
        }
    }
}