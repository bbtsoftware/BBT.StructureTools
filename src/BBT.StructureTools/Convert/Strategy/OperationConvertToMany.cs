// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TSourceValue">
    /// The type of the ist entries which shall be converted into
    /// the <typeparamref name="TTargetValue"/>s.</typeparam>
    /// <typeparam name="TTargetValue">
    /// The ist entries which shall be converted from
    /// the <typeparamref name="TSourceValue"/>s.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public class OperationConvertToMany<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        : IOperationConvertToMany<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTargetValue, TConvertIntention> convert;
        private readonly IConvertHelper convertHelper;

        /// <summary>
        /// The function to obtain the source values.
        /// </summary>
        private Func<TSource, IEnumerable<TSourceValue>> sourceFunc;

        /// <summary>
        /// The function to obtain the target values.
        /// </summary>
        private Func<TTarget, IEnumerable<TTargetValue>> targetFunc;

        /// <summary>
        /// The function to filter the target value corresponding to the source value.
        /// </summary>
        private Func<TSourceValue, TTargetValue, bool> filterFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConvertToMany{TSource,TTarget,TSourceValue,TTargetValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationConvertToMany(
            IConvert<TSourceValue, TTargetValue, TConvertIntention> convert,
            IConvertHelper convertHelper)
        {
            convert.Should().NotBeNull();
            convertHelper.Should().NotBeNull();

            this.convert = convert;
            this.convertHelper = convertHelper;
        }

        /// <summary>
        /// See <see cref="IOperationConvertToMany{TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention}"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Func<TTarget, IEnumerable<TTargetValue>> targetFunc,
            Func<TSourceValue, TTargetValue, bool> aFilterFunc)
        {
            sourceFunc.Should().NotBeNull();
            targetFunc.Should().NotBeNull();
            aFilterFunc.Should().NotBeNull();

            this.sourceFunc = sourceFunc;
            this.targetFunc = targetFunc;
            this.filterFunc = aFilterFunc;
        }

        /// <summary>
        /// See <see cref="IConvertOperation{TSource,TTarget}.Execute"/>.
        /// </summary>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            additionalProcessings.Should().NotBeNull();
            source.Should().NotBeNull();
            target.Should().NotBeNull();

            var sourceValues = this.sourceFunc(source).ToList();
            var targetValues = this.targetFunc(target).Where(x => x != null).ToList();

            foreach (var sourceValue in sourceValues)
            {
                if (!this.convertHelper.ContinueConvertProcess<TSourceValue, TTargetValue>(
                    sourceValue, additionalProcessings))
                {
                    continue;
                }

                var errorMsg = string.Format(CultureInfo.InvariantCulture, $"One result expected to convert from '{typeof(TSourceValue).Name}' to '{typeof(TTargetValue).Name}'");
                var targetValue = targetValues.SingleWithExceptionMessage(x => this.filterFunc(sourceValue, x), errorMsg);
                this.convert.Convert(sourceValue, targetValue, additionalProcessings);
            }
        }
    }
}
