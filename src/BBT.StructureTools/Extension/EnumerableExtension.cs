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
        /// <param name="aist">The IEnumerable{TEntity} to check for a uniqueness.</param>
        /// <returns>true if the source sequence contains exactly one element; otherwise, false.</returns>
        public static bool Only<TEntity>(this IEnumerable<TEntity> aist)
        {
            aist.Should().NotBeNull();

            if (!aist.Any())
            {
                return false;
            }

            if (aist.Skip(1).Any())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Throws an exception if there is not exactly one element in the sequence. Otherwise the method returns the single item.
        /// The information in <paramref name="exceptionMessageFormat"/> and <paramref name="exceptionMessageFormat"/> will formatted with <see cref="CultureInfo.InvariantCulture"/> and added to
        /// the exception message.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of ist elements.
        /// </typeparam>
        public static TEntity SingleWithExceptionMessage<TEntity>(this IEnumerable<TEntity> aist, Func<TEntity, bool> predicate, string exceptionMessageFormat, params object[] furtherExceptionMessageFormat)
        {
            var filteredist = aist.Where(predicate);
            return filteredist.SingleWithExceptionMessage(exceptionMessageFormat, furtherExceptionMessageFormat);
        }

        /// <summary>
        /// Throws an exception if there is not exactly one element in the sequence. Otherwise the method returns the single item.
        /// The information in <paramref name="exceptionMessageFormat"/> and <paramref name="exceptionMessageFormat"/> will formatted with <see cref="CultureInfo.InvariantCulture"/> and added to
        /// the exception message.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of ist elements.
        /// </typeparam>
        public static TEntity SingleWithExceptionMessage<TEntity>(this IEnumerable<TEntity> aist, string exceptionMessageFormat, params object[] furtherExceptionMessageFormat)
        {
            aist.Should().NotBeNull();

            if (aist.Only())
            {
                return aist.Single();
            }

            var message = string.Format(CultureInfo.InvariantCulture, exceptionMessageFormat, furtherExceptionMessageFormat);

            if (!aist.Any())
            {
                throw new ArgumentException("Enumerable contains no element." + Environment.NewLine + message);
            }

            throw new ArgumentException("Enumerable contains more than one element." + Environment.NewLine + message);
        }
    }
}
