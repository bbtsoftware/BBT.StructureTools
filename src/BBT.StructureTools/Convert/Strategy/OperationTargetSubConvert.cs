namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <inheritdoc/>
    internal class OperationTargetSubConvert<TSource, TTarget, TTargetValue, TConvertIntention>
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

        /// <inheritdoc/>
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
