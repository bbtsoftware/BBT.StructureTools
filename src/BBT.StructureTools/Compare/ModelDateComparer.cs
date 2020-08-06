namespace BBT.StructureTools.Compare
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// Compares and sorts two objects according to the delegate provided.
    /// </summary>
    /// <typeparam name="TModel">The type of the objects which shall be sorted.</typeparam>
    public class ModelDateComparer<TModel> : IComparer<TModel>
        where TModel : class
    {
        private readonly Func<TModel, DateTime> sortOrderDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelDateComparer{TModel}"/> class.
        /// </summary>
        public ModelDateComparer(Func<TModel, DateTime> sortOrderDelegate)
        {
            StructureToolsArgumentChecks.NotNull(sortOrderDelegate, nameof(sortOrderDelegate));

            this.sortOrderDelegate = sortOrderDelegate;
        }

        /// <inheritdoc/>
        public int Compare(TModel modelA, TModel modelB)
        {
            if (modelA == null)
            {
                return -1;
            }

            if (modelB == null)
            {
                return +1;
            }

            var firstSortCriterion = this.sortOrderDelegate.Invoke(modelA);
            var secondSortCriterion = this.sortOrderDelegate.Invoke(modelB);
            var compare = firstSortCriterion.CompareTo(secondSortCriterion);

            return compare;
        }
    }
}
