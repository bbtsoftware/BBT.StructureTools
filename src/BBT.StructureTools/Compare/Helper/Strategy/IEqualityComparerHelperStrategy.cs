﻿namespace BBT.StructureTools.Compare.Helper.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Compare;

    /// <summary>
    /// Strategy to compare the different attribute types.
    /// </summary>
    /// <typeparam name="TModel">Type which is compared.</typeparam>
    internal interface IEqualityComparerHelperStrategy<in TModel>
    {
        /// <summary>
        /// Compares a single element.
        /// </summary>
        bool IsElementEqualsOrExcluded(
            TModel candidate1,
            TModel candidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings,
            IEnumerable<IComparerExclusion> exclusions);

        /// <summary>
        /// Get the hash code of an element. If the return value is null, the element
        /// is zero and no hash code could calculated.
        /// </summary>
        int? GetElementHashCode(TModel model);
    }
}