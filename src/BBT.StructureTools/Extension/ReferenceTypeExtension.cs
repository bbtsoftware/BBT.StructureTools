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
        internal static void AddRangeToCollectionFilterNulvalues<TTarget, TValue>(
            this TTarget target,
            Expression<Func<TTarget, ICollection<TValue>>> targetExpression,
            IEnumerable<TValue> values)
            where TTarget : class
            where TValue : class
        {
            target.Should().NotBeNull();
            targetExpression.Should().NotBeNull();
            values.Should().NotBeNull();

            var targetExpressionMemberExpression = (MemberExpression)targetExpression.Body;
            var valueProperty = (PropertyInfo)targetExpressionMemberExpression.Member;

            var addMethod = targetExpression.Compile().Invoke(target);

            foreach (var value in values)
            {
                if (value != null)
                {
                    addMethod.Add(value);
                }
            }
        }

        /// <summary>
        /// Sets a property value according to an expression function.
        /// </summary>
        /// <typeparam name="T">Base target type.</typeparam>
        /// <typeparam name="TValue">type of the value being retrieved.</typeparam>
        internal static void SetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLamda, TValue valueToSet)
            where T : class
        {
            target.Should().NotBeNull();
            memberLamda.Should().NotBeNull();

            var info = memberLamda.GetMemberInfoFromExpression() as PropertyInfo;

            if (info != null)
            {
                info.SetValue(target, valueToSet, null);
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
        internal static TValue GetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLamda)
            where T : class
        {
            target.Should().NotBeNull();
            memberLamda.Should().NotBeNull();

            var info = memberLamda.GetMemberInfoFromExpression() as PropertyInfo;

            if (info != null)
            {
                return (TValue)info.GetValue(target);
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
        internal static string GetPropertyNameFromExpression<T>(this T target, Expression<Func<T, object>> expression)
        {
            expression.Should().NotBeNull();

            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            var operand = ((UnaryExpression)expression.Body).Operand;
            return ((MemberExpression)operand).Member.Name;
        }

        /// <summary>
        /// Returns the name of a property on a given Type T by an expression function.
        /// </summary>
        /// <typeparam name="T">Type on which the expression works.</typeparam>
        /// <typeparam name="TValue">TValue of the property.</typeparam>
        internal static MemberInfo GetMemberInfoFromExpression<T, TValue>(this Expression<Func<T, TValue>> expression)
        {
            expression.Should().NotBeNull();

            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member;
            }

            var operand = ((UnaryExpression)expression.Body).Operand;
            return ((MemberExpression)operand).Member;
        }
    }
}