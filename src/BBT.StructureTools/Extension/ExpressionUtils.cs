namespace BBT.StructureTools.Extension
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Utility for <see cref="System.Linq.Expressions.Expression"/>.
    /// </summary>
    internal static class ExpressionUtils
    {
        /// <summary>
        /// Extract the <see cref="PropertyInfo"/> out of a <see cref="System.Linq.Expressions.Expression"/>.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <typeparam name="TOwner">Type of the property owner.</typeparam>
        /// <typeparam name="TValueOfExpression">Type of the property value.</typeparam>
        /// <returns>The <see cref="PropertyInfo"/>.</returns>
        /// <exception cref="ArgumentException">If the property does not exist.</exception>
        internal static PropertyInfo GetProperty<TOwner, TValueOfExpression>(Expression<Func<TOwner, TValueOfExpression>> expression)
        {
            expression.NotNull(nameof(expression));

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
    }
}
