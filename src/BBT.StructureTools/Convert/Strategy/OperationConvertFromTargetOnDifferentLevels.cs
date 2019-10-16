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
    public class OperationConvertFromTargetOnDifferentLevels<TSource, TTarget, TSourceValue, TConvertIntention>
        : IOperationConvertFromTargetOnDifferentLevels<TSource, TTarget, TSourceValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTarget, TConvertIntention> mConvert;

        /// <summary>
        /// Declares the source value to convert from.
        /// </summary>
        private Func<TTarget, TSourceValue> mSourceFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConvertFromTargetOnDifferentLevels{TSource,TTarget,TSourceValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationConvertFromTargetOnDifferentLevels(
            IConvert<TSourceValue, TTarget, TConvertIntention> aConvert)
        {
            aConvert.Should().NotBeNull();

            this.mConvert = aConvert;
        }

        /// <summary>
        /// See <see cref="IOperationConvertFromTargetOnDifferentLevels{TSource, TTarget, TSourceValue, TConvertIntention}"/>.
        /// </summary>
        public void Initialize(Func<TTarget, TSourceValue> sourceFunc)
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
            additionalProcessings.Should().NotBeNull();

            additionalProcessings.Add(
                new GenericConvertPostProcessing<TSource, TTarget>(
                    (aX, aY) => this.mConvert.Convert(this.mSourceFunc(aY), aY, additionalProcessings)));
        }
    }
}
