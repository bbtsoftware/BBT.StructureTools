namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationConvertFromTargetOnDifferentLevels<TSource, TTarget, TSourceValue, TConvertIntention>
        : IOperationConvertFromTargetOnDifferentLevels<TSource, TTarget, TSourceValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTarget, TConvertIntention> convert;

        /// <summary>
        /// Declares the source value to convert from.
        /// </summary>
        private Func<TTarget, TSourceValue> sourceFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConvertFromTargetOnDifferentLevels{TSource,TTarget,TSourceValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationConvertFromTargetOnDifferentLevels(
            IConvert<TSourceValue, TTarget, TConvertIntention> convert)
        {
            StructureToolsArgumentChecks.NotNull(convert, nameof(convert));
            this.convert = convert;
        }

        /// <inheritdoc/>
        public void Initialize(Func<TTarget, TSourceValue> sourceFunc)
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            this.sourceFunc = sourceFunc;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            StructureToolsArgumentChecks.NotNull(additionalProcessings, nameof(additionalProcessings));

            var actualSource = this.sourceFunc(target);
            this.convert.Convert(actualSource, target, additionalProcessings);
        }
    }
}
