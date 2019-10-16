// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IConvert{TSourceClass,TTargetClass,TConvertIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public class Converter<TSource, TTarget, TConvertIntention> : IConvert<TSource, TTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvertOperations<TSource, TTarget> mConvertOperations;
        private readonly IConvertHelper mConvertHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="Converter{TSource, TTarget, TConvertIntention}" /> class.
        /// </summary>
        public Converter(
            IConvertRegistrations<TSource, TTarget, TConvertIntention> aConvertRegistrations,
            IConvertEngine<TSource, TTarget> aConvertEngine,
            IConvertHelper aConvertHelper)
        {
            aConvertEngine.Should().NotBeNull();
            aConvertRegistrations.Should().NotBeNull();
            aConvertHelper.Should().NotBeNull();

            this.mConvertHelper = aConvertHelper;
            var lRegistrations = aConvertEngine.StartRegistrations();
            aConvertRegistrations.DoRegistrations(lRegistrations);
            this.mConvertOperations = lRegistrations.EndRegistrations();
        }

        /// <summary>
        /// See <see cref="IConvert{TSourceClass, TTargetClass, TConvertIntention}.Convert"/>.
        /// </summary>
        public void Convert(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            this.mConvertHelper.DoConvertPreProcessing(source, target, additionalProcessings);
            this.mConvertOperations.Convert(source, target, additionalProcessings);
            this.mConvertHelper.DoConvertPostProcessing(source, target, additionalProcessings);
        }
    }
}
