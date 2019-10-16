// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare.Helper.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Compare;

    /// <summary>
    /// Strategy to compare the different attribute types.
    /// </summary>
    /// <typeparam name="TModel">Subtype of <see cref="IBaseModel"/>.</typeparam>
    internal interface IEqualityComparerHelperStrategy<in TModel>
    {
        /// <summary>
        /// Compares a single element.
        /// </summary>
        bool IsElementEqualsOrExcluded(
            TModel aCandidate1,
            TModel aCandidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings,
            IEnumerable<IComparerExclusion> aExclusions);

        /// <summary>
        /// Get the hash code of an element. If the return value is null, the element
        /// is zero and no hash code could calculated.
        /// </summary>
        int? GetElementHashCode(TModel aModel);
    }
}