namespace BBT.StructureTools.Compare
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides compare functionality.
    /// </summary>
    internal interface ICompareHelper
    {
        /// <summary>
        /// Start the compare post process if it's needed.
        /// </summary>
        /// <typeparam name="T">Type of class to compare.</typeparam>
        /// <typeparam name="TIntention">Type of comparer intention.</typeparam>
        void DoComparePostProcessing<T, TIntention>(
            T candidate1Nullable,
            T candidate2Nullable,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where T : class
            where TIntention : IBaseComparerIntention;
    }
}
