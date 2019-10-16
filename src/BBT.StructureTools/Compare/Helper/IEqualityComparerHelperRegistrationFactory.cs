// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare.Helper
{
    /// <summary>
    /// Factory for create an instance of EqualityComparerHelperRegistration.
    /// </summary>
    public interface IEqualityComparerHelperRegistrationFactory
    {
        /// <summary>
        /// Creates an  an instance of EqualityComparerHelperRegistration.
        /// </summary>
        /// <typeparam name="T">Creation type.</typeparam>
        /// <returns>Created instance of EqualityComparerHelperRegistration.</returns>
        IEqualityComparerHelperRegistration<T> Create<T>()
            where T : class;
    }
}