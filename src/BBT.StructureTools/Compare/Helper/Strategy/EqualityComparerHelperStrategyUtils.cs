// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare.Helper.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BBT.StructureTools.Compare;

    /// <summary>
    /// Static helpers for the <see cref="IEqualityComparerHelperStrategy{TModel}"/> implementations.
    /// </summary>
    public static class EqualityComparerHelperStrategyUtils
    {
        /// <summary>
        /// See <see cref="ReflectionUtils.GetPropertyName{T, TReturn}"/>.
        /// </summary>
        /// <typeparam name="T">Base type of expression.</typeparam>
        /// <typeparam name="TReturn">Return type.</typeparam>
        public static string GetPropertyName<T, TReturn>(Expression<Func<T, TReturn>> aExpression)
        {
            aExpression.Should().NotBeNull();


            return ReflectionUtils.GetPropertyName(aExpression);
        }

        /// <summary>
        /// Get the method name.
        /// </summary>
        /// <typeparam name="T">Base type of expression.</typeparam>
        /// <typeparam name="TReturn">Return type.</typeparam>
        public static string GetMethodName<T, TReturn>(Expression<Func<T, TReturn>> aExpression)
        {
            aExpression.Should().NotBeNull();


            var lMethodCallExpression = (MethodCallExpression)aExpression.Body;

            return lMethodCallExpression.Method.Name;
        }

        /// <summary>
        /// Check if the property exists in the exclusion list.
        /// </summary>
        public static bool IsPropertyExcluded(IEnumerable<IComparerExclusion> aExclusions, Type aTypeOfModel, string aName)
        {
            aExclusions.Should().NotBeNull();

            aTypeOfModel.Should().NotBeNull();

            Checks.AssertNotEmpty(aName, nameof(aName));

            // The exclusion can made for a model which inherits the property from an interface or
            // base class. We want to make sure the exclusion applies in any case.
            var lMostBasicDeclaringType = aTypeOfModel
                .GetAllInheritedTypesOrdered() // Ordered, so that the most basic/least specific type is first.
                .FirstOrDefault(aX => aX.GetProperty(aName) != null);

            if (lMostBasicDeclaringType == null)
            {
                return false;
            }

            return aExclusions.Any(aX =>
                aX.TypeOfComparerExclusion == TypeOfComparerExclusion.Property &&
                lMostBasicDeclaringType.IsAssignableFrom(aX.ExcludedModelType) &&
                aX.ExcludedPropertyName == aName);
        }

        /// <summary>
        /// Checks whether the two list are equivalent or not.
        /// </summary>
        /// <typeparam name="TModel">Type of model.</typeparam>
        public static bool AreListEquivalent<TModel>(
            IEnumerable<TModel> aList1,
            IEnumerable<TModel> aList2,
            Func<TModel, TModel, bool> aCompareFunc)
            where TModel : class
        {
            aCompareFunc.Should().NotBeNull();


            // ReSharper disable once PossibleUnintendedReferenceComparison
            // BER says this is OK since either both are null, or have the same reference which is
            // in case of our compare infra the correct definition of equal.
            if (aList1 == aList2)
            {
                return true;
            }

            var lList1List = aList1 as IList<TModel> ?? aList1.ToList();
            var lList2List = aList2 as IList<TModel> ?? aList2.ToList();

            if (lList1List.Count != lList2List.Count)
            {
                return false;
            }

            var lComparer = new DictionaryComparer<TModel>(aCompareFunc);
            var lCounterDictionary = new Dictionary<TModel, int>(lComparer);
            foreach (var lItem1 in lList1List)
            {
                if (lCounterDictionary.ContainsKey(lItem1))
                {
                    lCounterDictionary[lItem1]++;
                }
                else
                {
                    lCounterDictionary.Add(lItem1, 1);
                }
            }

            foreach (var lItem2 in lList2List)
            {
                if (lCounterDictionary.ContainsKey(lItem2))
                {
                    lCounterDictionary[lItem2]--;
                }
                else
                {
                    return false;
                }
            }

            return lCounterDictionary.Values.All(aX => aX == 0);
        }

        /// <summary>
        /// Evaluates type that is compared.
        /// </summary>
        /// <typeparam name="T">type of comparer.</typeparam>
        public static Type GetCompareType<T>(IEqualityComparer<T> aComparer)
        {
            aComparer.Should().NotBeNull();


            // evaluate type of a comparer
            // due to covariant restriction typeof(T) is not valid
            return aComparer
                .GetType()
                .GetMethods()
                .First(aX => aX.Name == nameof(aComparer.GetHashCode))
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
            private readonly Func<TModel, TModel, bool> mCompareFunc;

            /// <summary>
            /// Initializes a new instance of the <see cref="DictionaryComparer{TModel}"/> class.
            /// </summary>
            public DictionaryComparer(Func<TModel, TModel, bool> aCompareFunc)
            {
                this.mCompareFunc = aCompareFunc;
            }

            /// <summary>
            /// See <see cref="IEqualityComparer{T}.Equals(T, T)"/>.
            /// </summary>
            public bool Equals(TModel aModel1, TModel aModel2)
            {
                return this.mCompareFunc(aModel1, aModel2);
            }

            /// <summary>
            /// See <see cref="IEqualityComparer{T}.GetHashCode(T)"/>.
            /// </summary>
            public int GetHashCode(TModel aModel)
            {
                // The Hash code must not include any excluded properties!
                return aModel.GetType().GetHashCode();
            }
        }
    }
}
