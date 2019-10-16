// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Value
{
    using System;
    using FluentAssertions;

    /// <summary>
    /// Implementation for <see cref="IConvertValue{TSource, TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">Source type.</typeparam>
    /// <typeparam name="TTarget">Target type.</typeparam>
    public class ValueConverter<TSource, TTarget> : IConvertValue<TSource, TTarget>
    {
        private readonly IValueConvertMapping<TSource, TTarget> mValueConvertMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueConverter{TSource, TTarget}" /> class.
        /// </summary>
        public ValueConverter(IConvertValueRegistrations<TSource, TTarget> aConvertValueRegistrations)
        {
            aConvertValueRegistrations.Should().NotBeNull();

            var lRegistrations = new ConvertValueRegistration<TSource, TTarget>();
            aConvertValueRegistrations.DoRegistrations(lRegistrations);
            this.mValueConvertMap = lRegistrations.EndRegistrations();
        }

        /// <summary>
        /// See <see cref="IConvertValue{TSource, TTarget}"/>.
        /// </summary>
        public TTarget ConvertValue(TSource source)
        {
            if (this.mValueConvertMap.TryGetValue(source, out var target))
            {
                return target;
            }
            else if (this.mValueConvertMap.IsRegisteredForException(source))
            {
                throw new CopyConvertCompareException(FormattableString.Invariant($"Conversion of source value {source} of type {typeof(TSource)} to type {typeof(TTarget)} is not supported (by design)."));
            }
            else
            {
                throw new CopyConvertCompareException(FormattableString.Invariant($"Neither a mapping nor an exception is defined for the source value {source}."));
            }
        }
    }
}
