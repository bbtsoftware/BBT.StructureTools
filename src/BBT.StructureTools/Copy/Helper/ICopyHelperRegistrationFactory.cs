namespace BBT.StructureTools.Copy.Helper
{
    /// <summary>
    /// Factory for create an instance of CopyHelperRegistration.
    /// </summary>
    internal interface ICopyHelperRegistrationFactory
    {
        /// <summary>
        /// Creates an  an instance of ICopyHelperRegistration.
        /// </summary>
        /// <typeparam name="T">Creation type.</typeparam>
        /// <returns>Created instance of EqualityComparerHelperRegistration.</returns>
        IInternalCopyHelperRegistration<T> Create<T>()
            where T : class;
    }
}