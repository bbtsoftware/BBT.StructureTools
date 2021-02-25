namespace BBT.StructureTools.Extension
{
    using System;

    /// <summary>
    /// Utilities for value lookup.
    /// </summary>
    internal static class LookupUtils
    {
        /// <summary>
        /// Determines whether the specified value is the default value for type <typeparamref name="T"/>.
        /// For reference types the value can be <c>null</c>.
        /// </summary>
        /// <typeparam name="T">
        /// The data type.
        /// </typeparam>
        internal static bool IsDefaultValue<T>(T value)
        {
            if (value == null)
            {
                return true;
            }

            var defaultValue = default(T);

            var isDefault = value.Equals(defaultValue);

            return isDefault;
        }

        /// <summary>
        /// Returns <paramref name="specificValue"/> if
        ///  - <paramref name="upperLimitValue"/> is the <typeparamref name="T"/>'s default value
        ///    (the default value has the meaning 'not set')
        ///  - <paramref name="specificValue"/> is less than or equals <paramref name="upperLimitValue"/>.
        /// Returns <paramref name="upperLimitValue"/> if
        /// - <paramref name="specificValue"/> is greater than <paramref name="upperLimitValue"/>
        /// - <paramref name="specificValue"/> is the <typeparamref name="T"/>'s default value
        /// (the default value has the meaning 'not set').
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="specificValue">The specific value.</param>
        /// <param name="upperLimitValue">The upper limit value.</param>
        internal static T ApplyUpperLimit<T>(T specificValue, T upperLimitValue)
            where T : IComparable<T>
        {
            if (IsDefaultValue(upperLimitValue))
            {
                return specificValue;
            }

            if (IsDefaultValue(specificValue))
            {
                return upperLimitValue;
            }

            if (specificValue.CompareTo(upperLimitValue) <= 0)
            {
                return specificValue;
            }

            return upperLimitValue;
        }
    }
}
