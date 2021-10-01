namespace BBT.StructureTools.Extensions.Convert
{
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Provides methods to support conversions where target objects are created.
    /// Use this helper implementation if the target object (i.e. <typeparamref name="TTarget"/>)
    /// is created as well as converted using a <see cref="ICreateConvertStrategy{TSource, TTarget, TIntention}"/>.
    /// A generic strategy implementation is provided,
    /// <see cref="CreateConvertStrategy{TSource, TConcreteSource, TTarget, TConcreteTarget, TConcreteTargetImpl, TIntention}"/>.
    /// Register strategies for all concerned derived types in your IoC container in use.
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
    /// Provides methods to support conversions where target objects are created.
    /// Use this helper implementation if the target object (i.e. <typeparamref name="TTarget"/>)
    /// is created as well as converted using a <see cref="ICreateConvertStrategy{TSource, TTarget, TIntention}"/>.
    /// A generic strategy implementation is provided,
    /// <see cref="CreateConvertStrategy{TSource, TConcreteSource, TTarget, TConcreteTarget, TConcreteTargetImpl, TIntention}"/>.
    /// Register strategies for all concerned derived types in your IoC container in use.
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