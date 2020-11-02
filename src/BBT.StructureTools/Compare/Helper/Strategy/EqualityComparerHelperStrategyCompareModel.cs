namespace BBT.StructureTools.Compare.Helper.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class EqualityComparerHelperStrategyCompareModel<TModel, TTargetModel, TIntention>
        : IEqualityComparerHelperStrategy<TModel>
        where TTargetModel : class
        where TIntention : IBaseComparerIntention
    {
        /// <summary>
        /// Function to get the property value.
        /// </summary>
        private readonly Func<TModel, TTargetModel> func;

        /// <summary>
        /// Name of compared property.
        /// </summary>
        private readonly string propertyName;

        private readonly IComparer<TTargetModel, TIntention> comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerHelperStrategyCompareModel{TModel,TTargetModel,TIntention}"/> class.
        /// </summary>
        public EqualityComparerHelperStrategyCompareModel(
            Expression<Func<TModel, TTargetModel>> expression,
            IComparer<TTargetModel, TIntention> comparer)
        {
            expression.NotNull(nameof(expression));
            expression.NotNull(nameof(comparer));

            this.func = expression.Compile();
            this.propertyName = ReflectionUtils.GetPropertyName(expression);
            this.comparer = comparer;
        }

        /// <inheritdoc/>
        public bool IsElementEqualsOrExcluded(
            TModel candidate1,
            TModel candidate2,
            ICollection<IBaseAdditionalProcessing> additionalProcessings,
            IEnumerable<IComparerExclusion> exclusions)
        {
            if (EqualityComparerHelperStrategyUtils.IsPropertyExcluded(exclusions, typeof(TModel), this.propertyName))
            {
                return true;
            }

            var value1 = this.func.Invoke(candidate1) as TTargetModel;
            var value2 = this.func.Invoke(candidate2) as TTargetModel;

            return this.comparer.Equals(value1, value2, additionalProcessings, exclusions);
        }

        /// <inheritdoc/>
        public int? GetElementHashCode(TModel model)
        {
            if (this.func.Invoke(model) is TTargetModel value)
            {
                return this.comparer.GetHashCode(value);
            }

            return null;
        }
    }
}