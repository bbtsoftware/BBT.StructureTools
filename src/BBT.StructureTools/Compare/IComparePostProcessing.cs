﻿// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare
{
    /// <summary>
    /// Interface to add some additional functions on the end of the compare process.
    /// </summary>
    /// <typeparam name="T">Type of class to compare.</typeparam>
    /// <typeparam name="TIntention">Type of comparer intention of concern.</typeparam>
    public interface IComparePostProcessing<in T, TIntention> : IBaseAdditionalProcessing
        where T : class
        where TIntention : IBaseComparerIntention
    {
        /// <summary>
        /// This method will be called at the end of a compare process.
        /// </summary>
        void DoPostProcessing(T aCandidate1, T aCandidate2);
    }
}
