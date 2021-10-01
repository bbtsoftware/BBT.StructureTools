namespace BBT.StructureTools.Extensions.Convert
{
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Provides methods to support conversions where target objects are created.
    /// Use this helper implementation if the target object (i.e. <typeparamref name="TTargetImpl"/>)
    /// implements an interface (i.e. <typeparamref name="TTarget"/>).
    /// The creation of <typeparamref name="TTarget"/> as well as the conversion are based on <typeparamref name="TTargetImpl"/>
    /// (i.e. convert is performed consulting a <see cref="IConvertRegistrations{TSource, TTargetImpl, TConvertIntention}"/>).
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TTargetImpl">The type of the concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
    /// <typeparam name="TReverseRelation">The reverse relation property of <typeparamref name="TTarget"/>.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateTargetImplConvertTargetImplHelper<in TSource, TTarget, TTargetImpl, TReverseRelation, TConvertIntention>
        : ICreateConvertHelper<TSource, TTarget, TReverseRelation, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TTargetImpl : class, TTarget, new()
        where TReverseRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
    }

    /// <summary>
    /// Provides methods to support conversions where target objects are created.
    /// Use this helper implementation if the target object (i.e. <typeparamref name="TTargetImpl"/>)
    /// implements an interface (i.e. <typeparamref name="TTarget"/>).
    /// The creation of <typeparamref name="TTarget"/> as well as the conversion are based on <typeparamref name="TTargetImpl"/>
    /// (i.e. convert is performed consulting a <see cref="IConvertRegistrations{TSource, TTargetImpl, TConvertIntention}"/>).
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TTargetImpl">The type of the concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
    /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
    public interface ICreateTargetImplConvertTargetImplHelper<in TSource, TTarget, TTargetImpl, TConvertIntention>
        : ICreateConvertHelper<TSource, TTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConvertIntention : IBaseConvertIntention
    {
    }
}