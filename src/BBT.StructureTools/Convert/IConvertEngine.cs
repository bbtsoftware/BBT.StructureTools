namespace BBT.StructureTools.Convert
{
    /// <summary>
    /// Helper for the convert operations.
    /// </summary>
    /// <typeparam name="TSource">The source to copy from.</typeparam>
    /// <typeparam name="TTarget">The target to copy to.</typeparam>
    public interface IConvertEngine<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// Ensure that the equality comparer helper are empty. The helper has an internal state
        /// so it's necessary that every new comparer checks that helper is new instantiated.
        /// </summary>
        IConvertRegistration<TSource, TTarget> StartRegistrations();
    }
}
