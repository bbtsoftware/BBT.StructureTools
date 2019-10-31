// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Strategy to copy a value of type <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to copy from.</typeparam>
    /// <typeparam name="TTarget">The target to copy to.</typeparam>
    /// <typeparam name="TValue">The type of the value to copy.</typeparam>
    public class OperationCopyValue<TSource, TTarget, TValue>
        : IOperationCopyValue<TSource, TTarget, TValue>
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
        /// See <see cref="IOperationCopyValue{TSource,TTarget,TValue}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            this.sourceFunc = sourceFunc;
            this.targetexpression = targetExpression;
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

            var sourceValue = this.sourceFunc.Invoke(source);
            target.SetPropertyValue(this.targetexpression, sourceValue);
        }
    }
}
