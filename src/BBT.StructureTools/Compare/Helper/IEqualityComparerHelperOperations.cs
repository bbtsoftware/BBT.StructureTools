namespace BBT.StructureTools.Compare.Helper
{
    using System.Collections.Generic;
    using BBT.StructureTools.Compare;

    /// <summary>
    /// Helper for the equals and get hash code calculation.
    /// </summary>
    /// <typeparam name="TModel">Type which is compared.</typeparam>
    public interface IEqualityComparerHelperOperations<TModel>
        where TModel : class
    {
        /// <summary>
        /// Compares the registered attributes.
        /// </summary>
        /// <returns>True if the registrations are equal.</returns>
        bool AreRegistrationsEquals(TModel candidate1, TModel candidate2);

        /// <summary>
        /// Compares the registered attributes.
        /// </summary>
        /// <returns>True if the registrations are equal.</returns>
        bool AreRegistrationsEquals(
            TModel candidate1,
            TModel candidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings,
            IEnumerable<IComparerExclusion> exclusions);

        /// <summary>
        /// Get the hash code of the registered attributes.
        /// </summary>
        int GetHashCodeOfRegistrations(TModel model);
    }
}