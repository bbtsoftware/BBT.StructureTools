namespace BBT.StructureTools.Convert.Strategy
{
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Strategy to convert the derived type <typeparamref name="TSourceValue"/> of
    /// <typeparamref name="TSource"/>.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TSourceValue">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    internal interface IOperationSourceSubConvert<TSource, TTarget, TSourceValue, TConvertIntention>
        : IConvertOperation<TSource, TTarget>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TConvertIntention : IBaseConvertIntention
    {
    }
}
