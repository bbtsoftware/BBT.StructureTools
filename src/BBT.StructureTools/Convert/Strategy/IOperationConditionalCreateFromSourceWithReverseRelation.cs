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
    /// <typeparam name="TBaseSource">Contains the base type of the source which shall be converted (e.g. LiBaseCover).</typeparam>
    /// <typeparam name="TBaseTarget">Contains the base type of the target which shall be converted (e.g. LiClaimCover).</typeparam>
    /// <typeparam name="TIntention">Intention defining the conversion use case.</typeparam>
    public interface IOperationConditionalCreateFromSourceWithReverseRelation<TSource, TTarget, TBaseSource, TBaseTarget, TIntention>
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
        /// <param name="baseSourceFunc">Declares the source to convert from.</param>
        /// <param name="targetValueExpression">Declares the target value.</param>
        /// <param name="targetParentExpression">Declares the partent of the target value.</param>
        void Initialize(
            Func<TSource, TBaseSource> baseSourceFunc,
            Expression<Func<TTarget, TBaseTarget>> targetValueExpression,
            Expression<Func<TBaseTarget, TTarget>> targetParentExpression);
    }
}
