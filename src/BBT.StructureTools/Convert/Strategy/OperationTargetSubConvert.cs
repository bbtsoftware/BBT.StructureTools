namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

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
        public OperationTargetSubConvert(
            IConvert<TSource, TTargetValue, TConvertIntention> convert)
        {
            StructureToolsArgumentChecks.NotNull(convert, nameof(convert));
            this.convert = convert;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            StructureToolsArgumentChecks.NotNull(source, nameof(source));
            StructureToolsArgumentChecks.NotNull(target, nameof(target));
            StructureToolsArgumentChecks.NotNull(additionalProcessings, nameof(additionalProcessings));

            var targetValue = StructureToolsArgumentChecks.IsOfType<TTargetValue>(target, nameof(target));
            this.convert.Convert(source, targetValue, additionalProcessings);
        }
    }
}
