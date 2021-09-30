namespace BBT.StructureTools.Extensions.Convert
{
    using BBT.StructureTools.Convert;

    /// <summary>
    /// This derivation of <see cref="IConvertHelperFactory{TSource, TTarget, TConvertIntention}"/>
    /// provides an <see cref="ICreateConvertHelper{TSource, TTarget, TConcreteTarget, TConvertIntention}"/>
    /// implementation where the converter's generic type paramter 'TTarget' of
    /// <see cref="IConvert{TSource, TTarget, TConvertIntention}"/>
    /// uses the concrete target type <typeparamref name="TTargetImpl"/> (instead of <typeparamref name="TTarget"/>).
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TTargetImpl">The concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateTargetImplConvertTargetImplHelperFactory<in TSource, TTarget, TTargetImpl, TConvertIntention>
        : IConvertHelperFactory<TSource, TTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TTargetImpl : class, TTarget, new()
        where TConvertIntention : IBaseConvertIntention
    {
    }
}