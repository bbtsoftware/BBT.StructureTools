namespace BBT.StructureTools.Convert
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides methods to create <see cref="ICreateConvertHelper{TSource,TTarget,TConvertIntention}"/>
    /// and <see cref="ICreateConvertHelper{TSource,TTarget,TReverseRelation,TConvertIntention}"/>
    /// for convert scenarios where target classes are created with or without reveres relations.
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface IConvertHelperFactory<in TSource, TTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Creates a <see cref="ICreateConvertHelper{TSource,TTarget,TReverseRelation,TConvertIntention}"/>.
        /// </summary>
        /// <typeparam name="TReverseRelation">The <typeparamref name="TTarget"/>'s reverse relation.</typeparam>
        ICreateConvertHelper<TSource, TTarget, TReverseRelation, TConvertIntention> GetConvertHelper<TReverseRelation>(
            Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr)
            where TReverseRelation : class;

        /// <summary>
        /// Creates a <see cref="ICreateConvertHelper{TSource,TTarget,TConvertIntention}"/>.
        /// </summary>
        ICreateConvertHelper<TSource, TTarget, TConvertIntention> GetConvertHelper();
    }
}
