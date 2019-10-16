// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Strategy to copy a value of type <typeparamref name="TValue"/> or
    /// look-up value on <typeparamref name="TSource"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to copy from.</typeparam>
    /// <typeparam name="TTarget">The target to copy to.</typeparam>
    /// <typeparam name="TValue">The type of the value to copy.</typeparam>
    public interface IOperationCopyValueWithLookUp<TSource, TTarget, TValue>
        : IConvertOperation<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// Initializes the <see cref="IConvertOperation{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="sourceFunc">Declares the source attribute to copy from.</param>
        /// <param name="sourceLookUpFunc">Declares the source attribute to look up if <paramref name="sourceFunc"/> is not set.</param>
        /// <param name="targetExpression">Declares the target attribute to copy to.</param>
        void Initialize(
            Func<TSource, TValue> sourceFunc,
            Func<TSource, TValue> sourceLookUpFunc,
            Expression<Func<TTarget, TValue>> targetExpression);
    }
}
