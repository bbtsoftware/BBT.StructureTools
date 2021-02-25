namespace BBT.StructureTools.Provider
{
    using System;

    /// <summary>
    /// Provides default values for types where the <c>default</c>
    /// keyword doesn't reflect the actually used default value.
    /// </summary>
    public interface IDefaultValueProvider
    {
        /// <summary>
        /// Registers the default value <paramref name="value"/> for a type
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of <paramref name="value"/>.</typeparam>
        void RegisterDefault<T>(T value, params T[] additionalValues);

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
        T ApplyUpperLimit<T>(T specificValue, T upperLimitValue)
            where T : IComparable<T>;

        /// <summary>
        /// returns true if the value <paramref name="value"/> is the default.
        /// </summary>
        /// <typeparam name="T">Type of <paramref name="value"/>.</typeparam>
        bool IsDefault<T>(T value);
    }
}
