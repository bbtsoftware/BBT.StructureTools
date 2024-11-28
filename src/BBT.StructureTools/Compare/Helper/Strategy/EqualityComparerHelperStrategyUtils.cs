namespace BBT.StructureTools.Compare.Helper.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// Static helpers for the <see cref="IEqualityComparerHelperStrategy{TModel}"/> implementations.
    /// </summary>
    public static class EqualityComparerHelperStrategyUtils
    {
        /// <summary>
        /// Get the method name.
        /// </summary>
        /// <typeparam name="T">Base type of expression.</typeparam>
        /// <typeparam name="TReturn">Return type.</typeparam>
        public static string GetMethodName<T, TReturn>(Expression<Func<T, TReturn>> expression)
        {
            expression.NotNull(nameof(expression));

            var methodCallExpression = (MethodCallExpression)expression.Body;

            return methodCallExpression.Method.Name;
        }

        /// <summary>
        /// Check if the property exists in the exclusion list.
        /// </summary>
        public static bool IsPropertyExcluded(IEnumerable<IComparerExclusion> exclusions, Type typeOfModel, string name)
        {
            exclusions.NotNull(nameof(exclusions));
            typeOfModel.NotNull(nameof(typeOfModel));
            name.NotNullOrEmpty(nameof(name));

            // The exclusion can made for a model which inherits the property from an interface or
            // base class. We want to make sure the exclusion applies in any case.
            var mostBasicDeclaringType = typeOfModel
                .GetAllInheritedTypesOrdered() // Ordered, so that the most basic/least specific type is first.
                .FirstOrDefault(x => x.GetProperty(name) != null);

            if (mostBasicDeclaringType == null)
            {
                return false;
            }

            return exclusions.Any(x =>
                x.TypeOfComparerExclusion == TypeOfComparerExclusion.Property &&
                mostBasicDeclaringType.IsAssignableFrom(x.ExcludedModelType) &&
                x.ExcludedPropertyName == name);
        }

        /// <summary>
        /// Checks whether the two list are equivalent or not.
        /// </summary>
        /// <typeparam name="TModel">Type of model.</typeparam>
        public static bool AreListEquivalent<TModel>(
            IEnumerable<TModel> list1,
            IEnumerable<TModel> list2,
            Func<TModel, TModel, bool> compareFunc)
            where TModel : class
        {
            compareFunc.NotNull(nameof(compareFunc));

            // ReSharper disable once PossibleUnintendedReferenceComparison
            // BER says this is OK since either both are null, or have the same reference which is
            // in case of our compare infra the correct definition of equal.
            if (list1 == list2)
            {
                return true;
            }

            var list1List = list1 as IList<TModel> ?? list1.ToList();
            var list2List = list2 as IList<TModel> ?? list2.ToList();

            if (list1List.Count != list2List.Count)
            {
                return false;
            }

            var comparer = new DictionaryComparer<TModel>(compareFunc);
            var counterDictionary = new Dictionary<TModel, int>(comparer);
            foreach (var item1 in list1List)
            {
                if (counterDictionary.ContainsKey(item1))
                {
                    counterDictionary[item1]++;
                }
                else
                {
                    counterDictionary.Add(item1, 1);
                }
            }

            foreach (var item2 in list2List)
            {
                if (counterDictionary.ContainsKey(item2))
                {
                    counterDictionary[item2]--;
                }
                else
                {
                    return false;
                }
            }

            return counterDictionary.Values.All(x => x == 0);
        }

        /// <summary>
        /// Evaluates type that is compared.
        /// </summary>
        /// <typeparam name="T">type of comparer.</typeparam>
        public static Type GetCompareType<T>(IEqualityComparer<T> comparer)
        {
            comparer.NotNull(nameof(comparer));

            // evaluate type of a comparer
            // due to covariant restriction typeof(T) is not valid
            return comparer
                .GetType()
                .GetMethods()
                .First(x => x.Name == nameof(comparer.GetHashCode) && x.GetParameters().Length > 0)
                .GetParameters()
                .First()
                .ParameterType;
        }

        /// <summary>
        /// Provides the given comparer function as an <see cref="IEqualityComparer{TModel}"/>.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        private class DictionaryComparer<TModel> : IEqualityComparer<TModel>
            where TModel : class
        {
            /// <summary>
            /// Returns true if the 2 models are equal.
            /// </summary>
            private readonly Func<TModel, TModel, bool> compareFunc;

            /// <summary>
            /// Initializes a new instance of the <see cref="DictionaryComparer{TModel}"/> class.
            /// </summary>
            public DictionaryComparer(Func<TModel, TModel, bool> compareFunc)
            {
                this.compareFunc = compareFunc;
            }

            /// <inheritdoc/>
            public bool Equals(TModel model1, TModel model2)
            {
                return this.compareFunc(model1, model2);
            }

            /// <inheritdoc/>
            public int GetHashCode(TModel model)
            {
                // The Hash code must not include any excluded properties!
                return model.GetType().GetHashCode();
            }
        }
    }
}
