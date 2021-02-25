namespace BBT.StructureTools.Convert.Strategy
{
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Strategy to convert entities of a different source and target type than the calling converter.
    /// </summary>
    /// <typeparam name="TSource">The source class.</typeparam>
    /// <typeparam name="TTarget">The target class.</typeparam>
    /// <typeparam name="TSourceValue">The specific source class. Must derive from the source.</typeparam>
    /// <typeparam name="TTargetValue">The specific target class. Must derive from the target.</typeparam>
    /// <typeparam name="TConvertIntention">The intention used for the conversion.</typeparam>
    public interface IOperationSubConvert<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        : IConvertOperation<TSource, TTarget>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConvertIntention : IBaseConvertIntention
    {
    }
}
