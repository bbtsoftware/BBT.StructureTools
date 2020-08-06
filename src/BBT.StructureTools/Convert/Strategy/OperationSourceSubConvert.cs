namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools;
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

            // ToDo BBTL-5178: IF sollte nicht mehr nötig sein, wenn die entsprechenden
            // Registrierungen und Implementation die korrekte Basis aufrufen!
            if (source is TSourceValue sourceValue)
            {
                this.convert.Convert(sourceValue, target, additionalProcessings);
            }
        }
    }
}
