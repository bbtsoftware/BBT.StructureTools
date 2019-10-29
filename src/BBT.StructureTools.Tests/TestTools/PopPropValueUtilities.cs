using System;
using System.Linq.Expressions;
using System.Reflection;
using BBT.StructureTools.Extension;

namespace BBT.StructureTools.Tests.TestTools
{
    /// <summary>
    /// Extensions to generate property values
    /// for test classes.
    /// </summary>
    public static class PopPropValueExtensions
    {
        public static void CreateValueForProperty<T>(this T target, Expression<Func<T, string>> propex)
            where T : class
        {
            var propname = ReflectionUtils.GetPropertyName(propex);
            var lTargetType = typeof(T);
            var type = lTargetType.Name;

            var targetProperty = lTargetType.GetProperty(propname, BindingFlags.Public | BindingFlags.Instance);

            var value = $"{type}.{propname}_{DateTime.Now.ToLongTimeString()}";

            targetProperty.SetValue(target, value);
        }
    }
}
