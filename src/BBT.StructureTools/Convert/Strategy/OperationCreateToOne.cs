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
    /// Strategy to convert entities with a <c>ToOne</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TSourceValue">See link above.</typeparam>
    /// <typeparam name="TTargetValue">See link above.</typeparam>
    /// <typeparam name="TConcreteTargetValue">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public class OperationCreateToOne<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>
        : IOperationCreateToOne<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConcreteTargetValue : TTargetValue, new()
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, TSourceValue> mSourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, TTargetValue>> mTargetExpression;

        private ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention> mCreateConvertHelper;

        /// <summary>
        /// See <see cref="IOperationCreateToOne{TSource,TTarget,TSourceValue,TTargetValue,TConcreteTargetValue,TConvertIntention}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention> aCreateConvertHelper)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();
            aCreateConvertHelper.Should().NotBeNull();

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

            var lSourceValue = this.mSourceFunc.Invoke(source);

            if (lSourceValue == null)
            {
                return;
            }

            var lTargetValue = this.mCreateConvertHelper.CreateTarget(
                    lSourceValue,
                    additionalProcessings);

            target.SetPropertyValue(this.mTargetExpression, lTargetValue);
        }
    }
}
