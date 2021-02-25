namespace BBT.StructureTools.Tests.TestTools
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// Extensions to generate property values
    /// for test classes.
    /// </summary>
    public static class PopPropValueExtensions
    {
        public static void CreateStringForProperty<T>(this T target, Expression<Func<T, string>> propertyExpression)
            where T : class
        {
            var propertyName = ReflectionUtils.GetPropertyName(propertyExpression);
            var targetType = typeof(T);
            var type = targetType.Name;

            var targetProperty = targetType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            var value = $"{type}.{propertyName}_{DateTime.Now.ToLongTimeString()}";

            targetProperty.SetValue(target, value);
        }
    }
}
