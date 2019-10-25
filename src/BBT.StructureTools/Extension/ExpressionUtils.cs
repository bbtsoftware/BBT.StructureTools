// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Extension
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;
    using FluentAssertions;

    /// <summary>
    /// Utility for <see cref="System.Linq.Expressions.Expression"/>.
    /// </summary>
    internal class ExpressionUtils
    {
        /// <summary>
        /// Extract the <see cref="PropertyInfo"/> out of a <see cref="System.Linq.Expressions.Expression"/>.
        /// </summary>
        /// <param name="aExpression">The expression.</param>
        /// <typeparam name="TOwner">Type of the property owner.</typeparam>
        /// <typeparam name="TValueOfExpression">Type of the property value.</typeparam>
        /// <returns>The <see cref="PropertyInfo"/>.</returns>
        /// <exception cref="ArgumentException">If the property does not exist.</exception>
        public static PropertyInfo GetProperty<TOwner, TValueOfExpression>(Expression<Func<TOwner, TValueOfExpression>> aExpression)
        {
            aExpression.Should().NotBeNull();

            MemberExpression lMemberExpression;
            if (aExpression.Body is UnaryExpression lUnaryExpression)
            {
                lMemberExpression = lUnaryExpression.Operand as MemberExpression;
            }
            else
            {
                lMemberExpression = (MemberExpression)aExpression.Body;
            }

            var lProperty = lMemberExpression.Member as PropertyInfo;

            if (lProperty == null)
            {
                var lMessage = string.Format(CultureInfo.InvariantCulture, "Property {0} not found in Type {1}.", lMemberExpression.Member.Name, typeof(TOwner));
                throw new ArgumentException(lMessage);
            }

            if (lProperty.DeclaringType != null && (typeof(TOwner) != lProperty.DeclaringType) && !lProperty.DeclaringType.IsAssignableFrom(typeof(TOwner)))
            {
                var lMessage = string.Format(CultureInfo.InvariantCulture, "Property {0} not a member of Type {1}.", lMemberExpression.Member.Name, typeof(TOwner));
                throw new ArgumentException(lMessage);
            }

            return lProperty;
        }

        /// <summary>
        /// Extract the <see cref="PropertyInfo"/> out of a <see cref="System.Linq.Expressions.Expression"/>.
        /// </summary>
        /// <param name="aExpression">The expression.</param>
        /// <returns>The <see cref="PropertyInfo"/>.</returns>
        /// <exception cref="ArgumentException">If the property does not exist.</exception>
        public static PropertyInfo GetProperty(LambdaExpression aExpression)
        {
            aExpression.Should().NotBeNull();

            (aExpression.Body is MemberExpression).Should().BeTrue(string.Format(CultureInfo.InvariantCulture, "Body of {0} is not a MemberExpression.", aExpression));
            var lMemberExpression = (MemberExpression)aExpression.Body;

            (lMemberExpression.Member is PropertyInfo).Should().BeTrue(string.Format(CultureInfo.InvariantCulture, "Member of {0} is not a PropertyInfo.", lMemberExpression));
            var lProperty = (PropertyInfo)lMemberExpression.Member;

            return lProperty;
        }
    }
}
