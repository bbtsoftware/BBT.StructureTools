namespace BBT.StructureTools.Extensions.Convert
{
    using System.Collections.Generic;
    using BBT.StrategyPattern;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    public class CreateConvertFromStrategyHelper<TSource, TTarget, TIntention>
        : ICreateConvertFromStrategyHelper<TSource, TTarget, TIntention>
        where TSource : class
        where TTarget : class
        where TIntention : IBaseConvertIntention
    {
        private readonly IGenericStrategyProvider<ICreateConvertStrategy<TSource, TTarget, TIntention>, TSource> strategyProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateConvertFromStrategyHelper{TSource, TTarget, TIntention}" /> class.
        /// </summary>
        public CreateConvertFromStrategyHelper(IGenericStrategyProvider<ICreateConvertStrategy<TSource, TTarget, TIntention>, TSource> strategyProvider)
        {
            strategyProvider.NotNull(nameof(strategyProvider));

            this.strategyProvider = strategyProvider;
        }

        /// <inheritdoc/>
        public TTarget CreateTarget(TSource source, ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.NotNull(nameof(source));
            additionalProcessings.NotNull(nameof(additionalProcessings));

            var strategy = this.strategyProvider.GetStrategy(source);

            var concreteTarget = strategy.CreateTarget(source);
            strategy.Convert(source, concreteTarget, additionalProcessings);

            return concreteTarget;
        }
    }
}