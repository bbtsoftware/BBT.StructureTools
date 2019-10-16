// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StrategyPattern;
    using FluentAssertions;

    /// <summary>
    /// Generic implementation of <see cref="ISourceConvertStrategy{TSource, TTarget, TIntention}"/>
    /// for creating a specific target implementation before converting.
    /// </summary>
    /// <typeparam name="TSource">Source base type.</typeparam>
    /// <typeparam name="TTarget">Target base type.</typeparam>
    /// <typeparam name="TIntention">Conversion use case defining intention.</typeparam>
    /// <typeparam name="TSourceInterface">Source interface type - Specific interface of <typeparamref name="TSource"/>.</typeparam>
    /// <typeparam name="TCriterion">Criterion type - Specific interface of <typeparamref name="TTarget"/>.</typeparam>
    public class GenericTargetConvertStrategy<TSource, TTarget, TIntention, TSourceInterface, TCriterion> : ITargetConvertStrategy<TSource, TTarget, TIntention>
        where TSource : class
        where TTarget : class
        where TIntention : IBaseConvertIntention
        where TSourceInterface : class, TSource
        where TCriterion : class, TTarget
    {
        private readonly IConvert<TSourceInterface, TCriterion, TIntention> mConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericTargetConvertStrategy{TSource, TTarget, TIntention, TSourceInterface, TCriterion}"/> class.
        /// </summary>
        public GenericTargetConvertStrategy(
            IConvert<TSourceInterface, TCriterion, TIntention> aConverter)
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

            var lSourceCasted = source as TSourceInterface;
            var lTargetCasted = target as TCriterion;

            lSourceCasted.Should().NotBeNull();
            lTargetCasted.Should().NotBeNull();

            this.mConverter.Convert(lSourceCasted, lTargetCasted, additionalProcessings);
        }

        /// <summary>
        /// See <see cref="IGenericStrategy{T}"/>.
        /// </summary>
        public bool IsResponsible(TTarget aCriterion)
        {
            var lIsResponsible = aCriterion is TCriterion;
            return lIsResponsible;
        }
    }
}
