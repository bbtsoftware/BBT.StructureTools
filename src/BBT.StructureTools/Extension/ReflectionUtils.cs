namespace BBT.StructureTools.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using BBT.StructureTools.Compare;
    using FluentAssertions;

    /// <summary>
    /// ReflectionUtils.
    /// </summary>
    internal static class ReflectionUtils
    {
        /// <summary>Checks whether the passed object is cast to the specified type.
        /// can, and returns it in the type. Throws an exception if the object is dropped by a
        /// Other type or Nothing.
        /// The object is returned in the required type or an exception is thrown, it is
        /// is never returned Nothing.
        /// </summary>
        /// <param name="value">Object to check.</param>
        /// <exception cref="ArgumentNullException">Falls value <see langword="Nothing" /> is.
        /// </exception>
        /// <exception cref="CopyConvertCompareException">If the type of value is not
        /// is compatible with T.</exception>
        /// <typeparam name="T">Expected type.</typeparam>
        public static T CastIfTypeOrSubtypeOrThrow<T>(object value)
            where T : class
        {
            value.Should().NotBeNull();

            if (!(value is T generictValue))
            {
                throw new CopyConvertCompareException(FormattableString.Invariant($"An object of type '{typeof(T).Name}' was expected, but the object was of type '{value.GetType().Name}'."));
            }
            else
            {
                return generictValue;
            }
        }

        /// <summary>
        /// Returns the name of a property defined by an expression.
        /// <para/>
        /// Example calls in VB.NET: <br/>
        /// <c>Dim lPropertyName1 = ReflectionUtils.GetPropertyName(Function(x As SampleClass) x.Foo)</c><br/>
        /// <c>Dim lPropertyName2 = ReflectionUtils.GetPropertyName(Function(x As SampleClass) x.Foo1.Foo2)</c>.<br/>
        /// <para/>
        /// Example calls in C#: <br/>
        /// <c>var lPropertyName1 = ReflectionUtils.GetPropertyName((SampleClass x) => x.Foo);</c>.<br/>
        /// <c>var lPropertyName2 = ReflectionUtils.GetPropertyName((SampleClass x) => x.Foo1.Foo2);</c>.<br/>
        /// </summary>
        /// <typeparam name="T">The type that serves as the starting point for the property. Must be used with
        /// Use according to the examples given are not specified.</typeparam>
        /// <typeparam name="TReturn">The return type of the property. Must be used according to the
        /// given examples are not given.</typeparam>
        /// <param name="expression">The Property Expression.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "So a simple call is possible as shown in the example.")]
        public static string GetPropertyName<T, TReturn>(Expression<Func<T, TReturn>> expression)
        {
            expression.Should().NotBeNull();
            expression.Body.Should().BeAssignableTo<MemberExpression>();

            var memberExpression = (MemberExpression)expression.Body;
            var memberName = memberExpression.Member.Name;
            return memberName;
        }

        /// <summary>
        /// Gets all inherited types, including interfaces, the specified type itself, and <see cref="object"/>
        /// if it's a class.
        /// </summary>
        public static IEnumerable<Type> GetAllInheritedTypes(this Type extendedType)
        {
            extendedType.Should().NotBeNull();

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

        /// <summary>
        /// Gets all inherited types, ordered (more basic/inherited types first).
        /// </summary>
        public static IEnumerable<Type> GetAllInheritedTypesOrdered(this Type extendedType)
        {
            extendedType.Should().NotBeNull();

            var allInheritedTypesOrdered = extendedType.GetAllInheritedTypes().OrderBy(x => x, new TypeComparer()).ToList();
            return allInheritedTypesOrdered;
        }
    }
}
