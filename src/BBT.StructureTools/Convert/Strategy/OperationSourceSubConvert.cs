namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationSourceSubConvert<TSource, TTarget, TSourceValue, TConvertIntention>
        : IOperationSourceSubConvert<TSource, TTarget, TSourceValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTarget, TConvertIntention> convert;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationSourceSubConvert{TSource,TTarget,TSourceValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationSourceSubConvert(
            IConvert<TSourceValue, TTarget, TConvertIntention> convert)
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
            if (source is TSourceValue sourceValue)
            {
                this.convert.Convert(sourceValue, target, additionalProcessings);
            }
        }
    }
}
