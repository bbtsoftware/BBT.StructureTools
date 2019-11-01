namespace BBT.StructureTools.Convert.Value
{
    /// <summary>
    /// Interface for conversion of a value of the source type to a value of the target type.
    /// </summary>
    /// <typeparam name="TSource">Source type.</typeparam>
    /// <typeparam name="TTarget">Target type.</typeparam>
    public interface IConvertValue<TSource, TTarget>
    {
        /// <summary>
        /// Returns the corresponding target value for <paramref name="source"/>
        /// or throws an exception if there is no target value for the source value.
        /// </summary>
        TTarget ConvertValue(TSource source);
    }
}
