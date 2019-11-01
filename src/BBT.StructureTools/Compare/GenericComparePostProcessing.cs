namespace BBT.StructureTools.Compare
{
    using System;

    /// <inheritdoc/>
    public class GenericComparePostProcessing<T, TIntention> : IComparePostProcessing<T, TIntention>
        where T : class
        where TIntention : IBaseComparerIntention
    {
        private readonly Action<T, T> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericComparePostProcessing{T, TIntention}"/> class.
        /// </summary>
        public GenericComparePostProcessing(Action<T, T> action)
        {
            this.action = action;
        }

        /// <inheritdoc/>
        public void DoPostProcessing(T source, T target)
        {
            this.action.Invoke(source, target);
        }
    }
}
