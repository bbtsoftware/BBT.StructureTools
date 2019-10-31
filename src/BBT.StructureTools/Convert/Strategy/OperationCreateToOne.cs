﻿// Copyright © BBT Software AG. All rights reserved.

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
        private Func<TSource, TSourceValue> sourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, TTargetValue>> targetexpression;

        private ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention> createConvertHelper;

        /// <summary>
        /// See <see cref="IOperationCreateToOne{TSource,TTarget,TSourceValue,TTargetValue,TConcreteTargetValue,TConvertIntention}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention> createConvertHelper)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();
            createConvertHelper.Should().NotBeNull();

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

            var sourceValue = this.sourceFunc.Invoke(source);

            if (sourceValue == null)
            {
                return;
            }

            var targetValue = this.createConvertHelper.CreateTarget(
                    sourceValue,
                    additionalProcessings);

            target.SetPropertyValue(this.targetexpression, targetValue);
        }
    }
}
