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
    /// <typeparam name="TTargetValue">See link above.</typeparam>
    /// <typeparam name="TConcreteTargetValue">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public interface IOperationCreateFromSourceWithReverseRelation<TSource, TTarget, TTargetValue, TConcreteTargetValue, TConvertIntention>
        : IConvertOperation<TSource, TTarget>
        where TSource : class
        where TTarget : class
        where TTargetValue : class
        where TConcreteTargetValue : TTargetValue, new()
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Initializes the <see cref="IConvertOperation{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="targetExpression">Declares the property on target.</param>
        /// <param name="aCreateConvertHelper">Used to create the value on target.</param>
        void Initialize(
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSource, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> aCreateConvertHelper);
    }
}
