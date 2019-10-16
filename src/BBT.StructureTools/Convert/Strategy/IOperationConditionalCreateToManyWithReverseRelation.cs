// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// <c>To Many</c> conversion operation where the child converter is found via strategy.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TBaseSource">Contains the base type of the source which shall be converted (e.g. LiBaseCover).</typeparam>
    /// <typeparam name="TBaseTarget">Contains the base type of the target which shall be converted (e.g. LiClaimCover).</typeparam>
    /// <typeparam name="TIntention">Conversion intention which shall be used within the strategy.</typeparam>
    public interface IOperationConditionalCreateToManyWithReverseRelation<TSource, TTarget, TBaseSource, TBaseTarget, TIntention>
        : IConvertOperation<TSource, TTarget>
            where TSource : class
            where TTarget : class
            where TBaseSource : class
            where TBaseTarget : class
            where TIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Initializes the <see cref="IConvertOperation{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="source">Declares the source to convert from.</param>
        /// <param name="targetParent">Declares the target value.</param>
        /// <param name="aReverseRelationOnTarget">Declares the partent of the target value.</param>
        void Initialize(
            Func<TSource, IEnumerable<TBaseSource>> source,
            Expression<Func<TTarget, ICollection<TBaseTarget>>> targetParent,
            Expression<Func<TBaseTarget, TTarget>> aReverseRelationOnTarget);
    }
}
