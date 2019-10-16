// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare
{
    using System;

    /// <summary>
    /// Generic implementation of <see cref="IComparePostProcessing{T, TIntention}"/>.
    /// </summary>
    /// <typeparam name="T">See link above.</typeparam>
    /// <typeparam name="TIntention">See link above.</typeparam>
    public class GenericComparePostProcessing<T, TIntention> : IComparePostProcessing<T, TIntention>
        where T : class
        where TIntention : IBaseComparerIntention
    {
        /// <summary>
        /// The action.
        /// </summary>
        private readonly Action<T, T> mAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericComparePostProcessing{T, TIntention}"/> class.
        /// </summary>
        public GenericComparePostProcessing(Action<T, T> aAction)
        {
            this.mAction = aAction;
        }

        /// <summary>
        /// This method will called at the end of a copy process.
        /// </summary>
        public void DoPostProcessing(T source, T target)
        {
            this.mAction.Invoke(source, target);
        }
    }
}
