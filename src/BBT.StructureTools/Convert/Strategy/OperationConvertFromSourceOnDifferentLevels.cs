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
    /// <typeparam name="TTargetValue">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public class OperationConvertFromSourceOnDifferentLevels<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        : IOperationConvertFromSourceOnDifferentLevels<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTargetValue, TConvertIntention> mConvert;

        private Func<TSource, TSourceValue> mSourceFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConvertFromSourceOnDifferentLevels{TSource,TTarget,TSourceValue, TTargetValue, TConvertIntention}" /> class.
        /// </summary>
        public OperationConvertFromSourceOnDifferentLevels(
            IConvert<TSourceValue, TTargetValue, TConvertIntention> aConvert)
        {
            aConvert.Should().NotBeNull();

            this.mConvert = aConvert;
        }

        /// <summary>
        /// See <see cref="IOperationConvertFromSourceOnDifferentLevels{TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, TSourceValue> sourceFunc)
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
            target.Should().BeAssignableTo<TTargetValue>();

            // Need to use another collection, since the given collection may change and the enumeration fails.
            var lNewAdditionalProcessings = new List<IBaseAdditionalProcessing>(additionalProcessings);

            if (this.mSourceFunc(source) == null)
            {
                return;
            }

            additionalProcessings.Add(
                new GenericConvertPostProcessing<TSource, TTarget>(
                    (aX, aY) => this.mConvert.Convert(this.mSourceFunc(aX), aY as TTargetValue, lNewAdditionalProcessings)));
        }
    }
}
