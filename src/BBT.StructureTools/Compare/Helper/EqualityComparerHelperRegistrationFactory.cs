// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare.Helper
{
    /// <summary>
    /// Factory to create an instance of EqualityComparerHelperRegistration.
    /// </summary>
    public class EqualityComparerHelperRegistrationFactory : IEqualityComparerHelperRegistrationFactory
    {
        /// <summary>
        /// Creates an instance of EqualityComparerHelperRegistration.
        /// </summary>
        /// <typeparam name="T">Creation type.</typeparam>
        /// <returns>Created instance of EqualityComparerHelperRegistration.</returns>
        public IEqualityComparerHelperRegistration<T> Create<T>()
            where T : class
        {
            return new EqualityComparerHelperRegistration<T>();
        }
    }
}