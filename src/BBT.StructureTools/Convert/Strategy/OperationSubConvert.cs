namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationSubConvert<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        : IOperationSubConvert<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConverter<TSourceValue, TTargetValue, TConvertIntention> convert;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationSubConvert{TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention}" /> class.
        /// </summary>
        public OperationSubConvert(
            IConverter<TSourceValue, TTargetValue, TConvertIntention> convert)
        {
            convert.NotNull(nameof(convert));

            this.convert = convert;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));
            additionalProcessings.NotNull(nameof(additionalProcessings));

            // Safe guard if the conversion is the wrong way around, eg. base convert
            // is calling the child's convert.
            if (source is TSourceValue sourceValue && target is TTargetValue targetValue)
            {
                this.convert.Convert(sourceValue, targetValue, additionalProcessings);
            }
        }
    }
}
