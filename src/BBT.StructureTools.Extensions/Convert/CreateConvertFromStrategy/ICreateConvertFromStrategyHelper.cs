namespace BBT.StructureTools.Extensions.Convert
{
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Provides methods to support conversion.
    ///
    /// An <see cref="ICreateConvertHelper{TSource, TTarget, TReverseRelation, TConvertIntention}"/>
    /// implementation where the instance creator and converter are determined by
    /// <see cref="ICreateConvertStrategy{TSource, TTarget, TIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TReverseRelation">The reverse relation property of <typeparamref name="TTarget"/>.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateConvertFromStrategyHelper<in TSource, TTarget, TReverseRelation, TConvertIntention>
        : ICreateConvertHelper<TSource, TTarget, TReverseRelation, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TReverseRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
    }

    /// <summary>
    /// Provides methods to support conversion.
    ///
    /// An <see cref="ICreateConvertHelper{TSource, TTarget, TConcreteTarget, TConvertIntention}"/>
    /// implementation where the instance creator and converter are determined by
    /// <see cref="ICreateConvertStrategy{TSource, TTarget, TIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateConvertFromStrategyHelper<in TSource, TTarget, TConvertIntention>
        : ICreateConvertHelper<TSource, TTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConvertIntention : IBaseConvertIntention
    {
    }
}