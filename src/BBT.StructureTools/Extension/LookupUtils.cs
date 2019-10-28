// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Extension
{
    using System;

    /// <summary>
    /// Utilities for value lookup.
    /// </summary>
    public static class LookupUtils
    {
        /// <summary>
        /// Determines whether the specified value is the default value for type <typeparamref name="T"/>.
        /// For reference types the value can be <c>null</c>.
        /// </summary>
        /// <typeparam name="T">
        /// The data type.
        /// </typeparam>
        public static bool IsDefaultValue<T>(T aValue)
        {
            if (aValue == null)
            {
                return true;
            }

            var lDefault = default(T);

            var lIsDefault = aValue?.Equals(lDefault) == true;

            return lIsDefault;
        }

        /// <summary>
        /// Returns <paramref name="aSpecificValue"/> if not null or empty
        /// and <paramref name="aDefaultValue"/> otherwise.
        /// </summary>
        /// <typeparam name="T">Type of the value to look up.</typeparam>
        /// <param name="aSpecificValue">The specific value, will be returned if it is not empty.</param>
        /// <param name="aDefaultValue">The default value, will only be returned if <paramref name="aSpecificValue"/> is not empty.</param>
        public static T LookUpValue<T>(T aSpecificValue, T aDefaultValue)
        {
            if (IsDefaultValue(aSpecificValue))
            {
                return aDefaultValue;
            }

            return aSpecificValue;
        }

        /// <summary>
        /// Returns <paramref name="aSpecificValue"/> if
        ///  - <paramref name="aUpperLimitValue"/> is the <typeparamref name="T"/>'s default value
        ///    (the default value has the meaning 'not set')
        ///  - <paramref name="aSpecificValue"/> is less than or equals <paramref name="aUpperLimitValue"/>.
        /// Returns <paramref name="aUpperLimitValue"/> if
        /// - <paramref name="aSpecificValue"/> is greater than <paramref name="aUpperLimitValue"/>
        /// - <paramref name="aSpecificValue"/> is the <typeparamref name="T"/>'s default value
        /// (the default value has the meaning 'not set').
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="aSpecificValue">The specific value.</param>
        /// <param name="aUpperLimitValue">The upper limit value.</param>
        public static T ApplyUpperLimit<T>(T aSpecificValue, T aUpperLimitValue)
            where T : IComparable<T>
        {
            if (IsDefaultValue(aUpperLimitValue))
            {
                return aSpecificValue;
            }

            if (IsDefaultValue(aSpecificValue))
            {
                return aUpperLimitValue;
            }

            if (aSpecificValue.CompareTo(aUpperLimitValue) <= 0)
            {
                return aSpecificValue;
            }

            return aUpperLimitValue;
        }

        /// <summary>
        /// Returns <paramref name="aSpecificValue"/> if
        ///  - <paramref name="aLowerLimitValue"/> is the <typeparamref name="T"/>'s default value
        ///    (the default value has the meaning 'not set')
        ///  - <paramref name="aSpecificValue"/> is greater than or equals <paramref name="aLowerLimitValue"/>.
        /// Returns <paramref name="aLowerLimitValue"/> if
        /// - <paramref name="aSpecificValue"/> is greater than <paramref name="aLowerLimitValue"/>
        /// - <paramref name="aSpecificValue"/> is the <typeparamref name="T"/>'s default value
        ///   (the default value has the meaning 'not set').
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="aSpecificValue">The specific value.</param>
        /// <param name="aLowerLimitValue">The upper limit value.</param>
        public static T ApplyLowerLimit<T>(T aSpecificValue, T aLowerLimitValue)
            where T : IComparable<T>
        {
            if (IsDefaultValue(aLowerLimitValue))
            {
                return aSpecificValue;
            }

            if (IsDefaultValue(aSpecificValue))
            {
                return aLowerLimitValue;
            }

            if (aSpecificValue.CompareTo(aLowerLimitValue) >= 0)
            {
                return aSpecificValue;
            }

            return aLowerLimitValue;
        }
    }
}
