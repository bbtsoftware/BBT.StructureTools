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
    /// <typeparam name="TCriterion">Criterion type - Specific interface of <typeparamref name="TSource"/>.</typeparam>
    /// <typeparam name="TTargetInterface">Target interface type - Specific interface of <typeparamref name="TTarget"/>.</typeparam>
    internal class GenericSourceConvertStrategy<TSource, TTarget, TIntention, TCriterion, TTargetInterface> : ISourceConvertStrategy<TSource, TTarget, TIntention>
        where TSource : class
        where TTarget : class
        where TIntention : IBaseConvertIntention
        where TCriterion : class, TSource
        where TTargetInterface : class, TTarget
    {
        private readonly IConverter<TCriterion, TTargetInterface, TIntention> converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericSourceConvertStrategy{TSource, TTarget, TIntention, TCriterion, TTargetInterface}"/> class.
        /// </summary>
        public GenericSourceConvertStrategy(
            IConverter<TCriterion, TTargetInterface, TIntention> converter)
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

            var sourceCasted = ReflectionUtils.CastIfTypeOrSubtypeOrThrow<TCriterion>(source);
            var targetCasted = ReflectionUtils.CastIfTypeOrSubtypeOrThrow<TTargetInterface>(target);

            this.converter.Convert(sourceCasted, targetCasted, additionalProcessings);
        }

        /// <inheritdoc/>
        public bool IsResponsible(TSource criterion)
        {
            var isResponsible = criterion is TCriterion;
            return isResponsible;
        }
    }
}
