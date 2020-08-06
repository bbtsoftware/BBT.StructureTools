namespace BBT.StructureTools.Extension
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Common runtime checks that throw <see cref="ArgumentException"/> upon failure.
    /// </summary>
    internal static class StructureToolsArgumentChecks
    {
        /// <summary>
        /// Throws an exception if the specified parameter's value is null.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        [DebuggerStepThrough]
        internal static void NotNull<T>([ValidatedNotNull]this T value, string parameterName)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null or empty.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty.</exception>
        [DebuggerStepThrough]
        internal static void NotNullOrEmpty(this string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Empty list.", parameterName);
            }
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null, or not of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the parameter.</typeparam>
        /// <param name="value">The object which is checked.</param>
        /// <param name="objName">The name of the object variable to include in any thrown exception.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidCastException">Thrown if <paramref name="value"/> is not of type <typeparamref name="T"/>.</exception>
        [DebuggerStepThrough]
        internal static T IsOfType<T>(this object value, string objName)
        {
            value.NotNull(objName);

            if (value is T castedValue)
            {
                return castedValue;
            }

            throw new InvalidCastException(FormattableString.Invariant($"{objName} isn't of type {typeof(T)}."));
        }
    }
}
