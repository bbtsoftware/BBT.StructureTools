namespace BBT.StructureTools.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using BBT.StructureTools.Impl.Compare.Helper;

    /// <summary>
    /// ReflectionUtils.
    /// </summary>
    internal static class ReflectionUtils
    {
        /// <summary>
        /// Returns the name of a property defined by an expression.
        /// <para/>
        /// Example calls in VB.NET: <br/>
        /// <c>Dim PropertyName1 = ReflectionUtils.GetPropertyName(Function(x As SampleClass) x.Foo)</c><br/>
        /// <c>Dim PropertyName2 = ReflectionUtils.GetPropertyName(Function(x As SampleClass) x.Foo1.Foo2)</c>.<br/>
        /// <para/>
        /// Example calls in C#: <br/>
        /// <c>var PropertyName1 = ReflectionUtils.GetPropertyName((SampleClass x) => x.Foo);</c>.<br/>
        /// <c>var PropertyName2 = ReflectionUtils.GetPropertyName((SampleClass x) => x.Foo1.Foo2);</c>.<br/>
        /// </summary>
        /// <typeparam name="T">The type that serves as the starting point for the property.</typeparam>
        /// <typeparam name="TReturn">The return type of the property. Must be used according to the
        /// given examples.</typeparam>
        /// <param name="expression">The Property Expression.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "So a simple call is possible as shown in the example.")]
        internal static string GetPropertyName<T, TReturn>(Expression<Func<T, TReturn>> expression)
        {
            expression.NotNull(nameof(expression));
            expression.Body.IsOfType<MemberExpression>(nameof(expression.Body));

            var memberExpression = (MemberExpression)expression.Body;
            var memberName = memberExpression.Member.Name;
            return memberName;
        }

        /// <summary>
        /// Gets all inherited types, ordered (more basic/inherited types first).
        /// </summary>
        internal static IEnumerable<Type> GetAllInheritedTypesOrdered(this Type extendedType)
        {
            extendedType.NotNull(nameof(extendedType));

            var allInheritedTypesOrdered = extendedType.GetAllInheritedTypes().OrderBy(x => x, new TypeComparer()).ToList();
            return allInheritedTypesOrdered;
        }

        /// <summary>
        /// Gets all inherited types, including interfaces, the specified type itself, and <see cref="object"/>
        /// if it's a class.
        /// </summary>
        private static IEnumerable<Type> GetAllInheritedTypes(this Type extendedType)
        {
            extendedType.NotNull(nameof(extendedType));

            IList<Type> allInheritedTypes = new List<Type>();
            var current = extendedType;

            while (current != null)
            {
                allInheritedTypes.Add(current);
                current = current.BaseType;
            }

            var interfaces = extendedType.GetInterfaces();
            allInheritedTypes.AddRangeToMe(interfaces);
            return allInheritedTypes;
        }
    }
}
