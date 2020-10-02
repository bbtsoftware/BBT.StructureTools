namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Provider;

    /// <inheritdoc/>
    internal class OperationCopyFromHist<TSource, TTarget, TSourceValue, TTemporalDataType, TConvertIntention>
        : IOperationCopyFromHist<TSource, TTarget, TSourceValue, TTemporalDataType, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class, TTemporalDataType
        where TConvertIntention : IBaseConvertIntention
        where TTemporalDataType : class
    {
        private readonly IConvert<TSourceValue, TTarget, TConvertIntention> convert;
        private readonly IConvertHelper convertHelper;
        private readonly ITemporalDataHandler<TTemporalDataType> sourceValueTemporalDataHandler;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, IEnumerable<TSourceValue>> sourceFunc;

        /// <summary>
        /// Function to get the reference date vale.
        /// </summary>
        private Func<TSource, TTarget, DateTime> referenceDateFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyFromHist{TSource, TTarget, TSourceValue, TTemporalDataType, TConvertIntention}"/> class.
        /// </summary>
        public OperationCopyFromHist(
            IConvert<TSourceValue, TTarget, TConvertIntention> convert,
            IConvertHelper convertHelper,
            ITemporalDataHandler<TTemporalDataType> sourceValueTemporalDataHandler)
        {
            convert.NotNull(nameof(convert));
            convertHelper.NotNull(nameof(convertHelper));

            this.convert = convert;
            this.convertHelper = convertHelper;
            this.sourceValueTemporalDataHandler = sourceValueTemporalDataHandler;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Func<TSource, TTarget, DateTime> referenceDateFunc)
        {
            sourceFunc.NotNull(nameof(sourceFunc));
            referenceDateFunc.NotNull(nameof(referenceDateFunc));

            this.sourceFunc = sourceFunc;
            this.referenceDateFunc = referenceDateFunc;
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
            var referenceDate = this.referenceDateFunc.Invoke(source, target);

            var referenceDateFilterProcessing = new GenericFilterByReferenceDateProcessing<TSourceValue, TTarget, TTemporalDataType>(this.sourceValueTemporalDataHandler, referenceDate);
            additionalProcessings.Add(referenceDateFilterProcessing);

            // Ensure that only one of the source values is converted into target. This is achieved
            // by declaring additional processings of type IConvertInterception.
            var isAlreadyProcessed = false;
            foreach (var sourceValue in sourceValues)
            {
                if (this.convertHelper.ContinueConvertProcess<TSourceValue, TTarget>(
                    sourceValue, additionalProcessings))
                {
                    if (isAlreadyProcessed)
                    {
                        var msg = FormattableString.Invariant($"Conversion from '{typeof(TSourceValue)}' to '{typeof(TTarget)}' is called multiple times. Make sure it is called once at maximum.");
                        throw new StructureToolsException(msg);
                    }

                    this.convert.Convert(sourceValue, target, additionalProcessings);
                    isAlreadyProcessed = true;
                }
            }
        }
    }
}
