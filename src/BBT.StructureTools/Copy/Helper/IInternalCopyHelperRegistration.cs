namespace BBT.StructureTools.Copy.Helper
{
    using BBT.StructureTools.Copy.Operation;

    /// <summary>
    /// Internal part of <see cref="ICopyHelperRegistration{T}"/>.
    /// </summary>
    /// <typeparam name="T">see above.</typeparam>
    internal interface IInternalCopyHelperRegistration<T> : ICopyHelperRegistration<T>
        where T : class
    {
        /// <summary>
        /// Ends the registrations and start the operation phase.
        /// </summary>
        ICopyOperation<T> EndRegistrations();
    }
}
