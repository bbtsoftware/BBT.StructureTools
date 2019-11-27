namespace BBT.StructureTools.Convert
{
    /// <summary>
    /// Contains all registrations to covert of <typeparamref name="TSource"/>
    /// into <typeparamref name="TTarget"/> in context of <typeparamref name="TConvertIntention"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to convert from.</typeparam>
    /// <typeparam name="TTarget">The target to convert to.</typeparam>
    /// <typeparam name="TConvertIntention">The context of the conversion.</typeparam>
    public interface IConverterRegistrations<TSource, TTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Does the registrations.
        /// </summary>
        void DoRegistrations(IConvertRegistration<TSource, TTarget> registrations);
    }
}
