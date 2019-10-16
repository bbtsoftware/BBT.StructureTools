// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Extension
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
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
        /// <param name="aValue">Object to check.</param>
        /// <exception cref="ArgumentNullException">Falls aValue <see langword="Nothing" /> is.
        /// </exception>
        /// <exception cref="CopyConvertCompareException">If the type of aValue is not
        /// is compatible with T.</exception>
        public static T CastIfTypeOrSubtypeOrThrow<T>(object aValue)
            where T : class
        {
            aValue.Should().NotBeNull();

            var lTValue = aValue as T;
            if (lTValue == null)
            {
                throw new CopyConvertCompareException(string.Format($"An object of type '{typeof(T).Name}' was expected, but the object was of type '{aValue.GetType().Name}'."));
            }
            else
            {
                return lTValue;
            }
        }

        /// <summary>
        /// Returns the name of a property defined by an expression.
        /// <para/>
        /// Example calls in VB.NET: <br/>
        /// <c>Dim lPropertyName1 = ReflectionUtils.GetPropertyName(Function(aX As SampleClass) aX.Foo)</c><br/>
        /// <c>Dim lPropertyName2 = ReflectionUtils.GetPropertyName(Function(aX As SampleClass) aX.Foo1.Foo2)</c><br/>
        /// <para/>
        /// Example calls in C#: <br/>
        /// <c>var lPropertyName1 = ReflectionUtils.GetPropertyName((SampleClass aX) => aX.Foo);</c><br/>
        /// <c>var lPropertyName2 = ReflectionUtils.GetPropertyName((SampleClass aX) => aX.Foo1.Foo2);</c><br/>
        /// </summary>
        /// <typeparam name="T">The type that serves as the starting point for the property. Must be used with
        /// Use according to the examples given are not specified.</typeparam>
        /// <typeparam name="TReturn">The return type of the property. Must be used according to the
        /// given examples are not given.</typeparam>
        /// <param name="aExpression">The Property Expression.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "So a simple call is possible as shown in the example.")]
        public static string GetPropertyName<T, TReturn>(Expression<Func<T, TReturn>> aExpression)
        {
            aExpression.Should().NotBeNull();
            aExpression.Body.Should().BeAssignableTo<MemberExpression>();

            var lMemberExpression = (MemberExpression)aExpression.Body;
            var lMemberName = lMemberExpression.Member.Name;
            return lMemberName;
        }
    }
}
