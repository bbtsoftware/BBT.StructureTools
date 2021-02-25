namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationConvertFromSourceOnDifferentLevels<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        : IOperationConvertFromSourceOnDifferentLevels<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTargetValue, TConvertIntention> convert;

        private Func<TSource, TSourceValue> sourceFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConvertFromSourceOnDifferentLevels{TSource,TTarget,TSourceValue, TTargetValue, TConvertIntention}" /> class.
        /// </summary>
        public OperationConvertFromSourceOnDifferentLevels(
            IConvert<TSourceValue, TTargetValue, TConvertIntention> convert)
        {
            convert.NotNull(nameof(convert));
            this.convert = convert;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TSourceValue> sourceFunc)
        {
            sourceFunc.NotNull(nameof(sourceFunc));
            this.sourceFunc = sourceFunc;
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
            target.IsOfType<TTargetValue>(nameof(target));

            var actualSource = this.sourceFunc(source);
            if (actualSource == null)
            {
                return;
            }

            this.convert.Convert(actualSource, target as TTargetValue, additionalProcessings);
        }
    }
}
