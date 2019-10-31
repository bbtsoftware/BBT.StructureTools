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
        private readonly IConvert<TSourceValue, TTarget, TConvertIntention> convert;

        private readonly IConvertHelper convertHelper;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, IEnumerable<TSourceValue>> sourceFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyFromMany{TSource,TTarget,TSourceValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationCopyFromMany(
            IConvert<TSourceValue, TTarget, TConvertIntention> convert,
            IConvertHelper convertHelper)
        {
            convert.Should().NotBeNull();
            convertHelper.Should().NotBeNull();

            this.convert = convert;
            this.convertHelper = convertHelper;
        }

        /// <summary>
        /// See <see cref="IOperationCopyFromMany{TSource, TTarget, TSourceValue, TConvertIntention}.Initialize"/>.
        /// </summary>
        public void Initialize(Func<TSource, IEnumerable<TSourceValue>> sourceFunc)
        {
            sourceFunc.Should().NotBeNull();

            this.sourceFunc = sourceFunc;
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
