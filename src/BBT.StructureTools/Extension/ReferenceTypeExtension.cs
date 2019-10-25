// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using FluentAssertions;

    /// <summary>
    /// Contains extensions for reference types.
    /// </summary>
    internal static class ReferenceTypeExtension
    {
        /// <summary>
        /// An extension enabling addition of many elements to a Collection.
        /// </summary>
        /// <typeparam name="TTarget">The type of the owner of the collection property.</typeparam>
        /// <typeparam name="TValue">The type of the collection entry.</typeparam>
        public static void AddRangeToCollectionFilterNullValues<TTarget, TValue>(
            this TTarget aTarget,
            Expression<Func<TTarget, ICollection<TValue>>> aTargetExpression,
            IEnumerable<TValue> aValues)
            where TTarget : class
            where TValue : class
        {
            aTarget.Should().NotBeNull();
            aTargetExpression.Should().NotBeNull();
            aValues.Should().NotBeNull();

            var lTargetExpression = (MemberExpression)aTargetExpression.Body;
            var lValueProperty = (PropertyInfo)lTargetExpression.Member;

            var lAddMethodInfo = lValueProperty.PropertyType.GetMethod("Add", new[] { typeof(TValue) });
            var lValuePropertyValue = lValueProperty.GetValue(aTarget);

            foreach (var lValue in aValues)
            {
                if (lValue != null)
                {
                    lAddMethodInfo.Invoke(lValuePropertyValue, new[] { lValue });
                }
            }
        }

        /// <summary>
        /// Sets a property value according to an expression function.
        /// </summary>
        /// <typeparam name="T">Base target type.</typeparam>
        /// <typeparam name="TValue">type of the value being retrieved.</typeparam>
        public static void SetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLamda, TValue valueToSet)
            where T : class
        {
            target.Should().NotBeNull();
            memberLamda.Should().NotBeNull();

            var lInfo = memberLamda.GetMemberInfoFromExpression() as PropertyInfo;

            if (lInfo != null)
            {
                lInfo.SetValue(target, valueToSet, null);
            }
            else
            {
                throw new CopyConvertCompareException(FormattableString.Invariant($"Failed to set PropertyInfo from type {target}, Expression = {memberLamda.Name}"));
            }
        }

        /// <summary>
        /// Gets a property value according to an expression function.
        /// </summary>
        /// <typeparam name="T">Base target type.</typeparam>
        /// <typeparam name="TValue">type of the value being set.</typeparam>
        public static TValue GetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLamda)
            where T : class
        {
            target.Should().NotBeNull();
            memberLamda.Should().NotBeNull();

            var lInfo = memberLamda.GetMemberInfoFromExpression() as PropertyInfo;

            if (lInfo != null)
            {
                return (TValue)lInfo.GetValue(target);
            }
            else
            {
                throw new CopyConvertCompareException(FormattableString.Invariant($"Failed to retrieve PropertyInfo from type {target}, Expression = {memberLamda.Name}"));
            }
        }

        /// <summary>
        /// Returns the name of a property on a given Type T by an expression function.
        /// </summary>
        /// <typeparam name="T">Type on which the expression works.</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "target", Justification = "Needed for extension method.")]
        public static string GetPropertyNameFromExpression<T>(this T target, Expression<Func<T, object>> expression)
        {
            expression.Should().NotBeNull();

            if (expression.Body is MemberExpression lMemberExpression)
            {
                return lMemberExpression.Member.Name;
            }

            var lOperand = ((UnaryExpression)expression.Body).Operand;
            return ((MemberExpression)lOperand).Member.Name;
        }

        /// <summary>
        /// Returns the name of a property on a given Type T by an expression function.
        /// </summary>
        /// <typeparam name="T">Type on which the expression works.</typeparam>
        /// <typeparam name="TValue">TValue of the property.</typeparam>
        public static MemberInfo GetMemberInfoFromExpression<T, TValue>(this Expression<Func<T, TValue>> expression)
        {
            expression.Should().NotBeNull();

            if (expression.Body is MemberExpression lMemberExpression)
            {
                return lMemberExpression.Member;
            }

            var lOperand = ((UnaryExpression)expression.Body).Operand;
            return ((MemberExpression)lOperand).Member;
        }
    }
}