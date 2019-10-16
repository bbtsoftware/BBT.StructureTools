// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StrategyPattern;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Generic implementation of <see cref="ISourceConvertStrategy{TSource, TTarget, TIntention}"/>
    /// for creating a specific target implementation before converting.
    /// </summary>
    /// <typeparam name="TSource">Source base type.</typeparam>
    /// <typeparam name="TTarget">Target base type.</typeparam>
    /// <typeparam name="TIntention">Conversion use case defining intention.</typeparam>
    /// <typeparam name="TCriterion">Criterion type - Specific interface of <typeparamref name="TSource"/>.</typeparam>
    /// <typeparam name="TTargetInterface">Target interface type - Specific interface of <typeparamref name="TTarget"/>.</typeparam>
    public class GenericSourceConvertStrategy<TSource, TTarget, TIntention, TCriterion, TTargetInterface> : ISourceConvertStrategy<TSource, TTarget, TIntention>
        where TSource : class
        where TTarget : class
        where TIntention : IBaseConvertIntention
        where TCriterion : class, TSource
        where TTargetInterface : class, TTarget
    {
        private readonly IConvert<TCriterion, TTargetInterface, TIntention> mConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericSourceConvertStrategy{TSource, TTarget, TIntention, TCriterion, TTargetInterface}"/> class.
        /// </summary>
        public GenericSourceConvertStrategy(
            IConvert<TCriterion, TTargetInterface, TIntention> aConverter)
        {
            aConverter.Should().NotBeNull();

            this.mConverter = aConverter;
        }

        /// <summary>
        /// See <see cref="ISourceConvertStrategy{TBaseSource, TBaseTarget, TIntention}.Convert(TBaseSource, TBaseTarget, ICollection{IBaseAdditionalProcessing})"/>.
        /// </summary>
        public void Convert(TSource source, TTarget target, ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            var lSourceCasted = ReflectionUtils.CastIfTypeOrSubtypeOrThrow<TCriterion>(source);
            var lTargetCasted = ReflectionUtils.CastIfTypeOrSubtypeOrThrow<TTargetInterface>(target);

            this.mConverter.Convert(lSourceCasted, lTargetCasted, additionalProcessings);
        }

        /// <summary>
        /// See <see cref="IGenericStrategy{T}"/>.
        /// </summary>
        public bool IsResponsible(TSource aCriterion)
        {
            var lIsResponsible = aCriterion is TCriterion;
            return lIsResponsible;
        }
    }
}
