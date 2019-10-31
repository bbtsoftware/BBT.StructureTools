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
        private readonly IConvert<TSourceValue, TTargetValue, TConvertIntention> convert;

        private Func<TSource, TSourceValue> sourceFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConvertFromSourceOnDifferentLevels{TSource,TTarget,TSourceValue, TTargetValue, TConvertIntention}" /> class.
        /// </summary>
        public OperationConvertFromSourceOnDifferentLevels(
            IConvert<TSourceValue, TTargetValue, TConvertIntention> convert)
        {
            convert.Should().NotBeNull();

            this.convert = convert;
        }

        /// <summary>
        /// See <see cref="IOperationConvertFromSourceOnDifferentLevels{TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, TSourceValue> sourceFunc)
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
            target.Should().BeAssignableTo<TTargetValue>();

            // Need to use another collection, since the given collection my change and the enumeration fails.
            var newAdditionalProcessings = new List<IBaseAdditionalProcessing>(additionalProcessings);

            if (this.sourceFunc(source) == null)
            {
                return;
            }

            additionalProcessings.Add(
                new GenericConvertPostProcessing<TSource, TTarget>(
                    (x, y) => this.convert.Convert(this.sourceFunc(x), y as TTargetValue, newAdditionalProcessings)));
        }
    }
}
