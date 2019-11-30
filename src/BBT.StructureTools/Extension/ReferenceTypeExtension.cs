namespace BBT.StructureTools.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Contains extensions for reference types.
    /// </summary>
    internal static class ReferenceTypeExtension
    {
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
                throw new CopyConvertCompareException(FormattableString.Invariant($"Failed to set PropertyInfo from type {target}, Expression = {memberLambda.Name}"));
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
                throw new CopyConvertCompareException(FormattableString.Invariant($"Failed to retrieve PropertyInfo from type {target}, Expression = {memberLambda.Name}"));
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