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
        private readonly Action<T, T> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericComparePostProcessing{T, TIntention}"/> class.
        /// </summary>
        public GenericComparePostProcessing(Action<T, T> action)
        {
            this.action = action;
        }

        /// <summary>
        /// This method will called at the end of a copy process.
        /// </summary>
        public void DoPostProcessing(T source, T target)
        {
            this.action.Invoke(source, target);
        }
    }
}
