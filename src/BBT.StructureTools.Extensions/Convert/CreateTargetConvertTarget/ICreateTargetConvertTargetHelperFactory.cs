namespace BBT.StructureTools.Extensions.Convert
{
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Provides methods to create <see cref="ICreateTargetConvertTargetHelper{TSource,TTarget,TConvertIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateTargetConvertTargetHelperFactory<in TSource, TTarget, TConvertIntention>
        : IConvertHelperFactory<TSource, TTarget, TConvertIntention>
        where TSource : class
        where TTarget : class, new()
        where TConvertIntention : IBaseConvertIntention
    {
    }
}
