// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides methods to support conversion.
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TConcreteTarget">The type of the concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
    /// <typeparam name="TReverseRelation">The reverse relation property of <typeparamref name="TTarget"/>.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateConvertHelper<in TSource, TTarget, TConcreteTarget, TReverseRelation, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConcreteTarget : TTarget, new()
        where TReverseRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// See <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}.SetupReverseRelation"/>.
        /// </summary>
        void SetupReverseRelation(Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr);

        /// <summary>
        /// Creates a new <typeparamref name="TTarget"/> converted from <paramref name="source"/>.
        /// </summary>
        TTarget CreateTarget(
            TSource source,
            TReverseRelation reverseRelation,
            ICollection<IBaseAdditionalProcessing> additionalProcessings);
    }

    /// <summary>
    /// Provides methods to support conversion.
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TConcreteTarget">The type of the concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateConvertHelper<in TSource, TTarget, TConcreteTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConcreteTarget : TTarget, new()
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Creates a new <typeparamref name="TTarget"/> converted from <paramref name="source"/>.
        /// </summary>
        TTarget CreateTarget(
            TSource source,
            ICollection<IBaseAdditionalProcessing> additionalProcessings);
    }
}
