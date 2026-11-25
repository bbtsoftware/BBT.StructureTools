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
    /// <typeparam name="TReverseRelation">The reverse relation property of <typeparamref name="TTarget"/>.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateConvertHelper<in TSource, TTarget, TReverseRelation, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TReverseRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// See <see cref="ICreateConvertHelper{TSource,TTarget,TReverseRelation,TConvertIntention}.SetupReverseRelation"/>.
        /// </summary>
        void SetupReverseRelation(Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr);

        /// <summary>
        /// Creates <typeparamref name="TTarget"/> with <paramref name="reverseRelation"/>.
        /// </summary>
        TTarget Create(
            TSource source,
            TReverseRelation reverseRelation);

        /// <summary>
        /// Converts <paramref name="source"/> into <paramref name="target"/>.
        /// </summary>
        void Convert(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings);
    }

    /// <summary>
    /// Provides methods to support conversion.
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateConvertHelper<in TSource, TTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Creates <typeparamref name="TTarget"/>.
        /// </summary>
        TTarget Create(TSource source);

        /// <summary>
        /// Converts <paramref name="source"/> into <paramref name="target"/>.
        /// </summary>
        void Convert(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings);
    }
}
