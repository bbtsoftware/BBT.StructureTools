namespace BBT.StructureTools.Compare
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// Compares and sorts two objects according to the delegate provided.
    /// </summary>
    /// <typeparam name="TModel">The type of the objects which shall be sorted.</typeparam>
    public class ModelDateTimeComparer<TModel> : IComparer<TModel>
        where TModel : class
    {
        private readonly Func<TModel, DateTime> sortOrderDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelDateTimeComparer{TModel}"/> class.
        /// </summary>
        public ModelDateTimeComparer(Func<TModel, DateTime> sortOrderDelegate)
        {
            sortOrderDelegate.NotNull(nameof(sortOrderDelegate));

            this.sortOrderDelegate = sortOrderDelegate;
        }

        /// <inheritdoc/>
        public int Compare(TModel x, TModel y)
        {
            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return +1;
            }

            var firstSortCriterion = this.sortOrderDelegate.Invoke(x);
            var secondSortCriterion = this.sortOrderDelegate.Invoke(y);
            var compare = firstSortCriterion.CompareTo(secondSortCriterion);

            return compare;
        }
    }
}