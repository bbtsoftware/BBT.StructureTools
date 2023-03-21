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
        public void Convert(TSource source, TTarget target, ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            var strategy = this.strategyProvider.GetStrategy(source);
            strategy.Convert(source, target, additionalProcessings);
        }

        /// <inheritdoc/>
        public TTarget Create(TSource source, TReverseRelation reverseRelation)
        {
            var strategy = this.strategyProvider.GetStrategy(source);
            var concreteTarget = strategy.CreateTarget(source);
            concreteTarget.SetPropertyValue(this.reverseRelationExpr, reverseRelation);
            
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