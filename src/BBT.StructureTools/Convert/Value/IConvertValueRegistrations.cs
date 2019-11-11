namespace BBT.StructureTools.Convert.Value
{
    /// <summary>
    /// Contains all registrations to convert a value of <typeparamref name="TSource"/>
    /// into a value of <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">The source type to convert from.</typeparam>
    /// <typeparam name="TTarget">The target type to convert to.</typeparam>
    public interface IConvertValueRegistrations<TSource, TTarget>
    {
        /// <summary>
        /// Does the registrations.
        /// </summary>
        void DoRegistrations(IConvertValueRegistration<TSource, TTarget> registration);
    }
}
