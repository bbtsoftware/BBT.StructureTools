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
        /// <param name="expression">The expression.</param>
        /// <typeparam name="TOwner">Type of the property owner.</typeparam>
        /// <typeparam name="TValueOfExpression">Type of the property value.</typeparam>
        /// <returns>The <see cref="PropertyInfo"/>.</returns>
        /// <exception cref="ArgumentException">If the property does not exist.</exception>
        public static PropertyInfo GetProperty<TOwner, TValueOfExpression>(Expression<Func<TOwner, TValueOfExpression>> expression)
        {
            expression.Should().NotBeNull();

            MemberExpression memberExpression;
            if (expression.Body is UnaryExpression unaryExpression)
            {
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = (MemberExpression)expression.Body;
            }

            var property = memberExpression.Member as PropertyInfo;

            if (property == null)
            {
                var message = string.Format(CultureInfo.InvariantCulture, "Property {0} not found in Type {1}.", memberExpression.Member.Name, typeof(TOwner));
                throw new ArgumentException(message);
            }

            if (property.DeclaringType != null && (typeof(TOwner) != property.DeclaringType) && !property.DeclaringType.IsAssignableFrom(typeof(TOwner)))
            {
                var message = string.Format(CultureInfo.InvariantCulture, "Property {0} not a member of Type {1}.", memberExpression.Member.Name, typeof(TOwner));
                throw new ArgumentException(message);
            }

            return property;
        }

        /// <summary>
        /// Extract the <see cref="PropertyInfo"/> out of a <see cref="System.Linq.Expressions.Expression"/>.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The <see cref="PropertyInfo"/>.</returns>
        /// <exception cref="ArgumentException">If the property does not exist.</exception>
        public static PropertyInfo GetProperty(LambdaExpression expression)
        {
            expression.Should().NotBeNull();

            (expression.Body is MemberExpression).Should().BeTrue(string.Format(CultureInfo.InvariantCulture, "Body of {0} is not a MemberExpression.", expression));
            var memberExpression = (MemberExpression)expression.Body;

            (memberExpression.Member is PropertyInfo).Should().BeTrue(string.Format(CultureInfo.InvariantCulture, "Member of {0} is not a PropertyInfo.", memberExpression));
            var property = (PropertyInfo)memberExpression.Member;

            return property;
        }
    }
}
