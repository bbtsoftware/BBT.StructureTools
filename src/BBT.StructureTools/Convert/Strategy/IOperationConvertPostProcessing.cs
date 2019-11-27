namespace BBT.StructureTools.Convert.Strategy
{
    /// <summary>
    /// Converter operation post processing interface.
    /// </summary>
    /// <typeparam name="TSource">Source object type.</typeparam>
    /// <typeparam name="TTarget">Target object name.</typeparam>
    internal interface IOperationConvertPostProcessing<TSource, TTarget>
        : IConvertOperation<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
    }
}
