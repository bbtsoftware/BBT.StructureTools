// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Strategy to copy <typeparamref name="TSource"/> into corresponding property of <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to copy.</typeparam>
    /// <typeparam name="TTarget">The target to copy to.</typeparam>
    public class OperationCopySource<TSource, TTarget>
        : IOperationCopySource<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, TSource>> targetexpression;

        /// <summary>
        /// See <see cref="IOperationCopySource{TSource,TTarget}.Initialize"/>.
        /// </summary>
        public void Initialize(Expression<Func<TTarget, TSource>> targetExpression)
        {
            targetExpression.Should().NotBeNull();

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

            target.SetPropertyValue(this.targetexpression, source);
        }
    }
}
