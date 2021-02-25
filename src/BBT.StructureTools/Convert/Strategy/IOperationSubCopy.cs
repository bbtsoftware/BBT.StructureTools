namespace BBT.StructureTools.Convert.Strategy
{
    /// <summary>
    /// The single unit of a convert helper operation
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">Type of source class.</typeparam>
    /// <typeparam name="TTarget">Type of target class.</typeparam>
    /// <typeparam name="TValue">Type of the value (e.g. EA interface or base class) to copy.</typeparam>
    public interface IOperationSubCopy<TSource, TTarget, TValue> : IConvertOperation<TSource, TTarget>
        where TSource : class, TValue
        where TTarget : class, TValue
        where TValue : class
    {
    }
}
