// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TSourceValue">See link above.</typeparam>
    /// <typeparam name="TTargetValue">See link above.</typeparam>
    /// <typeparam name="TConcreteTargetValue">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public interface IOperationCreateToOneWithReverseRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>
        : IConvertOperation<TSource, TTarget>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConcreteTargetValue : TTargetValue, new()
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Initializes the <see cref="IConvertOperation{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="sourceFunc">Declares the source value.</param>
        /// <param name="targetExpression">Declares the target property.</param>
        /// <param name="aCreateConvertHelper">Used to create the target value.</param>
        void Initialize(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> aCreateConvertHelper);
    }
}
