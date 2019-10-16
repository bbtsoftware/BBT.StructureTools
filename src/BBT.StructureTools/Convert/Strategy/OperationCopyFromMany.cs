// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using FluentAssertions;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TSourceValue">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public class OperationCopyFromMany<TSource, TTarget, TSourceValue, TConvertIntention>
        : IOperationCopyFromMany<TSource, TTarget, TSourceValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTarget, TConvertIntention> mConvert;

        private readonly IConvertHelper mConvertHelper;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, IEnumerable<TSourceValue>> mSourceFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyFromMany{TSource,TTarget,TSourceValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationCopyFromMany(
            IConvert<TSourceValue, TTarget, TConvertIntention> aConvert,
            IConvertHelper aConvertHelper)
        {
            aConvert.Should().NotBeNull();
            aConvertHelper.Should().NotBeNull();

            this.mConvert = aConvert;
            this.mConvertHelper = aConvertHelper;
        }

        /// <summary>
        /// See <see cref="IOperationCopyFromMany{TSource, TTarget, TSourceValue, TConvertIntention}.Initialize"/>.
        /// </summary>
        public void Initialize(Func<TSource, IEnumerable<TSourceValue>> sourceFunc)
        {
            sourceFunc.Should().NotBeNull();

            this.mSourceFunc = sourceFunc;
        }

        /// <summary>
        /// See <see cref="IConvertOperation{TSource,TTarget}.Execute"/>.
        /// </summary>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            var lSourceValues = this.mSourceFunc.Invoke(source);

            // Ensure that only one of the source values is converted into target. This is achieved
            // by declaring additional processings of type IConvertInterception.
            var lIsAlreadyProcessed = false;
            foreach (var lSourceValue in lSourceValues)
            {
                if (this.mConvertHelper.ContinueConvertProcess<TSourceValue, TTarget>(lSourceValue, additionalProcessings))
                {
                    if (lIsAlreadyProcessed)
                    {
                        var lMsg = FormattableString.Invariant($"Conversion from '{typeof(TSourceValue)}' to '{typeof(TTarget)}' is called multiple times. Make sure it is called once at maximum.");
                        throw new CopyConvertCompareException(lMsg);
                    }

                    this.mConvert.Convert(lSourceValue, target, additionalProcessings);
                    lIsAlreadyProcessed = true;
                }
            }
        }
    }
}
