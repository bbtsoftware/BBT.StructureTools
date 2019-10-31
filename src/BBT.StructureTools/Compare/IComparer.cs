// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for model comparison.
    /// ---------------------------------
    /// Remark for hash code calculation:
    /// ---------------------------------
    /// Calculates a hash code for the object.
    /// Attention, hash codes for the same object can change in
    /// database operations with the object itself or one of it's children,
    /// because some base models include ID/GUID in the hash code calculation.
    /// </summary>
    /// <typeparam name="TModelToCompare">Model to compare.</typeparam>
    /// <typeparam name="TComparerIntention">Type of compare. A sub interface of <see cref="IBaseComparerIntention"/>.</typeparam>
    public interface IComparer<in TModelToCompare, TComparerIntention> : IEqualityComparer<TModelToCompare>
        where TModelToCompare : class
        where TComparerIntention : IBaseComparerIntention
    {
        /// <summary>
        /// Extend the equality comparer interface with additional action.
        /// </summary>
        bool Equals(
            TModelToCompare candidate1,
            TModelToCompare candidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings);

        /// <summary>
        /// Extend the equality comparer interface with the possibility to set compare exclusions.
        /// </summary>
        bool Equals(
            TModelToCompare candidate1,
            TModelToCompare candidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings,
            IEnumerable<IComparerExclusion> exclusions);
    }
}
