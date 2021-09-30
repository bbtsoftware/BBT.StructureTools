namespace BBT.StructureTools.Extensions.Convert
{
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Provides methods to support conversion.
    ///
    /// An <see cref="ICreateConvertHelper{TSource, TTarget, TReverseRelation, TConvertIntention}"/>
    /// implementation where the converter's generic type parameter 'TTarget' of
    /// <see cref="IConvert{TSource, TTarget, TConvertIntention}"/>
    /// uses the concrete target type <typeparamref name="TTargetImpl"/> (instead of <typeparamref name="TTarget"/>).
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
    /// Provides methods to support conversion.
    ///
    /// An <see cref="ICreateConvertHelper{TSource, TTarget, TConvertIntention}"/>
    /// implementation where the converter's generic type parameter 'TTarget' of
    /// <see cref="IConvert{TSource, TTarget, TConvertIntention}"/>
    /// uses the concrete target type <typeparamref name="TTargetImpl"/> (instead of <typeparamref name="TTarget"/>).
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