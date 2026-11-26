namespace BBT.StructureTools.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Contains extensions for reference types.
    /// </summary>
    internal static class ReferenceTypeExtension
    {
        /// <summary>
        /// Adds <paramref name="value"/> to <paramref name="targetList"/>.
        /// But only if <paramref name="value"/> is not already added.
        /// </summary>
        /// <typeparam name="TValue">Type of list entries.</typeparam>
        internal static void AddUnique<TValue>(
            this ICollection<TValue> targetList,
            TValue value)
            where TValue : class
        {
            targetList.NotNull(nameof(targetList));
            value.NotNull(nameof(value));

            if (!targetList.Contains(value))
            {
                targetList.Add(value);
            }
        }

        /// <summary>
        /// Gets the list of <paramref name="target"/> collection property
        /// according to <paramref name="targetExpression"/>.
        /// </summary>
        /// <typeparam name="TTarget">The owner of the list.</typeparam>
        /// <typeparam name="TValue">The type of list entries.</typeparam>
        internal static ICollection<TValue> GetList<TTarget, TValue>(
            this TTarget target,
            Expression<Func<TTarget, ICollection<TValue>>> targetExpression)
            where TTarget : class
            where TValue : class
        {
            target.NotNull(nameof(target));
            targetExpression.NotNull(nameof(targetExpression));

            var targetListFunc = targetExpression.Compile();
            var targetList = targetListFunc.Invoke(target);
            return targetList;
        }

        /// <summary>
        /// An extension enabling addition of many elements to an enumeration without adding null values within the origin.
        /// </summary>
        /// <typeparam name="TTarget">The type of the owner of the collection property.</typeparam>
        /// <typeparam name="TValue">The type of the collection entry.</typeparam>
        internal static void AddRangeFilterNullValues<TTarget, TValue>(
            this TTarget target,
            Expression<Func<TTarget, ICollection<TValue>>> targetExpression,
            IEnumerable<TValue> values)
            where TTarget : class
            where TValue : class
        {
            target.NotNull(nameof(target));
            targetExpression.NotNull(nameof(targetExpression));
            values.NotNull(nameof(values));

            var targetListFunc = targetExpression.Compile();
            var targetList = targetListFunc.Invoke(target);
            targetList.AddRangeToMe(
                values
                .Where(value => value != null)
                .Where(value => !targetList.Contains(value)));
        }

        /// <summary>
        /// Sets a property value according to an expression function.
        /// </summary>
        /// <typeparam name="T">Base target type.</typeparam>
        /// <typeparam name="TValue">type of the value being retrieved.</typeparam>
        internal static void SetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLambda, TValue valueToSet)
            where T : class
        {
            target.NotNull(nameof(target));
            memberLambda.NotNull(nameof(memberLambda));

            var info = memberLambda.GetMemberInfoFromExpression() as PropertyInfo;

            if (info != null)
            {
                info.SetValue(target, valueToSet, null);
            }
            else
            {
                throw new StructureToolsException(FormattableString.Invariant($"Failed to set PropertyInfo on {target}, Expression = {memberLambda}"));
            }
        }

        /// <summary>
        /// Gets a property value according to an expression function.
        /// </summary>
        /// <typeparam name="T">Base target type.</typeparam>
        /// <typeparam name="TValue">type of the value being set.</typeparam>
        internal static TValue GetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLambda)
            where T : class
        {
            target.NotNull(nameof(target));
            memberLambda.NotNull(nameof(memberLambda));

            var info = memberLambda.GetMemberInfoFromExpression() as PropertyInfo;

            if (info != null)
            {
                return (TValue)info.GetValue(target);
            }
            else
            {
                throw new StructureToolsException(FormattableString.Invariant($"Failed to retrieve PropertyInfo from type {target}, Expression = {memberLambda.Name}"));
            }
        }

        /// <summary>
        /// Returns the name of a property on a given Type T by an expression function.
        /// </summary>
        /// <typeparam name="T">Type on which the expression works.</typeparam>
        /// <typeparam name="TValue">TValue of the property.</typeparam>
        private static MemberInfo GetMemberInfoFromExpression<T, TValue>(this Expression<Func<T, TValue>> expression)
        {
            expression.NotNull(nameof(expression));

            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member;
            }

            var operand = ((UnaryExpression)expression.Body).Operand;
            return ((MemberExpression)operand).Member;
        }
    }
}