// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TSourceValue">
    /// The type of the list entries which shall be converted into
    /// the <typeparamref name="TTargetValue"/>s.</typeparam>
    /// <typeparam name="TTargetValue">
    /// The list entries which shall be converted from
    /// the <typeparamref name="TSourceValue"/>s.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface IOperationConvertToMany<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        : IConvertOperation<TSource, TTarget>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Initializes the <see cref="IConvertOperation{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="sourceFunc">Declares the source entries.</param>
        /// <param name="targetFunc">Declares the target entries.</param>
        /// <param name="aFilterFunc">Declares the filter function for target value.</param>
        void Initialize(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Func<TTarget, IEnumerable<TTargetValue>> targetFunc,
            Func<TSourceValue, TTargetValue, bool> aFilterFunc);
    }
}
