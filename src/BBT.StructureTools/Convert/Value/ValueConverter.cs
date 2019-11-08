namespace BBT.StructureTools.Convert.Value
{
    using System;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class ValueConverter<TSource, TTarget> : IConvertValue<TSource, TTarget>
    {
        private readonly IValueConvertMapping<TSource, TTarget> valueConvertMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueConverter{TSource, TTarget}" /> class.
        /// </summary>
        public ValueConverter(IConvertValueRegistrations<TSource, TTarget> convertValueRegistrations)
        {
            convertValueRegistrations.NotNull(nameof(convertValueRegistrations));

            var registrations = new ConvertValueRegistration<TSource, TTarget>();
            convertValueRegistrations.DoRegistrations(registrations);
            this.valueConvertMap = registrations.EndRegistrations();
        }

        /// <inheritdoc/>
        public TTarget ConvertValue(TSource source)
        {
            if (this.valueConvertMap.TryGetValue(source, out var target))
            {
                return target;
            }
            else if (this.valueConvertMap.IsRegisteredForException(source))
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
