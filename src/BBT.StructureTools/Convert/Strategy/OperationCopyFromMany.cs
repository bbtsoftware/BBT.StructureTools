namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationCopyFromMany<TSource, TTarget, TSourceValue, TConvertIntention>
        : IOperationCopyFromMany<TSource, TTarget, TSourceValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConverter<TSourceValue, TTarget, TConvertIntention> convert;
        private readonly IConvertHelper convertHelper;
        private Func<TSource, IEnumerable<TSourceValue>> sourceFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyFromMany{TSource,TTarget,TSourceValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationCopyFromMany(
            IConverter<TSourceValue, TTarget, TConvertIntention> convert,
            IConvertHelper convertHelper)
        {
            convert.NotNull(nameof(convert));
            convertHelper.NotNull(nameof(convertHelper));

            this.convert = convert;
            this.convertHelper = convertHelper;
        }

        /// <inheritdoc/>
        public void Initialize(Func<TSource, IEnumerable<TSourceValue>> sourceFunc)
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

            var sourceValues = this.sourceFunc.Invoke(source);

            // Ensure that only one of the source values is converted into target. This is achieved
            // by declaring additional processings of type IConvertInterception.
            var isAlreadyProcessed = false;
            foreach (var sourceValue in sourceValues)
            {
                if (this.convertHelper.ContinueConvertProcess<TSourceValue, TTarget>(sourceValue, additionalProcessings))
                {
                    if (isAlreadyProcessed)
                    {
                        var message = FormattableString.Invariant($"Conversion from '{typeof(TSourceValue)}' to '{typeof(TTarget)}' is called multiple times. Make sure it is called once at mximum.");
                        throw new CopyConvertCompareException(message);
                    }

                    this.convert.Convert(sourceValue, target, additionalProcessings);
                    isAlreadyProcessed = true;
                }
            }
        }
    }
}
