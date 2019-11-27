namespace BBT.StructureTools.Compare
{
    using BBT.StructureTools.Compare.Helper;

    /// <summary>
    /// Used to declare attributes used for comparison.
    /// </summary>
    /// <typeparam name="TModelToCompare">The model type which shall be compared.</typeparam>
    /// <typeparam name="TCompareIntention">The intention of the comparison.</typeparam>
    public interface IComparerRegistrations<TModelToCompare, TCompareIntention>
        where TModelToCompare : class
        where TCompareIntention : IBaseComparerIntention
    {
        /// <summary>
        /// Used to declare attributes used for comparison.
        /// </summary>
        void DoRegistrations(IEqualityComparerHelperRegistration<TModelToCompare> registrations);
    }
}
