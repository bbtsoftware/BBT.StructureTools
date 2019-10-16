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
    /// The type of the list entries which shall be converted into
    /// the <typeparamref name="TTargetValue"/>s.</typeparam>
    /// <typeparam name="TTargetValue">
    /// The list entries which shall be converted from
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
        private readonly IConvert<TSourceValue, TTargetValue, TConvertIntention> mConvert;
        private readonly IConvertHelper mConvertHelper;

        /// <summary>
        /// The function to obtain the source values.
        /// </summary>
        private Func<TSource, IEnumerable<TSourceValue>> mSourceFunc;

        /// <summary>
        /// The function to obtain the target values.
        /// </summary>
        private Func<TTarget, IEnumerable<TTargetValue>> mTargetFunc;

        /// <summary>
        /// The function to filter the target value corresponding to the source value.
        /// </summary>
        private Func<TSourceValue, TTargetValue, bool> mFilterFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConvertToMany{TSource,TTarget,TSourceValue,TTargetValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationConvertToMany(
            IConvert<TSourceValue, TTargetValue, TConvertIntention> aConvert,
            IConvertHelper aConvertHelper)
        {
            aConvert.Should().NotBeNull();
            aConvertHelper.Should().NotBeNull();

            this.mConvert = aConvert;
            this.mConvertHelper = aConvertHelper;
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

            this.mSourceFunc = sourceFunc;
            this.mTargetFunc = targetFunc;
            this.mFilterFunc = aFilterFunc;
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

            var lSourceValues = this.mSourceFunc(source).ToList();
            var lTargetValues = this.mTargetFunc(target).Where(aX => aX != null).ToList();

            foreach (var lSourceValue in lSourceValues)
            {
                if (!this.mConvertHelper.ContinueConvertProcess<TSourceValue, TTargetValue>(
                    lSourceValue, additionalProcessings))
                {
                    continue;
                }

                var lErrorMsg = string.Format(CultureInfo.InvariantCulture, $"One result expected to convert from '{typeof(TSourceValue).Name}' to '{typeof(TTargetValue).Name}'");
                var lTargetValue = lTargetValues.SingleWithExceptionMessage(aX => this.mFilterFunc(lSourceValue, aX), lErrorMsg);
                this.mConvert.Convert(lSourceValue, lTargetValue, additionalProcessings);
            }
        }
    }
}
