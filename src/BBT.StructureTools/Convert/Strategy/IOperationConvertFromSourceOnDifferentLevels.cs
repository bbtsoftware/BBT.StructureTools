﻿// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">Initial source class to convert from.</typeparam>
    /// <typeparam name="TTarget">Initial target class to convert from.</typeparam>
    /// <typeparam name="TSourceValue">The type of the new source type to convert.</typeparam>
    /// <typeparam name="TTargetValue">The type of the concret target (derived from <typeparamref name="TTarget"/>).</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public interface IOperationConvertFromSourceOnDifferentLevels<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
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
        /// <param name="sourceFunc">Declares the source value.</param>
        void Initialize(Func<TSource, TSourceValue> sourceFunc);
    }
}
