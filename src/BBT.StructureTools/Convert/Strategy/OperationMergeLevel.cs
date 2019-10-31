// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TSourceValue">See link above.</typeparam>
    /// <typeparam name="TTargetValue">See link above.</typeparam>
    /// <typeparam name="TConcreteTargetValue">See link above.</typeparam>
    /// <typeparam name="TMergeValue">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public class OperationMergeLevel<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TMergeValue, TConvertIntention>
        : IOperationMergeLevel<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TMergeValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConcreteTargetValue : TTargetValue, new()
        where TMergeValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvertHelper convertHelper;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, IEnumerable<TMergeValue>> mergeFunc;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TMergeValue, IEnumerable<TSourceValue>> sourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, ICollection<TTargetValue>>> targetexpression;

        private ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationMergeLevel{TSource,TTarget,TSourceValue,TTargetValue,TConcreteTargetValue,TMergeValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationMergeLevel(
            IConvertHelper convertHelper)
        {
            convertHelper.Should().NotBeNull();

            this.convertHelper = convertHelper;
        }

        /// <summary>
        /// See <see cref="IOperationMergeLevel{TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TMergeValue, TConvertIntention}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, IEnumerable<TMergeValue>> aMergeFunc,
            Func<TMergeValue, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper)
        {
            aMergeFunc.Should().NotBeNull();
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();
            createConvertHelper.Should().NotBeNull();

            this.mergeFunc = aMergeFunc;
            this.sourceFunc = sourceFunc;
            this.targetexpression = targetExpression;
            this.createConvertHelper = createConvertHelper;
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

            var mergeValues = this.mergeFunc.Invoke(source);

            var copies = new List<TTargetValue>();

            foreach (var mergeValue in mergeValues)
            {
                if (!this.convertHelper.ContinueConvertProcess<TMergeValue, TTargetValue>(
                    mergeValue, additionalProcessings))
                {
                    continue;
                }

                var sourceValues = this.sourceFunc.Invoke(mergeValue);

                foreach (var sourceValue in sourceValues)
                {
                    if (!this.convertHelper.ContinueConvertProcess<TSourceValue, TTargetValue>(
                    sourceValue, additionalProcessings))
                    {
                        continue;
                    }

                    var copy = this.createConvertHelper.CreateTarget(
                        sourceValue,
                        target,
                        additionalProcessings);
                    copies.Add(copy);
                }
            }

            target.AddRangeToCollectionFilterNulvalues(this.targetexpression, copies);
        }
    }
}
