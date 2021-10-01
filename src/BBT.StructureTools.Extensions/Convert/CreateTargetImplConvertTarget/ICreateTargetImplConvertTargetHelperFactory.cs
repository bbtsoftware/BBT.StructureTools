namespace BBT.StructureTools.Extensions.Convert
{
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Provides methods to create <see cref="ICreateTargetImplConvertTargetHelper{TSource,TTarget,TTargetImpl,TConvertIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TTargetImpl">The concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateTargetImplConvertTargetHelperFactory<in TSource, TTarget, TTargetImpl, TConvertIntention>
        : IConvertHelperFactory<TSource, TTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TTargetImpl : class, TTarget, new()
        where TConvertIntention : IBaseConvertIntention
    {
    }
}
