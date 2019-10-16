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
        private readonly IConvertHelper mConvertHelper;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, IEnumerable<TMergeValue>> mMergeFunc;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TMergeValue, IEnumerable<TSourceValue>> mSourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, ICollection<TTargetValue>>> mTargetExpression;

        private ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> mCreateConvertHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationMergeLevel{TSource,TTarget,TSourceValue,TTargetValue,TConcreteTargetValue,TMergeValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationMergeLevel(
            IConvertHelper aConvertHelper)
        {
            aConvertHelper.Should().NotBeNull();

            this.mConvertHelper = aConvertHelper;
        }

        /// <summary>
        /// See <see cref="IOperationMergeLevel{TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TMergeValue, TConvertIntention}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, IEnumerable<TMergeValue>> aMergeFunc,
            Func<TMergeValue, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> aCreateConvertHelper)
        {
            aMergeFunc.Should().NotBeNull();
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();
            aCreateConvertHelper.Should().NotBeNull();

            this.mMergeFunc = aMergeFunc;
            this.mSourceFunc = sourceFunc;
            this.mTargetExpression = targetExpression;
            this.mCreateConvertHelper = aCreateConvertHelper;
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

            var lMergeValues = this.mMergeFunc.Invoke(source);

            var lCopies = new List<TTargetValue>();

            foreach (var lMergeValue in lMergeValues)
            {
                if (!this.mConvertHelper.ContinueConvertProcess<TMergeValue, TTargetValue>(
                    lMergeValue, additionalProcessings))
                {
                    continue;
                }

                var lSourceValues = this.mSourceFunc.Invoke(lMergeValue);

                foreach (var lSourceValue in lSourceValues)
                {
                    if (!this.mConvertHelper.ContinueConvertProcess<TSourceValue, TTargetValue>(
                    lSourceValue, additionalProcessings))
                    {
                        continue;
                    }

                    var lCopy = this.mCreateConvertHelper.CreateTarget(
                        lSourceValue,
                        target,
                        additionalProcessings);
                    lCopies.Add(lCopy);
                }
            }

            target.AddRangeToCollectionFilterNullValues(this.mTargetExpression, lCopies);
        }
    }
}
