namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// Generic implementation of <see cref="ISourceConvertStrategy{TSource, TTarget, TIntention}"/>
    /// for creating a specific target implementation before converting.
    /// </summary>
    /// <typeparam name="TSource">Source base type.</typeparam>
    /// <typeparam name="TTarget">Target base type.</typeparam>
    /// <typeparam name="TIntention">Conversion use case defining intention.</typeparam>
    /// <typeparam name="TSourceInterface">Source interface type - Specific interface of <typeparamref name="TSource"/>.</typeparam>
    /// <typeparam name="TCriterion">Criterion type - Specific interface of <typeparamref name="TTarget"/>.</typeparam>
    internal class GenericTargetConvertStrategy<TSource, TTarget, TIntention, TSourceInterface, TCriterion> : ITargetConvertStrategy<TSource, TTarget, TIntention>
        where TSource : class
        where TTarget : class
        where TIntention : IBaseConvertIntention
        where TSourceInterface : class, TSource
        where TCriterion : class, TTarget
    {
        private readonly IConvert<TSourceInterface, TCriterion, TIntention> converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericTargetConvertStrategy{TSource, TTarget, TIntention, TSourceInterface, TCriterion}"/> class.
        /// </summary>
        public GenericTargetConvertStrategy(
            IConvert<TSourceInterface, TCriterion, TIntention> converter)
        {
            converter.NotNull(nameof(converter));

            this.converter = converter;
        }

        /// <inheritdoc/>
        public void Convert(TSource source, TTarget target, ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));
            additionalProcessings.NotNull(nameof(additionalProcessings));

            var sourceCasted = source as TSourceInterface;
            var targetCasted = target as TCriterion;

            sourceCasted.NotNull(nameof(sourceCasted));
            targetCasted.NotNull(nameof(targetCasted));

            this.converter.Convert(sourceCasted, targetCasted, additionalProcessings);
        }

        /// <inheritdoc/>
        public bool IsResponsible(TTarget criterion)
        {
            var isResponsible = criterion is TCriterion;
            return isResponsible;
        }
    }
}
