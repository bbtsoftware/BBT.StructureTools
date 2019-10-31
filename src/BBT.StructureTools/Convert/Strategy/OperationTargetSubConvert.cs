// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TTargetValue">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public class OperationTargetSubConvert<TSource, TTarget, TTargetValue, TConvertIntention>
        : IOperationTargetSubConvert<TSource, TTarget, TTargetValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TTargetValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSource, TTargetValue, TConvertIntention> convert;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationTargetSubConvert{TSource,TTarget,TTargetValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationTargetSubConvert(IConvert<TSource, TTargetValue, TConvertIntention> convert)
        {
            convert.Should().NotBeNull();

            this.convert = convert;
        }

        /// <summary>
        /// See <see cref="IConvertOperation{TSource,TTarget}.Execute"/>.
        /// </summary>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            var targetValue = ReflectionUtils.CastIfTypeOrSubtypeOrThrow<TTargetValue>(target);
            this.convert.Convert(source, targetValue, additionalProcessings);
        }
    }
}
