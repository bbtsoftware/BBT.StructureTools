// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;

    /// <summary>
    /// <see cref="IOperationCopyFromTemporalData{TSource, TTarget, TSourceValue, TConvertIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">see above.</typeparam>
    /// <typeparam name="TTarget">see above.</typeparam>
    /// <typeparam name="TSourceValue">see above.</typeparam>
    /// <typeparam name="TConvertIntention">see above.</typeparam>
    public class OperationCopyFromTemporalData<TSource, TTarget, TSourceValue, TConvertIntention>
        : IOperationCopyFromTemporalData<TSource, TTarget, TSourceValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTarget, TConvertIntention> mConvert;
        private readonly IConvertHelper mConvertHelper;
        private readonly ITemporalDataDescriptor<TSourceValue> mSourceTemporalCollectionDataDescriptor;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, IEnumerable<TSourceValue>> mSourceFunc;

        /// <summary>
        /// Function to get the reference date vale.
        /// </summary>
        private Func<TSource, TTarget, DateTime> mReferenceDateFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyFromTemporalData{TSource,TTarget,TSourceValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationCopyFromTemporalData(
            IConvert<TSourceValue, TTarget, TConvertIntention> aConvert,
            ITemporalDataDescriptor<TSourceValue> aSourceTemporalCollectionDataDescriptor,
            IConvertHelper aConvertHelper)
        {
            aConvert.Should().NotBeNull();
            aConvertHelper.Should().NotBeNull();

            this.mConvert = aConvert;
            this.mSourceTemporalCollectionDataDescriptor = aSourceTemporalCollectionDataDescriptor;
            this.mConvertHelper = aConvertHelper;
        }

        /// <summary>
        /// See <see cref="IOperationCopyFromTemporalData{TSource, TTarget, TSourceValue, TConvertIntention}"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, IEnumerable<TSourceValue>> aSourceFunc,
            Func<TSource, TTarget, DateTime> aReferenceDateFunc)
        {
            aSourceFunc.Should().NotBeNull();
            aReferenceDateFunc.Should().NotBeNull();

            this.mSourceFunc = aSourceFunc;
            this.mReferenceDateFunc = aReferenceDateFunc;
        }

        /// <summary>
        /// See <see cref="IConvertOperation{TSource,TTarget}.Execute"/>.
        /// </summary>
        public void Execute(
            TSource aSource,
            TTarget aTarget,
            ICollection<IBaseAdditionalProcessing> aAdditionalProcessings)
        {
            aSource.Should().NotBeNull();
            aTarget.Should().NotBeNull();
            aAdditionalProcessings.Should().NotBeNull();

            var lSourceValues = this.mSourceFunc.Invoke(aSource);
            var lReferenceDate = this.mReferenceDateFunc.Invoke(aSource, aTarget);

            var lReferenceDateFilterProcessing = new GenericFilterByReferenceDateProcessing<TSourceValue, TTarget>(lReferenceDate, this.mSourceTemporalCollectionDataDescriptor);
            aAdditionalProcessings.Add(lReferenceDateFilterProcessing);

            // Ensure that only one of the source values is converted into target. This is achieved
            // by declaring additional processings of type IConvertInterception.
            var lIsAlreadyProcessed = false;
            foreach (var lSourceValue in lSourceValues)
            {
                if (this.mConvertHelper.ContinueConvertProcess<TSourceValue, TTarget>(
                    lSourceValue, aAdditionalProcessings))
                {
                    if (lIsAlreadyProcessed)
                    {
                        var lMsg = FormattableString.Invariant($"Attempted to convert from '{typeof(TSourceValue)}' to '{typeof(TTarget)}' multiple times. Make sure the temporal data filter is unique.");
                        throw new InvalidOperationException(lMsg);
                    }

                    this.mConvert.Convert(lSourceValue, aTarget, aAdditionalProcessings);
                    lIsAlreadyProcessed = true;
                }
            }
        }
    }
}
