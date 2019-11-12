namespace BBT.StructureTools.Compare
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// Compares two int values of a specific model, this allows for example to sort models with the sort
    /// order of an enum value.
    /// </summary>
    /// <typeparam name="TModel">The model which shall be sorted.</typeparam>
    public class ModelIntComparer<TModel> : IComparer<TModel>
        where TModel : class
    {
        private readonly Func<TModel, int> sortOrderDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelIntComparer{TModel}"/> class.
        /// </summary>
        public ModelIntComparer(Func<TModel, int> sortOrderDelegate)
        {
            sortOrderDelegate.NotNull(nameof(sortOrderDelegate));

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
