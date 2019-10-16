// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;

    /// <summary>
    /// Compares two int values of a specifc model, this allows for example to sort models with the sort
    /// order of an enum value.
    /// </summary>
    /// <typeparam name="TModel">The model which shall be sorted.</typeparam>
    public class ModelIntComparer<TModel> : IComparer<TModel>
        where TModel : class
    {
        private readonly Func<TModel, int> mSortOrderDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelIntComparer{TModel}"/> class.
        /// </summary>
        public ModelIntComparer(Func<TModel, int> aSortOrderDelegate)
        {
            aSortOrderDelegate.Should().NotBeNull();

            this.mSortOrderDelegate = aSortOrderDelegate;
        }

        /// <summary>
        /// See <see cref="IComparer{T}.Compare"/>.
        /// </summary>
        public int Compare(TModel aModelA, TModel aModelB)
        {
            if (aModelA == null)
            {
                return -1;
            }

            if (aModelB == null)
            {
                return +1;
            }

            var lFirstSortCriterion = this.mSortOrderDelegate.Invoke(aModelA);
            var lSecondSortCriterion = this.mSortOrderDelegate.Invoke(aModelB);
            var lCompare = lFirstSortCriterion.CompareTo(lSecondSortCriterion);

            return lCompare;
        }
    }
}
