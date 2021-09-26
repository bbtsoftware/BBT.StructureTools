namespace BBT.StructureTools.Extensions.Convert
{
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Provides methods to support conversion.
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TReverseRelation">The reverse relation property of <typeparamref name="TTarget"/>.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateTargetConvertTargetHelper<in TSource, TTarget, TReverseRelation, TConvertIntention>
        : ICreateConvertHelper<TSource, TTarget, TReverseRelation, TConvertIntention>
        where TSource : class
        where TTarget : class, new()
        where TReverseRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
    }

    /// <summary>
    /// Provides methods to support conversion.
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateTargetConvertTargetHelper<in TSource, TTarget, TConvertIntention>
        : ICreateConvertHelper<TSource, TTarget, TConvertIntention>
        where TSource : class
        where TTarget : class, new()
        where TConvertIntention : IBaseConvertIntention
    {
    }
}
