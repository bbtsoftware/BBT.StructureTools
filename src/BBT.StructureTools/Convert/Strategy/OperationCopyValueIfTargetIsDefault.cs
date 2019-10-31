// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IOperationCopyValueIfTargetIsDefault{TSource, TTarget, TValue}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TValue">See link above.</typeparam>
    public class OperationCopyValueIfTargetIsDefault<TSource, TTarget, TValue>
        : IOperationCopyValueIfTargetIsDefault<TSource, TTarget, TValue>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, TValue> sourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, TValue>> targetexpression;

        /// <summary>
        /// See <see cref="IOperationCopyValue{TSource,TTarget,TValue}"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, TValue> aSourceFunc,
            Expression<Func<TTarget, TValue>> aTargetExpression)
        {
            aSourceFunc.Should().NotBeNull();
            aTargetExpression.Should().NotBeNull();

            this.sourceFunc = aSourceFunc;
            this.targetexpression = aTargetExpression;
        }

        /// <summary>
        /// See <see cref="IConvertOperation{TSource,TTarget}.Execute"/>.
        /// </summary>
        public void Execute(
            TSource aSource,
            TTarget aTarget,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            aSource.Should().NotBeNull();
            aTarget.Should().NotBeNull();

            var sourceValue = this.sourceFunc.Invoke(aSource);
            var targetValue = aTarget.GetPropertyValue(this.targetexpression);
            if (!LookupUtils.IsDefaultValue(targetValue))
            {
                return;
            }

            aTarget.SetPropertyValue(
                this.targetexpression,
                sourceValue);
        }
    }
}
