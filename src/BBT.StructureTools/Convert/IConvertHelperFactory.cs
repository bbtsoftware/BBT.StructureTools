// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides methods to create <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TConcreteTarget">The concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface IConvertHelperFactory<in TSource, TTarget, TConcreteTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConcreteTarget : TTarget, new()
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Creates a <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}"/>.
        /// </summary>
        /// <typeparam name="TReverseRelation">The <typeparamref name="TTarget"/>'s reverse relation.</typeparam>
        ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TReverseRelation, TConvertIntention> GetConvertHelper<TReverseRelation>(
            Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr)
            where TReverseRelation : class;

        /// <summary>
        /// Creates a <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}"/>.
        /// </summary>
        ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TConvertIntention> GetConvertHelper();
    }
}
