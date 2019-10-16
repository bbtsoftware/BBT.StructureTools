// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using FluentAssertions;

    /// <summary>
    /// EnumerableExtension.
    /// </summary>
    internal static class EnumerableExtension
    {
        /// <summary>
        /// Determines whether a sequence contains only one element.
        /// </summary>
        /// <typeparam name="TEntity">The type of the elements of source.</typeparam>
        /// <param name="aList">The IEnumerable{TEntity} to check for a uniqueness.</param>
        /// <returns>true if the source sequence contains exactly one element; otherwise, false.</returns>
        public static bool Only<TEntity>(this IEnumerable<TEntity> aList)
        {
            aList.Should().NotBeNull();

            if (!aList.Any())
            {
                return false;
            }

            if (aList.Skip(1).Any())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Throws an exception if there is not exactly one element in the sequence. Otherwise the method returns the single item.
        /// The information in <paramref name="aExceptionMessageFormat"/> and <paramref name="aExceptionMessageArguments"/> will formatted with <see cref="CultureInfo.InvariantCulture"/> and added to
        /// the exception message.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of list elements.
        /// </typeparam>
        public static TEntity SingleWithExceptionMessage<TEntity>(this IEnumerable<TEntity> aList, Func<TEntity, bool> aPredicate, string aExceptionMessageFormat, params object[] aExceptionMessageArguments)
        {
            var lFilteredList = aList.Where(aPredicate);
            return lFilteredList.SingleWithExceptionMessage(aExceptionMessageFormat, aExceptionMessageArguments);
        }

        /// <summary>
        /// Throws an exception if there is not exactly one element in the sequence. Otherwise the method returns the single item.
        /// The information in <paramref name="aExceptionMessageFormat"/> and <paramref name="aExceptionMessageArguments"/> will formatted with <see cref="CultureInfo.InvariantCulture"/> and added to
        /// the exception message.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of list elements.
        /// </typeparam>
        public static TEntity SingleWithExceptionMessage<TEntity>(this IEnumerable<TEntity> aList, string aExceptionMessageFormat, params object[] aExceptionMessageArguments)
        {
            aList.Should().NotBeNull();

            if (aList.Only())
            {
                return aList.Single();
            }

            var lMessage = string.Format(CultureInfo.InvariantCulture, aExceptionMessageFormat, aExceptionMessageArguments);

            if (!aList.Any())
            {
                throw new ArgumentException("Enumerable contains no element." + Environment.NewLine + lMessage);
            }

            throw new ArgumentException("Enumerable contains more than one element." + Environment.NewLine + lMessage);
        }
    }
}
