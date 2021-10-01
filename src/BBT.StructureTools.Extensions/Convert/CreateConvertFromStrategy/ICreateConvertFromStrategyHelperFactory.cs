namespace BBT.StructureTools.Extensions.Convert
{
    using BBT.StructureTools.Convert;

    /// <summary>
    /// This derivation of <see cref="IConvertHelperFactory{TSource, TTarget, TConvertIntention}"/>
    /// provides an <see cref="ICreateConvertHelper{TSource, TTarget, TConcreteTarget, TConvertIntention}"/>
    /// implementation where the instance creator and converter are determined by
    /// <see cref="ICreateConvertStrategy{TSource, TTarget, TIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateConvertFromStrategyHelperFactory<in TSource, TTarget, TConvertIntention>
        : IConvertHelperFactory<TSource, TTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConvertIntention : IBaseConvertIntention
    {
    }
}