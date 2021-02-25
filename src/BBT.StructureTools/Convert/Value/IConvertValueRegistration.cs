namespace BBT.StructureTools.Convert.Value
{
    /// <summary>
    /// Fluent interface for value conversion registrations, e.g. enum value conversions.
    /// </summary>
    /// <typeparam name="TSource">Type of the source.</typeparam>
    /// <typeparam name="TTarget">Type of the target.</typeparam>
    public interface IConvertValueRegistration<TSource, TTarget>
    {
        /// <summary>
        /// Register the <paramref name="targetValue"/> for a <paramref name="sourceValue"/>.
        /// </summary>
        IConvertValueRegistration<TSource, TTarget> Register(TSource sourceValue, TTarget targetValue);

        /// <summary>
        /// Register an exception for a <paramref name="sourceValue"/>, i.e. there is no corresponding value in the target type.
        /// </summary>
        IConvertValueRegistration<TSource, TTarget> RegisterException(TSource sourceValue);

        /// <summary>
        /// Complete the registrations.
        /// </summary>
        IValueConvertMapping<TSource, TTarget> EndRegistrations();
    }
}
