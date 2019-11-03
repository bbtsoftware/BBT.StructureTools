namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;

    /// <inheritdoc/>
    internal class OperationCopyFromTemporalData<TSource, TTarget, TSourceValue, TConvertIntention>
        : IOperationCopyFromTemporalData<TSource, TTarget, TSourceValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTarget, TConvertIntention> convert;
        private readonly IConvertHelper convertHelper;
        private readonly ITemporalDataDescriptor<TSourceValue> sourceTemporalCollectionDataDescriptor;
        private Func<TSource, IEnumerable<TSourceValue>> sourceFunc;
        private Func<TSource, TTarget, DateTime> referenceDateFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyFromTemporalData{TSource,TTarget,TSourceValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationCopyFromTemporalData(
            IConvert<TSourceValue, TTarget, TConvertIntention> convert,
            ITemporalDataDescriptor<TSourceValue> sourceTemporalCollectionDataDescriptor,
            IConvertHelper convertHelper)
        {
            convert.Should().NotBeNull();
            convertHelper.Should().NotBeNull();

            this.convert = convert;
            this.sourceTemporalCollectionDataDescriptor = sourceTemporalCollectionDataDescriptor;
            this.convertHelper = convertHelper;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, IEnumerable<TSourceValue>> aSourceFunc,
            Func<TSource, TTarget, DateTime> referenceDateFunc)
        {
            aSourceFunc.Should().NotBeNull();
            referenceDateFunc.Should().NotBeNull();

            this.sourceFunc = aSourceFunc;
            this.referenceDateFunc = referenceDateFunc;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource aSource,
            TTarget aTarget,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            aSource.Should().NotBeNull();
            aTarget.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            var sourceValues = this.sourceFunc.Invoke(aSource);
            var referenceDate = this.referenceDateFunc.Invoke(aSource, aTarget);

            var referenceDateFilterProcessing = new GenericFilterByReferenceDateProcessing<TSourceValue, TTarget>(referenceDate, this.sourceTemporalCollectionDataDescriptor);
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
                        var message = FormattableString.Invariant($"Attempted to convert from '{typeof(TSourceValue)}' to '{typeof(TTarget)}' multiple times. Make sure the temporal data filter is unique.");
                        throw new InvalidOperationException(message);
                    }

                    this.convert.Convert(sourceValue, aTarget, additionalProcessings);
                    isAlreadyProcessed = true;
                }
            }
        }
    }
}
